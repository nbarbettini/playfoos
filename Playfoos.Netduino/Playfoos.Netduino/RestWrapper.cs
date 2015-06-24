using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using Toolbox.NETMF.NET;

namespace NetchemiaFooz
{
    public class RestWrapper
    {
        public string targetURL { get; set; }
        public ushort targetPort { get; set; }

        public RestWrapper(string url, ushort port)
        {
            this.targetURL = url;
            this.targetPort = port;
        }
        
        public string PostEvent(string eventName, string data)
        {
            // Creates a new web session
            HTTP_Client WebSession = new HTTP_Client(new IntegratedSocket(this.targetURL, this.targetPort));
            HTTP_Client.HTTP_Response Response = WebSession.Post("/event", "eventName=" + eventName + "&data=" + data);

            // Did we get the expected response? (a "200 OK")
            if (Response.ResponseCode == 200)
                return Response.ResponseBody;
            else
                return null;
        }
    }
}
