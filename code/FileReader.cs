using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace encoder.code
{
    internal class FileReader
    {
        public static List<string>  GetAllTextFiles(string configFolder)
        {
            List<string> files = new List<string>();
            string exePath = System.AppDomain.CurrentDomain.BaseDirectory;            
            string folderPath= Path.Combine(exePath, configFolder);

            DirectoryInfo dinfo = new DirectoryInfo(folderPath);
            FileInfo[] Files = dinfo.GetFiles("*.txt");

            foreach (FileInfo file in Files)
            {
                files.Add(file.Name);
            }

            return files;
        }

        public static List<string> GetAllLinesFromTextFile(string configFolder, string fileName)
        {
            List<string> lines = new List<string>();
            string exePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string filePath = exePath + "\\" + configFolder + "\\" + fileName;

            StreamReader sr = new StreamReader(filePath);            
            string line = sr.ReadLine();                       
            while (line != null)
            {                 
                lines.Add(line);              
                line = sr.ReadLine();
            }
            //close the file
            sr.Close();

            return lines;
        }
    }
}
