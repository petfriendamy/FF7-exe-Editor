using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Core
{
    public static class GameData
    {
        public readonly static WeaponData[] CAIT_SITH_WEAPON_LIST = new WeaponData[13]
        {
            new WeaponData(0x65, "Yellow M-Phone", 4),
            new WeaponData(0x66, "Green M-Phone", 4),
            new WeaponData(0x67, "Blue M-Phone", 5),
            new WeaponData(0x68, "Red M-Phone", 6),
            new WeaponData(0x69, "Crystal M-Phone", 6),
            new WeaponData(0x6A, "White M-Phone", 3),
            new WeaponData(0x6B, "Black M-Phone", 4),
            new WeaponData(0x6C, "Silver M-Phone", 8),
            new WeaponData(0x6D, "Trumpet Shell", 0),
            new WeaponData(0x6E, "Gold M-Phone", 8),
            new WeaponData(0x6F, "Battle Trumpet", 6),
            new WeaponData(0x70, "Starlight Phone", 8),
            new WeaponData(0x71, "HP Shout", 8)
        };

        public readonly static WeaponData[] VINCENT_WEAPON_LIST = new WeaponData[13]
        {
            new WeaponData(0x72, "Quicksilver", 4),
            new WeaponData(0x73, "Shotgun", 4),
            new WeaponData(0x74, "Shortbarrel", 5),
            new WeaponData(0x75, "Lariat", 6),
            new WeaponData(0x76, "Winchester", 6),
            new WeaponData(0x77, "Peacemaker", 3),
            new WeaponData(0x78, "Buntline", 4),
            new WeaponData(0x79, "Long Barrel R", 8),
            new WeaponData(0x7A, "Silver Rifle", 0),
            new WeaponData(0x7B, "Sniper CR", 4),
            new WeaponData(0x7C, "Supershot", 6),
            new WeaponData(0x7D, "Outsider", 8),
            new WeaponData(0x7E, "Death Penalty", 8)
        };

        public readonly static ArmorData[] ARMOR_LIST = new ArmorData[32]
        {
            new ArmorData(0x00, "Bronze Bangle", 0),
            new ArmorData(0x01, "Iron Bangle", 1),
            new ArmorData(0x02, "Titan Bangle", 2),
            new ArmorData(0x03, "Mythril Armlet", 2),
            new ArmorData(0x04, "Carbon Bangle", 3),
            new ArmorData(0x05, "Silver Armlet", 4),
            new ArmorData(0x06, "Gold Armlet", 4),
            new ArmorData(0x07, "Diamond Bangle", 5),
            new ArmorData(0x08, "Crystal Bangle", 6),
            new ArmorData(0x09, "Platinum Bangle", 2),
            new ArmorData(0x0A, "Rune Armlet", 4),
            new ArmorData(0x0B, "Edincoat", 7),
            new ArmorData(0x0C, "Wizard Bracelet", 8),
            new ArmorData(0x0D, "Adaman Bangle", 2),
            new ArmorData(0x0E, "Gigas Armlet", 5),
            new ArmorData(0x0F, "Imperial Guard", 6),
            new ArmorData(0x10, "Aegis Armlet", 4),
            new ArmorData(0x11, "Fourth Bracelet", 5),
            new ArmorData(0x12, "Warrior Bangle", 4),
            new ArmorData(0x13, "Shinra Beta", 4),
            new ArmorData(0x14, "Shinra Alpha", 6),
            new ArmorData(0x15, "Four Slots", 4),
            new ArmorData(0x16, "Fire Armlet", 4),
            new ArmorData(0x17, "Aurora Armlet", 4),
            new ArmorData(0x18, "Bolt Armlet", 4),
            new ArmorData(0x19, "Dragon Armlet", 6),
            new ArmorData(0x1A, "Minerva Band", 6, ArmorSpecialPropterties.FemaleOnly),
            new ArmorData(0x1B, "Escort Guard", 6, ArmorSpecialPropterties.MaleOnly),
            new ArmorData(0x1C, "Mystile", 6),
            new ArmorData(0x1D, "Ziedrich", 0),
            new ArmorData(0x1E, "Precious Watch", 8),
            new ArmorData(0x1F, "Chocobracelet", 4)
        };

        public readonly static AccessoryData[] ACCESSORY_LIST = new AccessoryData[31]
        {
            new AccessoryData(0x21, "Protect Vest"),
            new AccessoryData(0x22, "Earring"),
            new AccessoryData(0x23, "Talisman"),
            new AccessoryData(0x24, "Choco Feather"),
            new AccessoryData(0x25, "Amulet"),
            new AccessoryData(0x26, "Champion Belt"),
            new AccessoryData(0x27, "Poison Ring"),
            new AccessoryData(0x28, "Tough Ring"),
            new AccessoryData(0x29, "Circlet"),
            new AccessoryData(0x2A, "Star Pendant"),
            new AccessoryData(0x2B, "Silver Glasses"),
            new AccessoryData(0x2C, "Headband"),
            new AccessoryData(0x2D, "Fairy Ring"),
            new AccessoryData(0x2E, "Jem Ring"),
            new AccessoryData(0x2F, "White Cape"),
            new AccessoryData(0x30, "Sprint Shoes"),
            new AccessoryData(0x31, "Peace Ring"),
            new AccessoryData(0x32, "Ribbon"),
            new AccessoryData(0x33, "Fire Ring"),
            new AccessoryData(0x34, "Ice Ring"),
            new AccessoryData(0x35, "Bolt Ring"),
            new AccessoryData(0x36, "Tetra Elemental"),
            new AccessoryData(0x37, "Safety Bit"),
            new AccessoryData(0x38, "Fury Ring"),
            new AccessoryData(0x39, "Curse Ring"),
            new AccessoryData(0x3A, "Protect Ring"),
            new AccessoryData(0x3B, "Cat's Bell"),
            new AccessoryData(0x3C, "Reflect Ring"),
            new AccessoryData(0x3D, "Water Ring"),
            new AccessoryData(0x3E, "Sneak Glove"),
            new AccessoryData(0x3F, "HypnoCrown")
        };

        public readonly static MateriaData[] MATERIA_LIST = new MateriaData[83]
        {
            new MateriaData(0x00, "MP Plus", MateriaType.Independent, 50000),
            new MateriaData(0x01, "HP Plus", MateriaType.Independent, 50000),
            new MateriaData(0x02, "Speed Plus", MateriaType.Independent, 100000),
            new MateriaData(0x03, "Magic Plus", MateriaType.Independent, 50000),
            new MateriaData(0x04, "Luck Plus", MateriaType.Independent, 100000),
            new MateriaData(0x05, "EXP Plus", MateriaType.Independent, 60000),
            new MateriaData(0x06, "Gil Plus", MateriaType.Independent, 80000),
            new MateriaData(0x07, "Enemy Away", MateriaType.Independent, 50000),
            new MateriaData(0x08, "Enemy Lure", MateriaType.Independent, 50000),
            new MateriaData(0x09, "Chocobo Lure", MateriaType.Independent, 30000),
            new MateriaData(0x0A, "Pre-Emptive", MateriaType.Independent, 80000),
            new MateriaData(0x0B, "Long Range", MateriaType.Independent, 80000),
            new MateriaData(0x0C, "Mega All", MateriaType.Independent, 160000),
            new MateriaData(0x0D, "Counter Attack", MateriaType.Independent, 100000),
            new MateriaData(0x0E, "Slash-All", MateriaType.Command, 130000),
            new MateriaData(0x0F, "Double Cut", MateriaType.Command, 150000),
            new MateriaData(0x10, "Cover", MateriaType.Independent, 40000),
            new MateriaData(0x11, "Underwater", MateriaType.Independent, 0, MateriaSpecialProperties.Underwater),
            new MateriaData(0x12, "HP <-> MP", MateriaType.Independent, 80000),
            new MateriaData(0x13, "W-Magic", MateriaType.Command, 250000),
            new MateriaData(0x14, "W-Summon", MateriaType.Command, 250000),
            new MateriaData(0x15, "W-Item", MateriaType.Command, 250000),
            new MateriaData(0x17, "All", MateriaType.Support, 35000),
            new MateriaData(0x18, "Counter", MateriaType.Support, 200000),
            new MateriaData(0x19, "Magic Counter", MateriaType.Support, 300000),
            new MateriaData(0x1A, "MP Turbo", MateriaType.Support, 120000),
            new MateriaData(0x1B, "MP Absorb", MateriaType.Support, 100000),
            new MateriaData(0x1C, "HP Absorb", MateriaType.Support, 100000),
            new MateriaData(0x1D, "Elemental", MateriaType.Support, 80000),
            new MateriaData(0x1E, "Added Effect", MateriaType.Support, 100000),
            new MateriaData(0x1F, "Sneak Attack", MateriaType.Support, 150000),
            new MateriaData(0x20, "Final Attack", MateriaType.Support, 160000),
            new MateriaData(0x21, "Added Cut", MateriaType.Support, 200000),
            new MateriaData(0x22, "Steal As Well", MateriaType.Support, 200000),
            new MateriaData(0x23, "Quadra Magic", MateriaType.Support, 200000),
            new MateriaData(0x24, "Steal", MateriaType.Command, 50000),
            new MateriaData(0x25, "Sense", MateriaType.Command, 40000),
            new MateriaData(0x27, "Throw", MateriaType.Command, 60000),
            new MateriaData(0x28, "Morph", MateriaType.Command, 100000),
            new MateriaData(0x29, "Deathblow", MateriaType.Command, 40000),
            new MateriaData(0x2A, "Manipulate", MateriaType.Command, 40000),
            new MateriaData(0x2B, "Mime", MateriaType.Command, 100000),
            new MateriaData(0x2C, "Enemy Skill", MateriaType.Command, 0, MateriaSpecialProperties.EnemySkill),
            new MateriaData(0x30, "Master Command", MateriaType.Command, 0, MateriaSpecialProperties.Master),
            new MateriaData(0x31, "Fire", MateriaType.Magic, 35000),
            new MateriaData(0x32, "Ice", MateriaType.Magic, 35000),
            new MateriaData(0x33, "Earth", MateriaType.Magic, 40000),
            new MateriaData(0x34, "Lightning", MateriaType.Magic, 35000),
            new MateriaData(0x35, "Restore", MateriaType.Magic, 40000),
            new MateriaData(0x36, "Heal", MateriaType.Magic, 60000),
            new MateriaData(0x37, "Revive", MateriaType.Magic, 55000),
            new MateriaData(0x38, "Seal", MateriaType.Magic, 20000),
            new MateriaData(0x39, "Mystify", MateriaType.Magic, 25000),
            new MateriaData(0x3A, "Transform", MateriaType.Magic, 24000),
            new MateriaData(0x3B, "Exit", MateriaType.Magic, 30000),
            new MateriaData(0x3C, "Poison", MateriaType.Magic, 38000),
            new MateriaData(0x3D, "Gravity", MateriaType.Magic, 40000),
            new MateriaData(0x3E, "Barrier", MateriaType.Magic, 45000),
            new MateriaData(0x40, "Comet", MateriaType.Magic, 60000),
            new MateriaData(0x41, "Time", MateriaType.Magic, 42000),
            new MateriaData(0x44, "Destruct", MateriaType.Magic, 45000),
            new MateriaData(0x45, "Contain", MateriaType.Magic, 60000),
            new MateriaData(0x46, "Full Cure", MateriaType.Magic, 100000),
            new MateriaData(0x47, "Shield", MateriaType.Magic, 100000),
            new MateriaData(0x48, "Ultima", MateriaType.Magic, 100000),
            new MateriaData(0x49, "Master Magic", MateriaType.Magic, 0, MateriaSpecialProperties.Master),
            new MateriaData(0x4A, "Choco/Mog", MateriaType.Summon, 35000),
            new MateriaData(0x4B, "Shiva", MateriaType.Summon, 50000),
            new MateriaData(0x4C, "Ifrit", MateriaType.Summon, 60000),
            new MateriaData(0x4D, "Titan", MateriaType.Summon, 80000),
            new MateriaData(0x4E, "Ramuh", MateriaType.Summon, 70000),
            new MateriaData(0x4F, "Odin", MateriaType.Summon, 80000),
            new MateriaData(0x50, "Leviathan", MateriaType.Summon, 100000),
            new MateriaData(0x51, "Bahamut", MateriaType.Summon, 120000),
            new MateriaData(0x52, "Kujata", MateriaType.Summon, 140000),
            new MateriaData(0x53, "Alexander", MateriaType.Summon, 150000),
            new MateriaData(0x54, "Phoenix", MateriaType.Summon, 180000),
            new MateriaData(0x55, "Neo Bahamut", MateriaType.Summon, 200000),
            new MateriaData(0x56, "Hades", MateriaType.Summon, 250000),
            new MateriaData(0x57, "Typoon", MateriaType.Summon, 250000),
            new MateriaData(0x58, "Bahamut ZERO", MateriaType.Summon, 250000),
            new MateriaData(0x59, "Knights of the Round", MateriaType.Summon, 500000),
            new MateriaData(0x5A, "Master Summon", MateriaType.Summon, 0, MateriaSpecialProperties.Master)
        };

        //find weapon by its hex value, or null if it doesn't exist
        public static WeaponData GetWeaponByHexValue(int hexValue)
        {
            if (hexValue >= CAIT_SITH_WEAPON_LIST[0].HexValue &&
                    hexValue <= CAIT_SITH_WEAPON_LIST[CAIT_SITH_WEAPON_LIST.Length - 1].HexValue)
            {
                return CAIT_SITH_WEAPON_LIST[hexValue - CAIT_SITH_WEAPON_LIST[0].HexValue];
            }
            else if (hexValue >= VINCENT_WEAPON_LIST[0].HexValue &&
                    hexValue <= VINCENT_WEAPON_LIST[VINCENT_WEAPON_LIST.Length - 1].HexValue)
            {
                return VINCENT_WEAPON_LIST[hexValue - VINCENT_WEAPON_LIST[0].HexValue];
            }
            return null;
        }

        //find armor by its hex value, or null if it doesn't exist
        public static ArmorData GetArmorByHexValue(int hexValue)
        {
            if (hexValue >= 0 && hexValue < ARMOR_LIST.Length)
            {
                return ARMOR_LIST[hexValue];
            }
            return null;
        }

        //find accessory by its hex value, or null if it doesn't exist
        public static AccessoryData GetAccessoryByHexValue(int hexValue)
        {
            if (hexValue >= ACCESSORY_LIST[0].HexValue &&
                    hexValue <= ACCESSORY_LIST[ACCESSORY_LIST.Length - 1].HexValue)
            {
                return ACCESSORY_LIST[hexValue - ACCESSORY_LIST[0].HexValue];
            }
            return null;
        }

        //find materia by its hex value, or null if it doesn't exist
        public static MateriaData GetMateriaByHexValue(int hexValue)
        {
            foreach (var m in MATERIA_LIST)
            {
                if (m.HexValue == hexValue)
                {
                    return m;
                }
            }
            return null;
        }
    }
}
