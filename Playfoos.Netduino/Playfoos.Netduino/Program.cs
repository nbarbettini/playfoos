using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace NetchemiaFooz
{
    public class Program
    {
        // workers for each thread
        //private static Thread breathingCycleWorker = new Thread(new ThreadStart(LedManager.BreathingMain));
        private static Thread flasherCycleWorker = new Thread(new ThreadStart(LedManager.FlasherMain));
        private static Thread eventListenerWorker = new Thread(new ThreadStart(EventListener.MainThread));
        private static Thread networkManagerWorker = new Thread(new ThreadStart(NetworkManager.MainThread));

        public static void Main()
        {
            // prep the glitch filter so we don't count stuff on bootup
            EventListener.switchLast = DateTime.Now.AddMilliseconds(1000);

            // watch for messages in the queue and push them out onto to the network 
            networkManagerWorker.Start();
            // start LED strip threads
            //breathingCycleWorker.Start();
            flasherCycleWorker.Start();
            LedManager.Startup(); // green flash
            // watch for events and put messages in the queue
            eventListenerWorker.Start();

            // put the main thread to sleep
            Thread.Sleep(Timeout.Infinite);
        }

        public static void YellowScore()
        {
            Debug.Print("Yellow score\n");
            NetworkManager.messageQueue.Add("score,1"); // 1 = yellow
            LedManager.flashYellow = true;
        }

        public static void BlackScore()
        {
            Debug.Print("Black score\n");
            NetworkManager.messageQueue.Add("score,0");  // 0 = black
            LedManager.flashBlack = true;

        }

    }
}