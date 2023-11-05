using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace encoder.code
{
    internal class Calcs
    {
        //pass in any number of bits to xor together
        public static int XorBits(params int[] bits)
        {
            int result = 0;

            foreach (int bit in bits)
            {
                result ^= bit;
            }

            return result;
        }
        public static int XorBits(List<int> bits)
        {
            int result = 0;

            foreach (int bit in bits)
            {
                result ^= bit;
            }

            return result;
        }

        public static int CalcOutput(List<ShiftRegister> shiftRegisters, List<int> outputSetup)
        {
         
                //which registers do we xor?
                List<int> registerValuesToUse = new List<int>();
                for (int i = 0; i < shiftRegisters.Count - 1; i++)
                {
                    if (outputSetup[i] == 1) registerValuesToUse.Add(shiftRegisters[i].Value);
                }
                //xor the necessary register values
                int returnValue = Calcs.XorBits(registerValuesToUse);
                return returnValue;
            
        }
    }
}
