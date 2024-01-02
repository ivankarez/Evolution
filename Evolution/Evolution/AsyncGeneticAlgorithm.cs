using Ivankarez.Evolution.Abstractions;
using Ivankarez.Evolution.Utils;

namespace Ivankarez.Evolution
{
    public class AsyncGeneticAlgorithm<DNA>
    {
        private static readonly Random random = new();

        public GeneticAlgorithmPopulation<DNA> CurrentPopulation { get; private set; }
        public IdSequence IdSequence { get; }
        public GeneticAlgorithmOptions Options { get; }
        public event Action<GeneticAlgorithmPopulation<DNA>> OnNewPopulation;

        private readonly IDnaOperationProvider<DNA> dnaOperationProvider;

        public AsyncGeneticAlgorithm(IDnaOperationProvider<DNA> dnaOperationProvider,
            IdSequence idSequence,
            GeneticAlgorithmOptions options)
        {
            this.dnaOperationProvider = dnaOperationProvider;
            IdSequence = idSequence;
            Options = options;
            CurrentPopulation = CreateFirstPopulation();
        }

        private GeneticAlgorithmPopulation<DNA> CreateFirstPopulation()
        {
            var population = new GeneticAlgorithmPopulation<DNA>(0, Options, dnaOperationProvider);
            for (int i = 0; i < Options.InitialPopulationSize; i++)
            {
                var newDna = dnaOperationProvider.CreateRandom();
                var id = IdSequence.Next();
                var newIndividual = new GeneticAlgorithmIndividual<DNA>(newDna, 0, id);
                Console.WriteLine("Create individual");
                population.AddIndividual(newIndividual);
            }

            return population;
        }

        public GeneticAlgorithmIndividual<DNA> GetNextToTest()
        {
            if (CurrentPopulation.IsAllIndividualsTested)
            {
                CreateNextPopulation();
            }

            return CurrentPopulation.GetNextToTest();
        }

        private void CreateNextPopulation()
        {
            var nextPopulation = new GeneticAlgorithmPopulation<DNA>(CurrentPopulation.Generation + 1, Options, dnaOperationProvider);

            // Clone the species to the next population
            foreach (var species in CurrentPopulation.Species)
            {
                nextPopulation.AddSpecies(new GeneticAlgorithmSpecies<DNA>(species.Holotype));
            }

            var individuals = CurrentPopulation.Species.SelectMany(s => s.Individuals)
                .Where(i => i.IsTested)
                .OrderBy(i => i, new FitnessBasedComparator<DNA>())
                .ToList();

            var speciesSizes = new Dictionary<GeneticAlgorithmSpecies<DNA>, int>();
            var index = 0;
            var survivors = new List<GeneticAlgorithmIndividual<DNA>>();
            while (survivors.Count < Options.SurvivorCount && index < individuals.Count)
            {
                var survivorCandidate = individuals[index];
                index++;
                var species = survivorCandidate.Species;
                if (speciesSizes.TryGetValue(species, out int value))
                {
                    if (value >= Options.MaxIndividualsInSpecies)
                    {
                        continue;
                    }
                }
                else
                {
                    speciesSizes[species] = 0;
                }

                speciesSizes[species]++;
                survivors.Add(survivorCandidate);
                nextPopulation.AddIndividual(survivorCandidate);
            }

            var newIndividualCount = Options.InitialPopulationSize - survivors.Count;
            for (int i = 0; i < newIndividualCount; i++)
            {
                var parent1 = survivors[random.Next(survivors.Count)];
                var parent2 = survivors[random.Next(survivors.Count)];
                var childDna = dnaOperationProvider.Crossover(parent1.Dna, parent2.Dna);
                var childDnaMutated = dnaOperationProvider.Mutate(childDna);
                var child = new GeneticAlgorithmIndividual<DNA>(childDnaMutated, nextPopulation.Generation, IdSequence.Next());
                nextPopulation.AddIndividual(child);
            }
            nextPopulation.TrimEmptySpecies();

            OnNewPopulation?.Invoke(nextPopulation);
            CurrentPopulation = nextPopulation;
        }
    }
}
