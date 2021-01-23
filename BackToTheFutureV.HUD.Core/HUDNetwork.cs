using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BackToTheFutureV.HUD.Core
{
    public static class HUDNetwork
    {
        private static UdpClient udp = new UdpClient(1985);

        private static HUDDisplay _display;

        private static void Receive(IAsyncResult ar)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 1985);

            byte[] ret = udp.EndReceive(ar, ref ip);

            HUDCommand command = HUDCommand.FromData(ret);

            if (command != null)
            {
                switch (command.Verb)
                {
                    case "HUDProperties":

                        _display.Properties = command.Get<HUDProperties>();

                        break;
                }
            }
            
            Start(_display);
        }

        public static void Start(HUDDisplay display)
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
