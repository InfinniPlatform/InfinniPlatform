using InfinniPlatform.DocumentStorage;

namespace InfinniPlatform.ServiceHost.Models
{
    public class Entity : Document
    {
        public Entity()
        {
            Name = nameof(Entity);
            Digit = 1;
        }

        public string Name { get; set; }
        public int Digit { get; set; }
    }
}