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
        public readonly ILogger<AutoresController> log;

        public AutoresController(ApplicationDbContext context, ILogger<AutoresController> log)
        {
            this.context = context;
            this.log = log;
        }

        [HttpGet]
        [HttpGet("list")]
        public async Task<ActionResult<List<Autor>>> Get()
        {
            log.LogInformation("Init Get");

            var authorCollection = await context.Autores.Include(autor => autor.Libro).ToListAsync();

            log.LogInformation("Finish Get");
            return authorCollection;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Autor>> GetById(int id)
        {
            log.LogInformation("Init GetById");
            var autor = await context.Autores.FirstOrDefaultAsync(autor => autor.ID == id);

            if (autor == null)
            {
                log.LogError($"Error in GetById Controller: id {id} not found");
                return NotFound($"Author with id: {id} not found");
            }

            log.LogInformation("Finish GetById");
            return autor;
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<Autor>> GetByName([FromRoute] string nombre)
        {
            log.LogInformation("Init GetByName");
            var autor = await context.Autores.FirstOrDefaultAsync(autor => autor.Nombre.Contains(nombre));

            if (autor == null)
            {
                log.LogError($"Error in GetByName Controller: name: {nombre} not found");
                return NotFound($"Author with name: {nombre} not found");
            }

            log.LogInformation("Finish GetByName");
            return autor;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Autor autor)
        {
            log.LogInformation("Init Post");
            var existAuthor = await context.Autores.AnyAsync(a => a.Nombre == autor.Nombre);

            if (existAuthor)
            {
                log.LogError($"Error in Post Controller: autor with name: {autor.Nombre} not found");
                return BadRequest($"Author with name: {autor.Nombre} already exists");
            }

            context.Add(autor);
            await context.SaveChangesAsync();

            log.LogInformation("Finish Post");
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            log.LogInformation("Init Put");
            if (autor.ID != id)
            {
                log.LogError($"Error in Put Controller: id {id} not found");
                return BadRequest($"Author with id: {id} not found");
            }

            context.Update(autor);
            await context.SaveChangesAsync();
            
            log.LogInformation("Finish Put");
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            log.LogInformation("Init Delete");
            var exist = await context.Autores.AnyAsync(autor => autor.ID == id);

            if (!exist)
            {
                log.LogError($"Error in Delete Controller: autor with id: {id} not found");
                return NotFound();
            }

            context.Remove(new Autor() { ID = id });
            await context.SaveChangesAsync();

            log.LogInformation("Finish Delete");
            return Ok();
        }
    }
}