using System.Collections.Generic;

namespace AI_Hospital.model
{
    class Virus : Disease, IInfected
    {
        public Virus(string name, List<Symptom> symptoms) : base(name, symptoms)
        {

        }

        public void TransitionOfInfectionToAPerson(Person person)
        {
            person.Disease = this;
        }

        public override bool Equals(object obj)
        {
            var virus = obj as Virus;

            if (virus == null || !this.GetType().Equals(obj.GetType()))
                return false;

            return this.Name.Equals(virus.Name);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}
