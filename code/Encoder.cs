using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace encoder.code
{
    public class Encoder
    {
        private Boolean _initialised = false;
        private int _shiftRegisterCount = 0;
        private List<ShiftRegister> _shiftRegisters = new List<ShiftRegister>();
        private List<int> _outputAsetup = new List<int>();
        private List<int> _outputBsetup = new List<int>();
        private List<int> _outputCsetup = new List<int>();
        private List<int> _inputValues = new List<int>();
        private List<int> _progressiveStates = new List<int>();
        private string _configFolder = "";
        private string _configFile = "";
        public string LastErrorMessage = "";

        public Encoder(string configFile, string configFolder)
        {
            _configFolder = configFolder;
            _configFile = configFile;
        }

        //input is readonly as it comes from the config file
        public string Input { get {
                return string.Join("", _inputValues); 
            }
        }

        //the calculated ABC values (readonly of course)
        public string ProgressiveStates
        {
            get
            {
                return string.Join("", _progressiveStates);
            }
        }

        public string Output { get; set; } = "";

        /// <summary>
        /// Reset everthing about the encoder
        /// </summary>
        public Boolean Initialise()
        {
            LastErrorMessage = "";
            return SetupTheMachine();
            
        }

        /// <summary>
        /// Perform the encoding of whatever is on the input and put it on the output to be read
        /// </summary>
        public Boolean Encode()
        {
            //has the encoder been initialised?
            if (!_initialised)
            {
                LastErrorMessage = "The ecoder has not been initialised";
                return false;
            }


            //load and check the machine
            if (!SetupTheMachine())
            {
                LastErrorMessage = "The machine could not be setup properly. Please check the config file";
                return false;
            }

            //do the encoding here
            return _encode();
            
        }

        private Boolean _encode()
        {
            Debug.WriteLine("");
            Debug.WriteLine("Start Encode with " + string.Join(", ", _inputValues));

            string outputString = "";

            //loop through every bit in the input
            for(int i = 0; i < _inputValues.Count;i++)
            {
                Debug.WriteLine(i + ") input bit is a " + _inputValues[i]);
                Debug.WriteLine("registers already have " + string.Join(", ", _shiftRegisters));

                //shuffle the bits along (Third takes value of Second,Second of First etc)
                for (int j= _shiftRegisterCount - 1; j>0; j--)
                {
                    _shiftRegisters[j].Value = _shiftRegisters[j-1].Value;                    
                }
                //put the current bit in the first
                _shiftRegisters[0].Value = _inputValues[i];


                for (int j = 0;j< _shiftRegisterCount; j++)
                {
                    _progressiveStates.Add(_shiftRegisters[j].Value);
                }


                Debug.WriteLine("registers after right shift " + string.Join(", ", _shiftRegisters));

                //with the bits shuffled, run the calcs!
                int valueA=0,valueB=0,valueC = 0;
                CalculateOutput(_outputAsetup, ref valueA);
                CalculateOutput(_outputBsetup, ref valueB);
                CalculateOutput(_outputCsetup, ref valueC);

                

                Debug.WriteLine("calculated ABC is " + valueA.ToString() + valueB.ToString() + valueC.ToString() );
                Debug.WriteLine("");

                outputString += (valueA);
                outputString += (valueB);
                outputString += (valueC);
            }
            
            Output= outputString;
            return true;
         
        }

        private void CalculateOutput(List<int> outputSetup, ref int returnValue)
        {
            returnValue =Calcs.CalcOutput(_shiftRegisters, outputSetup);
        }
        

        private Boolean SetupOutputA()
        {
            Boolean success = false;
            _outputAsetup = ConfigReader.GetConfigValueIntList(_configFolder, _configFile, "Output A", ref success);
            if (!success) { return false; }
            if(_outputAsetup.Count != _shiftRegisterCount) { return false; }
            return true;
        }
        private Boolean SetupOutputB()
        {
            Boolean success = false;
            _outputBsetup = ConfigReader.GetConfigValueIntList(_configFolder, _configFile, "Output B", ref success);
            if (!success) { return false; }
            if (_outputBsetup.Count != _shiftRegisterCount) { return false; }
            return true;
        }
        private Boolean SetupOutputC()
        {
            Boolean success = false;
            _outputCsetup = ConfigReader.GetConfigValueIntList(_configFolder, _configFile, "Output C", ref success);
            if (!success) { return false; }
            if (_outputCsetup.Count != _shiftRegisterCount) { return false; }
            return true;
        }

        private Boolean SetupInput()
        {
            Boolean success = false;
            _inputValues = ConfigReader.GetConfigValueIntList(_configFolder, _configFile, "Input", ref success);
            if (!success) { return false; }
            if (_inputValues.Count ==0) { return false; }
            return true;
        }

        private Boolean SetupShiftRegisters()
        {
            //read the setup file to see how many we need
            Boolean success = false;
            _shiftRegisterCount = ConfigReader.GetConfigValueInt(_configFolder,_configFile, "Number of shift registers", ref success);
            //if there was an error reading the config, return false
            if(!success) { return false; }

            //otherwise continue by creating an array as long as the number of shift registers
            _shiftRegisters = new List<ShiftRegister>();
            for(int j = 0; j < _shiftRegisterCount; j++)
            {
                _shiftRegisters.Add(new ShiftRegister());
            }
            

            List<int> _startingValues= ConfigReader.GetConfigValueIntList(_configFolder, _configFile, "Starting state", ref success);
            //if there was an error reading the config, return false
            if (!success) { return false; }

            //check that the number of default values matches the number of shiftregisters
            if(_startingValues.Count != _shiftRegisterCount) { return false; }

            //set each shiftregister value to its default value
            int i = 0;
            foreach (ShiftRegister shiftRegister in _shiftRegisters)
            {                
                shiftRegister.Value = _startingValues[i];
                i++;
            }

            return true;
        }

        private Boolean SetupTheMachine()
        {
            _initialised = false;

            //reset the last error message
            LastErrorMessage = "";
            
            //reset output            
            Output = "";
            
            //setup the shift registers. return true only if they reset properly
            if (!SetupShiftRegisters())
            {
                LastErrorMessage = "There was a problem loading the shift registers from the config file";
                return false;
            }

            //setup output A
            if (!SetupOutputA())
            {
                LastErrorMessage = "There was a problem loading the outputA from the config file";
                return false;
            }

            //setup output B
            if (!SetupOutputB())
            {
                LastErrorMessage = "There was a problem loading the outputB from the config file";
                return false;
            }

            //setup output C
            if (!SetupOutputC())
            {
                LastErrorMessage = "There was a problem loading the outputC from the config file";
                return false;
            }

            if (!SetupInput())
            {
                LastErrorMessage = "There was a problem loading the input from the config file";
                return false;
            }

            _initialised = true;
            return true;
        }

        /// <summary>
        /// check that the string on the input is valid
        /// </summary>
        /// <returns></returns>
        private Boolean CheckInput()
        {
            //check there is at least something in the input
            if(Input == null || Input.Length == 0)
            {
                return false;
            }

            //check that it only contains 1s and 0s
            foreach (char c  in Input.ToCharArray()) { 
                if(!c.Equals("0") && !c.Equals("1"))
                {
                    return false;
                }
            }

            return true;
            
        }
    }
}
