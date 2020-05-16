using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WpfApp1;
using HorusSearch.Classes;

namespace HorusSearch.Templates
{
    /// <summary>
    /// Logica di interazione per Page1.xaml
    /// </summary>
    public partial class BrowseView : Page
    {


        public BrowseView()
        {
            InitializeComponent();
            Browser.LoadingStateChanged += BrowserLoadingStateChanged;

        }
        private void BrowserLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                Console.WriteLine("This is called 3 times, why?");
            }
            if (e.IsLoading == false)
            {
                var frame = Browser.GetMainFrame();
                string cssfile;
                switch (SharedVar.extension)
                {
                    case ".html":
                        cssfile = SharedVar.styleB;
                        break;
                    default:
                        if (SourceChord.FluentWPF.SystemTheme.WindowsTheme.ToString() == "Dark")
                        {
                            cssfile = SharedVar.styleA.Replace("\\", "/");

                        }
                        else
                        {
                            cssfile = SharedVar.styleD.Replace("\\", "/");
                        }

                       
                        break;
                }

                string code = "if(document.getElementById('pageHeader')!=null){document.getElementById('pageHeader').outerHTML =''};";
                code += "var div = document.createElement('div');";
                code += "div.id = 'pageHeader';";
                code += "var img = document.createElement('link');";
                code += "img.type = 'text/css';";
                code += "img.media  = 'all';";
                code += "img.rel = 'stylesheet';";
                code += "img.href  = '" + cssfile + "';";
                code += "div.appendChild(img);";
                code += "document.body.appendChild(div);";
                frame.ExecuteJavaScriptAsync(code);



            }
        }


    }
}
