using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using BackToTheFutureV.HUD.Core;
using Microsoft.Xna.Framework;

namespace TimeCircuits.Android
{
    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        Icon = "@drawable/icon",
        AlwaysRetainTaskState = false,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.SensorLandscape,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden
    )]
    public class Program : AndroidGameActivity
    {
        private HUDDisplay game;
        private View _view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            game = new HUDDisplay();
            _view = game.Services.GetService(typeof(View)) as View;

            SetContentView(_view);

            Window.AddFlags(WindowManagerFlags.Fullscreen);
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);

            _view.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.HideNavigation | (StatusBarVisibility)SystemUiFlags.Immersive | (StatusBarVisibility)SystemUiFlags.Fullscreen;

            game.Graphics.IsFullScreen = true;

            game.Run();

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
        }
    }
}
