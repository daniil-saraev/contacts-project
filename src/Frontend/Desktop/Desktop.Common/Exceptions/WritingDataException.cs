using System;

namespace Desktop.Common.Exceptions
{
    public class WritingDataException : Exception
    {
        public WritingDataException() : base("Error occurred while saving data") { }

        public WritingDataException(string message) : base(message) { }

        public WritingDataException(Exception inner) : base("Error occurred while saving data", inner) { }
    }
}