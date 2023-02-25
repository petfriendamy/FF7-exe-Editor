using System;

namespace Editor.Core
{
    public class AccessoryData
    {
        public byte HexValue { get; }
        public string AccessoryName { get; }

        public AccessoryData(byte hexValue, string name)
        {
            HexValue = hexValue;
            AccessoryName = name;
        }
    }
}
