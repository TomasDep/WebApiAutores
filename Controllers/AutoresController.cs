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
        public async Task<ActionResult<List<Autor>>> Get() {
            return await context.Autores.Include(autor => autor.Libro).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Autor autor) {
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