using System;

namespace Europlan.Application
{
    public partial class UpdateSourceCollection
    {
        public class InvalidTypeCodeException : Exception
        {


            private const String MESSAGE_TEMPLATE = "Der in den Update-Quellen angegebene Typcode '{1}' (gefunden in Quelle {0}) kann nicht zugeordnet werden";


            public Int32 SourceCounter { get; private set; }

            public Char TypeCode { get; private set; }


            internal InvalidTypeCodeException(Int32 sourceCounter, Char typeCode)
                : base(String.Format(MESSAGE_TEMPLATE, sourceCounter, typeCode))
            {
                SourceCounter = sourceCounter;
                TypeCode = typeCode;
            }


        } 
    }
}
