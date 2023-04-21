using System;

namespace Editor.Core
{
    public class EquippedMateria
    {
        private byte[] data = new byte[4];
        private MateriaData? materiaID;

        public MateriaData? MateriaID
        {
            get { return materiaID; }
            set
            {
                materiaID = value;
                if (materiaID == null)
                {
                    for (int i = 0; i < 4; ++i)
                    {
                        data[i] = 0xFF;
                    }
                }
                else { data[0] = materiaID.HexValue; }
            }
        }
        public int CurrentAP
        {
            get
            {
                if (MateriaID == null) { return 0; }
                else
                {
                    var temp = new byte[4];
                    for (int i = 0; i < 3; ++i)
                    {
                        temp[i] = data[i + 1];
                    }
                    return Math.Min((int)BitConverter.ToUInt32(temp, 0), MateriaID.MaxAP);
                }
            }
            set
            {
                if (MateriaID != null)
                {
                    if (value >= MateriaID.MaxAP) //mastered
                    {
                        for (int i = 1; i < 4; ++i)
                        {
                            data[i] = 0xFF;
                        }
                    }
                    else //get current AP
                    {
                        var temp = BitConverter.GetBytes(value);
                        for (int i = 0; i < 3; ++i)
                        {
                            data[i + 1] = temp[i];
                        }
                    }
                }
            }
        }

        //constructor
        public EquippedMateria(byte[] data)
        {
            if (data.Length != 4)
            {
                throw new ArgumentException("Array is incorrect length.");
            }
            for (int i = 0; i < 4; ++i)
            {
                this.data[i] = data[i];
            }
            MateriaID = GameData.GetMateriaByHexValue(data[0]);
        }

        //clears the current materia slot
        public void ClearSlot()
        {
            MateriaID = null;
            CurrentAP = 0;
        }

        //returns data as byte array
        public byte[] GetByteArray()
        {
            var dataCopy = new byte[4];
            Array.Copy(data, dataCopy, 4);
            return dataCopy;
        }
    }
}
