using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Editor.Core
{
    public class CharName
    {
        public const int STRING_LENGTH = 12;
        private const int OFFSET = 0x20;
        private byte[] name = new byte[STRING_LENGTH];
        private bool isChocobo;

        public CharName(string name, bool isChocobo = false)
        {
            this.isChocobo = isChocobo;
            SetName(name);
        }

        public CharName(int[] bytes, bool isChocobo = false)
        {
            this.isChocobo = isChocobo;
            SetName(bytes);
        }

        //determine the max allowed length of the name
        private int GetLength()
        {
            return (isChocobo ? 5 : 9);
        }

        //check if a character value is valid for FF7
        private bool IsValidCharacter(int value)
        {
            if (value < 0 || value > 255) { return false; }
            if (value == 255) { return true; }

            //get underlying character
            var c = ((char)(value + OFFSET)).ToString();
            return (c == " " || c == "-" || Regex.IsMatch(c, "^([0-9]|[a-z]|[A-Z]|[,.+:;])$"));
        }

        //set name from a string
        public void SetName(string name)
        {
            if (name.Length > GetLength())
            {
                throw new ArgumentException("Name is too long.");
            }

            for (int i = 0; i < STRING_LENGTH; ++i)
            {
                if (i < name.Length)
                {
                    int b = name[i] - OFFSET;
                    if (!IsValidCharacter(b))
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
        public void SetName(int[] bytes)
        {
            if (bytes.Length > STRING_LENGTH)
            {
                throw new ArgumentException("Byte array is too long.");
            }

            for (int i = 0; i < name.Length; ++i)
            {
                if (i < name.Length)
                {
                    if (!IsValidCharacter(bytes[i]))
                    {
                        throw new ArgumentException("Invalid character.");
                    }
                    name[i] = (byte)bytes[i];
                }
                else if (i == name.Length) { name[i] = 0xFF; }
                else { name[i] = 0; }
            }
        }

        //return the name as a string
        public string GetName()
        {
            int length = GetLength();
            var temp = "";
            for (int i = 0; i < length; ++i)
            {
                if (name[i] == 0xFF) { break; }
                else { temp += (char)(name[i] + OFFSET); }
            }
            return temp.ToString();
        }

        //return a copy of the underlying byte array
        public byte[] GetByteArray()
        {
            var nameCopy = new byte[STRING_LENGTH];
            Array.Copy(name, nameCopy, STRING_LENGTH);
            return nameCopy;
        }
    }
}
