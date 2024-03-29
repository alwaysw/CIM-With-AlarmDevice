﻿using CIMComponent;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMCasstteConvertCIM.CasstteConverter
{
    internal class clsMemoryGroupOptions
    {

        internal enum GROUP_TYPE
        {
            AGVS,
            RACK,
        }
        internal clsMemoryGroupOptions(string bitStartAddress, string bitEndAddress, string wordStartAddress, string wordEndAddress, bool IsBitHexTable = true, bool IsWordHexTable = true)
        {
            this.IsBitHexTable = IsBitHexTable;
            this.IsWordHexTable = IsWordHexTable;
            this.bitStartAddress = bitStartAddress;
            this.bitEndAddress = bitEndAddress;
            this.wordStartAddress = wordStartAddress;
            this.wordEndAddress = wordEndAddress;
            MemoryTableIni();
        }

        internal MemoryTable memoryTable;
        internal MemoryTable memoryTable_read_back;

        internal string GROUP_Name { get; set; } = "AGVS";
        internal readonly GROUP_TYPE memGoupType = GROUP_TYPE.AGVS;

        internal string bitRegionName
        {
            get
            {
                char ss = bitStartAddress.First(chr => int.TryParse(chr.ToString(), out int _v));
                var indexEndOfWordRegName = bitStartAddress.IndexOf(ss);
                return bitStartAddress.Substring(0, indexEndOfWordRegName);
            }
        }

        internal string bitStartAddress_no_region
        {
            get
            {
                return bitStartAddress.Substring(bitRegionName.Length, bitStartAddress.Length - bitRegionName.Length);
            }
        }
        internal string bitEndAddress_no_region
        {
            get
            {
                return bitEndAddress.Substring(bitRegionName.Length, bitEndAddress.Length - bitRegionName.Length);
            }
        }

        public bool IsBitHexTable { get; }
        public bool IsWordHexTable { get; }
        internal string bitStartAddress { get; private set; } = "B0000";
        internal string bitEndAddress { get; private set; } = "B00FF";

        internal string wordRegionName
        {
            get
            {
                char ss = wordStartAddress.First(chr => int.TryParse(chr.ToString(), out int _v));
                var indexEndOfWordRegName = wordStartAddress.IndexOf(ss);
                return wordStartAddress.Substring(0, indexEndOfWordRegName);
            }
        }

        internal string wordStartAddress_no_region
        {
            get
            {
                return wordStartAddress.Substring(wordRegionName.Length, wordStartAddress.Length - wordRegionName.Length);
            }
        }
        internal string wordEndAddress_no_region
        {
            get
            {
                return wordEndAddress.Substring(wordRegionName.Length, wordEndAddress.Length - wordRegionName.Length);
            }
        }
        internal string wordStartAddress { get; private set; } = "W0000";
        internal string wordEndAddress { get; private set; } = "W03CF";
        internal int bitSize
        {
            get
            {
                bitStartAddress.SplitAddress(IsBitHexTable, out _, out int startNumber, out string startStr);
                bitEndAddress.SplitAddress(IsBitHexTable, out _, out int endNumber, out startStr);
                return endNumber - startNumber + 1;
            }
        }
        internal int wordSize
        {
            get
            {
                wordStartAddress.SplitAddress(IsWordHexTable, out _, out int startNumber, out string addressNumtStr);
                wordEndAddress.SplitAddress(IsWordHexTable, out _, out int endNumber, out addressNumtStr);
                return endNumber - startNumber + 1;
            }
        }


        internal List<string> bitAddresList
        {
            get
            {
                bitStartAddress.SplitAddress(true, out string bitRegionName, out int bitStartNumber, out string addressNumtStr);
                List<string> output = new List<string>();

                for (int i = 0; i < bitSize; i++)
                {
                    string addresNumber = (bitStartNumber + i).ToString("X4");
                    string Address = string.Format("{0}{1}", bitRegionName, addresNumber);
                    output.Add(Address);
                }
                return output;
            }
        }
        internal List<string> wordAddresList
        {
            get
            {
                wordStartAddress.SplitAddress(true, out string wordRegionName, out int wordStartNumber, out string addressNumtStr);
                List<string> output = new List<string>();

                for (int i = 0; i < wordSize; i++)
                {
                    string addresNumber = (wordStartNumber + i).ToString("X4");
                    string Address = string.Format("{0}{1}", wordRegionName, addresNumber);
                    output.Add(Address);
                }
                return output;
            }
        }
        internal void MemoryTableIni()
        {
            memoryTable = new MemoryTable(bitSize, IsBitHexTable, wordSize, IsWordHexTable, 32);
            memoryTable.SetMemoryStart(bitStartAddress, wordStartAddress);

            memoryTable_read_back = new MemoryTable(bitSize, IsBitHexTable, wordSize, IsWordHexTable, 32);
            memoryTable_read_back.SetMemoryStart(bitStartAddress, wordStartAddress);

        }
    }
}
