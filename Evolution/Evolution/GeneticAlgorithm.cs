using Ivankarez.Evolution.Abstractions;
using Ivankarez.Evolution.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ivankarez.Evolution
{
    public class GeneticAlgorithm<DNA>
    {
        private static readonly Random random = new Random();

        public IdSequence IndividualIdSequence { get; }
        public bool IsAllIndividualsTested => testingQueue.Count == 0;
        public long Generation { get; private set; }
        public IReadOnlyList<GeneticAlgorithmIndividual<DNA>> Population => population;

        private readonly List<GeneticAlgorithmIndividual<DNA>> population;
        private readonly Queue<GeneticAlgorithmIndividual<DNA>> testingQueue;
        private readonly ISelectionStrategy<DNA> selectionStrategy;
        private readonly IReproductionStrategy<DNA> reproductionStrategy;

        internal GeneticAlgorithm(List<GeneticAlgorithmIndividual<DNA>> population,
                                  ISelectionStrategy<DNA> selectionStrategy,
                                  IReproductionStrategy<DNA> reproductionStrategy,
                                  IdSequence individualIdSequence)
        {
            this.population = population;
            this.selectionStrategy = selectionStrategy;
            this.reproductionStrategy = reproductionStrategy;
            IndividualIdSequence = individualIdSequence;
            testingQueue = new Queue<GeneticAlgorithmIndividual<DNA>>(population.Where(i => !i.IsTested));
        }

        public GeneticAlgorithmIndividual<DNA> GetNextToTest()
        {
            if (IsAllIndividualsTested)
            {
                throw new InvalidOperationException("All individuals are tested");
            }

            return testingQueue.Dequeue();
        }

        public void CreateNextPopulation()
        {
            if (!IsAllIndividualsTested)
            {
                throw new InvalidOperationException("Cannot create next population because not all individuals are tested");
            }

            selectionStrategy.Apply(population);
            reproductionStrategy.Apply(population);
            foreach ( var individual in population.Where(i => !i.IsTested ))
            {
                testingQueue.Enqueue(individual);
            }
            Generation++;
        }
    }
}
