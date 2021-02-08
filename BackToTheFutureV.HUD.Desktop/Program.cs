using BackToTheFutureV.HUD.Core;
using System;

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
                game.Properties.IsHUDVisible = true;
                game.Properties.IsTickVisible = true;
                game.Properties.Empty = EmptyType.Off;
                game.Properties.Speed = 88;

                game.Properties.SetDate("red", DateTime.Now);
                game.Properties.SetDate("green", DateTime.Now);
                game.Properties.SetDate("yellow", DateTime.Now);

                for (int column = 0; column < 10; column++)
                    for (int row = 0; row < 20; row++)
                        game.Properties.LedState[column][row] = true;
#endif

                HUDNetwork.Start(game);

                game.Run();
            }
        }
    }
}
