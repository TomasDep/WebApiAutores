using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIAutores;
using WebAPIAutores.DTO;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<AutoresController> log;
        private readonly IMapper mapper;

        public AutoresController(
            ApplicationDbContext context,
            ILogger<AutoresController> log,
            IMapper mapper
        )
        {
            this.context = context;
            this.log = log;
            this.mapper = mapper;
        }

        [HttpGet]
        [HttpGet("list")]
        public async Task<List<AutorDTO>> Get()
        {
            log.LogInformation("Init Get");
            var autoresDB = await context.Autores.ToListAsync();
            var autores = mapper.Map<List<AutorDTO>>(autoresDB);
            log.LogInformation("Finish Get");
            return autores;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AutorDTO>> GetById(int id)
        {
            log.LogInformation("Init GetById");
            var autorDB = await context.Autores.FirstOrDefaultAsync(autor => autor.ID == id);
            if (autorDB == null)
            {
                log.LogError($"Error in GetById Controller: id {id} not found");
                return NotFound($"Author with id: {id} not found");
            }
            var autor = mapper.Map<AutorDTO>(autorDB);
            log.LogInformation("Finish GetById");
            return autor;
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<AutorDTO>>> GetByName([FromRoute] string nombre)
        {
            log.LogInformation("Init GetByName");
            var autoresDB = await context.Autores.Where(autor => autor.Nombre.Contains(nombre)).ToListAsync();
            if (autoresDB == null)
            {
                log.LogError($"Error in GetByName Controller: name: {nombre} not found");
                return NotFound($"Author with name: {nombre} not found");
            }
            var autores = mapper.Map<List<AutorDTO>>(autoresDB);
            log.LogInformation("Finish GetByName");
            return autores;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AddAutorDTO autorDto)
        {
            log.LogInformation("Init Post");
            var existAuthor = await context.Autores.AnyAsync(a => a.Nombre == autorDto.Nombre);
            if (existAuthor)
            {
                log.LogError($"Error in Post Controller: autor with name: {autorDto.Nombre} not found");
                return BadRequest($"Author with name: {autorDto.Nombre} already exists");
            }
            var autor = mapper.Map<Autor>(autorDto);
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