using System;

namespace AI_Hospital.model
{
    class County : EntityName
    {
        private int population;
        private int affectedPopulation;

        public int Population { get { return population; } set { population = value; } }
        public int AffectedPopulation { get { return affectedPopulation; } set { affectedPopulation = value; } }
        public double PercentageOfAffectedPopulation { get; set; }

        public County(string name, int population, int affectedPopulation) : base(name)
        {
            this.Population = population;
            this.AffectedPopulation = affectedPopulation;
            this.PercentageOfAffectedPopulation = ((double)AffectedPopulation / Population) * 100;
        }

        public override bool Equals(object obj)
        {
            var county = obj as County;

            if (county == null || !this.GetType().Equals(obj.GetType()))
                return false;

            return this.Name.Equals(county.Name);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}
