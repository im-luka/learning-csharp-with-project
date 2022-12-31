namespace AI_Hospital.model
{
    abstract class EntityName
    {
        private long id;
        private string name;

        public long Id { get { return id; } set { id = value; } }
        public string Name { get { return name; } set { name = value; } }

        public EntityName(long id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
