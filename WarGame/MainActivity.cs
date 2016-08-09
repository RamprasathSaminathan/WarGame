using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace WarGame
{
    [Activity(Label = "WarGame", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Button button = FindViewById<Button>(Resource.Id.startGame);
            button.Click += delegate
            {
                //start a new game and move to the game play activity
                var intent = new Intent(this, typeof(GamePlayActivity));
                StartActivity(intent);
            };
        }
    }
}

