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
        public const string baseURL = "http://192.168.5.1/playfoos/api";
        
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
                    string fullUri = "/api/" + eventName + "/" + data;
                    //Debug.Print(fullUri + "\n");

                    HTTP_Client WebSession = new HTTP_Client(new IntegratedSocket(baseURL, 80));
                    //Debug.Print("Socket Open\n");
                    HTTP_Client.HTTP_Response Response = WebSession.Post(fullUri);
                    //Debug.Print(Response.ToString() + "\n");

                    // Did we get the expected response? (a "200 OK")
                    //if (Response.ResponseCode == 200) { }
                    //    // todo

                    messageQueue.RemoveAt(0);
                }
                Thread.Sleep(10);
            }
        }

    }
}
