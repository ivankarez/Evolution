using Ivankarez.Evolution.Abstractions;
using System.Collections.Generic;

namespace Ivankarez.Evolution.Utils
{
    public static class SpecificationUtils
    {
        public static GeneticAlgorithmSpecies<DNA> GetMatchingSpecies<DNA>(IList<GeneticAlgorithmSpecies<DNA>> species,
            IDnaOperationProvider<DNA> operationProvider, DNA dna, float threshold)
        {
            if (species.Count == 0)
            {
                return null;
            }

            var closestSpecies = species[0];
            var closestSimilarity = operationProvider.CalculateSimilarity(closestSpecies.Holotype, dna);
            for (int i = 1; i < species.Count; i++)
            {
                var currentSpecies = species[i];
                var currentSimilarity = operationProvider.CalculateSimilarity(currentSpecies.Holotype, dna);
                if (currentSimilarity > closestSimilarity)
                {
                    closestSpecies = currentSpecies;
                    closestSimilarity = currentSimilarity;
                }
            }

            if (closestSimilarity > threshold)
            {
                return null;
            }

            return closestSpecies;
        }
    }
}
