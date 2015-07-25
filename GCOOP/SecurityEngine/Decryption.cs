using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace SecurityEngine
{
    public class Decryption
    {
        private String is_raw, is_encrypted, is_key = "CGI", retVal, tStr;

        private int sourcePtr, keyPtr, keyLen, sourceLen, tempVal, tempKey;

        public String DecryptStrBase64(String str_message)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Utility.PassphraseBase64));

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            byte[] DataToDecrypt = Convert.FromBase64String(str_message);

            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            return UTF8.GetString(Results);

        }

        public String DecryptAscii(String thestr)
        {

            is_encrypted = thestr;

            keyPtr = 0;
            keyLen = is_key.Length;
            sourceLen = is_encrypted.Length;
            is_raw = "";

            String s_tempKey;
            do
            {
                System.Text.Encoding ascii = System.Text.Encoding.ASCII;

                tempVal = Int32.Parse(Utility.Left(is_encrypted, 3));

                s_tempKey = Utility.Mid(Utility.KeyserialAscii, keyPtr, 1);
                Byte[] s_tempKey_encodedBytes = ascii.GetBytes(s_tempKey);
                s_tempKey = s_tempKey_encodedBytes.GetValue(0).ToString();
                tempKey = Int32.Parse(s_tempKey);

                tempVal -= tempKey;

                do
                {
                    if (tempVal < 0)
                    {
                        tempVal = tempVal + 255;
                    }
                } while (tempVal < 0);

                tStr = Convert.ToChar(tempVal).ToString();

                is_raw += tStr;
                keyPtr++;
                if (keyPtr > Utility.KeyserialAscii.Length) { keyPtr = 1; }

                is_encrypted = Utility.Mid(is_encrypted, 3);
            } while (is_encrypted.Length > 2);

            retVal = is_raw;

            return retVal;
        }
    }
}