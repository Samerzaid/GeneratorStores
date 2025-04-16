namespace GeneratorStores.DataAccess.Interfaces;

public interface IEntity<T>
{
    T Id { get; set; }
}

