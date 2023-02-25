using System;

namespace Editor.Core
{
    public enum Characters
    {
        Cloud, Barret, Tifa, Aeris, RedXIII, Yuffie, CaitSith, Vincent,
        Cid, YoungCloud, Sephiroth
    }

    public class WeaponData
    {
        private int materiaSlots;

        public byte HexValue { get; }
        public string WeaponName { get; }
        public int MateriaSlots
        {
            get { return materiaSlots; }
            private set
            {
                if (value < 0 || value > 8)
                {
                    throw new ArgumentOutOfRangeException("Invalid materia slot amount.");
                }
                materiaSlots = value;
            }
        }

        public WeaponData(byte hexValue, string name, int materiaSlots)
        {
            HexValue = hexValue;
            WeaponName = name;
            MateriaSlots = materiaSlots;
        }
    }
}
