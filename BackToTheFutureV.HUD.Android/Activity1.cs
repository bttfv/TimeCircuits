using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;
using System;

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
        private Display _display;
        private View _view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _display = new Display();
            _view = _display.Services.GetService(typeof(View)) as View;

            SetContentView(_view);

            Window.AddFlags(WindowManagerFlags.Fullscreen);
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);

            _view.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.HideNavigation | (StatusBarVisibility)SystemUiFlags.Immersive | (StatusBarVisibility)SystemUiFlags.Fullscreen;

            _display.Run();

#if DEBUG
            _display.IsHUDVisible = true;
            _display.IsTickVisible = true;
            _display.Empty = EmptyType.Off;
            _display.Speed = 88;

            _display.SetDate("red", DateTime.Now);
            _display.SetDate("green", DateTime.Now);
            _display.SetDate("yellow", DateTime.Now);
#endif

            Core.Network.Start(_display);
        }
    }
}
