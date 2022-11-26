using System;

namespace Desktop.Common.Exceptions
{
    public class ReadingDataException : Exception
    {
        public ReadingDataException() : base("Error occurred while loading data") { }

        public ReadingDataException(string message) : base(message) { }

        public ReadingDataException(Exception inner) : base("Error occurred while loading data", inner) { }
    }
}