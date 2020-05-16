# Horus Search
Search Bar Spotlight-like for Windows 10
![Horus Logo](/README/images/Splash.png)

# Welcome to Horus Search Readme!


Hi! I'm **Nuked** and I've decided to create this little program that looks DECENT to me and does a similar job of the Spotlight feature on OS X on Windows.


## What HORUS SEARCH does (for now):

 - Searches all the files on the **MFT** in all the NTFS partitions on your PC;
 - Creates a DB and uses it to categorize the results;
 - Implements the Levenshtein algorithm for basic search capabilities;
 - Memorizes the apps that you frequently launch and prioritizes them in the search results;
 - Updates the main DB every 30 minutes;
 - Shows the full preview of PDF, HTML, TXT, Images, Videos, Music files;
 - Detects the dark/light theme on Windows 10 and changes it accordingly.



## What HORUS SEARCH will do in the future:

 - Have a settings window;
 - Optimize the way to refresh the main DB;
 - Show the length of the music/video;
 - Show the preview of excel/doc/power-point and other types of common files;
 - Implement external plugins;
 

## HORUS SEARCH Short-Cut

 - Press **ALT+SPACE** to recall HS
 - Type **horus:reset** to clear the main DB
 - Type **horus:restart** to restart
 - Type **horus:kill** to close HS

## INSTALLATION AND FIRST START-UP

 - The installer will copy the program in the Program Files directory of your PC, sets it to start when Windows boots, and launch it;
 - HS will create a new database in C:\ProgramData\HorusSearch\ containing the list of all the files in your PC;
 - After that, you can use it normally.

## Dependencies

 - Your system needs to be 64bit;
 - For the current version, you will need at a minimum [Visual C++ Redistributable Packages for Visual Studio 2015](https://www.microsoft.com/en-us/download/details.aspx?id=48145)
 - Your system needs to have at least the .NET Framework 4.7 installed;

## How To Build
- Include Reference to System.Windows.Forms
- Include Reference to System.Imaging
- Via NuGet: CefSharp, System.Data.SQLite, Microsoft.Data.Sqlite,FluentWpf
- Include External Reference to NtfsReader.dll

## Notice

 - I **don't take ANY** responsibility on **ANY** damage that this program will do to your system;
 - This program has been tested successfully on a **RYZEN 7 2700X - 16GB RAM - NVIDIA RTX 2070 - 1 TB AVAILABLE ON 4TB TOTAL SPACE**
 - On the test system it took about 2 minutes to index 3 TB of files on 8 different partitions.
 
 ## External library used

 - [CefSharp] (https://github.com/cefsharp/)
 - [NtfsReader] (https://sourceforge.net/projects/ntfsreader/)
 


## Some screenshots

![Horus Search App](/README/images/app.jpg)

![Horus Search Image](/README/images/image.jpg)

![Horus Search Video](/README/images/video.jpg)
