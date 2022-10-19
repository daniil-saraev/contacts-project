using System;

namespace Desktop.Exceptions
{
    public class WritingDataException : Exception 
    {
        public WritingDataException() : base("Error occurred while writing data") { }
    }
    

}