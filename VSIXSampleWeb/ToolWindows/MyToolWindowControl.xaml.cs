using System.Windows;
using System.Windows.Controls;
using static VSIXSampleWeb.MyToolWindow;
using System.Windows.Threading;

namespace VSIXSampleWeb
{
    public partial class MyToolWindowControl : UserControl
    {
        private bool _init = false;
        public MyToolWindowControl()
        {
            InitializeComponent();

            if (_init == false)
            {
                WebView2.Dispatcher.Invoke(() => DispatcherPriority.Loaded);
                _init = true;
            }
            Loaded += MyToolWindowControl_Loaded;
        }
        public void Navigate()
        {
            if (_init == true)
            {
                WebView2.NavigateToUri(new Uri("https://www.bing.com/?cc=fr"));
            }
        }
        private void MyToolWindowControl_Loaded(object sender, RoutedEventArgs e)
        {
            Navigate();
        }
    }
}