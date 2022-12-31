namespace AI_Hospital.model
{
    abstract class EntityName
    {
        private string name;

        public string Name { get { return name; } set { name = value; } }

        public EntityName(string name)
        {
            this.Name = name;
        }
    }
}
