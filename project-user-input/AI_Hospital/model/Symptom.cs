using System;

namespace AI_Hospital.model
{
    class Symptom : EntityName
    {
        enum SymptomValue { RARE, MEDIUM, OFTEN }

        private string value;

        public string Value { get { return value; } set { this.value = value; } }

        public Symptom(string name, string value) : base(name)
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

            return this.Name.Equals(symptom.Name);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

    }
}
