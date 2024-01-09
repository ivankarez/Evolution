using System.Collections.Generic;

namespace Ivankarez.Evolution.Abstractions
{
    public interface ICrossoverStrategy<DNA>
    {
        public DNA Crossover(IEnumerable<GeneticAlgorithmIndividual<DNA>> suvivors);
    }
}
