using System;

namespace ContactBook.Exceptions
{
    public class WritingDataException : Exception
    {
        public WritingDataException() : base("Error occurred while saving data") { }

        public WritingDataException(string message) : base(message) { }
    }
}