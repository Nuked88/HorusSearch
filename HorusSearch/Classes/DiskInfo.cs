using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorusSearch.Classes
{
    class DiskInfo
    {
        public static List<string> listDrive()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            var drives = new List<string>();


            foreach (DriveInfo d in allDrives)
            {
                Debug.Write("Drive {0}", d.Name);
                Debug.Write(string.Concat("  Drive type: {0}", d.DriveType));
                if (d.DriveType.ToString() == "Fixed" && d.IsReady == true && d.DriveFormat.ToString() == "NTFS")
                {

                    drives.Add(d.Name.Replace(":\\", ""));

                    Debug.Write(string.Concat("  Volume label: {0}", d.VolumeLabel));
                    Debug.Write(string.Concat("  File system: {0}", d.DriveFormat));
                    Debug.Write(string.Concat(
                        "  Available space to current user:{0, 15} bytes",
                        d.AvailableFreeSpace));

                    Debug.Write(string.Concat(
                        "  Total available space:          {0, 15} bytes",
                        d.TotalFreeSpace));

                    Debug.Write(string.Concat(
                        "  Total size of drive:            {0, 15} bytes ",
                        d.TotalSize));

                }


            }
            return drives;

        }
    }
}
