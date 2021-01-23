using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;
using System;
using BackToTheFutureV.HUD.Core;

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
    public class Activity1 : AndroidGameActivity
    {
        private BackToTheFutureV.HUD.Core.HUDDisplay _display;
        private View _view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _display = new BackToTheFutureV.HUD.Core.HUDDisplay();
            _view = _display.Services.GetService(typeof(View)) as View;

            SetContentView(_view);

            Window.AddFlags(WindowManagerFlags.Fullscreen);
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);

            _view.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.HideNavigation | (StatusBarVisibility)SystemUiFlags.Immersive | (StatusBarVisibility)SystemUiFlags.Fullscreen;

            _display.Run();

#if DEBUG
            _display.Properties.IsHUDVisible = true;
            _display.Properties.IsTickVisible = true;
            _display.Properties.Empty = EmptyType.Off;
            _display.Properties.Speed = 88;

            _display.Properties.SetDate("red", DateTime.Now);
            _display.Properties.SetDate("green", DateTime.Now);
            _display.Properties.SetDate("yellow", DateTime.Now);
#endif

            HUDNetwork.Start(_display);
        }
    }
}
