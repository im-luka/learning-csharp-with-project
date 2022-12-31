using System;
using System.Collections.Generic;

namespace AI_Hospital.model
{
    [Serializable]
    class Disease : EntityName
    {
        private List<Symptom> symptomsList;

        public List<Symptom> SymptomsList { get { return symptomsList; } set { symptomsList = value; } }

        public Disease(long id, string name, List<Symptom> symptoms) : base(id, name)
        {
            this.SymptomsList = symptoms;
        }

        public override bool Equals(object obj)
        {
            var disease = obj as Disease;

            if (disease == null || !this.GetType().Equals(obj.GetType()))
                return false;

            return this.Id == disease.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

    }
}
