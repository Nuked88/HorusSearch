using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApp1.Classes
{
    public class SearchYasc
    {   

        private static ReadResults readResults;
        public List<_ResultList> Search(string q)

        {
   
            var itemsz = StartSearch(q);
            try

            {

                var items = itemsz;
                return items;
            }

            catch (Exception ex)

            {

                Console.WriteLine(ex.Message);
                List<_ResultList> items = new List<_ResultList>();

                items.Add(new _ResultList() { appicon = GetGlowingImage(SharedVar.foldericon), Title = "No Result", pathf = "C:\\" });
                return items;
            }

        }
        private ImageSource GetGlowingImage(string name)
        {
            BitmapImage glowIcon = new BitmapImage();
            glowIcon.BeginInit();
            glowIcon.UriSource = new Uri(name);
            glowIcon.EndInit();

            return glowIcon;
        }


        private string formatTextWithH(string text, string query)
        {
            //sanitize
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&lt;");
            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");



            string[] qpart = query.Trim().Split(' ');
            if (qpart.Length > 0)
            {
                string[] textpart = new string[qpart.Length];

                for (int i = 0; i < qpart.Length; i++)
                {
                    text = Regex.Replace(text, "(" + qpart[i] + ")", "ʭ$1ʬ", RegexOptions.IgnoreCase);
                }


            }
            //trick for avoid to Highlight Bold Tag
            text = text.Replace("ʭ", "<Bold>");
            text = text.Replace("ʬ", "</Bold>");

            return text;
        }

        private string replaceArray(string text,string[] array)
        {

            for (int i = 0; i < array.Length; i++)
            {
                text = text.Replace(array[i], "");
            }
            return text;
        }

        public string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }




        private List<_ResultList> StartSearch(string query)

        {
            readResults = new ReadResults();



    

            DataRow[] result = readResults.query(query);

            SharedVar.numResult = result.Count();
            // clear the old list of results		

             List<_ResultList> itemNoCat = new  List<_ResultList>();
            Dictionary <int, Dictionary<string,_ResultList>> items = new Dictionary<int, Dictionary<string, _ResultList>>();


            //resultsList.ItemsSource = items;
            ImageSource icona = GetGlowingImage(SharedVar.foldericon);
            // loop through the results, adding each result to the listbox.
            int i;
            int filemax = 0;
            int foldermax = 0;
            int max = (int)SharedVar.maxresultQuery;

            Dictionary<string, string[]> typeFiles = new Dictionary<string, string[]>();
            string[] application = { ".exe",".lnk" };
            string[] documents = { ".xls", ".xlsx", ".doc", ".docx", ".txt" };
            string[] archive = { ".zip", ".tar", ".gz", ".rar", ".7z" };
            string[] developer = { ".cs", ".cpp", ".bat", ".h", ".asp",".aspx",".axd",".asx",".asmx",".ashx",".css",".cfm",".yaws",".swf",".html",".htm",".xhtml",".jhtml",".jsp",".jspx",".wss",".do",".action",".js",".pl",".php",".php4",".php3",".phtml",".py",".rb",".rhtml",".SSI",".shtml",".xml",".rss",".svg" };


            typeFiles.Add("APPLICATIONS", new string[] { ".exe", ".lnk" });
            typeFiles.Add("DOCUMENTS", new string[] { ".xls", ".xlsx", ".doc", ".docx", ".txt",".psd",".ai" });
            typeFiles.Add("MUSIC", new string[] { ".mp3", ".wav", ".ogg", ".mid", ".midi", ".aiff", ".wma" });
            typeFiles.Add("IMAGE", new string[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".webp", ".tiff", ".ico" });
            typeFiles.Add("VIDEO", new string[] { ".avi", ".mp4", ".xvid", ".flv", ".wmv", ".mkv", ".3gp", ".m4v", ".webm", ".mov", ".yuv" });
            typeFiles.Add("DEVELOPER", new string[] { ".cs", ".cpp", ".bat", ".h", ".asp", ".aspx", ".axd", ".asx", ".asmx", ".ashx", ".css", ".cfm", ".yaws", ".swf", ".html", ".htm", ".xhtml", ".jhtml", ".jsp", ".jspx", ".wss", ".do", ".action", ".js", ".pl", ".php", ".php4", ".php3", ".phtml", ".py", ".rb", ".rhtml", ".SSI", ".shtml", ".xml", ".rss", ".svg" });
            typeFiles.Add("ARCHIVE", new string[] { ".zip", ".tar", ".gz", ".rar", ".7z" });


            string[] orderCategory= new string[typeFiles.Count+2];
            int x = 0;
            foreach (string keyz  in typeFiles.Keys)
            {
                orderCategory[x] = keyz;
                x++;
            }
            /**GENERIC CATEGORIES */
            orderCategory[x] = "FOLDERS";
            x++;
            orderCategory[x] = "OTHER";

            
           // string[] orderCategory = { "APPLICATIONS", "DOCUMENTS","MUSIC","IMAGE","VIDEO", "DEVELOPER","ARCHIVE", "FOLDERS", "OTHER" };
 




            if (SharedVar.numResult > 0)
            {
                string textlabel = "OTHER";
                string mlabel = "0,0,0,0";
                double hlabel;
                Visibility vlabel;

        



                Dictionary<string, int> p = new Dictionary<string, int>();
                /*Initialize index to zero*/
                foreach (string category in orderCategory)
                {
                    p[category] = 0;
                }
                for (i = 0; i < SharedVar.numResult; i++)
                {



                    // add it to the list box
                    string filename = Convert.ToString(result[i]["name"]);
                    long size = Convert.ToInt64(result[i]["size"]);
                    //HIGHLIGHT QUERY
                    string filenameForm = formatTextWithH(filename,query);
                    string appname =filename;

                    string type = Convert.ToString(result[i]["type"]);
                    string pathapp = Convert.ToString(result[i]["path"]).Replace("''","'");



               
                    if (pathapp != "\\")
                    {

                        if (type == "File" && filemax <= max)
                        {

                            //for every category assign the right type of file
                            foreach (string keyz in typeFiles.Keys)
                            {
                                

                                if(typeFiles[keyz].Any(filename.Contains))
                                {
                                    if (keyz == "APPLICATIONS")
                                    {
                                        //if application get name app
                                        FileVersionInfo myFileName = FileVersionInfo.GetVersionInfo(pathapp);
                                        if (myFileName.FileDescription != null && myFileName.FileDescription.Trim() != "")
                                        {
                                            appname = myFileName.FileDescription;
                                       //      appname = FirstLetterToUpper(replaceArray(filename, application));
                                        }
                                        else
                                        {
                                            appname = FirstLetterToUpper(replaceArray(filename, application));
                                        }
                                        filenameForm = formatTextWithH(appname, query);
                                    }
                                   
                                    textlabel = keyz;
                                    break;
                                }
                                else
                                {
                                    textlabel = "OTHER";
                                }

                            }


                        
                       
                            if (p[textlabel] == 0)
                            {
                                mlabel = "-12,1,1,10";
                                hlabel = 27;
                                vlabel = Visibility.Visible;
                            }
                            else
                            {
                                mlabel = "0,0,0,0";
                                hlabel = 0;
                                vlabel = Visibility.Hidden;
                            }

                            p[textlabel] = p[textlabel] + 1;


                            try
                            {
                                using (Icon ico = Icon.ExtractAssociatedIcon(pathapp))
                                {/*
                                    QuickZip.Tools.FileToIconConverter asd = new QuickZip.Tools.FileToIconConverter();
                                    icona = asd.getImage(pathapp, QuickZip.Tools.FileToIconConverter.IconSize.thumbnail);// Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                                    */

                                    icona = Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                                };
                            }
                            catch (Exception)
                            {
                                Debug.Write("icon not found");
                                // swallow and return nothing. You could supply a default Icon here as well
                            }

                            icona.Freeze();
                            Dictionary<string, _ResultList> titem = new Dictionary<string, _ResultList>();
                            titem.Add(textlabel, new _ResultList() { Sizeb = SharedVar.widthLong, appicon = icona, Title = appname, FileName = filename, TitleFormatted = filenameForm, Size = size ,pathf = pathapp, type = "file", hLabel = hlabel, textLabel = textlabel, vLabel = vlabel, mLabel = mlabel });

                            items[i] = titem;
                            
                            filemax++;


                        }
                        else if (type == "Dir" && foldermax <= max)
                        {
                            //if is folder
                            textlabel = "FOLDERS";

                            if (p[textlabel] == 0)
                            {
                                mlabel = "-12,1,1,10";
                                hlabel = 27;
                                vlabel = Visibility.Visible;
                            }
                            else
                            {
                                mlabel = "0,0,0,0";
                                hlabel = 0;
                                vlabel = Visibility.Hidden;
                            }



                            icona = GetGlowingImage(SharedVar.foldericon);
                            icona.Freeze();

                            Dictionary<string, _ResultList> titem = new Dictionary<string, _ResultList>();
                            titem.Add(textlabel, new _ResultList() { Sizeb = SharedVar.widthLong, appicon = icona, Title = appname, FileName = filename, TitleFormatted = filenameForm, Size =0, pathf = pathapp, type = "folder", hLabel = hlabel, textLabel = textlabel, vLabel = vlabel, mLabel= mlabel });

                            items[i] = titem;




                            foldermax++;
                            p[textlabel] = p[textlabel] + 1;

                        }






                        if (filemax >= max && foldermax >= max)
                        {
                            Debug.Write("Finish");
                            //exit for
                            break;

                        }




                    }

                }

            }

            SharedVar.autoList = new List<string>();
            

            /*ORDER CATEGORY*/
            foreach (string category in orderCategory)
            {
                try
                {
                    for( int ix =0; ix< items.Count;ix++)
                    {
                        Dictionary<string, _ResultList> obj = items[ix];

                        foreach (string objkey  in obj.Keys)
                        {
                            if (objkey == category)
                            {

                                //match category
                                itemNoCat.Add(obj[objkey]);
                                SharedVar.autoList.Add(obj[objkey].Title);

                            }



                        }
                       



                    }
                }catch (Exception e)
                {
                    Debug.Write(e+"no category found "+category);
                }
               
            }
            
                return itemNoCat;
        }

    }
}
