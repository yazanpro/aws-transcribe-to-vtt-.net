using System;
using System.IO;
using TranscriptToVTT;

namespace AWSTranscribeToVTT
{
    class Program
    {
        static void Main(string[] args)
        {
            //Sample of usage
            string jsonFilePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\", "samples\\aws.json"));

            if (!File.Exists(jsonFilePath))
            {
                Console.WriteLine("Oops! it looks like the following AWS transcription JSON file is either missing or inaccessible:");
                Console.WriteLine(jsonFilePath);
                Console.ReadKey();
                return;
            }

            Console.WriteLine("The following AWS transcription JSON file will be converted into a WebVTT format:");
            Console.WriteLine(jsonFilePath);
            Console.WriteLine($"{Environment.NewLine}>>>>>>>>>>>>Start the process by pressing any key<<<<<<<<<<<<");
            Console.ReadKey();
            Console.WriteLine("================================");

            /*It has been observed that a maximum of 80 characters per caption and a maximum of 5 seconds per caption produces a good result.*/
            string vttContent = TranscriptToVTTManager.GetVTTFromAWSTranscribeJson
            (
                json: File.ReadAllText(jsonFilePath),
                maxCharsPerCaption: 80,
                maxTimePerCaption: new TimeSpan(0, 0, 5),
                transcript: out string transcript
            );

            Console.WriteLine("Plain transcript:");
            Console.WriteLine(transcript);
            Console.WriteLine("================================");
            Console.WriteLine("WebVTT result:");
            Console.WriteLine(vttContent);

            Console.WriteLine($"{Environment.NewLine}================END================");
            Console.ReadKey();
        }
    }
}
