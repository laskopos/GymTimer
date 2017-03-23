using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Diagnostics;
using System.Threading;
using System.Timers;

namespace GymTimer
{
    [Activity(Label = "Lassen gymi", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        bool _timerStarted = false;

        System.Timers.Timer _timer;

        TextView _view;

        Thread _t;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _t = new Thread(new ThreadStart(StartTimer));
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _view = FindViewById<TextView>(Resource.Id.FirstTextView);
        }

        [Java.Interop.Export("OnClick")]
        public void OnClick(View v)
        {
            if (!_timerStarted)
            {
                _t.Start();
                _timerStarted = true;
            }
            else
            {
                _t.Abort();
                _timerStarted = false;
            }
        }

        private void StartTimer()
        {
            _timer = new System.Timers.Timer(1000);

            _timer.Elapsed += Timer_Elapsed;

            _timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_timerStarted)
            {
                System.Action action = delegate { _view.SetText(e.SignalTime.Second.ToString(), TextView.BufferType.Editable); };
                _view.Post(action);
            }
        }
    }
}

