using InfinniPlatform.DocumentStorage;

namespace InfinniPlatform.ServiceHost.Models
{
    public class Entity2 : Document
    {
        public Entity2()
        {
            Name = nameof(Entity2);
            Digit = 1;
        }

        public string Name { get; set; }
        public int Digit { get; set; }
    }
}