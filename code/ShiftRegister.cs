using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace encoder.code
{
    internal class ShiftRegister
    {
        public int Value { get; set; }

        
        public override string ToString()
        {
            //using ToString helps with charting to easily get the value
            return Value.ToString();
        }
    }


}
