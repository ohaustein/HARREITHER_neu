using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
    public class SiUtils {
        public enum Exponent {
            NONE,
            KILO,
            MEGA
        }


        public static string ValueToString(double value, int maxDecimalPlaces, int significantPlaces, string format) {
            return ValueToString(value, ExponentForValue(value), maxDecimalPlaces, significantPlaces, format);
        }

        public static string ValueToString(double value, Exponent exponent, int maxDecimalPlaces, int significantPlaces, string format) {
            return ValueToStringWithoutExponent(value, exponent, maxDecimalPlaces, significantPlaces, format) + ExponentString(exponent);
        }

        public static string ValueToString(double value, int maxDecimalPlaces, int significantPlaces) {
            return ValueToString(value, ExponentForValue(value), maxDecimalPlaces, significantPlaces);
        }

        public static string ValueToString(double value, Exponent exponent, int maxDecimalPlaces, int significantPlaces) {
            return ValueToStringWithoutExponent(value, exponent, maxDecimalPlaces, significantPlaces) + ExponentString(exponent);
        }

        public static string ValueToStringWithoutExponent(double value, int maxDecimalPlaces, int significantPlaces, string format) {
            return ValueToString(value, ExponentForValue(value), maxDecimalPlaces, significantPlaces, format);
        }

        public static string ValueToStringWithoutExponent(double value, Exponent exponent, int maxDecimalPlaces, int significantPlaces, string format) {
            return SiUtils.Round(value / ExponentFactor(exponent), maxDecimalPlaces, significantPlaces).ToString(format);
        }

        public static string ValueToStringWithoutExponent(double value, int maxDecimalPlaces, int significantPlaces) {
            return ValueToString(value, ExponentForValue(value), maxDecimalPlaces, significantPlaces);
        }

        public static string ValueToStringWithoutExponent(double value, Exponent exponent, int maxDecimalPlaces, int significantPlaces) {
            return SiUtils.Round(value / ExponentFactor(exponent), maxDecimalPlaces, significantPlaces).ToString();
        }

        private static double Round(double value, int maxDecimalPlaces, int significantPlaces) {
            int decimalPlaces = Math.Min(Math.Max(significantPlaces - 1 - (value == 0 ? 0 : (int)Math.Floor(Math.Log10(Math.Abs(value)))), 0), 2);
            return Math.Round(value, decimalPlaces);
        }

        public static Exponent ExponentForValue(double value) {
            if (Math.Abs(value) < 10000) {
                return Exponent.NONE;
            } else if (Math.Abs(value) < 10000000) {
                return Exponent.KILO;
            } else {
                return Exponent.MEGA;
            }
        }

        public static string ExponentString(double value) {
            return ExponentString(ExponentForValue(value));
        }

        public static string ExponentString(Exponent exponent) {
            switch (exponent) {
                case Exponent.NONE:
                    return "";
                case Exponent.KILO:
                    return EuroplanRes.Unit_Prefix_Kilo;
                case Exponent.MEGA:
                    return EuroplanRes.Unit_Prefix_Mega;
                default:
                    throw new Exception("Unsupported SI Prefix");

            }
        }

        public static int ExponentFactor(Exponent exponent) {
            switch (exponent) {
                case Exponent.NONE:
                    return 1;
                case Exponent.KILO:
                    return 1000;
                case Exponent.MEGA:
                    return 1000000;
                default:
                    throw new Exception("Unsupported SI Prefix");

            }
        }
    }
}
