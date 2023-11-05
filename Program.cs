using encoder.code;

//this is the folder for config files
//it should sit next to the exe
const string ConfigFolder = "ConfigFiles";

//the user should choose a config file to continue
string selectedConfigFile;
selectedConfigFile=UserPrompts.SelectConfigFile(ConfigFolder);

//create a new encoder, passing the selected config file to the encoder
Encoder encoder=new Encoder(selectedConfigFile, ConfigFolder);

//attempt to initialise the encoder
if (!encoder.Initialise())
{
    Console.WriteLine("FAILURE");
    Console.WriteLine("The encoder failed to initialise, the config file was probably invalid");
    return;
}

//attempt to encode the string
Boolean encodeSuccess=encoder.Encode();

//let the user know the outcome
if (encodeSuccess)
{
    Console.WriteLine("SUCCESS");
    Console.WriteLine(encoder.Output);
}
else
{
    Console.WriteLine("FAILURE");
    Console.WriteLine(encoder.LastErrorMessage);
}




