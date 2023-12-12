using Logger_Practice.ErrorHandling;
namespace Logger_practice.ErrorHandling
{
    class Program
    {
        static void Main()
        {
            string logFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFileHere", "LogFile.log");
            
            if (File.Exists(logFileName))
            {
                LogProcessor processor = new LogProcessor(logFileName);
                processor.ProcessLogs();
                using (var sr = new StreamReader(logFileName))
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