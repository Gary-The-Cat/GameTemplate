namespace GameAI.GeneticAlgorithm
{
    public interface INeighbour
    {
        object Genome { get; set; }

        void Mutate();

        // Fitness should always be non negative, larger value = fitter individual
        double GetFitness();

        INeighbour Clone();
    }
}