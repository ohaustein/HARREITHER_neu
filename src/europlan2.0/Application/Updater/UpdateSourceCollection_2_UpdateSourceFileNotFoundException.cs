using System;

namespace Europlan.Application
{
    public partial class UpdateSourceCollection
    {
        public class UpdateSourceFileNotFoundException : Exception
        {


            private const String MESSAGE_TEMPLATE = "Die Datei mit den Update-Quellen kann nicht gefunden werden (erwartet unter <{0}>)";


            public String FilePath { get; private set; }


            internal UpdateSourceFileNotFoundException(String filePath)
                : base(String.Format(MESSAGE_TEMPLATE, filePath))
            {
                FilePath = filePath;
            }


        }
    }
}
