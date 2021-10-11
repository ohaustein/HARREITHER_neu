using System;

namespace Europlan.Application
{
    public partial class Version : IComparable<Version>, IEquatable<Version>
    {
        public class VersionParsingException : Exception
        {


            private const String MESSAGE_TEMPLATE = "Beim Parsen der Versionsnummer <{0}> ist ein Fehler aufgetreten: {1}";


            public Exception Exception { get; private set; }

            public String Version { get; private set; }


            public VersionParsingException(String version, Exception exception)
                : base(String.Format(MESSAGE_TEMPLATE, version, exception.Message))
            {
                Exception = exception;
                Version = version;
            }


        }
    }
}
