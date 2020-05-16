
using System.Windows;
using System.Windows.Media;

namespace WpfApp1
{
    public class _ResultList
    {
        private string _name;
        string color = "#111";

        public ImageSource appicon { get; set; }

        public string TitleFormatted
        {

            get => _name;
            set
            { 
                if (SourceChord.FluentWPF.SystemTheme.WindowsTheme.ToString() == "Dark")
                {
                     color = "#fff";
                }
                _name = string.Format("<Section  xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"  xml:lang=\"en-us\"  xml:space=\"preserve\" Background=\"Transparent\"> <Paragraph  Background=\"Transparent\" Margin = \"0,0,0,0\" FontSize=\"16\" FontWeight=\"Medium\" FontFamily = \"Segoe UI\" Foreground=\"{1}\">{0}</Paragraph></Section>", value,color); 
            }
        }
        public double Sizeb { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; } //process name
        public long Size { get; set; }
        public string pathf { get; set; }
        public string type  { get; set; }
        public string group { get; set; }
        public string mLabel { get; set; }
        public double hLabel { get; set; }
        public string textLabel { get; set; }
        public Visibility vLabel { get; set; }
        



    }

}
