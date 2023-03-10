using Microsoft.Web.WebView2.Core;
using System.Windows.Controls;
using System.IO;
using System.Diagnostics;
using Microsoft.Web.WebView2.Wpf;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows;

namespace VSIXSampleWeb.ToolWindows
{
    /// <summary>
    /// Interaction logic for WebBrowserWindowControl.xaml
    /// </summary>
    public partial class WebBrowserWindowControl : UserControl
    {
        //public WebBrowserWindowControl()
        //{
        //    InitializeComponent();

        //}

        private Uri _currentUri;
        private WebView2 _webView;

        public WebBrowserWindowControl()
        {
            if (!Helper.IsWebView2Available())
            {
                Content = CreateDownloadButton();
            }
            else
            {
                _webView = new WebView2
                {
                    CreationProperties = new CoreWebView2CreationProperties
                    {
                        UserDataFolder = Path.Combine(Path.GetTempPath(), @"WebView2_Data\\")
                    }
                };
                _webView.NavigationStarting += NavigationStarting_CancelNavigation;
                Content = _webView;
            }
        }
        public void NavigateToUri(Uri uri)
        {
            if (_webView == null)
                return;

            _webView.Source = uri;
            _currentUri = _webView.Source;
        }

        public void NavigateToHtml(string html)
        {
            _webView?.EnsureCoreWebView2Async()
                .ContinueWith(_ => Dispatcher.Invoke(() => _webView?.NavigateToString(html)));
        }

        private void NavigationStarting_CancelNavigation(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            if (e.Uri.StartsWith("data:")) // when using NavigateToString
                return;

            var newUri = new Uri(e.Uri);
            if (newUri != _currentUri) e.Cancel = true;
        }

        public void Dispose()
        {
            _webView?.Dispose();
            _webView = null;
        }

        private object CreateDownloadButton()
        {
            var button = new Button
            {
                Content ="WEBVIEW2_NOT_AVAILABLE install it ",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(20, 6, 20, 6)
            };
            button.Click += (sender, e) => Process.Start("https://go.microsoft.com/fwlink/p/?LinkId=2124703");

            return button;
        }
    }
}