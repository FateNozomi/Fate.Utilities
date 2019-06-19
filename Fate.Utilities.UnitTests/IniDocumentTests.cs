using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;

namespace Fate.Utilities.UnitTests
{
    [TestFixture]
    public class IniDocumentTests
    {
        [Test]
        public void Load_ValidDocument_ValidSections()
        {
            IniDocument document = new FakeIniDocument();

            document.Load(CreateIniString());

            var expected = GetIniConfiguration();
            var actual = ((FakeIniDocument)document).GetConfiguration();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Save_ValidDocument_ValidSections()
        {
            IniDocument document = new FakeIniDocument();

            ((FakeIniDocument)document).SetConfiguration(GetIniConfiguration());
            document.Save(Arg.Any<string>());

            var expected = CreateIniString();
            var actual = ((FakeIniDocument)document).SavedIni;
            Assert.AreEqual(expected, actual);
        }

        [TestCase("Section", "key", null, "value")]
        [TestCase("InvalidSection", "key", null, null)]
        [TestCase("InvalidSection", "key", "value", "value")]
        public void ReadString_ValidDocument_ReturnsValidString(string section, string key, string defaultValue, string expected)
        {
            IniDocument document = new FakeIniDocument();
            document.Load(CreateIniString());

            string actual = document.ReadString(section, key, defaultValue);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WriteString_ValidString_WritesToDocument()
        {
            IniDocument document = new FakeIniDocument();

            document.WriteString("Section", "key", "value");
            document.Save(Arg.Any<string>());

            var expected = "[Section]\r\nkey=value\r\n\r\n";
            var actual = ((FakeIniDocument)document).SavedIni;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WriteString_MultipleSections_WritesToDocument()
        {
            IniDocument document = new FakeIniDocument();

            document.WriteString("Section", "key", "value");
            document.WriteString("Section", "test", "output");
            document.WriteString("Section9", "Lieutenant", "Daisuke Aramaki");
            document.WriteString("Section9", "Major", "Motoko Kusanagi");
            document.Save(Arg.Any<string>());

            var expected =
                "[Section]\r\n" +
                "key=value\r\n" +
                "test=output\r\n" +
                "\r\n" +
                "[Section9]\r\n" +
                "Lieutenant=Daisuke Aramaki\r\n" +
                "Major=Motoko Kusanagi\r\n" +
                "\r\n";
            var actual = ((FakeIniDocument)document).SavedIni;
            Assert.AreEqual(expected, actual);
        }

        [TestCase("Section", "byte", 0, 255)]
        [TestCase("InvalidSection", "key", 255, 255)]
        [TestCase("InvalidSection", "key", 1, 1)]
        public void ReadByte_ValidDocument_ReturnsValidByte(string section, string key, byte defaultValue, byte expected)
        {
            IniDocument document = new FakeIniDocument();
            document.Load(CreateIniString());

            byte actual = document.ReadByte(section, key, defaultValue);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WriteByte_ValidByte_WritesToDocument()
        {
            IniDocument document = new FakeIniDocument();

            document.WriteByte("Section", "byte", 255);
            document.Save(Arg.Any<string>());

            var expected = "[Section]\r\nbyte=255\r\n\r\n";
            var actual = ((FakeIniDocument)document).SavedIni;
            Assert.AreEqual(expected, actual);
        }

        [TestCase("Section", "sbyte", 0, -128)]
        [TestCase("InvalidSection", "key", -128, -128)]
        [TestCase("InvalidSection", "key", 127, 127)]
        public void ReadSByte_ValidDocument_ReturnsValidSByte(string section, string key, sbyte defaultValue, sbyte expected)
        {
            IniDocument document = new FakeIniDocument();
            document.Load(CreateIniString());

            sbyte actual = document.ReadSByte(section, key, defaultValue);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WriteSByte_ValidSByte_WritesToDocument()
        {
            IniDocument document = new FakeIniDocument();

            document.WriteSByte("Section", "sbyte", -128);
            document.Save(Arg.Any<string>());

            var expected = "[Section]\r\nsbyte=-128\r\n\r\n";
            var actual = ((FakeIniDocument)document).SavedIni;
            Assert.AreEqual(expected, actual);
        }

        [TestCase("Section", "short", 0, -32768)]
        [TestCase("InvalidSection", "key", -32768, -32768)]
        [TestCase("InvalidSection", "key", 32767, 32767)]
        public void ReadShort_ValidDocument_ReturnsValidShort(string section, string key, short defaultValue, short expected)
        {
            IniDocument document = new FakeIniDocument();
            document.Load(CreateIniString());

            short actual = document.ReadShort(section, key, defaultValue);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WriteShort_ValidShort_WritesToDocument()
        {
            IniDocument document = new FakeIniDocument();

            document.WriteShort("Section", "short", -32768);
            document.Save(Arg.Any<string>());

            var expected = "[Section]\r\nshort=-32768\r\n\r\n";
            var actual = ((FakeIniDocument)document).SavedIni;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadUShort_ValidDocument_ReturnsValidUShort()
        {
            IniDocument document = new FakeIniDocument();
            document.Load(CreateIniString());

            ushort actual = document.ReadUShort("Section", "ushort", 0);

            Assert.AreEqual(65535, actual);
        }

        [Test]
        public void WriteUShort_ValidUShort_WritesToDocument()
        {
            IniDocument document = new FakeIniDocument();

            document.WriteUShort("Section", "ushort", 65535);
            document.Save(Arg.Any<string>());

            var expected = "[Section]\r\nushort=65535\r\n\r\n";
            var actual = ((FakeIniDocument)document).SavedIni;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadInt_ValidDocument_ReturnsValidInt()
        {
            IniDocument document = new FakeIniDocument();
            document.Load(CreateIniString());

            int actual = document.ReadInt("Section", "int", 0);

            Assert.AreEqual(-2147483648, actual);
        }

        [Test]
        public void WriteInt_ValidInt_WritesToDocument()
        {
            IniDocument document = new FakeIniDocument();

            document.WriteInt("Section", "int", -2147483648);
            document.Save(Arg.Any<string>());

            var expected = "[Section]\r\nint=-2147483648\r\n\r\n";
            var actual = ((FakeIniDocument)document).SavedIni;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadUInt_ValidDocument_ReturnsValidUInt()
        {
            IniDocument document = new FakeIniDocument();
            document.Load(CreateIniString());

            uint actual = document.ReadUInt("Section", "uint", 0);

            Assert.AreEqual(4294967295, actual);
        }

        [Test]
        public void WriteUInt_ValidUInt_WritesToDocument()
        {
            IniDocument document = new FakeIniDocument();

            document.WriteUInt("Section", "uint", 4294967295);
            document.Save(Arg.Any<string>());

            var expected = "[Section]\r\nuint=4294967295\r\n\r\n";
            var actual = ((FakeIniDocument)document).SavedIni;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadFloat_ValidDocument_ReturnsValidFloat()
        {
            IniDocument document = new FakeIniDocument();
            document.Load(CreateIniString());

            double actual = document.ReadDouble("Section", "float", 0);

            Assert.AreEqual(3.141592, actual);
        }

        [Test]
        public void WriteFloat_ValidFloat_WritesToDocument()
        {
            IniDocument document = new FakeIniDocument();

            document.WriteDouble("Section", "float", 3.141592);
            document.Save(Arg.Any<string>());

            var expected = "[Section]\r\nfloat=3.141592\r\n\r\n";
            var actual = ((FakeIniDocument)document).SavedIni;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadDouble_ValidDocument_ReturnsValidDouble()
        {
            IniDocument document = new FakeIniDocument();
            document.Load(CreateIniString());

            double actual = document.ReadDouble("Section", "double", 0);

            Assert.AreEqual(3.14159265359, actual);
        }

        [Test]
        public void WriteDouble_ValidDouble_WritesToDocument()
        {
            IniDocument document = new FakeIniDocument();

            document.WriteDouble("Section", "double", 3.14159265359);
            document.Save(Arg.Any<string>());

            var expected = "[Section]\r\ndouble=3.14159265359\r\n\r\n";
            var actual = ((FakeIniDocument)document).SavedIni;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadDecimal_ValidDocument_ReturnsValidDecimal()
        {
            IniDocument document = new FakeIniDocument();
            document.Load(CreateIniString());

            decimal actual = document.ReadDecimal("Section", "decimal", 0);

            Assert.AreEqual(-99.999, actual);
        }

        [Test]
        public void WriteDecimal_ValidDecimal_WritesToDocument()
        {
            IniDocument document = new FakeIniDocument();

            document.WriteDecimal("Section", "decimal", -99.999m);
            document.Save(Arg.Any<string>());

            var expected = "[Section]\r\ndecimal=-99.999\r\n\r\n";
            var actual = ((FakeIniDocument)document).SavedIni;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadBool_ValidDocument_ReturnsValidBool()
        {
            IniDocument document = new FakeIniDocument();
            document.Load(CreateIniString());

            bool actual = document.ReadBool("Section", "bool", false);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void WriteBool_ValidBool_WritesToDocument()
        {
            IniDocument document = new FakeIniDocument();

            document.WriteBool("Section", "bool", true);
            document.Save(Arg.Any<string>());

            var expected = "[Section]\r\nbool=True\r\n\r\n";
            var actual = ((FakeIniDocument)document).SavedIni;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetSections_ValidDocument_ReturnsValidSections()
        {
            IniDocument document = new FakeIniDocument();
            document.Load(CreateIniString());

            var actual = document.GetSections();

            var expected = new List<string> { "Section", "General" };
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetSectionCount_ValidDocument_ReturnsValidCount()
        {
            IniDocument document = new FakeIniDocument();
            document.Load(CreateIniString());

            var actual = document.GetSectionCount();

            Assert.AreEqual(2, actual);
        }

        [TestCase("General", 4)]
        [TestCase("Section", 11)]
        public void GetPropertiesCount_ValidDocument_ValidatesCount(string section, int expected)
        {
            IniDocument document = new FakeIniDocument();
            document.Load(CreateIniString());

            var actual = document.GetPropertiesCount(section);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RemoveSection_ValidSection_RemovesSection()
        {
            IniDocument document = new FakeIniDocument();
            document.Load(CreateIniString());

            document.RemoveSection("Section");
            document.Save(Arg.Any<string>());

            var expected =
                "[General]\r\n" +
                "MagazineName=magazine.mgd\r\n" +
                "MagazinePath=C:\\magazine.mgd\r\n" +
                "LeadFrameName=leadframe.lfd\r\n" +
                "LeadFramePath=C:\\leadframe.lfd\r\n" +
                "\r\n";
            var actual = ((FakeIniDocument)document).SavedIni;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SortIni()
        {
            IniDocument document = new FakeIniDocument();
            document.Load(CreateIniString());

            document.SortIni();
            document.Save(Arg.Any<string>());

            var expected = ((FakeIniDocument)document).SavedIni;
            var actual = CreateSortedIniString();
            Assert.AreEqual(expected, actual);
        }

        private string CreateIniString()
        {
            string stub =
                "[Section]\r\n" +
                "key=value\r\n" +
                "byte=255\r\n" +
                "sbyte=-128\r\n" +
                "short=-32768\r\n" +
                "ushort=65535\r\n" +
                "int=-2147483648\r\n" +
                "uint=4294967295\r\n" +
                "float=3.141592\r\n" +
                "double=3.14159265359\r\n" +
                "decimal=-99.999\r\n" +
                "bool=True\r\n" +
                "\r\n" +
                "[General]\r\n" +
                "MagazineName=magazine.mgd\r\n" +
                "MagazinePath=C:\\magazine.mgd\r\n" +
                "LeadFrameName=leadframe.lfd\r\n" +
                "LeadFramePath=C:\\leadframe.lfd\r\n" +
                "\r\n";
            return stub;
        }

        private string CreateSortedIniString()
        {
            string stub =
                "[General]\r\n" +
                "LeadFrameName=leadframe.lfd\r\n" +
                "LeadFramePath=C:\\leadframe.lfd\r\n" +
                "MagazineName=magazine.mgd\r\n" +
                "MagazinePath=C:\\magazine.mgd\r\n" +
                "\r\n" +
                "[Section]\r\n" +
                "bool=True\r\n" +
                "byte=255\r\n" +
                "decimal=-99.999\r\n" +
                "double=3.14159265359\r\n" +
                "float=3.141592\r\n" +
                "int=-2147483648\r\n" +
                "key=value\r\n" +
                "sbyte=-128\r\n" +
                "short=-32768\r\n" +
                "uint=4294967295\r\n" +
                "ushort=65535\r\n" +
                "\r\n";
            return stub;
        }

        private Dictionary<string, Dictionary<string, string>> GetIniConfiguration()
        {
            var config = new Dictionary<string, Dictionary<string, string>>();
            var properties = new Dictionary<string, string>();
            properties.Add("key", "value");
            properties.Add("byte", "255");
            properties.Add("sbyte", "-128");
            properties.Add("short", "-32768");
            properties.Add("ushort", "65535");
            properties.Add("int", "-2147483648");
            properties.Add("uint", "4294967295");
            properties.Add("float", "3.141592");
            properties.Add("double", "3.14159265359");
            properties.Add("decimal", "-99.999");
            properties.Add("bool", "True");
            config.Add("Section", properties);

            properties = new Dictionary<string, string>();
            properties.Add("MagazineName", "magazine.mgd");
            properties.Add("MagazinePath", "C:\\magazine.mgd");
            properties.Add("LeadFrameName", "leadframe.lfd");
            properties.Add("LeadFramePath", "C:\\leadframe.lfd");
            config.Add("General", properties);
            return config;
        }
    }

    public class FakeIniDocument : IniDocument
    {
        public string SavedIni { get; private set; }

        public override void Load(string ini)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(ini)))
            {
                stream.Seek(0, SeekOrigin.Begin);
                ReadFile(stream);
            }
        }

        public override void Save(string filePath)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Seek(0, SeekOrigin.Begin);
                WriteFile(stream);

                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream))
                {
                    SavedIni = reader.ReadToEnd();
                }
            }
        }

        public Dictionary<string, Dictionary<string, string>> GetConfiguration()
        {
            return Sections;
        }

        public void SetConfiguration(Dictionary<string, Dictionary<string, string>> config)
        {
            Sections = config;
        }
    }
}
