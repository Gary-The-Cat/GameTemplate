using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GameAI.GeneticAlgorithm
{
    public class GeneticAlgorithm : IEnumerable
    {
        public int PopulationCount { get; set; } = 100;

        public int MaxNoImprovement { get; set; } = 20;

        public int MaxGenerations { get; set; } = 10000;

        public double MutationChance { get; set; } = 0.02;

        public bool MutateParentsAsChildren { get; set; } = false;

        public bool EnsureUniqueParents { get; set; } = false;

        public bool MutationEnabled { get; set; } = false;

        public bool CrossoverEnabled { get; set; }

        public Func<INeighbour> CreateIndividual { get; set; }

        public Func<INeighbour, INeighbour, INeighbour> CrossoverIndividuals { get; set; }

        private List<INeighbour> population;

        private Random random;

        public GeneticAlgorithm()
        {
            population = new List<INeighbour>();
            random = new Random();
        }

        public void StartBreeding()
        {
            this.SpawnPopulation();

            population = population.AsParallel().WithDegreeOfParallelism(100).OrderBy(i => i.GetFitness()).ToList();

            // Perform the optimization
            bool hasConverged = false;
            int noImprovement = 0;
            int generation = 0;
            double previousBestFitness = population[0].GetFitness();

            while (!hasConverged && noImprovement < MaxNoImprovement && generation < MaxGenerations)
            {
                // Perform Breeding
                DoGeneration();

                // Sort the new population
                population = population.OrderBy(i => Math.Abs(i.GetFitness())).ToList();

                // Check convergence
                hasConverged = CheckConvergence(ref previousBestFitness, ref noImprovement);

                // Increase current generation
                generation++;

                ReportProgress(generation, population.First().GetFitness());
            }
        }

        public void SpawnPopulation()
        {
            // User Sanity Checks
            if (CreateIndividual == null)
            {
                throw new Exception("The 'Create Individual' method has not been set.");
            }

            if (CrossoverIndividuals == null)
            {
                throw new Exception("The 'Crossover Operator' method has not been set.");
            }

            // Create the population
            for(int i = 0; i < PopulationCount; i++)
            {
                population.Add(CreateIndividual());
            }
        }

        public void ReplaceIndividual(INeighbour existingNeighbour, INeighbour neighbour)
        {
            population.Remove(existingNeighbour);
            population.Add(neighbour);
        }

        public List<INeighbour> PerformLocalSearch(int offspringCount)
        {
            var offspring = new List<INeighbour>();
            var parent = GetFittestIndividual();

            offspring.Add(parent);

            while(offspring.Count() < offspringCount)
            {
                var child = parent.Clone();
                child.Mutate();
                offspring.Add(child);
            }

            return offspring;
        }

        private void ReportProgress(int generation, double v)
        {
            Console.WriteLine($"Generation: {generation}, Best Fitness: {v}");
        }

        // Currently only testing no improvement for convergance
        private bool CheckConvergence(ref double previousBestFitness, ref int noImprovement)
        {
            // Check no improvement
            var currentBestFitness = population.First().GetFitness();
            if(currentBestFitness >= previousBestFitness)
            {
                noImprovement++;
            }
            else
            {
                noImprovement = 0;
            }

            previousBestFitness = currentBestFitness;
            
            return noImprovement < MaxNoImprovement;
        }

        public void DoGeneration()
        {
            population = population.OrderByDescending(p => p.GetFitness()).Take(PopulationCount).ToList();

            // Get individuals
            var newIndividuals = GetOffspring();

            population.AddRange(newIndividuals);

            foreach(var individual in population)
            {
                if(!IsUnique(individual, population))
                {
                    individual.Mutate();
                }
            }
        }

        private bool IsUnique(INeighbour individual, List<INeighbour> population)
        {
            var individualGenome = individual.Genome;

            foreach (var neighbourGenome in population.Select(p => p.Genome))
            {
                if(individualGenome == neighbourGenome)
                {
                    return false;
                }
            }

            return true;
        }

        private List<INeighbour> GetOffspring()
        {
            var newIndividuals = new List<INeighbour>();
            while (newIndividuals.Count < PopulationCount)
            {
                // Biased select two individuals from the parent population
                var (mother, father) = BiasedGetParents();

                var child = CrossoverIndividuals(mother, father);

                // Child mutation
                if (MutationEnabled && random.NextDouble() < MutationChance)
                {
                     child.Mutate();
                }

                newIndividuals.Add(child);

                // Parent mutation as new child
                if (MutateParentsAsChildren && random.NextDouble() < MutationChance)
                {
                    var mutatedParent = mother.Clone();
                    mutatedParent.Mutate();
                    newIndividuals.Add(mutatedParent);
                }
            }

            return newIndividuals;
        }

        private (INeighbour, INeighbour) BiasedGetParents()
        {
            var father = BiasedGetIndividual();
            var mother = BiasedGetIndividual();

            if (EnsureUniqueParents)
            {
                while (mother == father)
                {
                    mother = BiasedGetIndividual();
                }
            }

            return (mother, father);
        }

        private INeighbour BiasedGetIndividual()
        {
            var maxFitness = population.Max(p => p.GetFitness());
            var populationWeights = population.Select(i => i.GetFitness()/maxFitness).ToList();
            var sum = populationWeights.Sum();

            var weightedPopulationWeights = populationWeights.Select(i => i / sum).ToArray();

            var selection = random.NextDouble();
            double cumulativeTotal = 0;
            int selectedIndividual = -1;

            while(cumulativeTotal < selection)
            {
                cumulativeTotal += weightedPopulationWeights[selectedIndividual + 1];
                selectedIndividual++;
            }

            return population[selectedIndividual];
        }

        public IEnumerator GetEnumerator()
        {
            foreach(var individual in population)
            {
                yield return individual;
            }
        }

        public INeighbour GetFittestIndividual()
        {
            return population.First();
        }
    }
}
