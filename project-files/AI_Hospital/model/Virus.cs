using System;
using System.Collections.Generic;

namespace AI_Hospital.model
{
    [Serializable]
    class Virus : Disease, IInfected
    {
        public Virus(long id, string name, List<Symptom> symptoms) : base(id, name, symptoms)
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

            return this.Id == virus.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
