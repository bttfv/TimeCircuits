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
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.SensorLandscape,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden
    )]
    public class Activity1 : AndroidGameActivity
    {
        private Display _game;
        private View _view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _game = new Display();
            _view = _game.Services.GetService(typeof(View)) as View;

            SetContentView(_view);

            _game.IsHUDVisible = true;
            _game.IsTickVisible = true;
            _game.Empty = EmptyType.Off;
            _game.Speed = 88;

            _game.SetDate("red", DateTime.Now);
            _game.SetDate("green", DateTime.Now);
            _game.SetDate("yellow", DateTime.Now);

            _game.Run();
        }
    }
}
