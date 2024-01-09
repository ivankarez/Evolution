using System.Collections.Generic;

namespace Ivankarez.Evolution.Abstractions
{
    public interface IReproductionStrategy<DNA>
    {
        public void Apply(List<GeneticAlgorithmIndividual<DNA>> population);
    }
}
