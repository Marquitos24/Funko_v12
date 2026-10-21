using Funkos.Models;

namespace Funkos.Repositories;

public class FunkoRepository : IFunkoRepository
{

    private static List<Funko> _funkos = new();
    private static long _nextId = 1;
    
    public List<Funko> GetAll()
    {
        return _funkos.ToList();
    }

    public Funko? GetById(long id)
    {
        return _funkos.FirstOrDefault(f => f.Id == id);
    }

    public Funko Create(Funko newFunko)
    {
        var funko = new Funko
        {
            Id = _nextId++,
            Name = newFunko.Name,
            Precio = newFunko.Precio,
            Categoria = newFunko.Categoria,
            CreateAt = DateTime.UtcNow
        };
        _funkos.Add(funko);
        return funko;
    }

    public Funko? Update(long id, Funko updatedFunko)
    {
        var funko = GetById(id);

        if (funko is null) return null;
        
        funko.Name = updatedFunko.Name;
        funko.Precio = updatedFunko.Precio;
        funko.Categoria = updatedFunko.Categoria;
        funko.UpdateAt = DateTime.UtcNow;
        
        return funko;
    }
    
    public bool Delete(long id)
    {
        var funko = GetById(id);
        if (funko is null) return false;

        _funkos.Remove(funko);
        return true;
    }
    
    //Para testear
    public static void ResetState()
    {
        _funkos.Clear();
        _nextId = 1;
    }
    
}