using System;
using System.IO;

namespace Editor.Core
{
    public enum Language { English, Spanish, French, German }

    public class EXEeditor
    {
        //constants
        private readonly int[] EXE_HEADER = new int[3] { 0x4D, 0x5A, 0x90 };
        private const int NUM_NAMES = 10;
        private const long
            NAME_DATA_POS = 0x5206b8,
            CAIT_SITH_DATA_POS = 0x520C10,
            HEXT_OFFSET = 0x401600,
            NAME_ES_OFFSET = 0x780E0,
            CAIT_ES_OFFSET = 0x78110,
            NAME_FR_OFFSET = 0x77E70,
            CAIT_FR_OFFSET = 0x77E90,
            NAME_DE_OFFSET = 0x77370,
            CAIT_DE_OFFSET = 0x77388;

        //properties
        public string FilePath { get; private set; }
        public Language Language { get; private set; }
        public CharName[] Names = new CharName[NUM_NAMES];
        public CharData CaitSith { get; private set; }
        public CharData Vincent { get; private set; }


        public EXEeditor(string path)
        {
            ReadEXE(path);
        }

        private long GetNameOffset()
        {
            switch (Language)
            {
                case Language.Spanish:
                    return NAME_ES_OFFSET;
                case Language.French:
                    return NAME_FR_OFFSET;
                case Language.German:
                    return NAME_DE_OFFSET;
                default:
                    return 0;
            }
        }

        private long GetCaitOffset()
        {
            switch (Language)
            {
                case Language.Spanish:
                    return CAIT_ES_OFFSET;
                case Language.French:
                    return CAIT_FR_OFFSET;
                case Language.German:
                    return CAIT_DE_OFFSET;
                default:
                    return 0;
            }
        }

        public void ReadEXE(string path)
        {
            if (File.Exists(path))
            {
                //check EXE language
                string name = Path.GetFileNameWithoutExtension(path);
                if (name.EndsWith("_es"))
                {
                    Language = Language.Spanish;
                }
                else if (name.EndsWith("_fr"))
                {
                    Language = Language.French;
                }
                else if (name.EndsWith("_de"))
                {
                    Language = Language.German;
                }
                else
                {
                    Language = Language.English;
                }

                //attempt to open and read the EXE
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    //check if header is correct
                    for (int i = 0; i < EXE_HEADER.Length; ++i)
                    {
                        if (stream.ReadByte() != EXE_HEADER[i])
                        {
                            throw new FormatException("EXE appears to be invalid.");
                        }
                    }

                    //get character names
                    stream.Seek(NAME_DATA_POS + GetNameOffset(), SeekOrigin.Begin);
                    var temp = new int[CharName.STRING_LENGTH];
                    for (int i = 0; i < 10; ++i)
                    {
                        for (int j = 0; j < CharName.STRING_LENGTH; ++j)
                        {
                            temp[j] = stream.ReadByte();
                        }
                        Names[i] = new CharName(temp);
                    }

                    //get Cait Sith data
                    stream.Seek(CAIT_SITH_DATA_POS + GetCaitOffset(), SeekOrigin.Begin);
                    temp = new int[CharData.CHAR_DATA_LENGTH];
                    for (int i = 0; i < CharData.CHAR_DATA_LENGTH; ++i)
                    {
                        temp[i] = stream.ReadByte();
                    }
                    CaitSith = new CharData(temp);

                    //get Vincent data
                    for (int i = 0; i < CharData.CHAR_DATA_LENGTH; ++i)
                    {
                        temp[i] = stream.ReadByte();
                    }
                    Vincent = new CharData(temp);
                }
                FilePath = path;
            }
        }

        public void WriteEXE(string path = null)
        {
            if (path == null)
            {
                path = FilePath;
            }

            if (File.Exists(path))
            {
                try
                {
                    using (var stream = new FileStream(path, FileMode.Open, FileAccess.Write))
                    {
                        stream.Seek(NAME_DATA_POS + GetNameOffset(), SeekOrigin.Begin);
                        foreach (var n in Names)
                        {
                            var nData = n.GetByteArray();
                            foreach (var b in nData)
                            {
                                stream.WriteByte(b);
                            }
                        }

                        stream.Seek(CAIT_SITH_DATA_POS + GetCaitOffset(), SeekOrigin.Begin);
                        var csData = CaitSith.GetByteArray();
                        var vData = Vincent.GetByteArray();
                        foreach (var d in csData)
                        {
                            stream.WriteByte(d);
                        }
                        foreach (var d in vData)
                        {
                            stream.WriteByte(d);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new IOException("There was a problem editing the EXE.", ex);
                }
            }
        }

        //read data from a file
        public void ReadFile(string path)
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    var temp = new int[CharName.STRING_LENGTH];
                    for (int i = 0; i < NUM_NAMES; ++i)
                    {
                        for (int j = 0; j < CharName.STRING_LENGTH; ++j)
                        {
                            temp[j] = stream.ReadByte();
                        }
                        Names[i] = new CharName(temp);
                    }

                    temp = new int[CharData.CHAR_DATA_LENGTH];
                    for (int i = 0; i < CharData.CHAR_DATA_LENGTH; ++i)
                    {
                        temp[i] = stream.ReadByte();
                    }
                    if (CaitSith == null) { CaitSith = new CharData(temp); }
                    else { CaitSith.ReadData(temp); }

                    for (int i = 0; i < CharData.CHAR_DATA_LENGTH; ++i)
                    {
                        temp[i] = stream.ReadByte();
                    }
                    if (Vincent == null) { Vincent = new CharData(temp); }
                    else { Vincent.ReadData(temp); }
                }
            }
            catch (Exception ex)
            {
                throw new IOException("There was a problem reading the file.", ex);
            }
        }

        //write data to a file
        public void WriteFile(string path)
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    foreach (var n in Names)
                    {
                        var nData = n.GetByteArray();
                        foreach (var b in nData)
                        {
                            stream.WriteByte(b);
                        }
                    }
                    var csData = CaitSith.GetByteArray();
                    var vData = Vincent.GetByteArray();
                    foreach (var b in csData)
                    {
                        stream.WriteByte(b);
                    }
                    foreach (var b in vData)
                    {
                        stream.WriteByte(b);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new IOException("There was a problem writing the file.", ex);
            }
        }

        //read data from a byte array
        public void ReadBytes(byte[] bytes)
        {
            if (bytes.Length != (CharName.STRING_LENGTH * 10) + (CharData.CHAR_DATA_LENGTH * 2))
            {
                throw new ArgumentException("Byte array is incorrect length.");
            }
            int j = 0;
            var temp = new int[CharName.STRING_LENGTH];
            for (int i = 0; i < 10; ++i)
            {
                for (int n = 0; n < CharName.STRING_LENGTH; ++n)
                {
                    temp[n] = bytes[j];
                    j++;
                }
                Names[i] = new CharName(temp);
            }
            
            temp = new int[CharData.CHAR_DATA_LENGTH];
            for (int i = 0; i < CharData.CHAR_DATA_LENGTH; ++i)
            {
                temp[i] = bytes[j];
                j++;
            }
            if (CaitSith == null) { CaitSith = new CharData(temp); }
            else { CaitSith.ReadData(temp); }

            for (int i = 0; i < CharData.CHAR_DATA_LENGTH; ++i)
            {
                temp[i] = bytes[j];
                j++;
            }
            if (Vincent == null) { Vincent = new CharData(temp); }
            else { Vincent.ReadData(temp); }
        }

        //creates a Hext file
        public void CreateHextFile(string path, EXEeditor original)
        {
            try
            {
                if (Language != Language.English)
                {
                    throw new NotImplementedException("Currently unavailable for this language.");
                }

                using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.WriteLine(@"..\ff7.exe");
                        writer.WriteLine($"# {Language.GetName(typeof(Language), Language)} version");
                        writer.WriteLine();

                        //compare names
                        bool checker = false;
                        for (int i = 0; i < NUM_NAMES; ++i)
                        {
                            string name1 = Names[i].GetName(),
                                name2 = original.Names[i].GetName();

                            if (name1 != name2)
                            {
                                checker = true;
                                writer.WriteLine($"# {name2} -> {name1}");
                                var temp = Names[i].GetByteArray();
                                long pos = NAME_DATA_POS + HEXT_OFFSET + (temp.Length * i);
                                writer.Write($"{pos:X2} = ");
                                foreach (var x in temp)
                                {
                                    writer.Write($"{x:X2} ");
                                }
                                writer.WriteLine();
                            }
                        }
                        if (checker) { writer.WriteLine(); }

                        //compare Cait Sith's data
                        byte[] temp1 = CaitSith.GetByteArray(),
                            temp2 = original.CaitSith.GetByteArray();
                        bool diff = false;
                        checker = false;

                        writer.WriteLine("# Cait Sith's initial data");
                        for (int i = 0; i < CharData.CHAR_DATA_LENGTH; ++i)
                        {
                            if (temp1[i] != temp2[i])
                            {
                                checker = true;
                                if (!diff)
                                {
                                    long pos = CAIT_SITH_DATA_POS + HEXT_OFFSET + i;
                                    writer.Write($"{pos:X2} = ");
                                    diff = true;
                                }
                                writer.Write($"{temp1[i]:X2} ");
                            }
                            else
                            {
                                if (diff)
                                {
                                    writer.WriteLine();
                                    diff = false;
                                }
                            }
                        }
                        if (!checker) { writer.WriteLine("# No changes"); }
                        writer.WriteLine();

                        //compare Vincent's data
                        temp1 = Vincent.GetByteArray();
                        temp2 = original.Vincent.GetByteArray();
                        diff = false;
                        checker = false;

                        writer.WriteLine("# Vincent's initial data");
                        for (int i = 0; i < CharData.CHAR_DATA_LENGTH; ++i)
                        {
                            if (temp1[i] != temp2[i])
                            {
                                checker = true;
                                if (!diff)
                                {
                                    long pos = CAIT_SITH_DATA_POS + HEXT_OFFSET + CharData.CHAR_DATA_LENGTH + i;
                                    writer.Write($"{pos:X2} = ");
                                    diff = true;
                                }
                                writer.Write($"{temp1[i]:X2} ");
                            }
                            else
                            {
                                if (diff)
                                {
                                    writer.WriteLine();
                                    diff = false;
                                }
                            }
                        }
                        if (!checker) { writer.WriteLine("# No changes"); }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new IOException("There was a problem writing the file.", ex);
            }
        }
    }
}
