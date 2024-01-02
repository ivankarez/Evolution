namespace Ivankarez.Evolution.Utils
{
    public class FitnessBasedComparator<DNA> : IComparer<GeneticAlgorithmIndividual<DNA>>
    {
        public int Compare(GeneticAlgorithmIndividual<DNA> x, GeneticAlgorithmIndividual<DNA> y)
        {
            return x.Fitness.CompareTo(y.Fitness) * -1;
        }
    }
}
