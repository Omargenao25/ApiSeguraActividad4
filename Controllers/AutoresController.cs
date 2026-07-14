using ApiSeguraActividad4.Data;
using ApiSeguraActividad4.Models;
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
        public async Task<ActionResult<IEnumerable<Autor>>> GetAutoresTodos()
        {
            var autores = await _context.Autores.ToListAsync();
            return Ok(autores);
        }

        [HttpGet("paginado")]
        public async Task<ActionResult> GetAutoresPaginado(
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


      
        [HttpGet("{id}")]
        public async Task<ActionResult<Autor>> GetAutor(int id)
        {
            var autor = await _context.Autores.FindAsync(id);

            if (autor == null)
            {
                return NotFound("No se encontró el autor solicitado.");
            }

            return Ok(autor);
        }


        [HttpPost]
        public async Task<ActionResult<Autor>> PostAutor(Autor autor)
        {
            _context.Autores.Add(autor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAutor), new { id = autor.Id }, autor);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutAutor(int id, Autor autor)
        {
            if (id != autor.Id)
            {
                return BadRequest("El ID no coincide.");
            }

            var autorExiste = await _context.Autores.AnyAsync(a => a.Id == id);
            if (!autorExiste)
            {
                return NotFound("El autor no existe.");
            }

            _context.Entry(autor).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAutor(int id)
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor == null)
            {
                return NotFound("El autor no existe.");
            }

            var tieneLibros = await _context.Libros.AnyAsync(l => l.AutorId == id);
            if (tieneLibros)
            {
                return BadRequest("No se puede eliminar este autor porque tiene libros asociados.");
            }

            _context.Autores.Remove(autor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
