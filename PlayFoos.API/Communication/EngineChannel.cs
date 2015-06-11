using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PlayFoos.API.Communication
{
    public static class EngineChannel
    {
        public static IHubProxy Hub { get; private set; }
        private static HubConnection _connection;

        public static void Initialize(string url)
        {
            _connection = new HubConnection(url);
            _connection.TransportConnectTimeout = TimeSpan.FromSeconds(5);
            Hub = _connection.CreateHubProxy("NotifyHub");
            _connection.StateChanged += (s) =>
            {
                Console.WriteLine("Changed from {0} to {1}", s.OldState, s.NewState);
            };
            _connection.Error += (ex) =>
            {
                Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    await _connection.Start();
                });
            };

            // Rebroadcast behavior for all state update messages pushed from engine
            Hub.On<Core.Model.Game>("UpdateGameState", state =>
            {
                ClientChannel.Hub.Clients.All.UpdateGameState(state);
            });
        }

        public static async Task Start()
        {
            await _connection.Start();
        }

        public static async Task Update()
        {
            if (_connection.State == ConnectionState.Disconnected)
                await _connection.Start();

            await Hub.Invoke("Update");
        }

        public static void Stop()
        {
            _connection.Stop();
        }
    }
}