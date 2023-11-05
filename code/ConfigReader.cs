using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace encoder.code
{
    internal class ConfigReader
    {
        public static int GetConfigValueInt(string configFolder, string configFile, string configName, ref Boolean success)
        {
            //get all config file lines
            List<string> lines = FileReader.GetAllLinesFromTextFile(configFolder, configFile);

            //look at each line to see if it matches the parameter we are looking for
            foreach (string line in lines)
            {
                //if the righthand side (after the colon) looks like an int, then return it
                if (line.StartsWith(configName, StringComparison.OrdinalIgnoreCase))
                {                    
                    success = true;
                    return GetIntFromLine(line);
                }
            }

            if (configName == "Number of shift registers")
            {
                success = true;
                return 3;
            }

            success = false;
            return 0;
        }
        public static List<int> GetConfigValueIntList(string configFolder, string configFile, string configName, ref Boolean success)
        {
            //get all config file lines
            List<string> lines = FileReader.GetAllLinesFromTextFile(configFolder, configFile);

            //look at each line to see if it matches the parameter we are looking for
            foreach (string line in lines)
            {
                //if the righthand side (after the colon) looks like a comma list, then return it as a list
                if (line.StartsWith(configName, StringComparison.OrdinalIgnoreCase))
                {                    
                    success = true;
                    return GetIntListFromLine(line);
                }
            }

            return new List<int>();
        }
        private static int GetIntFromLine(string lineText)
        {
            if (!lineText.Contains(":"))
            {
                return 0;
            }
            string rightPart = lineText.Split(":")[1].Trim();
            int rightPartInt = 0;
            Int32.TryParse(rightPart, out rightPartInt);

            return rightPartInt;
        }
        private static List<int> GetIntListFromLine(string lineText)
        {
            if (!lineText.Contains(":"))
            {
                return new List<int>();
            }
            string rightPart = lineText.Split(":")[1].Trim();
            
            List<int> ints = new List<int>();
            
            foreach(string splitValue in rightPart.Split(","))
            {
                int intValue = 0;
                Int32.TryParse(splitValue, out intValue);
                ints.Add(intValue);
            }
            

            return ints;
        }
    }
}
