using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;

using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Classes
{
    class ReadResults
    {
        DataRow[] result;

       

        public DataRow[] query(string text)
        {
                                 
            if (File.Exists(SharedVar.dbpath))
            {     /*  id
                            name
                            type
                            path
                            size ColType.Integer
                            data ColType.DateTime)*/

               
                using (SqliteConnection conne = new SqliteConnection("data source=" + SharedVar.dbpath))
                { 
                    conne.Open();

                    /*  
                          Assembly assembly = Assembly.Load("System.Data.SQLite");

                          conn.LoadExtension(assembly.Location, "sqlite3_fts5_init");*/
                   // string sql = string.Format("SELECT name,type,path,size,data FROM files WHERE name MATCH '\"{0}\"*' ORDER BY name", text);


                    var dict = new Dictionary<string, string>();
                    var dict2 = new Dictionary<int, Dictionary<string, string>>();
               
                    DataTable arrow = new DataTable();

                    //define column
                    arrow.Columns.Add("name");
                    arrow.Columns.Add("type");
                    arrow.Columns.Add("path");
                    arrow.Columns.Add("size");
                    arrow.Columns.Add("data");
                    arrow.Columns.Add("weight");
                    arrow.Columns.Add("score");

                    //Read the newly inserted data:
                    var selectCmd = conne.CreateCommand();
                    selectCmd.CommandText = queryBuilderMatch(text, "ORDER BY score DESC");

                    using (var reader = selectCmd.ExecuteReader())
                    {// int key = 0;
                        while (reader.Read())
                        {//replace with datarowcollection
             
                                DataRow row = arrow.NewRow();

                                row["name"] = reader.GetString(0);
                                row["type"] = reader.GetString(1);
                                row["path"] = reader.GetString(2);
                                row["size"] = reader.GetString(3);
                                row["data"] = reader.GetString(4);
                                row["score"] = reader.GetString(5);
                            //preparing for Levenshtein

                            char[] sep = { ' ', '.' };

                                string[] namesplit = reader.GetString(0).Split(sep);
                                string[] querysplit = text.Split(' ');
                                IEnumerable<string> x = new List<string>(namesplit);
                                IEnumerable<string> y = new List<string>(querysplit);

                                row["weight"] = Levenshtein.EditDistance(x, y);
                                
                                arrow.Rows.Add(row);
                            }

                            //dict2.Add(key, dict);
                            //key++;
                        }

                    /*
                    DataView dv = new DataView(arrow);
                    dv.Sort = "weight, score ASC";
                    result = dv.ToTable().Select();
                    */


                    result = arrow.Select("", "weight DESC");
                    conne.Close();
                }
            }




        
            else { Debug.Write("NO DB FOUND"); }

            return result;
        }


        





        private string queryBuilderLike(string qi, string orderby ="")
        {
           
            string[] q = qi.Split(' ');
            string fq = "";
            int i;
            if (q.Length > 1)
            {
                for(i=0;i<q.Length;i++) 
                {
                    if (i == 0) {

                        fq += "LIKE '" + q[i] + "%'";
                    } else {
                        //make like
                        fq += " AND name LIKE '%" + q[i] + "%'";
                    }
                }
            }
            else
            {
                fq = string.Format("LIKE '{0}%'", qi);
            }

           return string.Format("SELECT * FROM files WHERE  name {0} {1} LIMIT 10", fq, orderby);
        }

        private string queryBuilderMatch(string qi, string orderby = "")
        {

            string[] q = qi.Trim().Split(' ');
            string fq = "";
            int i;
            if (q.Length > 1)
            {
                for (i = 0; i < q.Length; i++)
                {
                    if (i == 0)
                    {

                        fq += "\"" + q[i] + "\"*";
                    }
                    else
                    {
                        //make like
                        fq += " AND \"" + q[i] + "\"*";
                    }
                }
            }
            else
            {
                fq = string.Format("\"{0}\"*", qi);
            }

            return string.Format("SELECT name,type,path,size,data,score FROM files WHERE name MATCH '{0}'  {1} LIMIT {2}", fq, orderby,SharedVar.maxresultQuery);
        }
    }
}
