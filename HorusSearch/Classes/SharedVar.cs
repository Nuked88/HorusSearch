using System;
using System.Collections.Generic;
using System.IO;


namespace WpfApp1
{
    class SharedVar
    {
        
        public static string appname = "HorusSearch";

        public static string fullpath = AppDomain.CurrentDomain.BaseDirectory;

        public static string dbname = appname+"db.sqlite";

        public static string dbnameNew = appname+"db_new.sqlite";

        public static string dbnameBk = appname+"db_bk.sqlite";

        public static string dbnamePref = appname + "db_preferences.sqlite";

        public static string settingspath = setPath(Path.Combine(globalpath(), appname));

        public static string dbpath = Path.Combine(settingspath, dbname);

        public static string prefDbPath = Path.Combine(settingspath, dbnamePref);

        public static string newDbPath= Path.Combine(settingspath, dbnameNew);

        public static string bkDbPath = Path.Combine(settingspath, dbnameBk);
   
        public static string foldericon = "pack://application:,,,/Resources/Images/folder.png";

        public static string styleA = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"a.css");// "pack://application:,,,/Resources/css/a.css";

        public static string styleD = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "d.css");
        public static string styleALocal = Path.Combine(settingspath, "a.css");

        public static string extension;





        public static string styleB = "pack://application:,,,/Resources/css/b.css";

        public static string fase = "";

        public static bool isIndexing = true;

        internal static double widthLong = 939;

        internal static double heightShort = 400;

        internal static double heightInit = 53;

        internal static double normalHeight = 648;


        /* RESULT VAR */
        static List<_ResultList> dummy = new List<_ResultList>();
     


        internal static double numResult = 0;
        internal static string typeView;
        internal static int timebk=1000*60*2;
        internal static List<string> autoList;
        internal static string autoItem;





        public static  string IsTheme { get { return SourceChord.FluentWPF.SystemTheme.WindowsTheme.ToString(); } }
        public static string  IsThemeV = IsTheme;
        internal static object maxresultQuery=40;


        //load preferences
        public static Dictionary<string, int> listScore;

        private static string globalpath()
        {
            //      
            var systemPath = System.Environment.
                         GetFolderPath(
                             Environment.SpecialFolder.CommonApplicationData
                         );
           
            return systemPath;
        }

        public static List<_ResultList> setDummy()
        {


            dummy.Add( new _ResultList() { Sizeb = 0, Title = "", FileName = "", TitleFormatted = "", Size = 0, pathf = "", type = "file" });
            return dummy;

        }
        private static string setPath(string pathString)
        {
            System.IO.Directory.CreateDirectory(pathString);
            return pathString;
        }

    }
}
