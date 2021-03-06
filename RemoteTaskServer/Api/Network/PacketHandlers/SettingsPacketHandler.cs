﻿#region

using System;
using UlteriusServer.Api.Network.Messages;
using UlteriusServer.Utilities;
using UlteriusServer.WebSocketAPI.Authentication;
using vtortola.WebSockets;

#endregion

namespace UlteriusServer.Api.Network.PacketHandlers
{
    public class SettingsPacketHandler : PacketHandler
    {
        private MessageBuilder _builder;
        private AuthClient _authClient;
        private Packet _packet;
        private WebSocket _client;


        public void ChangeWebServerPort()
        {
            var port = int.Parse(_packet.Args[0].ToString());
            Settings.Get("WebServer").WebServerPort = port;
            Settings.Save();
            var currentPort = (int) Settings.Get("WebServer").WebServerPort;
            var data = new
            {
                changedStatus = true,
                WebServerPort = currentPort
            };
            _builder.WriteMessage(data);
        }

        public void ChangeWebFilePath()
        {
            var path = _packet.Args[0].ToString();
            Settings.Get("WebServer").WebFilePath = path;
            Settings.Save();
            var currentPath = (string) Settings.Get("WebServer").WebFilePath;
            var data = new
            {
                changedStatus = true,
                WebFilePath = currentPath
            };
            _builder.WriteMessage(data);
        }

        public void ChangeScreenSharePass()
        {
            var pass = _packet.Args[0].ToString();
            Settings.Get("ScreenShareService").ScreenSharePass = pass;
            Settings.Save();
            var currentPass = (string) Settings.Get("ScreenShareService").ScreenSharePass;
            var data = new
            {
                changedStatus = true,
                ScreenSharePass = currentPass
            };
            _builder.WriteMessage(data);
        }

        public void ChangeScreenSharePort()
        {
            var port = int.Parse(_packet.Args[0].ToString());
            Settings.Get("ScreenShareService").ScreenSharePort = port;
            Settings.Save();
            var currentPort = (int) Settings.Get("ScreenShareService").ScreenSharePort;
            var data = new
            {
                changedStatus = true,
                ScreenSharePort = currentPort
            };
            _builder.WriteMessage(data);
        }


        public void ChangeWebServerUse()
        {
            var useServer = Convert.ToBoolean(_packet.Args[0]);
            Settings.Get("WebServer").ToggleWebServer = useServer;
            Settings.Save();
            var currentStatus = (bool) Settings.Get("WebServer").ToggleWebServer;
            var data = new
            {
                changedStatus = true,
                UseWebServer = currentStatus
            };
            _builder.WriteMessage(data);
        }

        public void ChangeNetworkResolve()
        {
            var resolve = Convert.ToBoolean(_packet.Args[0]);
            Settings.Get("Network").SkipHostNameResolve = resolve;
            Settings.Save();
            var currentStatus = (bool) Settings.Get("Network").SkipHostNameResolve;
            var data = new
            {
                changedStatus = true,
                SkipHostNameResolve = currentStatus
            };
            _builder.WriteMessage(data);
        }

        public void ChangeTaskServerPort()
        {
            var port = int.Parse(_packet.Args[0].ToString());
            Settings.Get("TaskServer").TaskServerPort = port;
            Settings.Save();
            var currentPort = (int) Settings.Get("TaskServer").TaskServerPort;
            var data = new
            {
                changedStatus = true,
                TaskServerPort = currentPort
            };
            _builder.WriteMessage(data);
        }

        public void ChangeLoadPlugins()
        {
            var loadPlugins = Convert.ToBoolean(_packet.Args[0]);
            Settings.Get("Plugins").LoadPlugins = loadPlugins;
            Settings.Save();

            var currentStatus = Convert.ToBoolean(Settings.Get("Plugins").LoadPlugins);
            var data = new
            {
                changedStatus = true,
                LoadPlugins = currentStatus
            };
            _builder.WriteMessage(data);
        }

        public void ChangeUseTerminal()
        {
            var useTerminal = Convert.ToBoolean(_packet.Args[0]);
            Settings.Get("Terminal").AllowTerminal = useTerminal;
            Settings.Save();
            var currentStatus = Convert.ToBoolean(Settings.Get("Terminal").AllowTerminal);
            var data = new
            {
                changedStatus = true,
                AllowTerminal = currentStatus
            };
            _builder.WriteMessage(data);
        }

        public void GetCurrentSettings()

        {
            _builder.WriteMessage(Settings.GetRaw());
        }

        public override void HandlePacket(Packet packet)
        {
            _client = packet.Client;
            _authClient = packet.AuthClient;
            _packet = packet;
            _builder = new MessageBuilder(_authClient, _client, _packet.EndPoint, _packet.SyncKey);
            switch (_packet.PacketType)
            {
                case PacketManager.PacketTypes.ToggleWebServer:
                    ChangeWebServerUse();
                    break;
                case PacketManager.PacketTypes.ChangeWebServerPort:
                    ChangeWebServerPort();
                    break;
                case PacketManager.PacketTypes.ChangeWebFilePath:
                    ChangeWebFilePath();
                    break;
                case PacketManager.PacketTypes.ChangeTaskServerPort:
                    ChangeTaskServerPort();
                    break;
                case PacketManager.PacketTypes.ChangeScreenSharePort:
                    ChangeScreenSharePort();
                    break;
                case PacketManager.PacketTypes.ChangeScreenSharePass:
                    ChangeScreenSharePass();
                    break;
                case PacketManager.PacketTypes.ChangeNetworkResolve:
                    ChangeNetworkResolve();
                    break;
                case PacketManager.PacketTypes.ChangeLoadPlugins:
                    ChangeLoadPlugins();
                    break;
                case PacketManager.PacketTypes.ChangeUseTerminal:
                    ChangeUseTerminal();
                    break;
                case PacketManager.PacketTypes.GetCurrentSettings:
                    GetCurrentSettings();
                    break;
            }
        }
    }
}