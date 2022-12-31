using System;

namespace AI_Hospital.model
{
    [Serializable]
    class Symptom : EntityName
    {
        enum SymptomValue { RARE, MEDIUM, OFTEN }

        private string value;

        public string Value { get { return value; } set { this.value = value; } }

        public Symptom(long id, string name, string value) : base(id, name)
        {
            bool isValueCorrect = false;
            foreach(string val in Enum.GetNames(typeof(SymptomValue)))
            {
                if(val.Equals(value.ToUpper()))
                {
                    isValueCorrect = true;
                    this.Value = val;
                }
            }
            if (!isValueCorrect)
                this.Value = "Unknown value";
        }

        public override bool Equals(object obj)
        {
            var symptom = obj as Symptom;

            if (symptom == null || !this.GetType().Equals(obj.GetType()))
                return false;

            return this.Id == symptom.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

    }
}
