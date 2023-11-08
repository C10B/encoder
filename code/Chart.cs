using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace encoder.code
{
    internal class Chart
    {
        
        
        private List<string> chartLines = new List<string>();
        
        
        public List<string> generateAsciiChart(string inputBits, string outputBits)
        {
            const string colDivider = "|";
            const char gridDot = '■';
            int numberOfShiftRegisters = (int)(outputBits.Length / inputBits.Length);            
            int col1width = numberOfShiftRegisters + 4;
            int colXwidth = numberOfShiftRegisters;

            chartLines.Add("");//blank line
            chartLines.Add("Trellis Diagram");
          

            //row 1 is the inputs
            string rowString = "";
            rowString+="Input".PadLeft(col1width, ' ') + colDivider;
            rowString +="n/a".PadLeft(colXwidth, ' ') + colDivider;
            for (int  i = 0; i< inputBits.Length; i++)
            {
                rowString += inputBits[i].ToString().PadLeft(colXwidth, ' ') + colDivider;
            }
            chartLines.Add(rowString);


            //row 2 is the outputs
            rowString = "";
            rowString += "Output".PadLeft(col1width, ' ') + colDivider;
            rowString += "n/a".PadLeft(colXwidth, ' ') + colDivider;
            for (int i = 0; i < inputBits.Length; i++)
            {
                rowString += outputBits.Substring(i, numberOfShiftRegisters).ToString().PadLeft(colXwidth, ' ') + colDivider;
            }
            chartLines.Add(rowString);


            //loop through each possible output
            for (int row = 0; row < Math.Pow(2, numberOfShiftRegisters); row++)
            {
                //build the row
                rowString = "";
                string binaryString = Convert.ToString(row, 2).PadLeft(numberOfShiftRegisters, '0');
                string col1String = (binaryString + "(" + row + "):").PadLeft(col1width, ' ');
                rowString += col1String + colDivider;
                for (int col = -1; col < inputBits.Length; col++) {
                    string colXString = "";
                    colXString = "".PadLeft(colXwidth, gridDot);
                    rowString += colXString + colDivider;
                }
                chartLines.Add(rowString);
            }

            chartLines.Add("");//blank line

            return chartLines;
        }
    }
}
