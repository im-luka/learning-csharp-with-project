using System.Collections.Generic;

namespace AI_Hospital.model
{
    class Disease : EntityName
    {
        private List<Symptom> symptomsList;

        public List<Symptom> SymptomsList { get { return symptomsList; } set { symptomsList = value; } }

        public Disease(string name, List<Symptom> symptoms) : base(name)
        {
            this.SymptomsList = symptoms;
        }

        public override bool Equals(object obj)
        {
            var disease = obj as Disease;

            if (disease == null || !this.GetType().Equals(obj.GetType()))
                return false;

            return this.Name.Equals(disease.Name);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

    }
}
