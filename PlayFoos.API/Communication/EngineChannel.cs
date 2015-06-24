using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PlayFoos.API.Communication
{
    public sealed class EngineChannel : IEngineChannel
    {
        private readonly IClientChannel _clientChannel;

        private readonly IHubProxy _hub;
        private readonly HubConnection _connection;

        public EngineChannel(IClientChannel clientChannel, string url)
        {
            _clientChannel = clientChannel;

            _connection = new HubConnection(url);
            _connection.TransportConnectTimeout = TimeSpan.FromSeconds(5);
            _hub = _connection.CreateHubProxy("NotifyHub");
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
            _hub.On<Core.Model.Game>("UpdateGameState", state =>
            {
                _clientChannel.BroadcastGameState(state);
            });

            _connection.Start();
        }

        public async Task TriggerUpdate()
        {
            await StartIfNecessary();

            try
            {
                await _hub.Invoke("Update");
            }
            catch (Exception e)
            {
                // log and swallow
            }
        }

        public async Task StartIfNecessary()
        {
            try
            {
                if (_connection.State == ConnectionState.Disconnected)
                    await _connection.Start();
            }
            catch (Exception e)
            {
                // log and swallow
                // TODO
            }
        }

        public async Task Stop()
        {
            _connection.Stop();
        }

        public bool IsConnected()
        {
            return (_connection.State == ConnectionState.Connected);
        }
    }
}