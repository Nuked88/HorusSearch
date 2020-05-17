using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Filesystem.Ntfs;
using System.Diagnostics;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Security.Principal;
using System.Globalization;
using HorusSearch.Classes;

namespace WpfApp1.Classes
{

    public class NTFSRead
    {
      
        #region Insert Nodes

        /// <summary>
        /// Find most fragmented files and group by fragment count.
        /// </summary>
        /// <remarks>
        /// Requires RetrieveMode.Streams and RetrieveMode.Fragments
        /// </remarks>
        static void InsertNodes(IEnumerable<INode> nodes, DriveInfo driveInfo,bool update)
        {

            string dbmainpath = SharedVar.dbpath;

            if (update)
            {
                dbmainpath = SharedVar.newDbPath;
            }




            using (SqliteConnection conn = new SqliteConnection("data source="+ dbmainpath))
            {

                conn.Open();

                using (var transaction = conn.BeginTransaction())
                using (var command = conn.CreateCommand())
                {
                    command.CommandText =
                        "INSERT INTO files (type,name,size,path,data, score) " +
                        "VALUES($type, $name, $size, $fullpath, $data, $score);";


                    var typeParameter = command.CreateParameter();
                    typeParameter.ParameterName = "$type";
                    command.Parameters.Add(typeParameter);

                    var nameParameter = command.CreateParameter();
                    nameParameter.ParameterName = "$name";
                    command.Parameters.Add(nameParameter);

                    var sizeParameter = command.CreateParameter();
                    sizeParameter.ParameterName = "$size";
                    command.Parameters.Add(sizeParameter);

                    var fullNameParameter = command.CreateParameter();
                    fullNameParameter.ParameterName = "$fullpath";
                    command.Parameters.Add(fullNameParameter);

                    var lastChangeTimeParameter = command.CreateParameter();
                    lastChangeTimeParameter.ParameterName = "$data";
                    command.Parameters.Add(lastChangeTimeParameter);

                    var scoreParameter = command.CreateParameter();
                    scoreParameter.ParameterName = "$score";
                    command.Parameters.Add(scoreParameter);
                   

                    foreach (INode node in nodes)
                    {
                        string fname = node.FullName.Replace("'", "''");
                        typeParameter.Value = (node.Attributes & Attributes.Directory) != 0 ? "Dir" : "File";
                        nameParameter.Value = node.Name.Replace("'", "''");
                        sizeParameter.Value = node.Size;
                        fullNameParameter.Value = fname;
                        lastChangeTimeParameter.Value = node.LastChangeTime.ToLocalTime();
                        scoreParameter.Value =   SharedVar.listScore.FirstOrDefault(d => d.Key.Contains(fname)).Value;
                        
                        command.ExecuteNonQuery();

                    }
                    transaction.Commit();
                }


                using (SqliteCommand cmd = new SqliteCommand())
                {


                   using (var transaction = conn.BeginTransaction())
                    {
                        

                        transaction.Commit();


                    }

                    //   Console.WriteLine("Fragmentation Report has been saved to {0}", targetFile);
                }
                conn.Close();


            }
        }

        #endregion

        #region DatabaseOperation

        public static void createDB(bool update)
        {
            string dbmainpath = SharedVar.dbpath;

            if (update)
            {
                FileInfo fin = new FileInfo(SharedVar.newDbPath);
                if (fin.Exists)
                {
                    fin.Delete();
                    Console.WriteLine("Duplicated deleted.");

                }
                dbmainpath = SharedVar.newDbPath;
            }

            
            if (!File.Exists(dbmainpath))
            {
                System.Data.SQLite.SQLiteConnection.CreateFile(dbmainpath);
           


            using (SqliteConnection conn = new SqliteConnection("data source="+ dbmainpath))
            {
                    conn.Open();
                    string sql= "CREATE VIRTUAL TABLE files USING fts5(name, type, path, size, data, score,  tokenize=\"porter\")";
                    /*  
                          Assembly assembly = Assembly.Load("System.Data.SQLite");

                          conn.LoadExtension(assembly.Location, "sqlite3_fts5_init");*/
                   
                    using (SqliteCommand cmd = new SqliteCommand(sql,conn))
                {
                                       
                            
                        cmd.ExecuteNonQuery();
                        conn.Close();
                   
                }
            }
             }
        }


        public static void createDBPreferences()
        {
            if (!File.Exists(SharedVar.prefDbPath))
            {
                System.Data.SQLite.SQLiteConnection.CreateFile(SharedVar.prefDbPath);

                using (SqliteConnection conn = new SqliteConnection("data source=" + SharedVar.prefDbPath))
                {
                    conn.Open();
                    string sql = "CREATE TABLE process (" +
                        "id INTEGER PRIMARY KEY," +
                        "processpath TEXT NOT NULL," +
                        "score INT NOT NULL); " +
                        "CREATE  INDEX indexP ON process(processpath)";
                    /*  
                          Assembly assembly = Assembly.Load("System.Data.SQLite");

                          conn.LoadExtension(assembly.Location, "sqlite3_fts5_init");*/

                    using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                    {


                        cmd.ExecuteNonQuery();
                        conn.Close();

                    }
                }
            }

        }

        public static void insertProcess(string path)
        {
            using (SqliteConnection conn = new SqliteConnection("data source=" + SharedVar.prefDbPath))
            {
                conn.Open();
                string sql = "INSERT INTO process (processpath,score) VALUES('" + path.Replace("'", "''") + "', '1') ";
                if (getIndexProcess(path) > 0)
                {
                       sql = "UPDATE process SET score = score + 1 WHERE processpath = '" + path.Replace("'", "''") +"' ";
                }
              

                using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            using (SqliteConnection conn = new SqliteConnection("data source=" + SharedVar.dbpath))
            {
                conn.Open();
                         
                string sql = "UPDATE files SET score = score + 1 WHERE path = '" + path.Replace("'", "''") + "' ";

                using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                   
                }
                conn.Close();
            }

        }

        public static int getIndexProcess(string path)
        {

            int score=0;
            using (SqliteConnection conn = new SqliteConnection("data source=" + SharedVar.prefDbPath))
            {
                conn.Open();
         
                string sql = "SELECT processpath,score from process WHERE processpath='"+path+"'";

                using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                {

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {  
                            score = reader.GetInt32(1);                                   
                        }
                    }
                    conn.Close();
                }
            }
         
            return score;
        }
        public static Dictionary<string,int> getAllIndexProcess()
        {
            Dictionary<string, int> scores = new Dictionary<string, int>();
            createDBPreferences();
            using (SqliteConnection conn = new SqliteConnection("data source=" + SharedVar.prefDbPath))
            {
                conn.Open();

                string sql = "SELECT processpath,score from process ";

                using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                {

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            scores[reader.GetString(0)] = reader.GetInt32(1);
                        }
                    }
                    conn.Close();
                }
            }

            return scores;
        }



        public void deleteDB()
        {
            //delete DB
           
                File.Delete(SharedVar.dbpath);
                Console.WriteLine("File deleted.");

            

        }
        public bool checkDB(bool update)
        {


            string dbmainpath = SharedVar.dbpath;

            if (update)
            {
                FileInfo fin = new FileInfo(SharedVar.newDbPath);
            
                dbmainpath = SharedVar.newDbPath;
            }
            //check DB
            int key = 0;
            using (SqliteConnection conne = new SqliteConnection("data source=" + dbmainpath))
            {
                conne.Open();

                string sql = "SELECT name FROM files LIMIT 3";

                //Read the newly inserted data:
                var selectCmd = conne.CreateCommand();
                selectCmd.CommandText = sql;
                
                using (var reader = selectCmd.ExecuteReader())
                {
                    
                    
                    while (reader.Read())
                    {

                        key++;
                    }

                }

                conne.Close();
            
            }
            if (key > 0) { return true; } else { return false; }

        }
        public void replaceDB()
        {
            //replace DB
            System.IO.FileInfo fi = new System.IO.FileInfo(SharedVar.dbpath);
            System.IO.FileInfo fin = new System.IO.FileInfo(SharedVar.newDbPath);
            System.IO.FileInfo fib = new System.IO.FileInfo(SharedVar.bkDbPath);
            try
            {

                if (fib.Exists)
                {
                    fib.Delete();
                    Console.WriteLine("ex bk deleted.");

                }
                if (fi.Exists)
                {
                    GC.Collect();
                    // GC.WaitForPendingFinalizers();

                    fi.MoveTo(SharedVar.bkDbPath);
                    Console.WriteLine("BK ok.");

                }

                if (fin.Exists)
                {

                    fin.MoveTo(SharedVar.dbpath);

                    Console.WriteLine("Update ok.");


                }
            }
            catch
            {

            }
//            File.Replace(SharedVar.newDbPath, SharedVar.dbpath, SharedVar.bkDbPath);
           



        }
        #endregion

        #region Process Task
        public void getPriv()
        {
            if (!IsAdministrator())
            {
                Debug.Write("WARNING: PROCESS IS RUNNING AS USER");
                var psi = new ProcessStartInfo();
                psi.FileName = SharedVar.fullpath + "\\" + SharedVar.appname;
                psi.Arguments = "";
                psi.Verb = "runas";

                var process = new Process();
                process.StartInfo = psi;
                process.Start();
                Process.GetCurrentProcess().Kill();

                //process.WaitForExit();

            }
          
        }

        public void restartApp()
        {
            try
            {

                Debug.Write("WARNING: RESTART");
                var psi = new ProcessStartInfo();
                psi.FileName = SharedVar.fullpath + "\\" + SharedVar.appname;
                psi.Arguments = "";
                psi.Verb = "runas";

                var process = new Process();
                process.StartInfo = psi;
                process.Start();
                Process.GetCurrentProcess().Kill();
            }
            catch { }
                //process.WaitForExit();

            

        }
        public static bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                      .IsInRole(WindowsBuiltInRole.Administrator);
        }

        //[PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
        #endregion

        public void ntfsread(IProgress<int> progress, IProgress<string> fase, bool update)
        {
        
                string[] drivetoscan = DiskInfo.listDrive().ToArray();
                createDB(update);

                SharedVar.listScore = getAllIndexProcess();

                int i = 1;
                foreach (string drive in drivetoscan)
                {
                    progress?.Report(i * 100 / drivetoscan.Length - 1);
                    fase?.Report("Indexing Drive " + drive + ":\\ in progress. Please wait a few seconds.");
                    Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

                    DriveInfo driveToAnalyze = new DriveInfo(drive);

                    NtfsReader ntfsReader =
                        new NtfsReader(driveToAnalyze, RetrieveMode.All);

                    IEnumerable<INode> nodes =
                         ntfsReader.GetNodes(driveToAnalyze.Name)
                        .Where(n => (n.Attributes &
                                     (Attributes.Hidden | Attributes.System |
                                      Attributes.Temporary | Attributes.Device |
                                      Attributes.Directory | Attributes.Offline |
                                      Attributes.ReparsePoint | Attributes.SparseFile)) == 0)
                        .OrderByDescending(n => n.Size);
                    ntfsReader.Dispose();

                    InsertNodes(nodes, driveToAnalyze, update);
                    i++;
                }    
        }
    }
}
