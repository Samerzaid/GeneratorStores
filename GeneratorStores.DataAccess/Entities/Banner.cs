using GeneratorStores.DataAccess.Interfaces;

namespace GeneratorStores.DataAccess.Entities;

public class Banner : IEntity<int>
{
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? SubTitle { get; set; }

        public string? ImageName { get; set; }

        public short? Priority { get; set; }

        public string? Link { get; set; }

        public string? Position { get; set; }

}


