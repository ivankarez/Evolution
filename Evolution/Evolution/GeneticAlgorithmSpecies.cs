namespace Ivankarez.Evolution
{
    public class GeneticAlgorithmSpecies<DNA>
    {
        public DNA Holotype { get; }
        public long Id { get; }

        public GeneticAlgorithmSpecies(long id, DNA holotype)
        {
            Id = id;
            Holotype = holotype;
        }
    }
}
