using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using Microsoft.Win32;

namespace ImageViewer_RoutedCommands
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ImageData _image;
        static readonly RoutedUICommand _zoomNormalCommand = new RoutedUICommand("Zoom Normal", "Normal", typeof(Commands));

        private  RoutedUICommand ZoomNormalCommand
        {
            get { return _zoomNormalCommand; }
        }

        private  RoutedCommand ZoomNormal()
        {
            return this.ZoomNormalCommand;
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnOpen(object sender, ExecutedRoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.png;*.bmp;*.gif"
            };
            if (dlg.ShowDialog() == true)
            {
                _image = new ImageData(dlg.FileName);
                DataContext = _image;
            }
        }

        private void OnIsImageExist(object sender, CanExecuteRoutedEventArgs e)
        {
                e.CanExecute = _image != null;
        }

        private void OnZoomOut(object sender, ExecutedRoutedEventArgs e)
        {
            _image.Zoom /= 1.2;
        }

        private void OnZoomIn(object sender, ExecutedRoutedEventArgs e)
        {
            _image.Zoom *= 1.2;
        }

        private void OnZoomNormal(object sender, ExecutedRoutedEventArgs e)
        {
            _image.Zoom = 1.0;
        }
    }
    class ImageData : INotifyPropertyChanged
    {
        public string ImagePath { get; private set; }
        public ImageData(string path)
        {
            ImagePath = path;
        }
        double _zoom = 1.0;
        public double Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                OnPropertyChanged("Zoom");
            }
        }
        protected virtual void OnPropertyChanged(string name)
        {
            var pc = PropertyChanged;
            if (pc != null)
                pc(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class Commands
    {

        static readonly RoutedUICommand _zoomNormalCommand =
 new RoutedUICommand("Zoom Normal", "Normal",
 typeof(Commands));

        public static RoutedUICommand ZoomNormalCommand
        {
            get { return _zoomNormalCommand; }
        }
    }
}
