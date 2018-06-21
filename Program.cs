using System;

namespace IntelliSense_Experiment
{
    class Program
    {
        static void Main(string[] args)
        {
            //The file path can be whatever you want, this is the example file:
            string pathToScan = "test.txt";

            IntelliSense intelliSense = new IntelliSense(pathToScan);
            intelliSense.scan();

            Console.ReadKey();
        }
    }
}