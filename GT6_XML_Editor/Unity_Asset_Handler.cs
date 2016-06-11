using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GT6_XML_Editor
{
    public class Unity_Asset_Handler
    {
        public static UInt32 magic_unity = 78; // Actually it's the metadata size but since there is no magic, I'll use this as a magic
        public static string ENCODE_TYPE = "utf-8";
        public static string ID_rail = "rail_def";
        public static string ID_scenery = "coursemaker";

        private static byte[] pre_header = new byte[] 
        {    0x00, 0x00, 0x00, 0x4E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0F,
    0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x35, 0x2E, 0x32, 0x2E,
    0x30, 0x66, 0x33, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00,
    0x00, 0x31, 0x00, 0x00, 0x00, 0x93, 0x67, 0x39, 0xF4, 0x5F, 0x26, 0xFD,
    0x8B, 0x81, 0xA4, 0xEA, 0x70, 0xF2, 0xC8, 0xCA, 0xD4, 0x01, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x31, 0x00, 0x00, 0x00,
    0x31, 0x00, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};

        public static Stream Load_Asset(string FileName)
        {
            Stream xml_file = new MemoryStream();
            using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
            {
                byte[] header = new byte[System.Runtime.InteropServices.Marshal.SizeOf(magic_unity)];
                fs.Seek(0L, SeekOrigin.Begin);
                fs.Read(header, 0, 4);
                if (BitConverter.ToUInt32(header.Reverse().ToArray(), 0) == magic_unity)
                {
                    byte[] temp = new byte[4];
                    fs.Seek(0x0C, SeekOrigin.Begin);
                    fs.Read(temp, 0, 4);
                    fs.Seek(BitConverter.ToUInt32(temp.Reverse().ToArray(), 0), SeekOrigin.Begin);
                    fs.Read(temp, 0, 4);

                    if(BitConverter.ToInt32(temp, 0) == 0x08)
                    {
                        byte[] bytes = Encoding.GetEncoding(ENCODE_TYPE).GetBytes(ID_rail);
                        byte[] array = new byte[bytes.Length];
                        System.Diagnostics.Debug.WriteLine(fs.Position);
                        fs.Read(array, 0, bytes.Length);
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            if (bytes[i] != array[i])
                            {
                                throw new Exception("File ID is invalid. \"" + Encoding.UTF8.GetString(array, 0, array.Length) + "\"");
                            }
                        }
                        fs.Read(temp, 0, 4);
                        byte[] xml = new byte[BitConverter.ToUInt32(temp, 0)];
                        fs.Read(xml, 0, BitConverter.ToInt32(temp, 0));
                        xml_file = new MemoryStream(xml);
                        return (xml_file);
                    }
                    else if(BitConverter.ToInt32(temp, 0) == 0x0B)
                    {
                        byte[] bytes = Encoding.GetEncoding(ENCODE_TYPE).GetBytes(ID_scenery);
                        byte[] array = new byte[bytes.Length];
                        fs.Read(array, 0, bytes.Length);
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            if (bytes[i] != array[i])
                            {
                                throw new Exception("File ID is invalid. \"" + Encoding.UTF8.GetString(array, 0, array.Length) + "\"");
                            }
                        }
                        fs.ReadByte(); // Don't know why there is that one extra byte for scenery files?_?
                        fs.Read(temp, 0, 4);
                        byte[] xml = new byte[BitConverter.ToUInt32(temp, 0)];
                        fs.Read(xml, 0, BitConverter.ToInt32(temp, 0));
                        xml_file = new MemoryStream(xml);
                        return (xml_file);
                    }
                    else
                    {
                        throw new Exception("File is neither rail_def nor coursemaker");
                    }
                }
                return (xml_file);
            }
        }

        public static void Save_Asset(string FileName, dialog dlg)
        {
            if (!File.Exists(FileName))
                throw new Exception("File does not exist!");
            byte[] xml_file = File.ReadAllBytes(FileName);
            using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write))
            {
                fs.Seek(0, SeekOrigin.Begin);
                fs.Write(pre_header, 0, pre_header.Length);
                fs.Seek(0x1000, SeekOrigin.Begin);
                if(dlg == dialog.rail_def_1 || dlg == dialog.rail_def_2)
                {
                    byte[] lenght = BitConverter.GetBytes(ID_rail.Length);
                    fs.Write(lenght, 0, lenght.Length);
                    byte[] bytes = Encoding.GetEncoding(ENCODE_TYPE).GetBytes(ID_rail);
                    fs.Write(bytes, 0, bytes.Length);
                    byte[] _lenght = BitConverter.GetBytes(xml_file.Length);
                    fs.Write(_lenght, 0, _lenght.Length);
                    fs.Write(xml_file, 0, xml_file.Length);

                    // Let's fix the lengths
                    fs.Seek(0x04, SeekOrigin.Begin);
                    fs.Write(BitConverter.GetBytes((Int32)fs.Length).Reverse().ToArray(), 0, 4);
                    fs.Seek(0x4C, SeekOrigin.Begin);
                    fs.Write(BitConverter.GetBytes((Int32)(fs.Length - 0x1000)), 0, 4);
                }
                else if (dlg == dialog.coursemaker_1 || dlg == dialog.coursemaker_2)
                {
                    byte[] lenght = BitConverter.GetBytes(ID_scenery.Length);
                    fs.Write(lenght, 0, lenght.Length);
                    byte[] bytes = Encoding.GetEncoding(ENCODE_TYPE).GetBytes(ID_scenery);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.WriteByte(0);
                    byte[] _lenght = BitConverter.GetBytes(xml_file.Length);
                    fs.Write(_lenght, 0, _lenght.Length);
                    fs.Write(xml_file, 0, xml_file.Length);

                    // Let's fix the lengths
                    fs.Seek(0x04, SeekOrigin.Begin);
                    fs.Write(BitConverter.GetBytes((Int32)fs.Length).Reverse().ToArray(), 0, 4);
                    fs.Seek(0x4C, SeekOrigin.Begin);
                    fs.Write(BitConverter.GetBytes((Int32)(fs.Length - 0x1000)), 0, 4);
                }
            }
        }
    }
}
