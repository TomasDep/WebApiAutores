using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIAutores;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        public readonly ApplicationDbContext context;

        public AutoresController(ApplicationDbContext context) {
            this.context = context;
        }

        [HttpGet]
        [HttpGet("list")]
        public async Task<ActionResult<List<Autor>>> Get() {
            return await context.Autores.Include(autor => autor.Libro).ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Autor>> GetById(int id) {
            var autor =  await context.Autores.FirstOrDefaultAsync(autor => autor.ID == id);

            if (autor == null) {
                return NotFound($"Author with id: {id} not found");
            }

            return autor;
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<Autor>> GetByName([FromRoute] string nombre) {
            var autor =  await context.Autores.FirstOrDefaultAsync(autor => autor.Nombre.Contains(nombre));

            if (autor == null) {
                return NotFound($"Author with name: {nombre} not found");
            }

            return autor;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Autor autor) {
            var existAuthor = await context.Autores.AnyAsync(a => a.Nombre == autor.Nombre);

            if (existAuthor)
                return BadRequest($"Author with name: { autor.Nombre } already exists");

            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Autor autor, int id) {
            if (autor.ID != id) 
                return BadRequest($"Author with id: {id} not found");

            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id) {
            var exist = await context.Autores.AnyAsync(autor => autor.ID == id);

            if (!exist) return NotFound();

            context.Remove(new Autor() { ID = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}