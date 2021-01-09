using System;

namespace TimeCircuits.Desktop
{
    public static class Program
    {
        //[STAThread]
        static void Main()
        {
            using (var game = new Display())
            {
                //game.IsHUDVisible = true;
                //game.IsTickVisible = true;
                //game.Empty = EmptyType.Off;
                //game.Speed = 88;

                //game.SetDate("red", DateTime.Now);
                //game.SetDate("green", DateTime.Now);
                //game.SetDate("yellow", DateTime.Now);

                Core.Network.Start(game);

                game.Run();
              }                
        }
    }
}
