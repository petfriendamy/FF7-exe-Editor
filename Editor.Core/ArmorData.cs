using System;

namespace Editor.Core
{
    public enum ArmorSpecialPropterties { None, MaleOnly, FemaleOnly }

    public class ArmorData : ItemData
    {
        private int materiaSlots;

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
        public ArmorSpecialPropterties SpecialPropterties { get; }

        public ArmorData(byte hexValue, string name, int materiaSlots,
                ArmorSpecialPropterties specialPropterties = ArmorSpecialPropterties.None) :base (hexValue, name)
        {
            MateriaSlots = materiaSlots;
            SpecialPropterties = specialPropterties;
        }
    }
}
