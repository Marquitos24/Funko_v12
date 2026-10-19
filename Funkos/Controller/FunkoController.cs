using Funkos.Models;
using Funkos.Repositories;
using Funkos.Dto;
using Funkos.Mappers;
using Funkos.Service;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CSharpFunctionalExtensions;
using Funkos.Errors;
using Funkos.Helper;

namespace Funkos.Controller;

[ApiController]
[Route("api/[controller]")]
public class FunkoController(IFunkoService service) : ControllerBase
{
    [HttpGet]
    public ActionResult<List<FunkosDtoResponse>> GetAll([FromQuery] string? categoria,
        [FromQuery] decimal? precioMin,
        [FromQuery] decimal? precioMax)
    {
       var resultado = service.GetAll(categoria, precioMin, precioMax);
       if (resultado.Value.Count <= 0) { return Ok("Aún no hay Funkos"); }
       return resultado.IsSuccess 
           ? Ok(resultado.Value)
           : resultado.Error.ToHttpResult<List<FunkosDtoResponse>>();
    }
    
    [HttpGet("{id:long}")]
    public ActionResult<FunkosDtoResponse> GetById([FromRoute] long id)
    {
        var resultado = service.GetById(id);
                
        var actionLinks = FunkoLinkHelper.GenerateLinks(id,"/api/funkos");
        Response.Headers.Append("Link", actionLinks);
        
        return resultado.IsSuccess 
            ? Ok(resultado.Value)
            : resultado.Error.ToHttpResult<FunkosDtoResponse>();
    }
    
    [HttpPost]
    public ActionResult<FunkosDtoResponse> Create([FromBody] FunkoCreateDto dto)
    {
        var resultado = service.Add(dto);
        if (resultado.IsFailure) { return resultado.Error.ToHttpResult<FunkosDtoResponse>(); }
        //HETOAS
        var actionLinks = FunkoLinkHelper.GenerateLinks(resultado.Value.Id,"/api/funkos");
        Response.Headers.Append("Link", actionLinks);
        
            return resultado.IsSuccess 
                ? CreatedAtAction(nameof(GetById), new { id = resultado.Value.Id}, resultado.Value)
                : resultado.Error.ToHttpResult<FunkosDtoResponse>();
    }
    
    [HttpPut("{id:long}")]
    public ActionResult<FunkosDtoResponse> Update([FromRoute] long id, [FromBody] FunkoUpdateDto dto)
    {
        var actualizado = service.Update(id, dto);
        //HETOAS
        var actionLinks = FunkoLinkHelper.GenerateUpdateLinks(id,"/api/funkos");
        Response.Headers.Append("Link", actionLinks);
        return actualizado.IsSuccess 
            ? Ok(actualizado.Value)
            : actualizado.Error.ToHttpResult<FunkosDtoResponse>();
        
    }
    
    [HttpDelete("{id:long}")]
    public ActionResult<bool> Delete([FromRoute] long id)
    {
            var deleted = service.Delete(id);
            //HETOAS de Delete
            var actionLinks = FunkoLinkHelper.GenerateDeleteLinks(id,"/api/funkos");
            Response.Headers.Append("Link", actionLinks);
            if (deleted.IsFailure) { return deleted.Error.ToHttpResult<bool>(); }
            return NoContent();
    }
}