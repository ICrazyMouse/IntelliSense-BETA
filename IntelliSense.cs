using System;
using System.IO;

namespace IntelliSense_Experiment
{
    class IntelliSense
    {
        //The path variable that is used throughout the class:
        private string path;

        //Constructor, a file path needs to be passed for the IntelliSense to scan:
        public IntelliSense(string pathToScan)
        {
            this.path = pathToScan;
        }

        //Entry point for the scan:
        public void scan()
        {
            int lineNumber = 0;
            StreamReader sr = new StreamReader(path);

            while (!sr.EndOfStream)
            {
                lineNumber++;

                string line = sr.ReadLine();
                this.scanLine(line, lineNumber);
            }
        }

        //Checks if a line is the beginning of a void:
        private bool isVoid(int lineNumber)
        {
            string line = this.getLine(lineNumber);

            if (line.Contains("void") && line.Contains("{"))
            {
                return true;
            }

            if (line.Contains("void"))
            {
                string nextLine = this.getLine(lineNumber++);

                if (nextLine.Contains("{"))
                {
                    return true;
                }
            }

            return false;
        }

        //Returns the data of a line in a file:
        private string getLine(int lineNumber)
        {
            int number = 0;
            StreamReader sr = new StreamReader(path);

            while (!sr.EndOfStream)
            {
                number++;
                string line = sr.ReadLine();

                if (number == lineNumber)
                {
                    return line;
                }
            }

            return null;
        }

        //Scans line by line:
        private void scanLine(string line, int lineNumber)
        {
            if (line.Length < 1)
            {
                return;
            }

            line = line.ToLower();
            line = line.Trim();

            //Makes sure the IntelliSense doesn't flag for blank lines:
            if (string.IsNullOrEmpty(line))
            {
                return;
            }

            //Makes sure all return functions have a return statement:
            int lineCounter = 0;
            bool hasReturn = false;
            //Checks if the line is a start of a function that needs to return a value:
            if (line.Contains("(") && line.Contains(")") && !line.Contains("void"))
            {
                if (line.Contains(";") || line.Contains("if") || line.Contains("try") || line.Contains("catch") || line.Contains("while") || line.Contains("for"))
                {
                    return;
                }

                while (!this.getLine(lineNumber + lineCounter).Contains("}"))
                {
                    //The line counter increments by one each time, this way the next line is read every time the loop repeats:
                    lineCounter++;

                    //If the loop finds a return statement, there is no error that is shown and the loop is broken:
                    if (this.getLine(lineNumber + lineCounter).Contains("return"))
                    {
                        hasReturn = true;
                        break;
                    }

                    //If the loop has reached the end of the function without finding a return statement:
                    if (this.getLine(lineNumber + lineCounter).Contains("}"))
                    {
                        hasReturn = false;
                        break;
                    }
                }

                //Displays an error if there is no return statement:
                if (!hasReturn)
                {
                    Console.WriteLine("Line " + lineNumber.ToString() + ": Missing return statement");
                }
            }

            //Makes sure the IntelliSense doesn't flag the beginning of a void or function for semicolons:
            if (this.isVoid(lineNumber) || line.Contains("{") || line.Contains("}"))
            {
                return;
            }

            //Makes sure the IntelliSense doesn't flag anything else for semicolons:
            if (line.StartsWith("public") || line.StartsWith("private") || line.StartsWith("static") || line.StartsWith("//") || line.StartsWith("/*") || line.StartsWith("class") || line.StartsWith("if") || line.Contains("{") || line.Contains("}") || line.StartsWith("namespace") || line.StartsWith("while") || line.StartsWith("for") || line.StartsWith("catch") || line.StartsWith("try") || line.StartsWith("var") || line.StartsWith("#") || line.StartsWith("protected") || line.StartsWith("partial"))
            {
                return;
            }

            //If the line doesn't contain a semicolon:
            if (!line.Contains(";"))
            {
                Console.WriteLine("Line " + lineNumber.ToString() + ": Missing semicolon");
            }
        }
    }
}