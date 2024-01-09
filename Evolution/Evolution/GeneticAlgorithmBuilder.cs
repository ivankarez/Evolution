using Ivankarez.Evolution.Abstractions;
using Ivankarez.Evolution.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ivankarez.Evolution
{
    public class GeneticAlgorithmBuilder<DNA>
    {
        private readonly IDnaOperationProvider<DNA> dnaOperationProvider;

        private IdSequence individualIdSequence = new IdSequence();
        private IdSequence speciesIdSequence = new IdSequence();
        private List<GeneticAlgorithmIndividual<DNA>> currentPopulation = new List<GeneticAlgorithmIndividual<DNA>>();
        private Dictionary<long, GeneticAlgorithmSpecies<DNA>> species = new Dictionary<long, GeneticAlgorithmSpecies<DNA>>();
        private GeneticAlgorithmOptions options = new GeneticAlgorithmOptions();
        private long currentGeneration = 0L;
        private bool useRandomPopulation = true;
        private bool initializeSpecies = true;


        public GeneticAlgorithmBuilder(IDnaOperationProvider<DNA> dnaOperationProvider)
        {
            this.dnaOperationProvider = dnaOperationProvider ?? throw new ArgumentNullException(nameof(dnaOperationProvider));
        }

        public GeneticAlgorithmBuilder<DNA> WithOptions(GeneticAlgorithmOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            return this;
        }

        public GeneticAlgorithm<DNA> Build()
        {
            if (useRandomPopulation)
            {
                CreateFirstPopulation();
            }
            else if (initializeSpecies)
            {
                InitializeSpecies();
            }
            CheckNoMissingSpecies();

            return new GeneticAlgorithm<DNA>(dnaOperationProvider,
                individualIdSequence,
                speciesIdSequence,
                currentPopulation,
                species,
                options,
                currentGeneration);
        }

        private void CreateFirstPopulation()
        {
            currentPopulation.Clear();
            species.Clear();
            for (int i = 0; i < options.InitialPopulationSize; i++)
            {
                var newDna = dnaOperationProvider.CreateRandom();
                var id = individualIdSequence.Next();
                var matchingSpecies = SpecificationUtils.GetMatchingSpecies(species.Values.ToList(),
                    dnaOperationProvider,
                    newDna,
                    options.SpeciesSimilarityThreshold);
                if (matchingSpecies == null)
                {
                    var newSpecies = new GeneticAlgorithmSpecies<DNA>(speciesIdSequence.Next(), newDna);
                    species.Add(newSpecies.Id, newSpecies);
                    matchingSpecies = newSpecies;
                }
                var newIndividual = new GeneticAlgorithmIndividual<DNA>(0, newDna, id, matchingSpecies.Id, 0f, false);
                currentPopulation.Add(newIndividual);
            }
        }

        private void InitializeSpecies()
        {
            foreach (var individual in currentPopulation)
            {
                var matchingSpecies = SpecificationUtils.GetMatchingSpecies(species.Values.ToList(),
                    dnaOperationProvider,
                    individual.Dna,
                    options.SpeciesSimilarityThreshold);
                if (matchingSpecies == null)
                {
                    var newSpecies = new GeneticAlgorithmSpecies<DNA>(speciesIdSequence.Next(), individual.Dna);
                    species.Add(newSpecies.Id, newSpecies);
                    matchingSpecies = newSpecies;
                }
                individual.SpeciesId = matchingSpecies.Id;
            }
        }

        private void CheckNoMissingSpecies()
        {
            foreach (var individual in currentPopulation)
            {
                if (!species.ContainsKey(individual.SpeciesId))
                {
                    throw new InvalidOperationException($"Missing species. Cannot find species with id {individual.SpeciesId}");
                }
            }
        }
    }
}
