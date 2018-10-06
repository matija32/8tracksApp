using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using EightTracksPlayer.Utils.Browsing;

namespace EightTracksWpfPlayer.Views
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {

        private WebAccessProxy webAccessProxy = new WebAccessProxy();

        public AboutWindow()
        {
            InitializeComponent();
        }

        public void RequestNavigateHandler(object sender, RequestNavigateEventArgs e)
        {
            webAccessProxy.RequestNavigateHandler(sender, e);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }

    
}
