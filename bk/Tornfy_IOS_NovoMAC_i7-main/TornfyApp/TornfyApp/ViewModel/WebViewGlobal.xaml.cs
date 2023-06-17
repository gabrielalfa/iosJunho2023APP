	using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class WebViewGlobal : ContentPage
    {
        public WebViewGlobal(string url, string title)
        {
            InitializeComponent();
            webView.Source = url;
            Title = title;

        }
    }
}

