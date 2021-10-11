using System;
using System.Collections.Generic;

namespace Europlan.Application
{
    public partial class Version : IComparable<Version>, IEquatable<Version>
    {
        public class VersionComparer : IComparer<Version>, IEqualityComparer<Version>
        {


            internal VersionComparer() { }


            Int32 IComparer<Version>.Compare(Version item1, Version item2)
            {
                return Compare(item1, item2);
            }

            Boolean IEqualityComparer<Version>.Equals(Version item1, Version item2)
            {
                throw new NotImplementedException();
            }

            Int32 IEqualityComparer<Version>.GetHashCode(Version item)
            {
                return item.GetHashCode();
            }


        }
    }
}
