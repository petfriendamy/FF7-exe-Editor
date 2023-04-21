using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Core
{
    public static class GameData
    {
        public const ushort
            ITEM_COUNT = 105,
            WEAPON_COUNT = 128,
            ARMOR_COUNT = 32,
            ACCESSORY_COUNT = 32,
            MATERIA_COUNT = 91,

            ITEM_END = 128,
            WEAPON_END = ITEM_END + WEAPON_COUNT,
            ARMOR_END = WEAPON_END + ARMOR_COUNT,
            ACCESSORY_END = ARMOR_END + ACCESSORY_COUNT;

        public static ReadOnlyCollection<ItemData> ITEM_LIST = new ItemData[ITEM_COUNT]
        {
            new ItemData(0x00, "Potion"),
            new ItemData(0x01, "Hi-Potion"),
            new ItemData(0x02, "X-Potion"),
            new ItemData(0x03, "Ether"),
            new ItemData(0x04, "Turbo Ether"),
            new ItemData(0x05, "Elixir"),
            new ItemData(0x06, "Megalixir"),
            new ItemData(0x07, "Phoenix Down"),
            new ItemData(0x08, "Antidote"),
            new ItemData(0x09, "Soft"),
            new ItemData(0x0A, "Maiden's Kiss"),
            new ItemData(0x0B, "Cornucopia"),
            new ItemData(0x0C, "Echo Screen"),
            new ItemData(0x0D, "Hyper"),
            new ItemData(0x0E, "Tranquilizer"),
            new ItemData(0x0F, "Remedy"),
            new ItemData(0x10, "Smoke Bomb"),
            new ItemData(0x11, "Speed Drink"),
            new ItemData(0x12, "Hero Drink"),
            new ItemData(0x13, "Vaccine"),
            new ItemData(0x14, "Grenade"),
            new ItemData(0x15, "Shrapnel"),
            new ItemData(0x16, "Right Arm"),
            new ItemData(0x17, "Hourglass"),
            new ItemData(0x18, "Kiss of Death"),
            new ItemData(0x19, "Spider Web"),
            new ItemData(0x1A, "Dream Powder"),
            new ItemData(0x1B, "Mute Mask"),
            new ItemData(0x1C, "War Gong"),
            new ItemData(0x1D, "Loco Weed"),
            new ItemData(0x1E, "Fire Fang"),
            new ItemData(0x1F, "Fire Veil"),
            new ItemData(0x20, "Antarctic Wind"),
            new ItemData(0x21, "Ice Crystal"),
            new ItemData(0x22, "Bolt Plume"),
            new ItemData(0x23, "Swift Bolt"),
            new ItemData(0x24, "Earth Drum"),
            new ItemData(0x25, "Earth Mallet"),
            new ItemData(0x26, "Deadly Waste"),
            new ItemData(0x27, "M-Tentacles"),
            new ItemData(0x28, "Stardust"),
            new ItemData(0x29, "Vampire Fang"),
            new ItemData(0x2A, "Ghost Hand"),
            new ItemData(0x2B, "Vagyrisk Claw"),
            new ItemData(0x2C, "Light Curtain"),
            new ItemData(0x2D, "Lunar Curtain"),
            new ItemData(0x2E, "Mirror"),
            new ItemData(0x2F, "Holy Torch"),
            new ItemData(0x30, "Bird Wing"),
            new ItemData(0x31, "Dragon Scales"),
            new ItemData(0x32, "Impaler"),
            new ItemData(0x33, "Shrivel"),
            new ItemData(0x34, "Eye Drop"),
            new ItemData(0x35, "Molotov"),
            new ItemData(0x36, "S-Mine"),
            new ItemData(0x37, "8-Inch Cannon"),
            new ItemData(0x38, "Graviball"),
            new ItemData(0x39, "T/S Bomb"),
            new ItemData(0x3A, "Ink"),
            new ItemData(0x3B, "Dazers"),
            new ItemData(0x3C, "Dragon Fang"),
            new ItemData(0x3D, "Cauldron"),
            new ItemData(0x3E, "Sylkis Greens"),
            new ItemData(0x3F, "Reagan Greens"),
            new ItemData(0x40, "Mimett Greens"),
            new ItemData(0x41, "Curiel Greens"),
            new ItemData(0x42, "Pahsana Greens"),
            new ItemData(0x43, "Tantal Greens"),
            new ItemData(0x44, "Krakka Greens"),
            new ItemData(0x45, "Gysahl Greens"),
            new ItemData(0x46, "Tent"),
            new ItemData(0x47, "Power Source"),
            new ItemData(0x48, "Guard Source"),
            new ItemData(0x49, "Magic Source"),
            new ItemData(0x4A, "Mind Source"),
            new ItemData(0x4B, "Speed Source"),
            new ItemData(0x4C, "Luck Source"),
            new ItemData(0x4D, "Zeio Nut"),
            new ItemData(0x4E, "Carob Nut"),
            new ItemData(0x4F, "Porov Nut"),
            new ItemData(0x50, "Pram Nut"),
            new ItemData(0x51, "Lasan Nut"),
            new ItemData(0x52, "Saraha Nut"),
            new ItemData(0x53, "Luchile Nut"),
            new ItemData(0x54, "Pepio Nut"),
            new ItemData(0x55, "Battery"),
            new ItemData(0x56, "Tissue"),
            new ItemData(0x57, "Omnislash"),
            new ItemData(0x58, "Catastrophe"),
            new ItemData(0x59, "Final Heaven"),
            new ItemData(0x5A, "Great Gospel"),
            new ItemData(0x5B, "Cosmo Memory"),
            new ItemData(0x5C, "All Creation"),
            new ItemData(0x5D, "Chaos"),
            new ItemData(0x5E, "Highwind"),
            new ItemData(0x5F, "1/35 Soldier"),
            new ItemData(0x60, "Super Sweeper"),
            new ItemData(0x61, "Masamune Blade"),
            new ItemData(0x62, "Save Crystal"),
            new ItemData(0x63, "Combat Diary"),
            new ItemData(0x64, "Autograph"),
            new ItemData(0x65, "Gambler"),
            new ItemData(0x66, "Desert Rose"),
            new ItemData(0x67, "Earth Harp"),
            new ItemData(0x68, "Guide Book")
        }.AsReadOnly();

        public static ReadOnlyCollection<WeaponData> WEAPON_LIST = new WeaponData[WEAPON_COUNT]
        {
            new WeaponData(0x00, "Buster Sword", Characters.Cloud, 2),
            new WeaponData(0x01, "Mythril Saber", Characters.Cloud, 3),
            new WeaponData(0x02, "Hardedge", Characters.Cloud, 4),
            new WeaponData(0x03, "Butterfly Edge", Characters.Cloud, 4),
            new WeaponData(0x04, "Enhance Sword", Characters.Cloud, 8),
            new WeaponData(0x05, "Organics", Characters.Cloud, 6),
            new WeaponData(0x06, "Crystal Sword", Characters.Cloud, 6),
            new WeaponData(0x07, "Force Stealer", Characters.Cloud, 3),
            new WeaponData(0x08, "Rune Blade", Characters.Cloud, 4),
            new WeaponData(0x09, "Murasame", Characters.Cloud, 5),
            new WeaponData(0x0A, "Nail Bat", Characters.Cloud, 0),
            new WeaponData(0x0B, "Yoshiyuki", Characters.Cloud, 2),
            new WeaponData(0x0C, "Apocalypse", Characters.Cloud, 3),
            new WeaponData(0x0D, "Heaven's Cloud", Characters.Cloud, 6),
            new WeaponData(0x0E, "Ragnarok", Characters.Cloud, 6),
            new WeaponData(0x0F, "Ultima Weapon", Characters.Cloud, 8),

            new WeaponData(0x10, "Leather Glove", Characters.Tifa, 1),
            new WeaponData(0x11, "Metal Knuckle", Characters.Tifa, 2),
            new WeaponData(0x12, "Mythril Claw", Characters.Tifa, 3),
            new WeaponData(0x13, "Grand Glove", Characters.Tifa, 4),
            new WeaponData(0x14, "Tiger Fang", Characters.Tifa, 4),
            new WeaponData(0x15, "Diamond Knuckle", Characters.Tifa, 5),
            new WeaponData(0x16, "Dragon Claw", Characters.Tifa, 6),
            new WeaponData(0x17, "Crystal Glove", Characters.Tifa, 6),
            new WeaponData(0x18, "Motor Drive", Characters.Tifa, 3),
            new WeaponData(0x19, "Platinum Fist", Characters.Tifa, 4),
            new WeaponData(0x1A, "Kaiser Knuckle", Characters.Tifa, 8),
            new WeaponData(0x1B, "Work Glove", Characters.Tifa, 0),
            new WeaponData(0x1C, "Powersoul", Characters.Tifa, 4),
            new WeaponData(0x1D, "Master Fist", Characters.Tifa, 6),
            new WeaponData(0x1E, "God's Hand", Characters.Tifa, 4),
            new WeaponData(0x1F, "Premium Heart", Characters.Tifa, 8),

            new WeaponData(0x20, "Gatling Gun", Characters.Barret, 1),
            new WeaponData(0x21, "Assault Gun", Characters.Barret, 2),
            new WeaponData(0x22, "Cannon Ball", Characters.Barret, 3),
            new WeaponData(0x23, "Atomic Scissors", Characters.Barret, 4),
            new WeaponData(0x24, "Heavy Vulcan", Characters.Barret, 4),
            new WeaponData(0x25, "Chainsaw", Characters.Barret, 5),
            new WeaponData(0x26, "Microlaser", Characters.Barret, 6),
            new WeaponData(0x27, "A-M Cannon", Characters.Barret, 6),
            new WeaponData(0x28, "W Machine Gun", Characters.Barret, 3),
            new WeaponData(0x29, "Drill Arm", Characters.Barret, 4),
            new WeaponData(0x2A, "Solid Bazooka", Characters.Barret, 8),
            new WeaponData(0x2B, "Rocket Punch", Characters.Barret, 0),
            new WeaponData(0x2C, "Enemy Launcher", Characters.Barret, 5),
            new WeaponData(0x2D, "Pile Banger", Characters.Barret, 6),
            new WeaponData(0x2E, "Max Ray", Characters.Barret, 6),
            new WeaponData(0x2F, "Missing Score", Characters.Barret, 8),

            new WeaponData(0x30, "Mythril Clip", Characters.RedXIII, 3),
            new WeaponData(0x31, "Diamond Pin", Characters.RedXIII, 4),
            new WeaponData(0x32, "Silver Barrette", Characters.RedXIII, 4),
            new WeaponData(0x33, "Gold Barrette", Characters.RedXIII, 5),
            new WeaponData(0x34, "Adaman Clip", Characters.RedXIII, 6),
            new WeaponData(0x35, "Crystal Comb", Characters.RedXIII, 6),
            new WeaponData(0x36, "Magic Comb", Characters.RedXIII, 3),
            new WeaponData(0x37, "Plus Barrette", Characters.RedXIII, 4),
            new WeaponData(0x38, "Centclip", Characters.RedXIII, 8),
            new WeaponData(0x39, "Hairpin", Characters.RedXIII, 0),
            new WeaponData(0x3A, "Seraph Comb", Characters.RedXIII, 4),
            new WeaponData(0x3B, "Behemoth Horn", Characters.RedXIII, 6),
            new WeaponData(0x3C, "Spring Gun Clip", Characters.RedXIII, 6),
            new WeaponData(0x3D, "Limited Moon", Characters.RedXIII, 8),

            new WeaponData(0x3E, "Guard Stick", Characters.Aeris, 0),
            new WeaponData(0x3F, "Mythril Rod", Characters.Aeris, 2),
            new WeaponData(0x40, "Full Metal Staff", Characters.Aeris, 3),
            new WeaponData(0x41, "Striking Staff", Characters.Aeris, 4),
            new WeaponData(0x42, "Prism Staff", Characters.Aeris, 4),
            new WeaponData(0x43, "Aurora Rod", Characters.Aeris, 5),
            new WeaponData(0x44, "Wizard Staff", Characters.Aeris, 3),
            new WeaponData(0x45, "Wizer Staff", Characters.Aeris, 4),
            new WeaponData(0x46, "Fairy Tale", Characters.Aeris, 7),
            new WeaponData(0x47, "Umbrella", Characters.Aeris, 0),
            new WeaponData(0x48, "Princess Guard", Characters.Aeris, 7),

            new WeaponData(0x49, "Spear", Characters.Cid, 4),
            new WeaponData(0x4A, "Slash Lance", Characters.Cid, 5),
            new WeaponData(0x4B, "Trident", Characters.Cid, 6),
            new WeaponData(0x4C, "Mast Ax", Characters.Cid, 6),
            new WeaponData(0x4D, "Partisan", Characters.Cid, 6),
            new WeaponData(0x4E, "Viper Halberd", Characters.Cid, 4),
            new WeaponData(0x4F, "Javelin", Characters.Cid, 5),
            new WeaponData(0x50, "Grow Lance", Characters.Cid, 6),
            new WeaponData(0x51, "Mop", Characters.Cid, 0),
            new WeaponData(0x52, "Dragoon Lance", Characters.Cid, 8),
            new WeaponData(0x53, "Scimitar", Characters.Cid, 2),
            new WeaponData(0x54, "Flayer", Characters.Cid, 6),
            new WeaponData(0x55, "Spirit Lance", Characters.Cid, 4),
            new WeaponData(0x56, "Venus Gospel", Characters.Cid, 8),

            new WeaponData(0x57, "4-point Shuriken", Characters.Yuffie, 3),
            new WeaponData(0x58, "Boomerang", Characters.Yuffie, 4),
            new WeaponData(0x59, "Pinwheel", Characters.Yuffie, 4),
            new WeaponData(0x5A, "Razor Ring", Characters.Yuffie, 5),
            new WeaponData(0x5B, "Hawkeye", Characters.Yuffie, 6),
            new WeaponData(0x5C, "Crystal Cross", Characters.Yuffie, 6),
            new WeaponData(0x5D, "Wind Slash", Characters.Yuffie, 3),
            new WeaponData(0x5E, "Twin Viper", Characters.Yuffie, 4),
            new WeaponData(0x5F, "Spiral Shuriken", Characters.Yuffie, 8),
            new WeaponData(0x60, "Superball", Characters.Yuffie, 0),
            new WeaponData(0x61, "Magic Shuriken", Characters.Yuffie, 3),
            new WeaponData(0x62, "Rising Sun", Characters.Yuffie, 4),
            new WeaponData(0x63, "Oritsuru", Characters.Yuffie, 8),
            new WeaponData(0x64, "Conformer", Characters.Yuffie, 8),

            new WeaponData(0x65, "Yellow M-Phone", Characters.CaitSith, 4),
            new WeaponData(0x66, "Green M-Phone", Characters.CaitSith, 4),
            new WeaponData(0x67, "Blue M-Phone", Characters.CaitSith, 5),
            new WeaponData(0x68, "Red M-Phone", Characters.CaitSith, 6),
            new WeaponData(0x69, "Crystal M-Phone", Characters.CaitSith, 6),
            new WeaponData(0x6A, "White M-Phone", Characters.CaitSith, 3),
            new WeaponData(0x6B, "Black M-Phone", Characters.CaitSith, 4),
            new WeaponData(0x6C, "Silver M-Phone", Characters.CaitSith, 8),
            new WeaponData(0x6D, "Trumpet Shell", Characters.CaitSith, 0),
            new WeaponData(0x6E, "Gold M-Phone", Characters.CaitSith, 8),
            new WeaponData(0x6F, "Battle Trumpet", Characters.CaitSith, 6),
            new WeaponData(0x70, "Starlight Phone", Characters.CaitSith, 8),
            new WeaponData(0x71, "HP Shout", Characters.CaitSith, 8),

            new WeaponData(0x72, "Quicksilver", Characters.Vincent, 4),
            new WeaponData(0x73, "Shotgun", Characters.Vincent, 4),
            new WeaponData(0x74, "Shortbarrel", Characters.Vincent, 5),
            new WeaponData(0x75, "Lariat", Characters.Vincent, 6),
            new WeaponData(0x76, "Winchester", Characters.Vincent, 6),
            new WeaponData(0x77, "Peacemaker", Characters.Vincent, 3),
            new WeaponData(0x78, "Buntline", Characters.Vincent, 4),
            new WeaponData(0x79, "Long Barrel R", Characters.Vincent, 8),
            new WeaponData(0x7A, "Silver Rifle", Characters.Vincent, 0),
            new WeaponData(0x7B, "Sniper CR", Characters.Vincent, 4),
            new WeaponData(0x7C, "Supershot", Characters.Vincent, 6),
            new WeaponData(0x7D, "Outsider", Characters.Vincent, 8),
            new WeaponData(0x7E, "Death Penalty", Characters.Vincent, 8),

            new WeaponData(0x7F, "Masamune", Characters.Sephiroth, 6)
        }.AsReadOnly();

        public static ReadOnlyCollection<ArmorData> ARMOR_LIST = new ArmorData[ARMOR_COUNT]
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
        }.AsReadOnly();

        public static ReadOnlyCollection<AccessoryData> ACCESSORY_LIST = new AccessoryData[ACCESSORY_COUNT]
        {
            new AccessoryData(0x00, "Power Wrist"),
            new AccessoryData(0x01, "Protect Vest"),
            new AccessoryData(0x02, "Earring"),
            new AccessoryData(0x03, "Talisman"),
            new AccessoryData(0x04, "Choco Feather"),
            new AccessoryData(0x05, "Amulet"),
            new AccessoryData(0x06, "Champion Belt"),
            new AccessoryData(0x07, "Poison Ring"),
            new AccessoryData(0x08, "Tough Ring"),
            new AccessoryData(0x09, "Circlet"),
            new AccessoryData(0x0A, "Star Pendant"),
            new AccessoryData(0x0B, "Silver Glasses"),
            new AccessoryData(0x0C, "Headband"),
            new AccessoryData(0x0D, "Fairy Ring"),
            new AccessoryData(0x0E, "Jem Ring"),
            new AccessoryData(0x0F, "White Cape"),
            new AccessoryData(0x10, "Sprint Shoes"),
            new AccessoryData(0x11, "Peace Ring"),
            new AccessoryData(0x12, "Ribbon"),
            new AccessoryData(0x13, "Fire Ring"),
            new AccessoryData(0x14, "Ice Ring"),
            new AccessoryData(0x15, "Bolt Ring"),
            new AccessoryData(0x16, "Tetra Elemental"),
            new AccessoryData(0x17, "Safety Bit"),
            new AccessoryData(0x18, "Fury Ring"),
            new AccessoryData(0x19, "Curse Ring"),
            new AccessoryData(0x1A, "Protect Ring"),
            new AccessoryData(0x1B, "Cat's Bell"),
            new AccessoryData(0x1C, "Reflect Ring"),
            new AccessoryData(0x1D, "Water Ring"),
            new AccessoryData(0x1E, "Sneak Glove"),
            new AccessoryData(0x1F, "HypnoCrown")
        }.AsReadOnly();

        public static ReadOnlyCollection<MateriaData> MATERIA_LIST = new MateriaData[MATERIA_COUNT]
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
            new MateriaData(0x16, "(unused)", MateriaType.Support, 0, MateriaSpecialProperties.Unused),
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
            new MateriaData(0x26, "(unused)", MateriaType.Command, 0, MateriaSpecialProperties.Unused),
            new MateriaData(0x27, "Throw", MateriaType.Command, 60000),
            new MateriaData(0x28, "Morph", MateriaType.Command, 100000),
            new MateriaData(0x29, "Deathblow", MateriaType.Command, 40000),
            new MateriaData(0x2A, "Manipulate", MateriaType.Command, 40000),
            new MateriaData(0x2B, "Mime", MateriaType.Command, 100000),
            new MateriaData(0x2C, "Enemy Skill", MateriaType.Command, 0, MateriaSpecialProperties.EnemySkill),
            new MateriaData(0x2D, "(unused)", MateriaType.Independent, 0, MateriaSpecialProperties.Unused),
            new MateriaData(0x2E, "(unused)", MateriaType.Independent, 0, MateriaSpecialProperties.Unused),
            new MateriaData(0x2F, "(unused)", MateriaType.Independent, 0, MateriaSpecialProperties.Unused),
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
            new MateriaData(0x3F, "(unused)", MateriaType.Magic, 0, MateriaSpecialProperties.Unused),
            new MateriaData(0x40, "Comet", MateriaType.Magic, 60000),
            new MateriaData(0x41, "Time", MateriaType.Magic, 42000),
            new MateriaData(0x42, "(unused)", MateriaType.Magic, 0, MateriaSpecialProperties.Unused),
            new MateriaData(0x43, "(unused)", MateriaType.Magic, 0, MateriaSpecialProperties.Unused),
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
        }.AsReadOnly();

        public static ReadOnlyDictionary<int, string> SHOP_NAMES = new Dictionary<int, string>
        {
            { 0, "Sector 7 Weapon Shop" },
            { 1, "Sector 7 Item Shop" },
            { 2, "Sector 7 Drug Store" },
            { 3, "Sector 8 Weapon Shop" },
            { 4, "Sector 8 Item Shop" },
            { 5, "Sector 8 Materia Shop" },
            { 6, "Wall Market Weapon Shop" },
            { 7, "Wall Market Materia Shop" },
            { 8, "Wall Market Item Shop" },
            { 9, "Sector 7 Pillar Shop" },
            { 10, "Shinra HQ Shop" },
            { 11, "Kalm Weapon Shop" },
            { 12, "Kalm Item Shop" },
            { 13, "Kalm Materia Shop" },
            { 14, "Choco Billy's Greens Store (Disc 1)" },
            { 15, "Choco Billy's Greens Store (Disc 2)" },
            { 16, "Fort Condor Item Shop (Disc 1)" },
            { 17, "Fort Condor Materia Shop (Disc 1)" },
            { 18, "Lower Junon Weapon Shop" },
            { 19, "Upper Junon Weapon Shop #1 (Disc 1)" },
            { 20, "Upper Junon Item Shop (Disc 1)" },
            { 21, "Upper Junon Materia Shop #1" },
            { 22, "Upper Junon Weapon Shop #2 (Disc 1)" },
            { 23, "Upper Junon Accessory Shop (Disc 1)" },
            { 24, "Upper Junon Materia Shop #2 (Disc 1)" },
            { 25, "Cargo Ship Item Shop" },
            { 26, "Costa Del Sol Weapon Shop (Disc 1)" },
            { 27, "Costa Del Sol Materia Shop (Disc 1)" },
            { 28, "Costa Del Sol Item Shop (Disc 1)" },
            { 29, "North Corel Weapons Shop" },
            { 30, "North Corel Item Shop" },
            { 31, "North Corel General Store" },
            { 32, "Gold Saucer Hotel Shop" },
            { 33, "Corel Prison General Store" },
            { 34, "Gongaga Weapon Shop" },
            { 35, "Gongaga Item Shop" },
            { 36, "Gongaga Accessory Shop" },
            { 37, "Cosmo Canyon Weapon Shop" },
            { 38, "Cosmo Canyon Item Shop" },
            { 39, "Cosmo Canyon Materia Shop" },
            { 40, "Nibelheim General Store" },
            { 41, "Rocket Town Weapon Shop (Disc 1)" },
            { 42, "Rocket Town Item Shop (Disc 1)" },
            { 43, "Wutai Weapon Shop" },
            { 44, "Wutai Item Shop" },
            { 45, "Temple of the Ancients Shop" },
            { 46, "Icicle Inn Weapon Shop" },
            { 47, "Mideel Weapon Shop" },
            { 48, "Mideel Accessory Shop" },
            { 49, "Mideel Item Shop" },
            { 50, "Mideel Materia Shop" },
            { 51, "Fort Condor Item Shop (Disc 2)" },
            { 52, "Fort Condor Materia Shop (Disc 2)" },
            { 53, "Chocobo Sage's Greens Store" },
            { 54, "Upper Junon Weapon Shop #1 (Disc 2)" },
            { 55, "Upper Junon Item Shop (Disc 2)" },
            { 57, "Upper Junon Weapon Shop #2 (Disc 2)" },
            { 58, "Upper Junon Accessory Shop (Disc 2)" },
            { 59, "Upper Junon Materia Shop #2 (Disc 2)" },
            { 60, "Costa Del Sol Weapon Shop (Disc 2)" },
            { 61, "Costa Del Sol Materia Shop (Disc 2)" },
            { 62, "Costa Del Sol Item Shop (Disc 2)" },
            { 63, "Rocket Town Weapon Shop (Disc 2)" },
            { 64, "Rocket Town Item Shop (Disc 2)" },
            { 65, "Bone Village Shop" }
        }.AsReadOnly();

        //find item by its hex value, or null if it doesn't exist
        public static ItemData? GetItemByHexValue(ushort hexValue)
        {
            if (hexValue < ITEM_END)
            {
                if (hexValue < ITEM_COUNT)
                {
                    return ITEM_LIST[hexValue];
                }
            }
            else if (hexValue < WEAPON_END)
            {
                return GetWeaponByHexValue((byte)(hexValue - ITEM_END));
            }
            else if (hexValue < ARMOR_END)
            {
                return GetArmorByHexValue((byte)(hexValue - WEAPON_END));
            }
            else if (hexValue < ACCESSORY_END)
            {
                return GetAccessoryByHexValue((byte)(hexValue - ARMOR_END));
            }
            return null;
        }

        //find weapon by its hex value, or null if it doesn't exist
        public static WeaponData? GetWeaponByHexValue(byte hexValue)
        {
            if (hexValue >= 0 && hexValue < WEAPON_COUNT)
            {
                return WEAPON_LIST[hexValue];
            }
            return null;
        }

        //find armor by its hex value, or null if it doesn't exist
        public static ArmorData? GetArmorByHexValue(byte hexValue)
        {
            if (hexValue >= 0 && hexValue < ARMOR_COUNT)
            {
                return ARMOR_LIST[hexValue];
            }
            return null;
        }

        //find accessory by its hex value, or null if it doesn't exist
        public static AccessoryData? GetAccessoryByHexValue(byte hexValue)
        {
            if (hexValue >= 0 && hexValue < ACCESSORY_COUNT)
            {
                return ACCESSORY_LIST[hexValue];
            }
            return null;
        }

        //find materia by its hex value, or null if it doesn't exist
        public static MateriaData? GetMateriaByHexValue(byte hexValue)
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

        public static ushort GetItemIndex(ItemData item)
        {
            if (item is WeaponData)
            {
                return (ushort)(item.HexValue + ITEM_END);
            }
            else if (item is ArmorData)
            {
                return (ushort)(item.HexValue + WEAPON_END);
            }
            else if (item is AccessoryData)
            {
                return (ushort)(item.HexValue + ARMOR_END);
            }
            else
            {
                return item.HexValue;
            }
        }
    }
}
