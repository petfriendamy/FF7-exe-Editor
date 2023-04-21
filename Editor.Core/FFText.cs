using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;

namespace Editor.Core
{
    public class FFText
    {
        private ReadOnlyCollection<char> CHAR_TABLE = new char[]
        {
            ' ', '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ':', ';', '<', '=', '>', '?',
            '@', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O',
            'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '[', '\\', ']', '^', '_',
            '`', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o',
            'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '{', '?', '}', '~', '?',
            'Ä', 'Á', 'Ç', 'É', 'Ñ', 'Ö', 'Ü', 'á', 'à', 'â', 'ä', 'ã', 'å', 'ç', 'é', 'è',
            'ê', 'ë', 'í', 'ì', 'î', 'ï', 'ñ', 'ó', 'ò', 'ö', 'õ', 'ú', 'ù', 'û', 'ü'
        }.AsReadOnly();

        public const int CHAR_NAME_LENGTH = 12, SHOP_NAME_LENGTH = 20;
        private byte[] name;

        public int MaxLength { get; }

        public FFText(string name, int length)
        {
            MaxLength = length;
            this.name = new byte[length];
            SetName(name);
        }

        public FFText(byte[] bytes)
        {
            MaxLength = bytes.Length;
            name = new byte[MaxLength];
            SetName(bytes);
        }

        //set name from a string
        public void SetName(string name)
        {
            if (name.Length > MaxLength)
            {
                throw new ArgumentException("Name is too long.");
            }

            for (int i = 0; i < MaxLength; ++i)
            {
                if (i < name.Length)
                {
                    var b = CHAR_TABLE.IndexOf(name[i]);
                    if (b == -1)
                    {
                        throw new ArgumentException("Invalid character.");
                    }
                    this.name[i] = (byte)b;
                }
                else if (i == name.Length) { this.name[i] = 0xFF; }
                else { this.name[i] = 0; }
            }
        }

        //set name from a byte array
        public void SetName(byte[] bytes)
        {
            if (bytes.Length > MaxLength)
            {
                throw new ArgumentException("Byte array is too long.");
            }

            for (int i = 0; i < MaxLength; ++i)
            {
                if (i < name.Length)
                {
                    name[i] = bytes[i];
                }
                else if (i == name.Length) { name[i] = 0xFF; }
                else { name[i] = 0; }
            }
        }

        //return the name as a string
        public override string ToString()
        {
            var temp = "";
            for (int i = 0; i < MaxLength; ++i)
            {
                if (name[i] == 0xFF) { break; }
                else { temp += CHAR_TABLE[name[i]]; }
            }
            return temp.ToString();
        }

        //return a copy of the underlying byte array
        public byte[] GetByteArray()
        {
            var nameCopy = new byte[MaxLength];
            Array.Copy(name, nameCopy, MaxLength);
            return nameCopy;
        }
    }
}
