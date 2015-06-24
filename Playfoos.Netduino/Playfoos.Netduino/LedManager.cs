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
        private const int BlackSide = 0;
        private const int YellowSide = 1;
        private const int FlashRepeat = 3;

        // public flags that are set by the main thread when a flash needs to happen
        public static bool flashYellow = false;
        public static bool flashBlack = false;

        // a strip flash thread worker
        public static void FlasherMain()
        {
            while (true)
            {
                if (flashYellow || flashBlack)
                {
                    if (flashYellow) { WriteLeds(Yellow, YellowSide, true); flashYellow = false; }
                    if (flashBlack) { WriteLeds(White, BlackSide, true); flashBlack = false; }
                }
                Thread.Sleep(100);
            }
        }

        public static void Init()
        {
            // Turn both sides on (white and yellow)
            WriteLeds(White, BlackSide, false);
            WriteLeds(Yellow, YellowSide, false);
        }

        private static void WriteLeds(int Color, int Side, bool Flash)
        {
            int startLed = Side * 16;
            int maxLed = 16;

            stripInterrupt = true;
            if (Flash)
            {
                for (int repeat = 1; repeat <= FlashRepeat; repeat++)
                {
                    for (int i = startLed; i < (startLed + maxLed); i++) { Chain.SetBrightness(i, 0, true); }
                    Chain.Write();
                    Thread.Sleep(75);

                    for (int i = startLed; i < (startLed + maxLed); i++)
                    {
                        Chain.SetBrightness(i, 255, true);
                        Chain.SetColor(i, Color, true);
                    }
                    Chain.Write();
                    Thread.Sleep(50);
                }
            }
            
            // Set back to normal
            for (int i = startLed; i < (startLed + maxLed); i++)
            {
                Chain.SetBrightness(i, 255, true);
                Chain.SetColor(i, Color, true);
            }
            Chain.Write();
            stripInterrupt = false;
        }
    }
}
