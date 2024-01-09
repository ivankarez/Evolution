namespace Ivankarez.Evolution
{
    public class GeneticAlgorithmOptions
    {
        public int InitialPopulationSize { get; set; } = 100;
        public float SurvivorRatio { get; set; } = 0.5f;
        public float SpeciesSimilarityThreshold { get; set; } = 0.3f;
        public int MaxIndividualsInSpecies { get; set; } = 10;
    }
}
