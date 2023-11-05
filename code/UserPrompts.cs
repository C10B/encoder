using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace encoder.code
{
    internal class UserPrompts
    {
        /// <summary>
        /// Returns the full path of a selected config file
        /// </summary>
        /// <returns></returns>
        public static string SelectConfigFile(string ConfigFolder)
        {
            //repeat the request until the user enters something valid
            string? selectedConfigFile = null;
            while (selectedConfigFile == null)
            {
                selectedConfigFile = _SelectConfigFile(ConfigFolder);
                if (selectedConfigFile == null) Console.WriteLine("Sorry, that was not recognised, try again");
            }
            Console.WriteLine("You chose config file " + selectedConfigFile);
            return selectedConfigFile;
        }

        private static string? _SelectConfigFile(string ConfigFolder) {

            //show a list of all txt files in the config folder
            //and prompt the user to enter a number
            Console.WriteLine("Type a number to choose a file...");
            List<string> configFiles = FileReader.GetAllTextFiles(ConfigFolder);
            int i = 1;
            foreach (string configFile in configFiles)
            {
                Console.WriteLine("(" + i + ") " + configFile);
                i++;
            }

            //read the user input
            string selection = Console.ReadLine() + "";

            //turn the string into a number
            int selectionNumber = 0;
            Int32.TryParse(selection, out selectionNumber);

            //return the selected filename
            if(selectionNumber > 0 && selectionNumber<=configFiles.Count) { 
                return configFiles[selectionNumber-1];
            }
            else
            {
                return null;
            }
        }
    }
}
