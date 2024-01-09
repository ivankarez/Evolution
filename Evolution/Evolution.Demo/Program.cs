using Ivankarez.Evolution.Abstractions;

namespace Ivankarez.Evolution.Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var dnaProvider = new DnaOperationProvider();
            var target = dnaProvider.CreateRandom();

            var options = new GeneticAlgorithmOptions
            {
                InitialPopulationSize = 500,
                SpeciesSimilarityThreshold = .7f,
                MaxIndividualsInSpecies = 10,
            };
            var gaBuilder = new GeneticAlgorithmBuilder<float[]>(dnaProvider);
            var geneticAlgorithm = gaBuilder.WithOptions(options).Build();
            
            while(geneticAlgorithm.Generation < 5000)
            {
                while (!geneticAlgorithm.IsAllIndividualsTested)
                {
                    var nextToTest = geneticAlgorithm.GetNextToTest();
                    var fitness = -MathF.Min(10, dnaProvider.CalculateSimilarity(nextToTest.Dna, target));
                    nextToTest.SetFitness(fitness);
                }
                var currentPopulation = geneticAlgorithm.Population;
                var ordered = currentPopulation.OrderByDescending(i => i.Fitness).ToList();
                Console.Clear();
                Console.WriteLine($"-----Summary of Gen {geneticAlgorithm.Generation}-----");
                Console.WriteLine($"  Best: {ordered[0].Fitness:f4}");
                Console.WriteLine($"  Average: {ordered.Average(i => i.Fitness):f4}");
                Console.WriteLine($"  Worst: {ordered[^1].Fitness:f4}");
                Console.WriteLine($"  Species: {geneticAlgorithm.Species.Count}");
                geneticAlgorithm.CreateNextPopulation();
            }

            Console.WriteLine("Finished");
        }
    }

    class DnaOperationProvider : IDnaOperationProvider<float[]>
    {
        private readonly Random random = new();

        public float[] CreateRandom()
        {
            var size = 100;
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
                if (random.NextDouble() < 0.01)
                {
                    mutatedDna[i] = dna[i] + (float)(random.NextDouble() * .2 - .1);
                }
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

            return similarity / dna1.Length;
        }
    }
}
