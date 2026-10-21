using Funkos.Models;

namespace Funkos.Repositories;

public interface IFunkoRepository
{
    List<Funko> GetAll();
    Funko? GetById(long id);
    Funko Create(Funko newFunko);
    Funko? Update(long id, Funko updatedFunko);
    bool Delete(long id);
  
}   