using Funkos.DbContext;
using Funkos.Mongo;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using ProveedorModel = Funkos.Models.Proveedor;

namespace Funkos.Repositories.Proveedor;

public class ProveedorRepository(MongoDbContext context) : IProveedorRepository
{
    private readonly IMongoCollection<ProveedorModel> _collection =  context.Proveedores;
    
    public async Task<IEnumerable<ProveedorModel>>  FindAllAsync(CancellationToken cancellationToken = default)
    {
        return await _collection.Find(FiltroActivos).ToListAsync(cancellationToken);
    }

    public async Task<ProveedorModel?> FindByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        //Eq = Equals                                                                                       & = porque NO combinamos 2 valores, si no que unimos condiciones
        var filtro = Builders<ProveedorModel>.Filter.Eq(p => p.Id, id) & FiltroActivos;

        return await _collection.Find(filtro).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ProveedorModel> CreateAsync(ProveedorModel newProv, CancellationToken cancellationToken = default)
    { 
        newProv.CreateAt = DateTime.UtcNow;
        await _collection.InsertOneAsync(newProv,  cancellationToken: cancellationToken);
       // NO es necesario un SaveChangesAsync()
       return newProv;
    }

    public async Task<ProveedorModel?> UpdateAsync(ObjectId id, ProveedorModel updatedProv, CancellationToken cancellationToken = default)
    {
        updatedProv.UpdateAt= DateTime.UtcNow;
        var filtro = Builders<ProveedorModel>.Filter.Eq(p => p.Id, id) & FiltroActivos;
        var result = await _collection.ReplaceOneAsync(filtro, updatedProv,  cancellationToken: cancellationToken);
        return result.IsAcknowledged ? updatedProv : null;
        // IsAcknowledged = Indica si se ha confirmado que ha recibido y reconocido la operación de escritura.
    }
    
    public async Task<bool> DeleteAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var update = Builders<ProveedorModel>.Update
            .Set(p => p.IsDeleted, true)
            .Set(p => p.DeleteAt, DateTime.UtcNow);
        //set = Asignar un valor a un campo del documento
        
        var filtro = Builders<ProveedorModel>.Filter.Eq(p => p.Id, id) & FiltroActivos;
        
        var result = await _collection.UpdateOneAsync(filtro, update, cancellationToken: cancellationToken);
        return result.IsAcknowledged && result.ModifiedCount > 0;
                                            // ModifiedCount = Indica cuántos documentos han sido modificados en la operación.
    }
    
/// <summary>
/// 
/// </summary>
/// <param name="id"></param>
/// <param name="cancellationToken"></param>
/// <returns></returns>
    public async Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default)
    {
        if (!ObjectId.TryParse(id, out var idMongo)) return false;
        var filtro = Builders<ProveedorModel>.Filter.Eq(p => p.Id, idMongo) & FiltroActivos;
        return await _collection.Find(filtro).AnyAsync(cancellationToken);
    }
    
    public async Task<bool> ExistsByNameAsync(string name, ObjectId? id = null, CancellationToken cancellationToken = default)
    {
        var filtro = Builders<ProveedorModel>.Filter.Eq(p => p.Name, name) & FiltroActivos;

        //.Ne == distinto   &= ==> operador compuesto (filtro = filtro & otroFiltro;)
        if (id.HasValue)  filtro &= Builders<ProveedorModel>.Filter.Ne(p => p.Id, id.Value);

        return await _collection.Find(filtro).AnyAsync(cancellationToken);
    }
    
    
    
    private static FilterDefinition<ProveedorModel> FiltroActivos =>
        Builders<ProveedorModel>.Filter.Eq(p => p.IsDeleted, false);
}