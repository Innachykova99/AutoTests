namespace Logger_Practice.Entities
{
    class CriticalErrorException : Exception
    {
        public CriticalErrorException(string message) : base(message) { }
    }
}
