using Ivankarez.Evolution.Abstractions;

namespace Ivankarez.Evolution
{
    public class GeneticAlgorithmPopulation<DNA>
    {
        public int Generation { get; private set; }
        public GeneticAlgorithmOptions Options { get; }
        public bool IsAllIndividualsTested => individualsToTest.Count == 0;
        public IReadOnlyList<GeneticAlgorithmSpecies<DNA>> Species => species;

        private readonly List<GeneticAlgorithmSpecies<DNA>> species;
        private readonly Queue<GeneticAlgorithmIndividual<DNA>> individualsToTest;
        private readonly IDnaOperationProvider<DNA> dnaOperationProvider;

        public GeneticAlgorithmPopulation(int generation,
            GeneticAlgorithmOptions options,
            IDnaOperationProvider<DNA> dnaOperationProvider)
        {
            Generation = generation;
            Options = options;
            this.dnaOperationProvider = dnaOperationProvider;
            species = new List<GeneticAlgorithmSpecies<DNA>>();
            individualsToTest = new Queue<GeneticAlgorithmIndividual<DNA>>();
        }

        public void AddIndividual(GeneticAlgorithmIndividual<DNA> individual)
        {
            var species = GetSpeciesForDna(individual.Dna);
            species.AddIndividual(individual);
            if (!individual.IsTested)
            {
                individualsToTest.Enqueue(individual);
            }
        }

        public GeneticAlgorithmSpecies<DNA> GetSpeciesForDna(DNA dna)
        {
            GeneticAlgorithmSpecies<DNA> closestSpecies = null;
            var bestSimilarity = float.MaxValue;

            foreach (var species in species)
            {
                var similarity = dnaOperationProvider.CalculateSimilarity(species.Holotype, dna);
                if (similarity < bestSimilarity)
                {
                    closestSpecies = species;
                    bestSimilarity = similarity;
                }
            }

            if (bestSimilarity < Options.SpeciesSimilarityThreshold)
            {
                return closestSpecies;
            }

            var newSpecies = new GeneticAlgorithmSpecies<DNA>(dna);
            species.Add(newSpecies);

            return newSpecies;
        }

        public void TrimEmptySpecies()
        {
            var emptySpecies = species.Where(s => s.Individuals.Count == 0).ToList();
            foreach (var species in emptySpecies)
            {
                this.species.Remove(species);
            }
        }

        public void AddSpecies(GeneticAlgorithmSpecies<DNA> species)
        {
            this.species.Add(species);
        }

        public GeneticAlgorithmIndividual<DNA> GetNextToTest()
        {
            return individualsToTest.Dequeue();
        }
    }
}
