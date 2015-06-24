using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace NetchemiaFooz
{
    static class EventListener
    {
        // ultrasonic detectors
        private static TristatePort yellowDetector = new TristatePort(Pins.GPIO_PIN_D1, false, false, ResistorModes.Disabled);
        private static TristatePort blackDetector = new TristatePort(Pins.GPIO_PIN_D2, false, false, ResistorModes.Disabled);

        // software glitch filters
        public static DateTime switchLast;
        private const int interruptTimeout = 2000;
        private const int distanceLimit = 115; //mm

        public static void MainThread()
        {
            bool y1 = false;
            bool y2 = true;
            long yTick1;
            long yTick2;
            int yellowDistance;

            bool b1 = false;
            bool b2 = true;
            long bTick1;
            long bTick2;
            int blackDistance;

            while (true)
            {
                // ** PULSE YELLOW SENSOR
                // First we need to pulse the port ... high, low.  It seems to work fine, without adding any delay.
                yellowDetector.Active = true;                // Put port in write mode
                yellowDetector.Write(true);                  // Pulse pin
                yellowDetector.Write(false);
                yellowDetector.Active = false;                        // Put port in read mode;

                y1 = false;
                while (y1 == false) { y1 = yellowDetector.Read(); }   // Wait for the line to go high.
                yTick1 = System.DateTime.Now.Ticks;      // Save start ticks.

                y2 = true;
                while (y2 == true) { y2 = yellowDetector.Read(); }    // Wait for the line to go low.
                yTick2 = System.DateTime.Now.Ticks;      // Save end ticks. 

                yellowDistance = ((int)(yTick2 - yTick1)) * 10 / 583;
                if (yellowDistance == 0) yellowDistance = 999;

                // ** PULSE BLACK SENSOR
                // First we need to pulse the port ... high, low.  It seems to work fine, without adding any delay.
                blackDetector.Active = true;                // Put port in write mode
                blackDetector.Write(true);                  // Pulse pin
                blackDetector.Write(false);
                blackDetector.Active = false;                        // Put port in read mode;

                b1 = false;
                while (b1 == false) { b1 = blackDetector.Read(); }   // Wait for the line to go high.
                bTick1 = System.DateTime.Now.Ticks;      // Save start ticks.

                b2 = true;
                while (b2 == true) { b2 = blackDetector.Read(); }    // Wait for the line to go low.
                bTick2 = System.DateTime.Now.Ticks;      // Save end ticks. 

                blackDistance = ((int)(bTick2 - bTick1)) * 10 / 583;
                if (blackDistance == 0) blackDistance = 999;

                //Debug.Print("Yellow: " + yellowDistance.ToString() + "mm   Black: " + blackDistance.ToString() + "mm");

                // ** SCORING
                if (switchLast.AddMilliseconds(interruptTimeout) < DateTime.Now)
                {
                    if (yellowDistance < distanceLimit || blackDistance < distanceLimit)
                    {
                        // yellow scoring
                        if (yellowDistance < distanceLimit) { Program.YellowScore(); }
                        // black scoring
                        else if (blackDistance < distanceLimit) { Program.BlackScore(); }
                        switchLast = DateTime.Now;
                    }
                }

                // pulse pretty fast
                Thread.Sleep(10);
            }
        }

    }
}
