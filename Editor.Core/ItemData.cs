namespace Editor.Core
{
    public class ItemData
    {
        public byte HexValue { get; protected set; }
        public string Name { get; protected set; }

        public ItemData(byte hexValue, string name)
        {
            HexValue = hexValue;
            Name = name;
        }
    }
}
