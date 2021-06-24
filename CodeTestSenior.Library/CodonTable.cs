using System.Collections.Generic;

namespace CodeTestSenior.Library
{
    public class CodonTable
    {
        public HashSet<string> Starts { get; set; } = new HashSet<string>();
        public HashSet<string> Stops { get; set; } = new HashSet<string>();
        public List<CodonMap> CodonMap { get; set; } = new List<CodonMap>();
    }
}