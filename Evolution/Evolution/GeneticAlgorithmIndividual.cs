namespace Ivankarez.Evolution
{
    public class GeneticAlgorithmIndividual<DNA>
    {
        public DNA Dna { get; set; }
        public long Generation { get; set; }
        public long Id { get; set; }
        public float Fitness { get; private set; } = 0f;
        public bool IsTested { get; private set; } = false;
        internal GeneticAlgorithmSpecies<DNA> Species { get; set; } = null;

        public GeneticAlgorithmIndividual(DNA dna, long generation, long id)
        {
            Dna = dna;
            Generation = generation;
            Id = id;
        }

        public void SetFitness(float fitness)
        {
            Fitness = fitness;
            IsTested = true;
        }
    }
}
