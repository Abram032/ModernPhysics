using System;
using System.Collections.Generic;
using System.Linq;

namespace ModernPhysics.Web.Utils
{
    public class CharacterParser : ICharacterParser
    {
        private List<char> polishChars = new List<char> { 'ą', 'ę', 'ó', 'ś', 'ł', 'ż', 'ź', 'ć', 'ń', 'Ą', 'Ę', 'Ó', 'Ś', 'Ł', 'Ż', 'Ź', 'Ć', 'Ń' };
        private List<char> generalChars = new List<char> { 'a', 'e', 'o', 's', 'l', 'z', 'z', 'c', 'n', 'A', 'E', 'O', 'S', 'L', 'Z', 'Z', 'C', 'N' };
        public string ParsePolishChars(string str) =>
            new String(
                str.Select(c => 
                    (polishChars.Contains(c)) ? 
                    generalChars[polishChars.FindIndex(i => i.Equals(c))] : c)
                    .ToArray()
            );
    }
}