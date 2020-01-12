using Quiz.Core;
using System;
using System.Windows;
using System.Windows.Threading;
namespace Quiz
{
    /// <summary>
    /// Iteration logic for HelpPage.xaml
    /// </summary>
    public partial class HelpPage : BaseSideMenuPage<SideMenuViewModel>
    {
        DispatcherTimer _timer;
        TimeSpan _time;
        static HelpPage __instance = null;
        public static HelpPage Instance { get; set; }
        public HelpPage()
        {
            if(__instance == null)
            {
                InitializeComponent();
                _time = TimeSpan.FromSeconds(300);

                _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
                {
                    tbTime.Text = _time.ToString("c");
                    if (_time == TimeSpan.Zero) _timer.Stop();
                    _time = _time.Add(TimeSpan.FromSeconds(-1));
                }, Application.Current.Dispatcher);
                __instance = this;
            }
        }
        public void startTimer()
        {
            _timer.Start();
        }
    }
}
