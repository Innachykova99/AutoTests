using System.Text;
using System.Text.RegularExpressions;

namespace Logger_Practice.ErrorHandling
{
    class LogProcessor
    {
        string filePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFileHere", "LogFile.log"));
        protected string logFileName;

        public LogProcessor(string fileName)
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

                float ratio = CalculateRatio(logContent.Length, errorLines.Count);
                DisplayRatio(ratio);
                return ratio;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found error {ex.Message}");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"Divide by zero error {ex.Message}")  ;
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

        private List<string> FindErrorLines(string[] logContent)
        {
            var errorLines = new List<string>();

            foreach (string line in logContent)
            {
                if (Regex.IsMatch(line, @"\bERROR\b", RegexOptions.IgnoreCase))
                {
                    errorLines.Add(line);
                }
                if (line.ToUpper().Contains("CRITICAL ERROR"))
                {
                    Console.WriteLine($"CRITICAL ERROR: {line}");
                }
            }

            ProcessErrors(errorLines);
            return errorLines;
        }

        private void ProcessErrors(List<string> errorLines)
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
            
        }
    }
}
