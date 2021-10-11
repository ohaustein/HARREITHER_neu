using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Europlan.Application
{
    public partial class Version : IComparable<Version>, IEquatable<Version>
    {

        private const String PARSING_REGEX_PATTERN = @"(?<Generation>[0-9]+)(\.(?<MajorVersion>[0-9]+)(\.(?<MinorVersion>[0-9]+)(\.(?<Release>[0-9]+))?)?)?(\-(?<Comment>.*))?";
        private const String PARSING_REGEX_GROUPNAME_GENERATION = "Generation";
        private const String PARSING_REGEX_GROUPNAME_MAJORVERSION = "MajorVersion";
        private const String PARSING_REGEX_GROUPNAME_MINORVERSION = "MinorVersion";
        private const String PARSING_REGEX_GROUPNAME_RELEASE = "Release";
        private const String PARSING_REGEX_GROUPNAME_COMMENT = "Comment";

        private const String SHORT_VERSION_FORMAT = "{0}.{1}.{2}.{3}";
        private const String LONG_VERSION_FORMAT = "{0:0}.{1:00}.{2:000}.{3:0000}";
        private const String LONG_VERSION_FORMAT_WITH_COMMENT = "{0:0}.{1:00}.{2:000}.{3:0000}-{4}";
        private const String USER_FRIENDLY_TAG_0 = "Develop";
        private const String USER_FRIENDLY_TAG_1 = "Alpha";
        private const String USER_FRIENDLY_TAG_2 = "Beta";
        private const String USER_FRIENDLY_TAG_3 = "Release";


        public static VersionComparer Comparer
        {
            get
            {
                if (comparer == null) comparer = new VersionComparer();
                return comparer;
            }
        }
        private static VersionComparer comparer;


        public static Boolean AreEqual(Version item1, Version item2)
        {
            if (ReferenceEquals(item1, null)) return false;
            if (ReferenceEquals(item2, null)) return false;
            if (ReferenceEquals(item1, item2)) return true;
            if (item1.Generation == item2.Generation)
            {
                if (item1.MajorVersion == item2.MajorVersion)
                {
                    if (item1.MinorVersion == item2.MinorVersion)
                    {
                        if (item1.Release.HasValue && item2.Release.HasValue)
                        {
                            if (item1.Release.Value == item2.Release.Value)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (item1.Release.HasValue == item2.Release.HasValue)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static Boolean AreEqual(Version item1, System.Version item2)
        {
            if (ReferenceEquals(item1, null)) return false;
            if (ReferenceEquals(item2, null)) return false;
            if (item1.Generation == item2.Major)
            {
                if (item1.MajorVersion == item2.Minor)
                {
                    if (item1.MinorVersion == item2.Build)
                    {
                        if (item1.Release.HasValue)
                        {
                            if (item1.Release.Value == item2.MinorRevision)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        public static Int32 Compare(Version item1, Version item2)
        {
            if (ReferenceEquals(item1, null)) throw new ArgumentNullException("item1"); // TODO nameof
            if (ReferenceEquals(item2, null)) throw new ArgumentNullException("item2"); // TODO nameof
            if (ReferenceEquals(item1, item2)) return 0;
            var comparisonResultGeneration = item1.Generation.CompareTo(item2.Generation);
            if (comparisonResultGeneration == 0)
            {
                var comparisonResultMajorVersion = item1.MajorVersion.CompareTo(item2.MajorVersion);
                if (comparisonResultMajorVersion == 0)
                {
                    var comparisonResultMinorVersion = item1.MinorVersion.CompareTo(item2.MinorVersion);
                    if (comparisonResultMinorVersion == 0 && (item1.Release.HasValue || item2.Release.HasValue))
                    {
                        if (item1.Release.HasValue == true && item2.Release.HasValue == false) return -1;
                        if (item1.Release.HasValue == false && item2.Release.HasValue == true) return 1;
                        var comparisonResultRelease = item1.Release.Value.CompareTo(item2.Release.Value);
                        return comparisonResultRelease;
                    }
                    else
                    {
                        return comparisonResultMinorVersion;
                    }
                }
                else
                {
                    return comparisonResultMajorVersion;
                }
            }
            else
            {
                return comparisonResultGeneration;
            }
        }

        public static Int32 Compare(Version item1, System.Version item2)
        {
            if (ReferenceEquals(item1, null)) throw new ArgumentNullException("item1"); // TODO nameof
            if (ReferenceEquals(item2, null)) throw new ArgumentNullException("item2"); // TODO nameof
            if (ReferenceEquals(item1, item2)) return 0;
            var comparisonResultGeneration = item1.Generation.CompareTo(item2.Major);
            if (comparisonResultGeneration == 0)
            {
                var comparisonResultMajorVersion = item1.MajorVersion.CompareTo(item2.Minor);
                if (comparisonResultMajorVersion == 0)
                {
                    var comparisonResultMinorVersion = item1.MinorVersion.CompareTo(item2.Build);
                    if (comparisonResultMinorVersion == 0 && item1.Release.HasValue)
                    {
                        var comparisonResultRelease = item1.Release.Value.CompareTo(item2.MinorRevision);
                        return comparisonResultRelease;
                    }
                    else
                    {
                        return comparisonResultMinorVersion;
                    }
                }
                else
                {
                    return comparisonResultMajorVersion;
                }
            }
            else
            {
                return comparisonResultGeneration;
            }
        }

        public static Version Parse(String text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            try
            {
                var version = null as Version;
                if (TryParse(text, out version))
                {
                    return version;
                }
                else
                {
                    throw new InvalidVersionNumberException(text);
                }
            }
            catch (InvalidVersionNumberException) { throw; }
            catch (Exception ex) { throw new VersionParsingException(text, ex); }
        }

        public static Boolean TryParse(String text, out Version version)
        {
            if (text == null) throw new ArgumentNullException("text"); // TODO nameof
            try
            {
                var match = Regex.Match(text, PARSING_REGEX_PATTERN);
                if (match.Success)
                {
                    var generation = ParseVersionNumber(match, PARSING_REGEX_GROUPNAME_GENERATION, 0);
                    var majorVersion = ParseVersionNumber(match, PARSING_REGEX_GROUPNAME_MAJORVERSION, 0);
                    var minorVersion = ParseVersionNumber(match, PARSING_REGEX_GROUPNAME_MINORVERSION, 0);
                    var release = ParseVersionNumber(match, PARSING_REGEX_GROUPNAME_RELEASE);
                    var comment = match.Groups[PARSING_REGEX_GROUPNAME_COMMENT].Value;
                    if (String.IsNullOrEmpty(comment))
                    {
                        version = new Version(generation, majorVersion, minorVersion, release);
                        return true;
                    }
                    else
                    {
                        version = new Version(generation, majorVersion, minorVersion, release, comment);
                        return true;
                    }
                }
                else
                {
                    version = null;
                    return false;
                }
            }
            catch (Exception ex) { throw new VersionParsingException(text, ex); }
        }

        private static Int32 ParseVersionNumber(Match match, String groupName, Int32 defaultValue)
        {
            try
            {
                var value = match.Groups[groupName].Value;
                var result = Int32.Parse(value);
                return result;
            }
            catch
            {
                return defaultValue;
            }
        }

        private static Int32? ParseVersionNumber(Match match, String groupName)
        {
            try
            {
                var value = match.Groups[groupName].Value;
                var result = Int32.Parse(value);
                return result;
            }
            catch
            {
                return null;
            }
        }

        public static String ToLongString(Version version)
        {
            if (ReferenceEquals(version, null)) throw new ArgumentNullException("version"); // TODO nameof
            if (version.HasComment)
            {
                var result = String.Format(LONG_VERSION_FORMAT_WITH_COMMENT, version.Generation, version.MajorVersion, version.MinorVersion, version.Release.GetValueOrDefault(0), version.Comment);
                return result;
            }
            else
            {
                var result = String.Format(LONG_VERSION_FORMAT, version.Generation, version.MajorVersion, version.MinorVersion, version.Release.GetValueOrDefault(0));
                return result;
            }
        }

        public static String ToShortString(Version version)
        {
            if (ReferenceEquals(version, null)) throw new ArgumentNullException("version"); // TODO nameof
            var result = String.Format(SHORT_VERSION_FORMAT, version.Generation, version.MajorVersion, version.MinorVersion, version.Release.GetValueOrDefault(0));
            return result;
        }

        public static String ToUserFriendlyString(Version version)
        {
            if (ReferenceEquals(version, null)) throw new ArgumentNullException("version"); // TODO nameof
            var versionStringBuilder = new StringBuilder();
            var versionSuffixList = new List<String>();

            if (version.Generation == 0)
            {
                versionStringBuilder.Append(USER_FRIENDLY_TAG_0);
            }
            else if (version.Generation == 2)
            {
                versionStringBuilder.Append("II ");
            }
            else if (version.Generation == 3)
            {
                versionStringBuilder.Append("III ");
            }
            else if (version.Generation == 4)
            {
                versionStringBuilder.Append("IV ");
            }
            else if (version.Generation == 5)
            {
                versionStringBuilder.Append("V ");
            }
            else if (version.Generation == 6)
            {
                versionStringBuilder.Append("VI ");
            }
            else if (version.Generation == 7)
            {
                versionStringBuilder.Append("VII ");
            }
            else if (version.Generation == 8)
            {
                versionStringBuilder.Append("VIII ");
            }
            else if (version.Generation == 9)
            {
                versionStringBuilder.Append("IX ");
            }
            else if (version.Generation == 10)
            {
                versionStringBuilder.Append("X ");
            }

            if (version.Generation > 0)
            {
                if (version.MajorVersion == 0 && version.MinorVersion == 0)
                {
                    versionStringBuilder.Append(USER_FRIENDLY_TAG_1);
                }
                else if (version.MajorVersion == 0 && version.MinorVersion == 1)
                {
                    versionStringBuilder.Append(USER_FRIENDLY_TAG_2);
                }
                else if (version.MajorVersion == 0 && version.MinorVersion > 1)
                {
                    versionStringBuilder.AppendFormat("{0} {1}", USER_FRIENDLY_TAG_2, version.MinorVersion);
                }
                else if (version.MajorVersion > 0 && version.MinorVersion == 0)
                {
                    versionStringBuilder.AppendFormat("{0}", version.MajorVersion);
                }
                else if (version.MajorVersion > 0 && version.MinorVersion > 0)
                {
                    versionStringBuilder.AppendFormat("{0}.{1}", version.MajorVersion, version.MinorVersion);
                }

                if (version.Release.HasValue && version.Release.Value != 0)
                {
                    versionSuffixList.Add(String.Format("{0} {1}", USER_FRIENDLY_TAG_3, version.Release));
                }
            }

            if (version.HasComment)
            {
                versionSuffixList.Add(version.Comment);
            }

            if (versionSuffixList.Count > 0)
            {
                versionStringBuilder.AppendFormat(" ({0})", String.Join("; ", versionSuffixList));
            }

            return versionStringBuilder.ToString();
        }

        public static Boolean operator ==(Version item1, Version item2)
        {
            return AreEqual(item1, item2);
        }

        public static Boolean operator ==(Version item1, System.Version item2)
        {
            return AreEqual(item1, item2);
        }

        public static Boolean operator !=(Version item1, Version item2)
        {
            return AreEqual(item1, item2) == false;
        }

        public static Boolean operator !=(Version item1, System.Version item2)
        {
            return AreEqual(item1, item2) == false;
        }

        public static Boolean operator >(Version item1, Version item2)
        {
            return Compare(item1, item2) == 1;
        }

        public static Boolean operator >(Version item1, System.Version item2)
        {
            return Compare(item1, item2) == 1;
        }

        public static Boolean operator <(Version item1, Version item2)
        {
            return Compare(item1, item2) == -1;
        }

        public static Boolean operator <(Version item1, System.Version item2)
        {
            return Compare(item1, item2) == -1;
        }


        private Int32 HashCode;


        public String Comment { get; private set; }

        public Int32 Generation { get; private set; }

        public Boolean HasComment { get { return !String.IsNullOrEmpty(Comment); } }

        public Int32 MajorVersion { get; private set; }

        public Int32 MinorVersion { get; private set; }

        public Int32? Release { get; private set; }


        public Version(Int32 generation, Int32 majorVersion, Int32 minorVersion, Int32? release, String comment = null)
        {
            HashCode = generation.GetHashCode() ^ majorVersion.GetHashCode() ^ minorVersion.GetHashCode();
            Comment = comment;
            Generation = generation;
            MajorVersion = majorVersion;
            MinorVersion = minorVersion;
            Release = release;
        }


        public Int32 CompareTo(Version other)
        {
            if (ReferenceEquals(other, null)) throw new ArgumentNullException("other"); // TODO nameof
            return Compare(this, other);
        }

        public override Boolean Equals(Object other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            else if (other is Version)
            {
                return Equals(other as Version);
            }
            else
            {
                return base.Equals(other);
            }
        }

        public Boolean Equals(Version other)
        {
            return AreEqual(this, other);
        }

        public override Int32 GetHashCode()
        {
            return HashCode;
        }

        public String ToLongString()
        {
            return ToLongString(this);
        }

        public String ToShortString()
        {
            return ToShortString(this);
        }

        public override String ToString()
        {
            return ToShortString();
        }

        public String ToUserFriendlyString()
        {
            return ToUserFriendlyString(this);
        }


    }
}
