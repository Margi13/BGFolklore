using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Common.Common
{
    public class EncryptDecrypt
    {
        static char[] Symbols = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '@', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ', '!', '\"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/', ':', ';', '<', '=', '>', '?', '[', '\\', ']', '^', '_', '`', '{', '|', '}', '~' };
        static readonly char[,] Matrix = NTable("bgfolklore");
        private static char[,] NTable(string key)
        {
            char[,] matrix = new char[key.Length, Symbols.Length];
            for (int i = 0; i < key.Length; i++)
            {
                matrix[i, 0] = key[i];
                for (int j = 1; j < Symbols.Length; j++)
                {
                    if (matrix[i, j - 1] == 'z')
                    {
                        matrix[i, j] = '@';
                    }
                    else if (matrix[i, j - 1] == 'Z')
                    {
                        matrix[i, j] = '0';
                    }
                    else if (matrix[i, j - 1] == '9')
                    {
                        matrix[i, j] = ' ';
                    }
                    else if (matrix[i, j - 1] == '/')
                    {
                        matrix[i, j] = ':';
                    }
                    else if (matrix[i, j - 1] == '?')
                    {
                        matrix[i, j] = '[';
                    }
                    else if (matrix[i, j - 1] == '`')
                    {
                        matrix[i, j] = '{';
                    }
                    else if (matrix[i, j - 1] == '~')
                    {
                        matrix[i, j] = 'a';
                    }
                    else
                    {
                        matrix[i, j] = (char)((int)matrix[i, j - 1] + 1);
                    }
                }
            }
            return matrix;
        }

        public static string Encryption(string wordToEncrypt)
        {
            if(wordToEncrypt == null)
            {
                return null;
            }
            string result = "";
            for (int i = 0; i < wordToEncrypt.Length; i++)
            {
                for (int j = 0; j < Symbols.Length; j++)
                {
                    if (wordToEncrypt[i] == Symbols[j])
                    {
                        result += Matrix[i % 5, j];
                        break;
                    }
                }
            }
            return result;
        }
        public static string Decryption(string wordToDecrypt)
        {
            if (wordToDecrypt == null)
            {
                return null;
            }
            string result = "";
            for (int i = 0; i < wordToDecrypt.Length; i++)
            {
                for (int j = 0; j < Symbols.Length; j++)
                {
                    if (wordToDecrypt[i] == Matrix[i % 5, j])
                    {
                        result += Symbols[j];
                        break;
                    }
                }
            }
            return result;
        }
    }
}
