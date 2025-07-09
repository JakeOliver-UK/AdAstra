namespace AdAstra.Engine.Extensions
{
    internal static class IntExtensions
    {
        public static string ToRoman(this int value)
        {
            if (value < 1 || value > 3999) return string.Empty;

            string[] thousands = ["", "M", "MM", "MMM"];
            string[] hundreds = ["", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM"];
            string[] tens = ["", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC"];
            string[] units = ["", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX"];

            return thousands[value / 1000] + hundreds[(value % 1000) / 100] + tens[(value % 100) / 10] + units[value % 10];
        }

        public static string ToOrdinal(this int value)
        {
            if (value < 0) return string.Empty;

            int lastDigit = value % 10;
            int lastTwoDigits = value % 100;

            if (lastTwoDigits >= 11 && lastTwoDigits <= 13) return $"{value}th";
            
            return lastDigit switch
            {
                1 => $"{value}st",
                2 => $"{value}nd",
                3 => $"{value}rd",
                _ => $"{value}th"
            };
        }

        public static char ToLowerAlphaChar(this int value)
        {
            if (value < 0 || value > 25) return ' ';

            return (char)('a' + value);
        }

        public static char ToUpperAlphaChar(this int value)
        {
            if (value < 0 || value > 25) return ' ';

            return (char)('A' + value);
        }
    }
}
