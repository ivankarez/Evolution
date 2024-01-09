namespace Ivankarez.Evolution.Abstractions
{
    public interface IMutationStrategy<DNA>
    {
        public DNA Apply(DNA dna);
    }
}
