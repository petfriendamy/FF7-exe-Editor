using System;

namespace Editor.Core
{
    public enum MateriaType { Magic, Support, Independent, Command, Summon }
    public enum MateriaSpecialProperties { Normal, EnemySkill, Underwater, Master }

    public class MateriaData
    {
        public const int MATERIA_SLOTS = 16;

        public byte HexValue { get; }
        public string MateriaName { get; }
        public MateriaType MateriaType { get; }
        public int MaxAP { get; }
        public MateriaSpecialProperties SpecialPropterties { get; }

        public MateriaData(byte hexValue, string name, MateriaType type, int maxAP,
            MateriaSpecialProperties specialType = MateriaSpecialProperties.Normal)
        {
            HexValue = hexValue;
            MateriaName = name;
            MateriaType = type;
            MaxAP = maxAP;
            SpecialPropterties = specialType;
        }
    }
}
