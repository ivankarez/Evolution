using Ivankarez.Evolution.Abstractions;

namespace Evolution.Test
{
    public class TestDnaOperationProvider : IDnaOperationProvider<float[]>
    {
        public const int DnaLength = 10;
        private static readonly Random random = new();

        public float CalculateSimilarity(float[] dna1, float[] dna2)
        {
            // Calculate manhattan distance
            var distance = 0f;
            for (int i = 0; i < dna1.Length; i++)
            {
                distance += Math.Abs(dna1[i] - dna2[i]);
            }
            return distance;
        }

        public float[] CreateRandom()
        {
            var dna = new float[DnaLength];
            for (int i = 0; i < dna.Length; i++)
            {
                dna[i] = (float)random.NextDouble() * 2 - 1;
            }
            return dna;
        }

        public float[] Crossover(float[] parent1, float[] parent2)
        {
            var child = new float[DnaLength];
            for (int i = 0; i < child.Length; i++)
            {
                child[i] = random.NextDouble() < 0.5 ? parent1[i] : parent2[i];
            }
            return child;
        }

        public float[] Mutate(float[] source)
        {
            var child = new float[DnaLength];
            for (int i = 0; i < child.Length; i++)
            {
                child[i] = source[i] + (float)random.NextDouble() * 0.1f - 0.05f;
            }
            return child;
        }
    }
}
