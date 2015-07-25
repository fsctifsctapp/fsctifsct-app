using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace SecurityEngine
{
    public class Encryption
    {
        private String is_raw, is_encrypted, is_key = "CGI", retVal, tStr;
        private int sourcePtr, keyPtr, keyLen, sourceLen, tempVal, tempKey;

        public String EncryptStrBase64(String str_message)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Utility.PassphraseBase64));

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            byte[] DataToEncrypt = UTF8.GetBytes(str_message);

            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            return Convert.ToBase64String(Results);


        }

        public String EncryptAscii(String thestr)
        {

            retVal = is_raw;
            is_raw = thestr;

            keyPtr = 0;
            keyLen = is_key.Length;
            sourceLen = is_raw.Length;
            is_encrypted = "";
            tempVal = 0;

            String s_tempVal, s_tempKey;
            for (sourcePtr = 1; sourcePtr <= sourceLen; sourcePtr++)
            {

                System.Text.Encoding ascii = System.Text.Encoding.ASCII;


                s_tempVal = Utility.Right(is_raw, sourceLen - sourcePtr + 1);
                Byte[] s_tempVal_encodedBytes = ascii.GetBytes(s_tempVal);
                s_tempVal = s_tempVal_encodedBytes.GetValue(0).ToString();
                tempVal = Int32.Parse(s_tempVal);

                s_tempKey = Utility.Mid(Utility.KeyserialAscii, keyPtr, 1);
                Byte[] s_tempKey_encodedBytes = ascii.GetBytes(s_tempKey);
                s_tempKey = s_tempKey_encodedBytes.GetValue(0).ToString();
                tempKey = Int32.Parse(s_tempKey);

                tempVal += tempKey;
                // Added this section to ensure that ASCII Values stay within 0 to 255 range
                do
                {
                    if (tempVal > 255)
                    {
                        tempVal = tempVal - 255;
                    }
                } while (tempVal > 255);

                tStr = tempVal.ToString("000");
                is_encrypted += tStr;
                keyPtr++;
                if (keyPtr > Utility.KeyserialAscii.Length) { keyPtr = 1; };

            }
            return is_encrypted;
        }
    }
}
