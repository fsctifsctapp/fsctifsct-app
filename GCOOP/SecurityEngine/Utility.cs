using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecurityEngine
{
    public static class Utility
    {
        public static String PassphraseBase64
        {
            get { return "1627384950"; }
        }

        public static String KeyserialAscii
        {
            get{return "135797531";}
        }

        public static String Left(string param, int length)
        {
            string result = param.Substring(0, length);
            return result;
        }

        public static String Right(string param, int length)
        {
            int temp = param.Length - length;
            string result = param.Substring(temp, length);
            return result;
        }

        public static String Mid(string param, int startIndex, int length)
        {
            string result = param.Substring(startIndex, length);
            return result;
        }

        public static String Mid(string param, int startIndex)
        {
            string result = param.Substring(startIndex);
            return result;
        }
    }
}
