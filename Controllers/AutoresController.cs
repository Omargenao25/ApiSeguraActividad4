using ApiSeguraActividad4.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiSeguraActividad4.Controllers
{
    [Route("api/autores")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly BibliotecaActualizadaDbContext _context;

        public AutoresController(BibliotecaActualizadaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAutores(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string? buscar = null,
            [FromQuery] string? ordenarPor = null,
            [FromQuery] string? direccion = "asc")
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 5;

       
            var query = _context.Autores.AsQueryable();

     
            if (!string.IsNullOrEmpty(buscar))
            {
                query = query.Where(a => a.Nombre.Contains(buscar) || a.Nacionalidad.Contains(buscar));
            }

            if (!string.IsNullOrEmpty(ordenarPor))
            {
                bool esDesc = direccion?.ToLower() == "desc";

                if (ordenarPor.ToLower() == "nombre")
                {
                    query = esDesc ? query.OrderByDescending(a => a.Nombre) : query.OrderBy(a => a.Nombre);
                }
                else if (ordenarPor.ToLower() == "nacionalidad")
                {
                    query = esDesc ? query.OrderByDescending(a => a.Nacionalidad) : query.OrderBy(a => a.Nacionalidad);
                }
                else if (ordenarPor.ToLower() == "anio")
                {
                    query = esDesc ? query.OrderByDescending(a => a.AnioNacimiento) : query.OrderBy(a => a.AnioNacimiento);
                }
            }
            else
            {
                query = query.OrderBy(a => a.Id);
            }

        
            int totalElementos = await query.CountAsync();
            int totalPaginas = (int)Math.Ceiling((double)totalElementos / pageSize);

            var autores = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                TotalElementos = totalElementos,
                PaginaActual = page,
                TamanoPagina = pageSize,
                TotalPaginas = totalPaginas,
                Elementos = autores
            });
        }
    }
}
