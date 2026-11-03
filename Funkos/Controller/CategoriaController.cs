using Funkos.Dto.Categoria;
using Funkos.Errors;
using Funkos.Helper;
using Funkos.Service.Categoria;
using Microsoft.AspNetCore.Mvc;

namespace Funkos.Controller;


[ApiController]
[Route("api/[controller]")]
public class CategoriaController(ICategoriaService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CategoriaDtoResponse>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var resultado = await service.GetAllAsync(cancellationToken);
        if (resultado.Value.Count <= 0) return Ok("Aún no hay Categorias");
        return resultado.IsSuccess
            ? Ok(resultado.Value)
            : resultado.Error.ToHttpResult<List<CategoriaDtoResponse>>();
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<CategoriaWithFunkosResponseDto>> GetByIdAync([FromRoute] long id, CancellationToken cancellationToken)
    {
        var resultado = await service.GetByIdAsync(id, cancellationToken);

        var actionLinks = LinkHelper.GenerateLinks(id, "/api/categorias");
        Response.Headers.Append("Link", actionLinks);

        return resultado.IsSuccess
            ? Ok(resultado.Value)
            : resultado.Error.ToHttpResult<CategoriaWithFunkosResponseDto>();
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDtoResponse>> CreateAsync([FromBody] CategoriaCreateDto dto, CancellationToken cancellationToken)
    {
        var resultado = await service.AddAsync(dto, cancellationToken);
        if (resultado.IsFailure)
        {
            return resultado.Error.ToHttpResult<CategoriaDtoResponse>();
        }

        //HETOAS
        var actionLinks = LinkHelper.GenerateLinks(resultado.Value.Id, "/api/categorias");
        Response.Headers.Append("Link", actionLinks);

        return resultado.IsSuccess
            ? CreatedAtAction(nameof(GetByIdAync), new { id = resultado.Value.Id }, resultado.Value)
            : resultado.Error.ToHttpResult<CategoriaDtoResponse>();
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<CategoriaDtoResponse>> UpdateAsync([FromRoute] long id,
        [FromBody] CategoriaUpdateDto dto, CancellationToken cancellationToken)
    {
        var actualizado = await service.UpdateAsync(id, dto, cancellationToken);
        //HETOAS
        var actionLinks = LinkHelper.GenerateUpdateLinks(id, "/api/categorias");
        Response.Headers.Append("Link", actionLinks);
        return actualizado.IsSuccess
            ? Ok(actualizado.Value)
            : actualizado.Error.ToHttpResult<CategoriaDtoResponse>();

    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<bool>> DeleteAsync([FromRoute] long id, CancellationToken cancellationToken)
    {
        var deleted = await service.DeleteAsync(id, cancellationToken);
        //HETOAS de Delete
        var actionLinks = LinkHelper.GenerateDeleteLinks(id, "/api/categorias");
        Response.Headers.Append("Link", actionLinks);
        if (deleted.IsFailure)
        {
            return deleted.Error.ToHttpResult<bool>();
        }

        return NoContent();
    }
}