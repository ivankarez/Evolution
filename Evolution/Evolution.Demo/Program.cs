using Ivankarez.Evolution.Abstractions;
using Ivankarez.Evolution.Utils;

namespace Ivankarez.Evolution.Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var target = 0.245f;

            var geneticAlgorithm = new AsyncGeneticAlgorithm<float[]>(new DnaOperationProvider(),
                new IdSequence(),
                new GeneticAlgorithmOptions
                {
                    InitialPopulationSize = 50,
                    SurvivorCount = 15,
                    SpeciesSimilarityThreshold = 3,
                    MaxIndividualsInSpecies = 3,
                });
            geneticAlgorithm.OnNewPopulation += (_) =>
            {
                var currentPopulation = geneticAlgorithm.CurrentPopulation;
                var individuals = currentPopulation.Species.SelectMany(s => s.Individuals);
                var bestIndividual = individuals.OrderByDescending(i => i.Fitness).First();
                Console.WriteLine($"-----Summary of Gen {currentPopulation.Generation}-----");
                Console.WriteLine($"  Best: {bestIndividual.Fitness:f4}");
                Console.WriteLine($"  Average: {individuals.Average(i => i.Fitness):f4}");
                Console.WriteLine($"  Species: {currentPopulation.Species.Count}");
                foreach (var species in currentPopulation.Species)
                {
                    Console.WriteLine($"    Max: {species.Individuals[^1].Fitness:f4}, Count: {species.Individuals.Count}");
                }
            };
            
            while(geneticAlgorithm.CurrentPopulation.Generation < 200)
            {
                var nextToTest = geneticAlgorithm.GetNextToTest();
                var fitness = -MathF.Abs(nextToTest.Dna.Sum() - target);
                nextToTest.SetFitness(fitness);
            }

            Console.WriteLine("Finished");
        }
    }

    class DnaOperationProvider : IDnaOperationProvider<float[]>
    {
        private readonly Random random = new();

        public float[] CreateRandom()
        {
            var size = 10;
            var dna = new float[size];
            for (int i = 0; i < size; i++)
            {
                dna[i] = (float)(random.NextDouble() * 2 - 1);
            }

            return dna;
        }

        public float[] Mutate(float[] dna)
        {
            var mutatedDna = new float[dna.Length];
            for (int i = 0; i < dna.Length; i++)
            {
                mutatedDna[i] = dna[i] + (float)(random.NextDouble() * 2 - 1);
            }

            return mutatedDna;
        }

        public float[] Crossover(float[] dna1, float[] dna2)
        {
            var crossoverPoint = random.Next(0, dna1.Length);
            var crossoverDna = new float[dna1.Length];
            for (int i = 0; i < crossoverPoint; i++)
            {
                crossoverDna[i] = dna1[i];
            }
            for (int i = crossoverPoint; i < dna1.Length; i++)
            {
                crossoverDna[i] = dna2[i];
            }

            return crossoverDna;
        }

        public float CalculateSimilarity(float[] dna1, float[] dna2)
        {
            var similarity = 0f;
            for (int i = 0; i < dna1.Length; i++)
            {
                similarity += MathF.Abs(dna1[i] - dna2[i]);
            }

            return similarity;
        }
    }
}
