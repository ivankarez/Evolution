using Ivankarez.Evolution.Utils;

namespace Ivankarez.Evolution
{
    public class GeneticAlgorithmSpecies<DNA>
    {
        public DNA Holotype { get; private set; }
        public IReadOnlyList<GeneticAlgorithmIndividual<DNA>> Individuals => individuals;

        private readonly SortedList<GeneticAlgorithmIndividual<DNA>> individuals;

        public GeneticAlgorithmSpecies(DNA holotype)
        {
            Holotype = holotype;
            individuals = new SortedList<GeneticAlgorithmIndividual<DNA>>(new FitnessBasedComparator<DNA>());

        }

        public void AddIndividual(GeneticAlgorithmIndividual<DNA> individual)
        {
            individual.Species = this;
            individuals.Add(individual);
        }

        public void RemoveIndividual(GeneticAlgorithmIndividual<DNA> individual)
        {
            individuals.Remove(individual);
        }
    }
}
