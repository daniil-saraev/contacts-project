using System;

namespace Desktop.Exceptions
{
    public class ReadingDataException : Exception
    {
        public ReadingDataException() : base("Error occurred while reading data") { }
    }    
}