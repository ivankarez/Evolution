namespace Ivankarez.Evolution.Abstractions
{
    public interface IDnaOperationProvider<DNA>
    {
        public DNA CreateRandom();
        public DNA Crossover(DNA parent1, DNA parent2);
        public DNA Mutate(DNA source);
        public float CalculateSimilarity(DNA dna1, DNA dna2);
    }
}
