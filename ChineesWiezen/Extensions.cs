using ChineesWiezen.Data;
using System;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace ChineesWiezen
{
    public static class Extensions
    {
        public static SolidColorBrush ToBrush(this string hexvalue)
        {
            byte R = Convert.ToByte(hexvalue.Substring(1, 2), 16);
            byte G = Convert.ToByte(hexvalue.Substring(3, 2), 16);
            byte B = Convert.ToByte(hexvalue.Substring(5, 2), 16);
            return new SolidColorBrush(Color.FromArgb(Convert.ToByte(255), R, G, B));
        }

        public static SolidColorBrush ToColor(this Suit suit)
        {
            if (suit.Value <= 2)
            {
                return "#D54A0F".ToBrush();
            }
            else if (suit.Value == 5)
            {
                return new SolidColorBrush(Colors.Blue);
            }

            return new SolidColorBrush(Colors.Black);
        }
    }
}
