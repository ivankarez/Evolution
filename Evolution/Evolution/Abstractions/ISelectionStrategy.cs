using System.Collections.Generic;

namespace Ivankarez.Evolution.Abstractions
{
    public interface ISelectionStrategy<DNA>
    {
        public void Apply(List<GeneticAlgorithmIndividual<DNA>> population);
    }
}
