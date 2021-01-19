using System;
using System.Linq;
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

            byte[] ret = udp.EndReceive(ar, ref ip);

            string message = Encoding.ASCII.GetString(ret);

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

            if (message.StartsWith("SetLedState="))
            {
                int pos = "SetLedState=".Count();

                for (int column = 0; column < 10; column++)
                    for (int row = 0; row < 20; row++)
                    {
                        _display.ledState[column][row] = Convert.ToBoolean(ret[pos]);
                        pos++;
                    }                        
            }

            Start(_display);
        }

        public static void Start(Display display)
        {
            _display = display;

            if (udp == null)
                udp = new UdpClient(1985);

            udp.BeginReceive(Receive, new object());
        }

        public static void Stop()
        {
            udp.Dispose();
            udp = null;
        }
    }
}
