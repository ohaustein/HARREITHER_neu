using System;

namespace Europlan.Application
{
    public partial class Version : IComparable<Version>, IEquatable<Version>
    {
        public class InvalidVersionNumberException : Exception
        {


            private const String MESSAGE_TEMPLATE = "Die Versionsnummer <{0}> ist ungültig";


            public String Version { get; private set; }


            public InvalidVersionNumberException(String version)
                : base(String.Format(MESSAGE_TEMPLATE, version))
            {
                Version = version;
            }


        }
    }
}
