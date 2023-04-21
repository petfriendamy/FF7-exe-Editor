using System;

namespace Editor.Core
{
    public class CharData
    {
        public const int CHAR_DATA_LENGTH = 132;
        private const int MATERIA_OFFSET = 64;
        private byte[] data = new byte[CHAR_DATA_LENGTH];
        private WeaponData? weapon;
        private ArmorData? armor;
        private AccessoryData? accessory;

        public byte ID
        {
            get { return data[0]; }
            set
            {
                if (value < 0 || value > 10)
                {
                    throw new ArgumentOutOfRangeException("Invalid character ID.");
                }
                data[0] = value;
            }
        }
        public byte Level
        {
            get { return data[1]; }
            set
            {
                if (value < 1 || value > 99)
                {
                    throw new ArgumentOutOfRangeException("Invalid level.");
                }
                data[1] = value;
            }
        }
        public byte Strength
        {
            get { return data[2]; }
            set { data[2] = value; }
        }
        public byte Vitality
        {
            get { return data[3]; }
            set { data[3] = value; }
        }
        public byte Magic
        {
            get { return data[4]; }
            set { data[4] = value; }
        }
        public byte Spirit
        {
            get { return data[5]; }
            set { data[5] = value; }
        }
        public byte Dexterity
        {
            get { return data[6]; }
            set { data[6] = value; }
        }
        public byte Luck
        {
            get { return data[7]; }
            set { data[7] = value; }
        }
        public byte StrBonus
        {
            get { return data[8]; }
            set { data[8] = value; }
        }
        public byte VitBonus
        {
            get { return data[9]; }
            set { data[9] = value; }
        }
        public byte MagBonus
        {
            get { return data[10]; }
            set { data[10] = value; }
        }
        public byte SprBonus
        {
            get { return data[11]; }
            set { data[11] = value; }
        }
        public byte DexBonus
        {
            get { return data[12]; }
            set { data[12] = value; }
        }
        public byte LuckBonus
        {
            get { return data[13]; }
            set { data[13] = value; }
        }
        public byte LimitLevel
        {
            get { return data[14]; }
            set { data[14] = value; }
        }
        public byte LimitBar
        {
            get { return data[15]; }
            set { data[15] = value; }
        }
        public WeaponData? Weapon
        {
            get { return weapon; }
            set
            {
                weapon = value;
                if (weapon != null)
                {
                    data[28] = weapon.HexValue;
                }
            }
        }
        public ArmorData? Armor
        {
            get { return armor; }
            set
            {
                armor = value;
                if (armor != null)
                {
                    data[29] = armor.HexValue;
                }
            }
        }
        public AccessoryData? Accessory
        {
            get { return accessory; }
            set
            {
                accessory = value;
                if (accessory == null) { data[30] = 0xFF; }
                else { data[30] = accessory.HexValue; }
            }
        }
        public ushort CurrHealth
        {
            get { return BitConverter.ToUInt16(data, 44); }
            set
            {
                if (value > MaxHealth)
                {
                    throw new ArgumentException("Current HP must be less than or equal to max HP.");
                }
                var temp = BitConverter.GetBytes(value);
                data[44] = temp[0];
                data[45] = temp[1];
            }
        }
        public ushort MaxHealth
        {
            get { return BitConverter.ToUInt16(data, 46); }
            set
            {
                if (value < CurrHealth)
                {
                    throw new ArgumentException("Max HP must be greater or equal to current HP.");
                }
                if (value > 9999)
                {
                    throw new ArgumentOutOfRangeException("Max HP must not exceed 9999.");
                }
                var temp = BitConverter.GetBytes(value);
                data[46] = temp[0];
                data[47] = temp[1];
            }
        }
        public ushort CurrMana
        {
            get { return BitConverter.ToUInt16(data, 48); }
            set
            {
                if (value > MaxMana)
                {
                    throw new ArgumentException("Current HP must be less than or equal to max HP.");
                }
                var temp = BitConverter.GetBytes(value);
                data[48] = temp[0];
                data[49] = temp[1];
            }
        }
        public ushort MaxMana
        {
            get { return BitConverter.ToUInt16(data, 50); }
            set
            {
                if (value < CurrMana)
                {
                    throw new ArgumentException("Max MP must be greater or equal to current MP.");
                }
                if (value > 9999)
                {
                    throw new ArgumentOutOfRangeException("Max MP must not exceed 9999.");
                }
                var temp = BitConverter.GetBytes(value);
                data[50] = temp[0];
                data[51] = temp[1];
            }
        }
        public EquippedMateria[] Materia { get; private set; } = new EquippedMateria[MateriaData.MATERIA_SLOTS];

        public CharData(byte[] data)
        {
            ReadData(data);
        }

        public void ReadData(byte[] data)
        {
            if (data.Length != CHAR_DATA_LENGTH)
            {
                throw new ArgumentException("Data is incorrect length.");
            }

            //copy data to self
            Array.Copy(data, this.data, CHAR_DATA_LENGTH);

            //get equipment data
            Weapon = GameData.GetWeaponByHexValue(data[28]);
            Armor = GameData.GetArmorByHexValue(data[29]);
            Accessory = GameData.GetAccessoryByHexValue(data[30]);
            for (int i = 0; i < MateriaData.MATERIA_SLOTS; ++i) //materia data
            {
                var temp = new byte[4]
                {
                    data[MATERIA_OFFSET + (i * 4)],
                    data[MATERIA_OFFSET + (i * 4) + 1],
                    data[MATERIA_OFFSET + (i * 4) + 2],
                    data[MATERIA_OFFSET + (i * 4) + 3]
                };
                Materia[i] = new EquippedMateria(temp);
            }
        }

        public byte[] GetByteArray()
        {
            var dataCopy = new byte[CHAR_DATA_LENGTH];
            Array.Copy(data, dataCopy, CHAR_DATA_LENGTH);

            //update materia data
            for (int i = 0; i < MateriaData.MATERIA_SLOTS; ++i)
            {
                var mBytes = Materia[i].GetByteArray();
                for (int j = 0; j < 4; ++j)
                {
                    dataCopy[MATERIA_OFFSET + (i * 4) + j] = mBytes[j];
                }
            }
            return dataCopy;
        }

        public bool HasDifferences(CharData other)
        {
            var temp1 = GetByteArray();
            var temp2 = other.GetByteArray();
            for (int i = 0; i < CHAR_DATA_LENGTH; ++i)
            {
                if (temp1[i] != temp2[i]) { return true; }
            }
            return false;
        }
    }
}
