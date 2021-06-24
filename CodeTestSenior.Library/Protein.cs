using System;
using System.Collections.Generic;

namespace CodeTestSenior.Library
{
    public class Protein
    {
        private readonly List<char> _aminoAcids = new List<char>();
        public void AddAminoAcid(char aminoAcid)
        {
            _aminoAcids.Add(aminoAcid);
        }

        public string GetProtein()
        {
            return new String(_aminoAcids.ToArray());
        }
    }
}