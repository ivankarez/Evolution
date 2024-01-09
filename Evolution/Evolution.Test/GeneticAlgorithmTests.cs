using Ivankarez.Evolution;
using Ivankarez.Evolution.Utils;

namespace Evolution.Test
{
    public class GeneticAlgorithmTests
    {
        [Test]
        public void TestNotEvaluateTheSameDnaTwice()
        {
            var evaluatedIds = new HashSet<long>();
            var algo = new GeneticAlgorithm<float[]>(new TestDnaOperationProvider(), new IdSequence(), new GeneticAlgorithmOptions
            {
                InitialPopulationSize = 5,
                MaxIndividualsInSpecies = 1
            });
            for (int i = 0; i < 5; i++)
            {
                while (!algo.Population.IsAllIndividualsTested)
                {
                    var individual = algo.GetNextToTest();
                    if (evaluatedIds.Contains(individual.Id))
                    {
                        Assert.Fail("Individual is tested twice");
                    }
                    evaluatedIds.Add(individual.Id);
                    individual.SetFitness(1f);
                }
                algo.CreateNextPopulation();
            }
        }
    }
}