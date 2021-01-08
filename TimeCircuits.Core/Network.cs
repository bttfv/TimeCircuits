using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TimeCircuits.Core
{
    public static class Network
    {
        private static UdpClient udp = new UdpClient(1985);

        private static Display _display;

        private static void Receive(IAsyncResult ar)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 1985);

            string message = Encoding.ASCII.GetString(udp.EndReceive(ar, ref ip));

            if (message.StartsWith("Speed="))
            {
                message = message.Replace("Speed=", "");

                _display.Speed = int.Parse(message);
            }

            if (message.StartsWith("Empty="))
            {
                message = message.Replace("Empty=", "");

                _display.Empty = (EmptyType)int.Parse(message);
            }

            if (message.StartsWith("IsHUDVisible="))
            {
                message = message.Replace("IsHUDVisible=", "");

                _display.IsHUDVisible = bool.Parse(message);
            }

            if (message.StartsWith("IsTickVisible="))
            {
                message = message.Replace("IsTickVisible=", "");

                _display.IsTickVisible = bool.Parse(message);
            }

            if (message.StartsWith("SetOff=1"))
                _display.SetOff();

            if (message.StartsWith("SetDate="))
            {
                message = message.Replace("SetDate=", "");

                string[] split = message.Split('|');

                _display.SetDate(split[0], DateTime.Parse(split[1]));
            }

            if (message.StartsWith("SetVisible="))
            {
                message = message.Replace("SetVisible=", "");

                string[] split = message.Split('|');

                _display.SetVisible(split[0], bool.Parse(split[1]), bool.Parse(split[2]), bool.Parse(split[3]), bool.Parse(split[4]), bool.Parse(split[5]), bool.Parse(split[6]), bool.Parse(split[7]));
            }

            StartListening(_display);
        }

        public static void StartListening(Display display)
        {
            _display = display;
            udp.BeginReceive(Receive, new object());
        }
    }
}
