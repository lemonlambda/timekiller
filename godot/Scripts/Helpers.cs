using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Timekiller.Helpers {
	public static class UniqueName {
		private static HashSet<string> usedNames = new HashSet<string>();
	    private static readonly string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "qu", "r", "s", "t", "v", "w", "x", "y", "z" };
	    private static readonly string[] vowels = { "a", "e", "i", "o", "u", "y" };
	    private static readonly string[] vowelPairs = { "ae", "ai", "au", "ea", "ee", "ei", "ie", "io", "oa", "oe", "oi", "ou", "ue" };
	    private static readonly string[] consonantClusters = { "bl", "br", "cl", "cr", "dr", "fl", "fr", "gl", "gr", "pl", "pr", "sl", "sm", "sn", "sp", "st", "sw", "tr", "tw", "wh", "wr" };

	    public static string GenerateStarName(int minLength = 5, int maxLength = 9)
	    {
	        Random rand = new Random();
	        int length = rand.Next(minLength, maxLength + 1);
	        bool useConsonant = rand.Next(0, 2) == 0;
	        string name = "";

	        while (name.Length < length)
	        {
	            if (useConsonant && rand.NextDouble() < 0.3 && name.Length <= length - 2)
	            {
	                name += consonantClusters[rand.Next(consonantClusters.Length)];
	            }
	            else if (!useConsonant && rand.NextDouble() < 0.4 && name.Length <= length - 2)
	            {
	                name += vowelPairs[rand.Next(vowelPairs.Length)];
	            }
	            else
	            {
	                name += useConsonant ? consonants[rand.Next(consonants.Length)] : vowels[rand.Next(vowels.Length)];
	            }
	            useConsonant = !useConsonant;
	        }

	        name = name.Substring(0, Math.Min(name.Length, length));
	        return char.ToUpper(name[0]) + name.Substring(1);
	    }
						
		public static string GenerateUniqueSystemName() {
		
			while (true) {
				string name = GenerateStarName();
				Random r = new Random();
				int number = r.Next(0, 99);
				string systemName = $"{name}-{number.ToString()}";
				
				if (!usedNames.Contains(systemName)) {
					usedNames.Add(systemName);
					return systemName;
				}
			}
		}
	}
}
