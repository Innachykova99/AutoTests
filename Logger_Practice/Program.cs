using System.Text.RegularExpressions;

namespace Logger_practice
{
    class BaseLogProcessor
    {
        string filePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFileHere", "LogFile.log"));
        protected string logFileName;

        public BaseLogProcessor(string fileName)
        {
            logFileName = fileName;
        }

        public virtual float ProcessLogs()
        {
            try
            {
                string[] logContent = File.ReadAllLines(logFileName);
                var errorLines = FindErrorLines(logContent);
                foreach (var errorLine in errorLines)
                {
                    Console.WriteLine(errorLine);
                }
                ProcessErrors(errorLines);
                float ratio = CalculateRatio(logContent.Length, errorLines.Count);
                DisplayRatio(ratio);
                return ratio;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found error");
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Divide by zero error");
            }
            catch (CriticalErrorException ex)
            {
                Console.WriteLine($"CRITICAL ERROR: {ex.Message}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return 0;
        }

        protected virtual List<string> FindErrorLines(string[] logContent)
        {
            var errorLines = new List<string>();

            foreach (string line in logContent)
            {
                if (Regex.IsMatch(line, @"\bERROR\b", RegexOptions.IgnoreCase))
                {
                    errorLines.Add(line);
                    if (line.ToUpper().Contains("CRITICAL ERROR"))
                    {
                        Console.WriteLine($"CRITICAL ERROR: {line}"); 
                        throw new CriticalErrorException(line);
                    }
                }
            }

            return errorLines;
        }

        protected virtual void ProcessErrors(List<string> errorLines)
        {
            string errorsLogFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errors");

            if (!Directory.Exists(errorsLogFolderPath))
            {
                Directory.CreateDirectory(errorsLogFolderPath);
            }

            string errorsLogFilePath = Path.Combine(errorsLogFolderPath, "errors.log");

            foreach (string error in errorLines)
            {
                Console.WriteLine(error);
                File.AppendAllText(errorsLogFilePath, error + Environment.NewLine);
            }
        }

        protected virtual float CalculateRatio(int totalRecords, int errorRecords)
        {
            if (errorRecords == 0)
            {
                return float.PositiveInfinity;
            }
            return (float)totalRecords / errorRecords;
        }

        protected virtual void DisplayRatio(float ratio)
        {
            Console.WriteLine($"Ratio of total records to error records: {ratio}");
        }
    }

    class CriticalErrorException : Exception
    {
        public CriticalErrorException(string message) : base(message) { }
    }

    class Program
    {
        static void Main()
        {
            string logFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFileHere", "LogFile.log");
            BaseLogProcessor processor = new BaseLogProcessor(logFileName);
            processor.ProcessLogs();

            string filePath = @"C:\Users\chiko\Logs_Practice\Logger_Practice\LogFileHere\LogFile.log";
            if (File.Exists(filePath))
            {
                using (var sr = new StreamReader(filePath))
                {
                    string fileContent = sr.ReadToEnd();
                    Console.WriteLine(fileContent);
                }
            }
            else
            {
                Console.WriteLine("File not found!");
            }
        }
    }
}