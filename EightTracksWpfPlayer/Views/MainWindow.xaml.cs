using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Navigation;
using EightTracksPlayer.Utils;
using EightTracksPlayer.Utils.Browsing;
using EightTracksPlayer.ViewModels;

namespace EightTracksWpfPlayer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Settings mSettings = new Settings(@"MainWindow");

        private WebAccessProxy webAccessProxy = new WebAccessProxy();

        public MainWindow()
        {
            //RxApp.DeferredScheduler = RxApp.TaskpoolScheduler;
            ViewModel = new MainWindowViewModel();
            InitializeComponent();
        }

        /// <summary>
        /// Returns the settings container.
        /// </summary>
        public Settings Settings
        {
            get { return mSettings; }
        }

        public MainWindowViewModel ViewModel { get; protected set; }

        public void RequestNavigateHandler(object sender, RequestNavigateEventArgs e)
        {
            webAccessProxy.RequestNavigateHandler(sender, e);
        }

        private void ShowAboutWindow(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Top = this.Top + 40;
            aboutWindow.Left = this.Left + 40;

            aboutWindow.Show(); 
        }

        public Visibility DebugModeOn
        {
            get
            {
                    return (ConfigurationManager.AppSettings["AudioPlayer"] != null &&
                       ConfigurationManager.AppSettings["AudioPlayer"].Equals("DirectX WMI 3.14169"))
                       ? Visibility.Visible
                       : Visibility.Hidden;
            }
        }

        public int DebugModeButtonWidth
        {
            get { return DebugModeOn == Visibility.Visible ? 100 : 0; }
        }
    }
}