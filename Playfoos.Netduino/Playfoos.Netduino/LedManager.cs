using System;
using Microsoft.SPOT;
using Toolbox.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.Threading;

namespace NetchemiaFooz
{
    class LedManager
    {
        // There's a 1M strip (32 LEDs) connected to the first SPI bus on the Netduino
        private static RgbLedStrip Chain = new RgbLedStrip(RgbLedStrip.Chipsets.LPD8806, 32, SPI_Devices.SPI1);
        private const int Yellow = 0xffff00;
        private const int Blue = 0x0000ff;
        private const int Green = 0x00ff00;
        private const int White = 0xffffff;
        private static bool stripInterrupt = false;

        // pulsing light cycle
        private static DateTime lastBreath = DateTime.Now;
        public static DateTime lastScore = DateTime.Now;

        // public flags that are set by the main thread when a flash needs to happen
        public static bool flashYellow = false;
        public static bool flashBlack = false;

        // an idle "breathing" pulse thread worker
        public static void BreathingMain()
        {
            while (true)
            {
                if (lastScore.AddSeconds(30) > DateTime.Now) continue;
                if (lastBreath.AddMilliseconds(5000) > DateTime.Now) continue;

                Chain.SetBrightnessAll(0, true);
                Chain.SetColorAll(White, true);
                Chain.Write();
                for (int i = 0; i < 255; i++)
                {
                    if (stripInterrupt) break;

                    Chain.SetBrightnessAll((byte)i, true); Chain.Write();
                    Thread.Sleep(1);
                }
                for (int i = 0; i < 255; i++)
                {
                    if (stripInterrupt) break;

                    Chain.SetBrightnessAll((byte)(255 - i), true); Chain.Write();
                    Thread.Sleep(1);
                }
                lastBreath = DateTime.Now;
                Thread.Sleep(100);
            }
        }

        // a strip flash thread worker
        public static void FlasherMain()
        {
            while (true)
            {
                if (flashYellow || flashBlack)
                {
                    if (flashYellow) { FlashColor(Yellow, 2); flashYellow = false; }
                    if (flashBlack) { FlashColor(Blue, 2); flashBlack = false; }
                    lastBreath = DateTime.Now;
                }
                Thread.Sleep(100);
            }
        }

        public static void Startup()
        {
            // A friendly green flash to say "we're ready"
            FlashColor(Green, 1);
            Thread.Sleep(100);
            FlashColor(Green, 1);
        }

        private static void FlashColor(int Color, int Repeat)
        {
            stripInterrupt = true;
            Chain.SetBrightnessAll(0, true);
            Chain.SetColorAll(Color, true);
            Chain.Write();
            for (int i = 0; i < Repeat; i++)
            {
                Chain.SetBrightnessAll(255, true); Chain.Write();
                Thread.Sleep(75);
                Chain.SetBrightnessAll(0, true); Chain.Write();
                Thread.Sleep(50);
            }
            stripInterrupt = false;
        }

    }
}
