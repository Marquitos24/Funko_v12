using Funkos.Dto.Proveedor;
using Funkos.Errors;
using Funkos.Helper;
using Funkos.Service.Proveedor;
using Microsoft.AspNetCore.Mvc;

namespace Funkos.Controller;

[ApiController]
[Route("api/[controller]")]
public class ProveedorController(IProveedorService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ProveedorDtoResponse>>> GetAllAsync(CancellationToken cancellationToken)
    {
       var resultado = await service.GetAllAsync(cancellationToken);
       if (resultado.Value.Count <= 0) { return Ok("Aún no hay Proveedores"); }
       return resultado.IsSuccess 
           ? Ok(resultado.Value)
           : resultado.Error.ToHttpResult<List<ProveedorDtoResponse>>();
    }
    
    [HttpGet("{id:long}")]
    public async Task<ActionResult<ProveedorConFunkosDtoResponse>> GetByIdAync([FromRoute] long id, CancellationToken cancellationToken)
    {
        var resultado = await service.GetByIdAsync(id, cancellationToken);
                
        var actionLinks = LinkHelper.GenerateLinks(id,"/api/proveedores");
        Response.Headers.Append("Link", actionLinks);
        
        return resultado.IsSuccess 
            ? Ok(resultado.Value)
            : resultado.Error.ToHttpResult<ProveedorConFunkosDtoResponse>();
    }
    
    [HttpPost]
    public async Task<ActionResult<ProveedorDtoResponse>> CreateAsync([FromBody] ProveedorCreateDto dto, CancellationToken cancellationToken)
    {
        var resultado = await service.AddAsync(dto, cancellationToken);
        if (resultado.IsFailure) { return resultado.Error.ToHttpResult<ProveedorDtoResponse>(); }
        //HETOAS
        var actionLinks = LinkHelper.GenerateLinks(resultado.Value.Id,"/api/proveedores");
        Response.Headers.Append("Link", actionLinks);
        
            return resultado.IsSuccess 
                ? CreatedAtAction(nameof(GetByIdAync), new { id = resultado.Value.Id}, resultado.Value)
                : resultado.Error.ToHttpResult<ProveedorDtoResponse>();
    }
    
    [HttpPut("{id:long}")]
    public async Task<ActionResult<ProveedorDtoResponse>> UpdateAsync([FromRoute] long id, [FromBody] ProveedorUpdateDto dto, CancellationToken cancellationToken)
    {
        var actualizado = await service.UpdateAsync(id, dto, cancellationToken);
        //HETOAS
        var actionLinks = LinkHelper.GenerateUpdateLinks(id,"/api/proveedores");
        Response.Headers.Append("Link", actionLinks);
        return actualizado.IsSuccess 
            ? Ok(actualizado.Value)
            : actualizado.Error.ToHttpResult<ProveedorDtoResponse>();
        
    }
    
    [HttpDelete("{id:long}")]
    public async Task<ActionResult<bool>> DeleteAsync([FromRoute] long id, CancellationToken cancellationToken)
    {
            var deleted = await service.DeleteAsync(id, cancellationToken);
            //HETOAS de Delete
            var actionLinks = LinkHelper.GenerateDeleteLinks(id,"/api/proveedores");
            Response.Headers.Append("Link", actionLinks);
            if (deleted.IsFailure) { return deleted.Error.ToHttpResult<bool>(); }
            return NoContent();
    }
}