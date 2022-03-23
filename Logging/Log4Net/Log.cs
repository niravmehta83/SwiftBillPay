using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Web;

namespace Logging.Log4Net
{
    public class Log
    {
        static string _file = "";
        public  Log(string file)
                    {            
           if(!File.Exists(file))
            {
                File.Create(file);

            }
           _file = file;
        }

        public void WriteLog(string error)
        {
            // This text is always added, making the file longer over time
            using (StreamWriter sw = File.AppendText(_file))
            {
                sw.WriteLine(error);
                
            }
           
        }
    }
}
