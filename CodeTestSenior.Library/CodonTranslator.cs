using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTestSenior.Library
{

    /*  
     *  The genetic code is a set of rules by which DNA or mRNA is translated into proteins (amino acid sequences).
     *  
     *  1) Three nucleotides (or tri-nucleotide), called codons, specify which amino acid will be used.
     *  2) Since codons are defined by three nucleotides, every sequence can be read in three reading frames, depending on the starting point.
     *     The actual reading frame used for translation is determined by a start codon. 
     *     In our case, we will define the start codon to be the most commonly used ATG (in some organisms there may be other start codons).
     *  3) Translation begins with the start codon, which is translated as Methionine (abbreviated as 'M').
     *  4) Translation continues until a stop codon is encountered. There are three stop codons (TAG, TGA, TAA)
     *  
     * 
     *  Included in this project is a comma seperated value (CSV) text file with the codon translations.
     *  Each line of the file has the codon, followed by a space, then the amino acid (or start or stop)
     *  For example, the first line:
     *  CTA,L
     *  should be interpreted as: "the codon CTA is translated to the amino acid L"
     *  
     *  
     *  You should not assume that the input sequence begins with the start codon. Any nucleotides before the start codon should be ignored.
     *  You should not assume that the input sequence ends with the stop codon. Any nucleotides after the stop codon should be ignored.
     * 
     *  For example, if the input DNA sequence is GAACAAATGCATTAATACAAAAA, the output amino acid sequence is MH.
     *  GAACAA ATG CAT TAA TACAAAAA
     *         \ / \ /
     *          M   H
     *          
     *  ATG -> START -> M
     *  CAT -> H
     *  TAA -> STOP
     *  
     */


    public class CodonTranslator
    {
        private readonly string _codonTableFileName;
        private readonly CodonSerialization _codonSerialization;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="codonTableFileName">Filename of the DNA codon table.</param>
        public CodonTranslator(string codonTableFileName)
        {
            _codonTableFileName = codonTableFileName;
            _codonSerialization = new CodonSerialization(); // Normally this would be injected into the constructor
        }

        private int IndexOfFirstStart(string dna, HashSet<string> codonTableStarts)
        {
            var index = -1;
            
            // Since there is only one Start this whole thing probably isn't necessary, but since
            // the json Start input was an array, I wanted to cover my bases in case a new Start codon was added.
            
            foreach (var start in codonTableStarts)
            {
                var i = dna.IndexOf(start, StringComparison.Ordinal);
                if (i >= 0 && (index == -1 || i < index))
                {
                    index = i;
                }
            }

            return index;
        }

        /// <summary>
        /// Translates a sequence of DNA into a sequence of amino acids.
        /// </summary>
        /// <param name="dna">DNA sequence to be translated.</param>
        /// <returns>Amino acid sequence</returns>
        public string Translate(string dna)
        {
            var codonTable = _codonSerialization.Deserialize(_codonTableFileName);
            var dict = codonTable.CodonMap.ToDictionary(key => key.Codon, value => value.AminoAcid);
            
            Protein currentProtein = null;
            
            // Index of first Start codon for off balance start
            var indexOfFirstStart = IndexOfFirstStart(dna, codonTable.Starts);

            // Move by 3 to just look at a single frame
            for (var frameIndex = indexOfFirstStart; frameIndex + 3 <= dna.Length; frameIndex += 3)
            {
                // This generates a new string everytime through. I experimented with ReadOnlySpan, but HashSet.Contains
                // wouldn't like it. And again, due to time constraints I accepted this as ok for now.
                var frame = dna.Substring(frameIndex, 3);

                // Check if frame is a start codon
                if (currentProtein == null)
                {
                    if (codonTable.Starts.Contains(frame))
                    {
                        currentProtein = new Protein();
                        currentProtein.AddAminoAcid(MapAminoAcid(frame, dict));
                    }

                    continue;
                }
                
                if (codonTable.Stops.Contains(frame))
                {
                    return currentProtein.GetProtein();
                }

                currentProtein.AddAminoAcid(MapAminoAcid(frame, dict));
            }

            return string.Empty;
        }

        private char MapAminoAcid(string frame, Dictionary<string, string> map)
        {
            if (map.TryGetValue(frame, out var aminoAcid))
            {
                if (aminoAcid.Length != 1)
                {
                    throw new Exception($"Not a valid amino acid: {aminoAcid}");
                }

                return aminoAcid[0];
            }

            throw new Exception("Unable to determine amino acid");
        }
    }
}
