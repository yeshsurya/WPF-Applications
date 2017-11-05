using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrimeCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CancellationTokenSource _cts;
        public MainWindow()
        {
            InitializeComponent();
            _cts = new CancellationTokenSource();
        }
        static int CountPrimes(int from, int to, System.Threading.CancellationToken ct )
        {
            
            int total = 0;
            for (int i = from; i <= to; i++)
            {
                if (ct.IsCancellationRequested)
                    return -1;
                bool isPrime = true;
                int limit = (int)Math.Sqrt(i);
                for (int j = 2; j <= limit; j++)
                    if (i % j == 0)
                    {
                        isPrime = false;
                        break;
                    }
                if (isPrime)
                    total++;
            }
            return total;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int first = int.Parse(_from.Text), last = int.Parse(_to.Text);
                _calcButton.IsEnabled = false;
                _cancelButton.IsEnabled = true;
                var button = (Button)sender;
                button.IsEnabled = false;
                System.Threading.ThreadPool.QueueUserWorkItem(_ =>
                {
                    int total = CountPrimes(first, last, _cts.Token);
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        _result.Text = total < 0 ? "Cancelled!" : "Total Primes: " + total.ToString();
                        _cancelButton.IsEnabled = false;
                        _calcButton.IsEnabled = true;
                    }));
                });
            }
            catch (Exception ex)
            {

            }
        }

        private void _cancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }
        }
        /*In WPF, only the thread that created a DispatcherObject may access that object. 
* For example, a background thread that is spun off from the main UI thread cannot update the contents of a Button that was created on the UI thread. 
* In order for the background thread to access the Content property of the Button, 
* the background thread must delegate the work to the Dispatcher associated with the UI thread. 
* This is accomplished by using either Invoke or BeginInvoke. 
* Invoke is synchronous and BeginInvoke is asynchronous. 
* The operation is added to the event queue of the Dispatcher at the specified DispatcherPriority.*/
    }
}
