namespace Ivankarez.Evolution
{
    public class GeneticAlgorithmIndividual<DNA>
    {
        public DNA Dna { get; }
        public long Generation { get;}
        public long Id { get; }
        public long SpeciesId { get; set; }
        public float Fitness { get; private set; }
        public bool IsTested { get; private set; }

        public GeneticAlgorithmIndividual(long id, DNA dna, long generation, long speciesId, float fitness, bool isTested)
        {
            Dna = dna;
            Generation = generation;
            Id = id;
            SpeciesId = speciesId;
            Fitness = fitness;
            IsTested = isTested;
        }

        public void SetFitness(float fitness)
        {
            Fitness = fitness;
            IsTested = true;
        }
    }
}
