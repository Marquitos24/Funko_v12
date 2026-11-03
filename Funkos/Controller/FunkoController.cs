using Funkos.Models;
using Funkos.Repositories;
using Funkos.Dto.Funko;
using Funkos.Mappers;
using Funkos.Service;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CSharpFunctionalExtensions;
using Funkos.Dto;
using Funkos.Errors;
using Funkos.Helper;

namespace Funkos.Controller;

[ApiController]
[Route("api/[controller]")]
public class FunkoController(IFunkoService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<FunkosDtoResponse>>> GetAllAsync([FromQuery] string? categoria,
        [FromQuery] decimal? precioMin,
        [FromQuery] decimal? precioMax, CancellationToken cancellationToken)
    {
       var resultado = await service.GetAllAsync(categoria, precioMin, precioMax, cancellationToken);
       if (resultado.Value.Count <= 0) { return Ok("Aún no hay Funkos"); }
       return resultado.IsSuccess 
           ? Ok(resultado.Value)
           : resultado.Error.ToHttpResult<List<FunkosDtoResponse>>();
    }
    
    [HttpGet("{id:long}")]
    public async Task<ActionResult<FunkosDtoResponse>> GetByIdAync([FromRoute] long id, CancellationToken cancellationToken)
    {
        var resultado = await service.GetByIdAsync(id, cancellationToken);
                
        var actionLinks = LinkHelper.GenerateLinks(id,"/api/funkos");
        Response.Headers.Append("Link", actionLinks);
        
        return resultado.IsSuccess 
            ? Ok(resultado.Value)
            : resultado.Error.ToHttpResult<FunkosDtoResponse>();
    }
    
    [HttpPost]
    public async Task<ActionResult<FunkosDtoResponse>> CreateAsync([FromBody] FunkoCreateDto dto, CancellationToken cancellationToken)
    {
        var resultado = await service.AddAsync(dto, cancellationToken);
        if (resultado.IsFailure) { return resultado.Error.ToHttpResult<FunkosDtoResponse>(); }
        //HETOAS
        var actionLinks = LinkHelper.GenerateLinks(resultado.Value.Id,"/api/funkos");
        Response.Headers.Append("Link", actionLinks);
        
            return resultado.IsSuccess 
                ? CreatedAtAction(nameof(GetByIdAync), new { id = resultado.Value.Id}, resultado.Value)
                : resultado.Error.ToHttpResult<FunkosDtoResponse>();
    }
    
    [HttpPut("{id:long}")]
    public async Task<ActionResult<FunkosDtoResponse>> UpdateAsync([FromRoute] long id, [FromBody] FunkoUpdateDto dto, CancellationToken cancellationToken)
    {
        var actualizado = await service.UpdateAsync(id, dto, cancellationToken);
        //HETOAS
        var actionLinks = LinkHelper.GenerateUpdateLinks(id,"/api/funkos");
        Response.Headers.Append("Link", actionLinks);
        return actualizado.IsSuccess 
            ? Ok(actualizado.Value)
            : actualizado.Error.ToHttpResult<FunkosDtoResponse>();
        
    }
    
    [HttpDelete("{id:long}")]
    public async Task<ActionResult<bool>> DeleteAsync([FromRoute] long id, CancellationToken cancellationToken)
    {
            var deleted = await service.DeleteAsync(id, cancellationToken);
            //HETOAS de Delete
            var actionLinks = LinkHelper.GenerateDeleteLinks(id,"/api/funkos");
            Response.Headers.Append("Link", actionLinks);
            if (deleted.IsFailure) { return deleted.Error.ToHttpResult<bool>(); }
            return NoContent();
    }
}