using System;

namespace ContactBook.Exceptions
{
    public class ReadingDataException : Exception
    {
        public ReadingDataException() : base("Error occurred while loading data") { }

        public ReadingDataException(string message) : base(message) { }
    }
}