namespace Logger_Practice.ErrorHandling
{
    class CriticalErrorException : Exception
    {
        public CriticalErrorException(string message) : base(message) { }
    }
}
