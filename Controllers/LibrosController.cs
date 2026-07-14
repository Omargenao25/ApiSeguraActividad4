using ApiSeguraActividad4.Data; 
using ApiSeguraActividad4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiSeguraActividad4.Controllers
{
    [Route("api/libros")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly BibliotecaActualizadaDbContext _context;

        public LibrosController(BibliotecaActualizadaDbContext context)
        {
            _context = context;
        }

  
        [HttpGet("paginado")]
        public async Task<ActionResult> GetLibrosPaginado(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string? buscar = null,
            [FromQuery] string? ordenarPor = null,
            [FromQuery] string? direccion = "asc")
        {

            if (page < 1)
            {
                return BadRequest("La página solicitada debe ser mayor o igual a 1.");
            }
            if (pageSize < 1)
            {
                return BadRequest("El tamaño de página debe ser mayor o igual a 1.");
            }

            var query = _context.Libros.AsQueryable();

       
            if (!string.IsNullOrEmpty(buscar))
            {
                query = query.Where(l => l.Titulo.Contains(buscar) || l.Genero.Contains(buscar));
            }

         
            if (!string.IsNullOrEmpty(ordenarPor))
            {
                bool esDesc = direccion?.ToLower() == "desc";

                if (ordenarPor.ToLower() == "titulo")
                {
                    query = esDesc ? query.OrderByDescending(l => l.Titulo) : query.OrderBy(l => l.Titulo);
                }
                else if (ordenarPor.ToLower() == "precio")
                {
                    query = esDesc ? query.OrderByDescending(l => l.Precio) : query.OrderBy(l => l.Precio);
                }
                else if (ordenarPor.ToLower() == "anio")
                {
                    query = esDesc ? query.OrderByDescending(l => l.AnioPublicacion) : query.OrderBy(l => l.AnioPublicacion);
                }
            }
            else
            {
                query = query.OrderBy(l => l.Id);
            }

            int totalElementos = await query.CountAsync();
            int totalPaginas = (int)Math.Ceiling((double)totalElementos / pageSize);

       
            var libros = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

       
            return Ok(new
            {
                TotalElementos = totalElementos,
                PaginaActual = page,
                TamanoPagina = pageSize,
                TotalPaginas = totalPaginas,
                Elementos = libros
            });
        }

    
        [HttpGet("{id}")]
        public async Task<ActionResult<Libro>> GetLibro(int id)
        {
            var libro = await _context.Libros.FindAsync(id);

            if (libro == null)
            {
                return NotFound("No se encontró el libro solicitado.");
            }

            return Ok(libro);
        }

 
        [HttpGet("/api/autores/{autorId}/libros")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosPorAutor(int autorId)
        {
            var autorExiste = await _context.Autores.AnyAsync(a => a.Id == autorId);
            if (!autorExiste)
            {
                return NotFound("El autor especificado no existe.");
            }

            var librosDelAutor = await _context.Libros
                .Where(l => l.AutorId == autorId)
                .ToListAsync();

            return Ok(librosDelAutor);
        }

        [HttpPost]
        public async Task<ActionResult<Libro>> PostLibro(Libro libro)
        {
         
            var autorExiste = await _context.Autores.AnyAsync(a => a.Id == libro.AutorId);
            if (!autorExiste)
            {
                return BadRequest("No se puede registrar el libro porque el AutorId no existe.");
            }

            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLibro), new { id = libro.Id }, libro);
        }

      
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibro(int id, Libro libro)
        {
            if (id != libro.Id)
            {
                return BadRequest("El ID no coincide.");
            }

            var libroExiste = await _context.Libros.AnyAsync(l => l.Id == id);
            if (!libroExiste)
            {
                return NotFound("El libro no existe.");
            }

            var autorExiste = await _context.Autores.AnyAsync(a => a.Id == libro.AutorId);
            if (!autorExiste)
            {
                return BadRequest("El AutorId asignado no es válido.");
            }

            _context.Entry(libro).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

   
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibro(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound("El libro no existe.");
            }

            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}