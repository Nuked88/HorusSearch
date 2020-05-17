using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Input;
using System.IO;
using WpfApp1.Classes;
using static HorusSearch.Classes.Program;
using System.Windows.Media.Imaging;
using HorusSearch.Classes.ico;
using CefSharp.Wpf;
using System.Windows.Media;
using HorusSearch.Classes;
using System.Windows.Threading;
using System.Threading;
using System.Collections.Concurrent;

namespace WpfApp1
{
   
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        #region init var
        int lastrn;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }


        int len;
        public static string actualPath;
        public static string filetype;
        _ResultList item;
        public static string qqq;
        public static System.Windows.Controls.ListBox resultsList2;
        public static List<_ResultList> items;
        NTFSRead nread;
        SetTimer timerupdate;
        //initialize hotkeys class
        KeyboardHook hook = new KeyboardHook();
        //tray minimize
#pragma warning disable CS0414 // Il campo 'MainWindow.notifyIcon' è assegnato, ma il suo valore non viene mai usato
        private NotifyIcon notifyIcon = null;
#pragma warning restore CS0414 // Il campo 'MainWindow.notifyIcon' è assegnato, ma il suo valore non viene mai usato
        Page b;
#pragma warning disable CS0649 // Non è possibile assegnare un valore diverso al campo 'MainWindow.exBrowser'. Il valore predefinito è null.
        ChromiumWebBrowser exBrowser;
#pragma warning restore CS0649 // Non è possibile assegnare un valore diverso al campo 'MainWindow.exBrowser'. Il valore predefinito è null.
        Task task = null;
   

        List<string> nameList;
        #endregion

        #region Init Function
        public MainWindow()
        {
        
            InitializeComponent();
            //check if launced as admin waysearch
            nread = new NTFSRead();
            nread.getPriv();
            timerupdate = new SetTimer();
            timerupdate.startTimer(30);
            mainw.Topmost = true;
            this.ShowInTaskbar = true;
            WindowState = WindowState.Normal;

            this.Left = (SystemParameters.PrimaryScreenWidth/2) - mainw.Width/2;
            this.Top = (SystemParameters.PrimaryScreenHeight / 4) ;

            //event handler
            browseContent.LoadCompleted += MainFrame_ContentRendered;
            resultsList.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(ListBoxItem_MouseDoubleClick);
            resultsList.MouseDown+= new System.Windows.Input.MouseButtonEventHandler(ListBoxItem_MouseDown);
            resultsList.KeyDown += new System.Windows.Input.KeyEventHandler(ListBoxItem_KeyDown);
            resultsList.SelectionChanged += new SelectionChangedEventHandler(itemSelected);
            resultsList.MouseLeftButtonUp += new MouseButtonEventHandler(ListBoxItem_Selected);
            SourceChord.FluentWPF.SystemTheme.ThemeChanged += this.SystemTheme_ThemeChanged;
            search.PreviewKeyDown += search_PreviewKeyDown;
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            Loaded += OnLoaded;
            StateChanged += OnStateChanged;
            // register the event that is fired after the key press.
            hook.KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);

            //DESIGN CHANGE
            changeDesign("small");
  
            // register the controls
            hook.RegisterHotKey(ModifierKeys.Alt ,Keys.Space);

        }
        #endregion

        #region Get all exception
        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Process unhandled exception
            // Prevent default unhandled exception processing
            e.Handled = true;
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = string.Format("An unhandled exception occurred: {0}", e.Exception.Message);
            Debug.Write("Exception Handled");
           // MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            // OR whatever you want like logging etc. MessageBox it's just example
            // for quick debugging etc.
            e.Handled = true;
        }
        #endregion

        #region TextBox-TextChanged-txtAuto
        private void autoComplete(int nchar)
        {
            
            nameList = SharedVar.autoList;
            string typedString = search.Text;

            List<string> autoList = new List<string>();
            autoList.Clear();
         

                foreach (string item in nameList)
                {
                    if (!string.IsNullOrEmpty(search.Text))
                    {
                        if (item.StartsWith(typedString, StringComparison.CurrentCultureIgnoreCase))
                    {
                        string torep = item.Substring(0, nchar);
                        SharedVar.autoItem = typedString.Substring(0, nchar) + item.Replace(torep, "");
                        autoList.Add(SharedVar.autoItem);

                            break;
                        }
                    }
                else
                {
                    //reset
                    autoList.Clear();

                }
                }

                if (autoList.Count > 0)
                {
                    lbSuggestion.ItemsSource = autoList;
                    lbSuggestion.Visibility = Visibility.Visible;
                }
                else if (search.Text.Equals("", StringComparison.CurrentCultureIgnoreCase))
                {
                    lbSuggestion.Visibility = Visibility.Collapsed;
                    lbSuggestion.ItemsSource = null;
                }
                else
                {
                    lbSuggestion.Visibility = Visibility.Collapsed;
                    lbSuggestion.ItemsSource = null;
                }
            
        }

        #endregion

        #region Init/Loading Function
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            
         //   notifyIcon.Visible = true;
            search.Focusable = true;
            search.Focus();

            

            //inizialize db only if not exist
            if (!File.Exists(SharedVar.dbpath))
            {nread = new NTFSRead();

                initDB();
            }
            else if (!nread.checkDB(false))
            {
                Debug.Write("Corrupted Database Found, starting rebuild.");
                nread.deleteDB();
                initDB();
               
            }
            else
            {
                onNormalStart();
                SharedVar.isIndexing = false;
                changeDesign("endLoading");

            }
      
        }

        private void initDB()
        {
            SharedVar.isIndexing = true;
            changeDesign("startLoading");
            

            var progress = new Progress<int>(progressPercent => pBar.Value = progressPercent);
            var fase = new Progress<string>(Text => search.Text = Text);

            search.IsEnabled = false;
            Task task = Task.Run(async delegate {
                nread.ntfsread(progress, fase, false);

            }).ContinueWith(t => endLoading());

        }


        //end first initialization
        private void endLoading()
        {
            //reset all var
            SharedVar.isIndexing = false;
            search.Dispatcher.Invoke(() =>
            {
                search.Text = "";
                search.IsEnabled = true;
                search.Focusable = true;
                search.Focus();

                changeDesign("endLoading");


            });


        }
        #endregion

        #region Search Logic
        /* END TRAY ICON STUFF */
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        //SEARCH FIRST INTERACTION
        private  void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
     

            if (SharedVar.isIndexing == false)
            {
                resultsList2 = resultsList;
                len = search.Text.ToCharArray().Count();


                //QUICK HACK: CHANGE COLOR BASED ON WINDOWS THEME
                if (SourceChord.FluentWPF.SystemTheme.WindowsTheme.ToString() == "Light")
                {
               
                    SolidColorBrush black = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0));
                    search.Foreground = black;
                    search.CaretBrush = black;
              
                }
                else
                {

                    SolidColorBrush white = new SolidColorBrush(Color.FromArgb(200, 255, 255, 255));
                    search.Foreground = white;
                    search.CaretBrush = white;

                }


                if (search.Text != "" && len > 2)
                {
                   
                    changeDesign("startLoading");
                    try //reset player when user type
                    {
                       
                        SharedVar.typeView = "v5";
                        browseContent.Source = new Uri("Templates/EmptyView.xaml", UriKind.Relative);
                    }
                    catch { }
                    //custom command

                    switch (search.Text)
                    {
                        case "horus:reset":
                            nread.deleteDB();
                            initDB();

                            break;
                        case "horus:restart":
                            nread.restartApp();
                            break;
                        case "horus:kill":
                            Process.GetCurrentProcess().Kill();
                            break;
                            
                        default:
                            break;

                    }

                   
                    
             
                    qqq = search.Text;
                    int rn = RandomNumber(0, 99999);
                  
                 
                Task task = Task.Run(async delegate
                   {
                       lastrn = rn;

                       getResult();
                       Console.WriteLine("Task {0} executing", rn);
                   }).ContinueWith(t => printResults(rn));

                  

                    

                }
               
                else
                {
                    lbSuggestion.Visibility=Visibility.Collapsed;
                    changeDesign("small");
                }
            }
        }


        public void printResults(int rn)
        {    
       
            if (rn == lastrn)
            {
                Console.WriteLine("start print result {0}",rn);
                resultsList.Dispatcher.Invoke(() =>
                {

                    changeDesign("endLoading");
                    try
                    {
                        if (items.Count > 0)
                        {
                            autoComplete(search.Text.ToCharArray().Count());

                            changeDesign("withResult");
                            resultsList.ItemsSource = items;
                        }
                        else
                        {
                            lbSuggestion.Items.Clear(); //clear suggestion box if no results
                            changeDesign("small");

                        }
                    }
                    catch
                    {
                        
                        changeDesign("small");

                    }

                });
            }
            else
            {
                Console.WriteLine("Not printing because is not the last rn (current{0} last {1})",rn,lastrn);
            }
        }


       



        private static  void getResult()
        {
        
                // AsyncSearch obj = new AsyncSearch();
                SearchYasc obj = new SearchYasc();

                items = obj.Search(qqq);
  

        }





        private void search_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
             try
                {
           if (e.Key == Key.Enter)
            {
                
                           
                    
                    var listBoxIteme =
                       resultsList.ItemsSource.GetEnumerator();
                    listBoxIteme.MoveNext();
                    _ResultList listBoxItem = (_ResultList)listBoxIteme.Current;


                    openProc(listBoxItem.pathf.ToString());
 //                   listBoxItem.Focus();
                 

            }
                else if (e.Key == Key.Down)
                {



                    resultsList.Focus();




                    var listBoxItem =
                        (ListBoxItem)resultsList
                                .ItemContainerGenerator
                                .ContainerFromItem(resultsList.SelectedItem);

                    listBoxItem.Focus();


                }
                else if(e.Key == Key.Tab)
            {
                search.Text = SharedVar.autoItem;
            }
                }
                catch {
                }

        }

        #endregion

        #region FrameLogic
        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {

            b = browseContent.Content as Page;
           
            MediaElement McMediaElement;
            Slider timelineSlider;
            MediaTimeline tl;
            Border playerStack;



            switch (SharedVar.typeView)
            {
                case "v1":
                    resetPlayer();
                    ChromiumWebBrowser exBrowser = b.FindName("Browser") as ChromiumWebBrowser;
                    exBrowser.Address = "about:blank";
                    exBrowser.Address = "file:///" + item.pathf;
                    break;
                case "v2":
                    resetPlayer();

                    setinfo();
                    break;
                case "v3":
                    //video
                    setinfo();
                    McMediaElement = b.FindName("McMediaElement") as MediaElement;
                     timelineSlider = b.FindName("timelineSlider") as Slider;
                    playerStack = b.FindName("playerStack") as Border;
                  
               
                    tl = new MediaTimeline(new Uri(item.pathf, UriKind.Relative));
                   // McMediaElement.Source = new Uri(item.pathf);
                    McMediaElement.Clock = tl.CreateClock(true) as MediaClock;
                    McMediaElement.MediaOpened += (o, ex) =>
                    {
                        timelineSlider.Maximum = McMediaElement.NaturalDuration.TimeSpan.Seconds;
                        McMediaElement.Clock.Controller.Begin();
                    };
                    break;
                case "v4":
                    //audio
                    setinfo();
                    McMediaElement = b.FindName("McMediaElement") as MediaElement;
                    timelineSlider = b.FindName("timelineSlider") as Slider;
                    playerStack = b.FindName("playerStack") as Border;
                    tl = new MediaTimeline(new Uri(item.pathf, UriKind.Relative));
                    // McMediaElement.Source = new Uri(item.pathf);

                    //add width for audio
                    playerStack.Height = 70;
                    McMediaElement.Height = 70;
                    McMediaElement.Clock = tl.CreateClock(true) as MediaClock;
                    McMediaElement.MediaOpened += (o, ex) =>
                    {
                        timelineSlider.Maximum = McMediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                        McMediaElement.Clock.Controller.Begin();
                    };

                    break;
                default:
                    break;
            }
       

        }

        private void setinfo()
        {
            BitmapSource iconApp;
            //GET INFO
            FileInfo currentFileInfo = new FileInfo(item.pathf);
            FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(item.pathf);
            string pathico = Path.Combine(SharedVar.settingspath, "icons", item.Title + ".png");

            if (File.Exists(pathico))
            {
                iconApp = new BitmapImage(new Uri(pathico));
            }
            else
            {
                //GET HQ ICON
                //  QuickZip.Tools.FileToIconConverter asd = new QuickZip.Tools.FileToIconConverter();
                iconApp = ExtractIco.extract(item.pathf);
            }

            //SET INFO
            var IcoApp = b.FindName("IcoApp") as Image;
            var NameApp = b.FindName("NameApp") as System.Windows.Controls.Label;
            var VersionApp = b.FindName("VersionApp") as System.Windows.Controls.Label;
            var Kind = b.FindName("Kind") as System.Windows.Controls.Label;
            var Size = b.FindName("Size") as System.Windows.Controls.Label;
            var Created = b.FindName("Created") as System.Windows.Controls.Label;
            var Modified = b.FindName("Modified") as System.Windows.Controls.Label;
            var LastOpened = b.FindName("LastOpened") as System.Windows.Controls.Label;


            IcoApp.Source = iconApp; // Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            if (myFileVersionInfo.FileDescription != null)
            {
                NameApp.Content = myFileVersionInfo.FileDescription;
            }
            else
            {
                NameApp.Content = item.Title;
            }

            VersionApp.Content = myFileVersionInfo.FileVersion;
            Kind.Content = item.textLabel;
            Size.Content = FileSizeFormatter.FormatSize(item.Size);
            Created.Content = currentFileInfo.CreationTime.ToString();
            Modified.Content = currentFileInfo.LastWriteTime.ToString();
            LastOpened.Content = currentFileInfo.LastAccessTime.ToString();
        }

        private  void resetPlayer()
        {
            try
            {
                MediaElement McMediaElement = b.FindName("McMediaElement") as MediaElement;
                MediaTimeline tl = new MediaTimeline(new Uri(item.pathf, UriKind.Relative));
                if (McMediaElement != null)
                {
                    McMediaElement.MediaOpened += (o, ex) =>
                    {
                        McMediaElement.Clock.Controller.Stop();
                    };
                }
            }
            catch { }

        }



        private void itemSelected(object sender, SelectionChangedEventArgs e)
        {
            //  Debug.Write(resultsList.SelectedIndex);
         

            item = (_ResultList)resultsList.SelectedItem;
   
            string[] stringArray = { ".pdf", ".txt", ".jpg", ".png", ".html", ".php", ".xml" };
            string[] audioArray = { ".mp3", ".ogg", ".wav" };
            string[] videoArray = { ".mp4", ".avi", ".wmv" };

            try
            {

                Debug.Write("Index: "+resultsList.SelectedIndex.ToString()+" index2:"+ item.FileName+"\n");
                
              
                SharedVar.extension = Path.GetExtension(item.Title);
                filetype = item.type;
                
                if (stringArray.Any(SharedVar.extension.Contains) || filetype == "folder")
                {

                    //if is a supported extension or folder
                    SharedVar.typeView = "v1";
                    changeDesign("withResultKnownSelected");
                    browseContent.Source = new Uri("Templates/BrowserView.xaml", UriKind.Relative);

                }
                else if (videoArray.Any(SharedVar.extension.Contains))//(Array.IndexOf(videoArray, SharedVar.extension) > 0) //
                {
                    changeDesign("withResultKnownSelected");
                    SharedVar.typeView = "v3";
                    browseContent.Source = new Uri("Templates/VPlayerView.xaml", UriKind.Relative);

                }
                else if (audioArray.Any(SharedVar.extension.Contains))//(Array.IndexOf(audioArray, SharedVar.extension) > 0) 
                {
                    changeDesign("withResultKnownSelected");
                    SharedVar.typeView = "v4";
                    browseContent.Source = new Uri("Templates/SPlayerView.xaml", UriKind.Relative);

                }
                else if(Array.IndexOf(stringArray, SharedVar.extension)==-1 )
                {
                    changeDesign("withResultUnknownSelected");

                    SharedVar.typeView = "v2";
                    browseContent.Source = new Uri("Templates/GenericView.xaml", UriKind.Relative);

                }
                else
                {
                    SharedVar.typeView = "v5";
                    changeDesign("outSelected");

                }
              
            }
            catch 
            {
                //
            }
        }
        #endregion

        #region List Logic

        private void ListBoxItem_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                openProc();
            }
        }


        private void ListBoxItem_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            openProc();
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            //todo ?
        }

        private void ListBoxItem_MouseDown(object sender, MouseButtonEventArgs e)
        {

            try
            {
                if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
                {
                    Point p = e.GetPosition(this);
                    //      System.Windows.Forms.ListBox lst = sender as System.Windows.Forms.ListBox;
                    //  resultsList.SelectedIndex = lst.IndexFromPoint((int)p.X,(int) p.Y);
                    openFolder();
                }
            }
            catch { }

        }


        private void openFolder()
        {
            _ResultList item = (_ResultList)resultsList.SelectedItem;

            try
            {

                ProcessStartInfo pInfo = new ProcessStartInfo(item.pathf.Replace(item.FileName,""));
                Process.Start(pInfo);

            }
            catch (Win32Exception ex)
            {
                if (ex.ErrorCode == -2147467259)
                //ErrorCode for No application is associated with the specified file for
                //this operation
                {

                    Process.Start("rundll32.exe", string.Format(" shell32.dll,OpenAs_RunDLL \"{0}\"", item.pathf.Replace(item.FileName, "")));

                }
            }
        }


        private void openProc(string path = null)
        {
            WindowState = WindowState.Minimized;
            //TEMP
            NTFSRead.createDBPreferences();

            
            if (path == null)
            {
                _ResultList item = (_ResultList)resultsList.SelectedItem;
                path = item.pathf.ToString();
            }
            NTFSRead.insertProcess(path);
            try
            {
               
                ProcessStartInfo pInfo = new ProcessStartInfo(path);
                Process.Start(pInfo);

            }
            catch (Win32Exception ex)
            {
                if (ex.ErrorCode == -2147467259)
                //ErrorCode for No application is associated with the specified file for
                //this operation
                {

                    Process.Start("rundll32.exe", string.Format(" shell32.dll,OpenAs_RunDLL \"{0}\"", path));

                }
            }



        }
        #endregion

        #region Design Changes

        public void onNormalStart()
        {
            //start minimized after the first fime
            WindowState = WindowState.Minimized;
        }


        protected override void OnDeactivated(EventArgs e)
        {


            base.OnDeactivated(e);
            if (SharedVar.isIndexing == false)
            {
                search.Text = "";
            }
            SharedVar.typeView = "v5";
            browseContent.Source = new Uri("Templates/EmptyView.xaml", UriKind.Relative);
            WindowState = WindowState.Minimized;
            Debug.Write("lost focus");
            /* */
        }

        private void OnStateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Topmost = false;
                this.ShowInTaskbar = false;
                resetPlayer();
                //    notifyIcon.Visible = true;
            }
            else
            {
                //   notifyIcon.Visible = true;
                this.ShowInTaskbar = true;
                this.Topmost = true;
                this.Activate();

                search.Focusable = true;
                FocusManager.SetFocusedElement(mainw, search);
                Keyboard.Focus(search);
                KeyPress.press();

            }
        }

        private void SystemTheme_ThemeChanged(object sender, EventArgs e)
        {
            //    SharedVar.IsThemeV =  SourceChord.FluentWPF.SystemTheme.WindowsTheme.ToString();

            if (SourceChord.FluentWPF.SystemTheme.WindowsTheme.ToString() != SharedVar.IsThemeV)
            {

                nread.restartApp();
            }

        }

        private void changeDesign(string type)
        {
            try { 
            switch (type)
            {
                case "small":
                    lbSuggestion.Visibility=Visibility.Collapsed;
                    mainw.Height = SharedVar.heightInit;
                    mainw.Width = SharedVar.widthLong;
                    resultsList.Width = SharedVar.widthLong;
                    search.Width = SharedVar.widthLong;
                    reswCol.Width = new GridLength(1, GridUnitType.Star);
                    prewCol.Width = new GridLength(0);
                    break;
                case "withResult":
                    mainw.Width = SharedVar.widthLong;
                    mainw.Height = SharedVar.heightShort;
                    resultsList.Width = SharedVar.widthLong;
                    reswCol.Width = new GridLength(1, GridUnitType.Star);
                    prewCol.Width = new GridLength(0);

                    break;
                case "withResultKnownSelected":
                    mainw.Width = SharedVar.widthLong;
                    mainw.Height = SharedVar.normalHeight;
                    resultsList.Width = SharedVar.widthLong;
                    reswCol.Width = new GridLength(0.4, GridUnitType.Star);
                    prewCol.Width = new GridLength(0.6, GridUnitType.Star);
                    browseContent.Visibility = Visibility.Visible;
                   /* specificViewer.Visibility = Visibility.Visible;
                    genericViewer.Visibility = Visibility.Hidden;*/
                    break;
                case "withResultUnknownSelected":
                    mainw.Width = SharedVar.widthLong;
                    mainw.Height = SharedVar.normalHeight;
                    resultsList.Width = SharedVar.widthLong;
                    reswCol.Width = new GridLength(0.4, GridUnitType.Star);
                    prewCol.Width = new GridLength(0.6, GridUnitType.Star);
                    browseContent.Visibility = Visibility.Visible;
                    /*specificViewer.Visibility = Visibility.Hidden;
                    genericViewer.Visibility = Visibility.Visible;*/
                    break;
                case "outSelected":                    
                    exBrowser.Address = "about:blank";
                    mainw.Height = SharedVar.heightShort;
                    mainw.Width = SharedVar.widthLong;
                    resultsList.Width = SharedVar.widthLong;
                    prewCol.Width = new GridLength(0);
                    browseContent.Visibility = Visibility.Hidden;
                 /*   specificViewer.Visibility = Visibility.Hidden;
                    genericViewer.Visibility = Visibility.Hidden;*/
                    break;
                case "startLoading":
                    // set size
                    pBar.Width = SharedVar.widthLong;
                    mainw.Height = SharedVar.heightInit;
                    //set progress bar
                    pBar.IsIndeterminate = true;
                    pBar.Visibility = Visibility.Visible;
                    break;
                case "endLoading":
                    pBar.IsIndeterminate = false;
                    pBar.Visibility = Visibility.Hidden;
                    break;
                default:
                    break;
            }
        }
            catch { }
            }
        #endregion

        #region Windows ShortCut Behaviour
        

        void notifyIcon_Click(object sender, EventArgs e)
        {
            WindowState = WindowState.Normal;
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            // NOP
        }


        void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            // show the keys pressed in a label.


            if (this.WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;

                search.Focusable = true;

                search.Focus();

                Keyboard.ClearFocus();
                Keyboard.Focus(search);
                search.IsEnabled = true;

            }
            else
            {
                search.Text = "";

                WindowState = WindowState.Minimized;
            }
        }
        /* TRAY ICON STUFF 


         protected override void OnInitialized(EventArgs e)
         {

             //Tray Icon Settings
             notifyIcon = new NotifyIcon();
             notifyIcon.Click += new EventHandler(notifyIcon_Click);

             notifyIcon.DoubleClick += new EventHandler(notifyIcon_DoubleClick);
             notifyIcon.Icon = IconFromFilePath("pack://application:,,,/Images/trayicon.ico");
 }


         public static Icon IconFromFilePath(string filePath)
         {
             var result = (Icon)null;

             try
             {
                 result = System.Drawing.Icon.ExtractAssociatedIcon(filePath);
             }
             catch (System.Exception)
             {
                 // swallow and return nothing. You could supply a default Icon here as well
             }

             return result;
         }*/

        #endregion
    }



}
