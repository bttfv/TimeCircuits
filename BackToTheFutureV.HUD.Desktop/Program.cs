using System;
using BackToTheFutureV.HUD.Core;

namespace TimeCircuits.Desktop
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new HUDDisplay())
            {

#if DEBUG
                //game.Properties.IsHUDVisible = true;
                //game.Properties.IsTickVisible = true;
                //game.Properties.Empty = EmptyType.Off;
                //game.Properties.Speed = 88;

                //game.Properties.SetDate("red", DateTime.Now);
                //game.Properties.SetDate("green", DateTime.Now);
                //game.Properties.SetDate("yellow", DateTime.Now);
#endif

                HUDNetwork.Start(game);

                game.Run();
              }                
        }
    }
}
