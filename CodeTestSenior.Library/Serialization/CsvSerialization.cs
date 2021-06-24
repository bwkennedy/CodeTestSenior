using System;
using System.IO;

namespace CodeTestSenior.Library.Serialization
{
    public class CsvSerialization
    {
        public CodonTable Deserialize(string codonTableFileName)
        {
            var codonTable = new CodonTable();

            foreach (var line in File.ReadLines(codonTableFileName))
            {
                var pair = line.Split(',');

                if (pair.Length != 2)
                {
                    throw new FormatException($"{codonTableFileName} not in correct csv format");
                }

                if (pair[1].Equals("START", StringComparison.InvariantCultureIgnoreCase))
                {
                    codonTable.Starts.Add(pair[0]);
                }
                else if (pair[1].Equals("STOP", StringComparison.InvariantCultureIgnoreCase))
                {
                    codonTable.Stops.Add(pair[0]);
                }
                else if (pair[1].Length == 1)
                {
                    codonTable.CodonMap.Add(new Library.CodonMap(){Codon = pair[0], AminoAcid = pair[1]});
                }
                else
                {
                    throw new FormatException($"Unable to parse line '{line}'");
                }

            }

            return codonTable;
        }
    }
}