using System;

namespace Editor.Core
{
    public class WeaponData : ItemData
    {
        private int materiaSlots;

        public Characters EquipableBy { get; }
        public int MateriaSlots
        {
            get { return materiaSlots; }
            private set
            {
                if (value < 0 || value > 8)
                {
                    throw new ArgumentOutOfRangeException(nameof(MateriaSlots));
                }
                materiaSlots = value;
            }
        }

        public WeaponData(byte hexValue, string name, Characters equipableBy, int materiaSlots)
            :base (hexValue, name)
        {
            EquipableBy = equipableBy;
            MateriaSlots = materiaSlots;
        }
    }
}
