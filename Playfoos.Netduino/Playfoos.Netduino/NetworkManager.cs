using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using Toolbox.NETMF.NET;

namespace NetchemiaFooz
{
    static class NetworkManager
    {
        public const string hostname = "192.168.5.5";
        
        // the Program thread can push stuff onto this list
        public static ArrayList messageQueue = new ArrayList();

        public static void MainThread()
        {
            while (true)
            {
                if (messageQueue.Count > 0)
                {
                    string eventName = messageQueue[0].ToString().Split(',')[0];
                    string data = messageQueue[0].ToString().Split(',')[1];
                    string fullUri = "/playfoos/api/" + eventName + "/" + data;

                    HTTP_Client WebSession = new HTTP_Client(new IntegratedSocket(hostname, 80));
                    Debug.Print("POST " + WebSession.Hostname + ":" + WebSession.Port + fullUri);
                    HTTP_Client.HTTP_Response Response = WebSession.Post(fullUri);
                    Debug.Print(Response.ToString() + "\n");

                    messageQueue.RemoveAt(0);
                }
                Thread.Sleep(10);
            }
        }

    }
}
