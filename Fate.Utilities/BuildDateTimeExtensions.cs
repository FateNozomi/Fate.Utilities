﻿using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fate.Utilities
{
    // Get build date and time by reading the COFF header
    // http://msdn.microsoft.com/en-us/library/ms680313
    public static class BuildDateTimeExtensions
    {
        public static DateTime GetBuildDateTime(this Assembly assembly)
        {
            var path = assembly.Location;
            if (File.Exists(path))
            {
                var buffer = new byte[Math.Max(Marshal.SizeOf(typeof(IMAGE_FILE_HEADER)), 4)];
                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    fileStream.Position = 0x3C;
                    fileStream.Read(buffer, 0, 4);
                    fileStream.Position = BitConverter.ToUInt32(buffer, 0); // COFF header offset
                    fileStream.Read(buffer, 0, 4); // "PE\0\0"
                    fileStream.Read(buffer, 0, buffer.Length);
                }

                var pinnedBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                try
                {
                    var coffHeader = (IMAGE_FILE_HEADER)Marshal.PtrToStructure(pinnedBuffer.AddrOfPinnedObject(), typeof(IMAGE_FILE_HEADER));

                    return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1) + new TimeSpan(coffHeader.TimeDateStamp * TimeSpan.TicksPerSecond));
                }
                finally
                {
                    pinnedBuffer.Free();
                }
            }

            return default(DateTime);
        }

        private struct IMAGE_FILE_HEADER
        {
            public ushort Machine;
            public ushort NumberOfSections;
            public uint TimeDateStamp;
            public uint PointerToSymbolTable;
            public uint NumberOfSymbols;
            public ushort SizeOfOptionalHeader;
            public ushort Characteristics;

            public IMAGE_FILE_HEADER(
                ushort machine,
                ushort numberOfSections,
                uint dateTimeStamp,
                uint pointerToSymbolTable,
                uint numberOfSymbols,
                ushort sizeOfOptionalHeader,
                ushort characteristics)
            {
                Machine = machine;
                NumberOfSections = numberOfSections;
                TimeDateStamp = dateTimeStamp;
                PointerToSymbolTable = pointerToSymbolTable;
                NumberOfSymbols = numberOfSymbols;
                SizeOfOptionalHeader = sizeOfOptionalHeader;
                Characteristics = characteristics;
            }
        }
    }
}
