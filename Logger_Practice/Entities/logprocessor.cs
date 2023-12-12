using System.Text;
using System.Text.RegularExpressions;

namespace Logger_Practice.Entities
{
    class logprocessor
    {
        string filePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFileHere", "LogFile.log"));
        protected string logFileName;

        public logprocessor(string fileName)
        {
            logFileName = fileName;
        }

        public float ProcessLogs()
        {
            try
            {
                string[] logContent = File.ReadAllLines(logFileName);
                var errorLines = FindErrorLines(logContent);
                foreach (var errorLine in errorLines)
                {
                    Console.WriteLine(errorLine);
                }
                // ProcessErrorsNew(errorLines);

                float ratio = CalculateRatio(logContent.Length, errorLines.Count);
                DisplayRatio(ratio);
                return ratio;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("File not found error");

            }
            catch (DivideByZeroException ex)
            {
                //Console.WriteLine("Divide by zero error");
                throw ex;
            }
            catch (CriticalErrorException ex)
            {
                //Console.WriteLine($"CRITICAL ERROR: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return 0;
        }

        private List<string> FindErrorLines(string[] logContent)
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
                        // throw new CriticalErrorException(line); 
                    }
                }
            }

            return errorLines;
        }

        private void ProcessErrors(List<string> errorLines)
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

        private void ProcessErrorsNew(List<string> errorLines)
        {
            {
                File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errors.log"), string.Empty);
                var sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errors.log"), false, Encoding.Default);

                foreach (var errorLine in errorLines)
                {
                    sw.WriteLine(errorLine);
                }
                sw.Dispose();
            }

            private float CalculateRatio(int totalRecords, int errorRecords)
            {
                if (errorRecords == 0)
                {
                    return float.PositiveInfinity;
                }
                return (float)totalRecords / errorRecords;
            }

            private void DisplayRatio(float ratio)
            {
                Console.WriteLine($"Ratio of total records to error records: {ratio}");
            }
        }
    }
}
