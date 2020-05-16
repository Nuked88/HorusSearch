using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp1.Classes
{
    class SetTimer
    {
        NTFSRead nread;
        private static Timer _timer;

        public void startTimer(int minutes)
        {
            _timer = new Timer(Callback, null, SharedVar.timebk, Timeout.Infinite);

        }
        private void Callback(Object state)
        {
            // Long running operation
            Debug.Write("START UPDATE");
            refreshData();
            _timer.Change(SharedVar.timebk, Timeout.Infinite);
        }
        /*
        public void startTimer2(int minutes)
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(minutes);

            var timer = new System.Threading.Timer((e) =>
            {
                //      refreshData();
            }, null, startTimeSpan, periodTimeSpan);
        }
        */

        public void refreshData()
        {
            if (SharedVar.isIndexing == false)
            {
                DateTime creation = File.GetLastWriteTime(SharedVar.dbpath);
                String middle = (DateTime.Now - creation).TotalMinutes.ToString().Replace(',', '.');
                float timemax =float.Parse(middle, CultureInfo.InvariantCulture.NumberFormat);
               
                if (timemax > 30)
                {
                    nread = new NTFSRead();
                    string T;
                    int T2;

                    var progress = new Progress<int>(progressPercent => T2 = progressPercent);

                    var fase = new Progress<string>(Text => T = Text);

                    Task task = Task.Run(async delegate
                    {
                        nread.ntfsread(progress, fase, true);

                    }).ContinueWith(t => endRefresh());
                }
            }
        }
        private void endRefresh()
        {

            //checkNewdb
            if (nread.checkDB(true))
            {
                nread.replaceDB();

            }
            //rename new db as old

        }

    }
}
