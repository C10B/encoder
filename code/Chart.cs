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
        
        
        public List<string> generateAsciiChart(string inputBits, string outputBits, string progressiveStates, string startingBits)
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
                rowString += outputBits.Substring(i* numberOfShiftRegisters, numberOfShiftRegisters).ToString().PadLeft(colXwidth, ' ') + colDivider;
            }
            chartLines.Add(rowString);


            //loop through each possible output row
            //(ABC give 8 possibilities)
            for (int row = 0; row < 8; row++)
            {
                //build the row
                rowString = "";
                string binaryString = Convert.ToString(row, 2).PadLeft(numberOfShiftRegisters, '0');
                string col1String = (binaryString + "(" + row + "):").PadLeft(col1width, ' ');
                rowString += col1String + colDivider;
                for (int col = -1; col < inputBits.Length; col++) {
                    string colXString = "";
                    if (checkBits(binaryString, progressiveStates, outputBits, numberOfShiftRegisters, startingBits,col))
                    {
                        colXString = "".PadLeft(colXwidth, gridDot);
                    }
                    else
                    {
                        colXString = "".PadLeft(colXwidth, ' ');
                    }
                    rowString += colXString + colDivider;
                }
                chartLines.Add(rowString);
            }

            chartLines.Add("");//blank line

            return chartLines;
        }

        private Boolean checkBits(string binaryString,string progressiveStates, string outputBits, int numberOfShiftRegisters, string startingBits,int col)
        {
            //binary string is the state on the left
            //progressiveStates is a string of states during the encoding (these are the blocks on the char)
            //but note we have a col of -1 which will use the default bits as nothing has happened yet
            if (col < 0)
            {
                return (binaryString == startingBits);                
            }
            else
            {
                string compareBits = progressiveStates.Substring(col * numberOfShiftRegisters, numberOfShiftRegisters);
                return (binaryString == compareBits);
            }
            

        }
    }
}
