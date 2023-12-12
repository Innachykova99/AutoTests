using Logger_Practice.Entities;

namespace Logger_practice.Entities
{
    class Program
    {
        static void Main()
        {
            string logFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFileHere", "LogFile.log");
            logprocessor processor = new logprocessor(logFileName);
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