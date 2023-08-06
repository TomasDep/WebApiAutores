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
        public async Task<List<AutorDto>> Get()
        {
            log.LogInformation("Init Get");
            var autoresDB = await context.Autores.ToListAsync();
            var autores = mapper.Map<List<AutorDto>>(autoresDB);
            log.LogInformation("Finish Get");
            return autores;
        }

        [HttpGet("{id:int}", Name = "GetAutorById")]
        public async Task<ActionResult<AutorLibroDto>> GetById(int id)
        {
            log.LogInformation("Init GetById");
            var autorDB = await context.Autores
                .Include(autor => autor.AutorLibro)
                .ThenInclude(autorLibro => autorLibro.Libro)
                .FirstOrDefaultAsync(autor => autor.ID == id);
            if (autorDB == null)
            {
                log.LogError($"Error in GetById Controller: id {id} not found");
                return NotFound($"Author with id: {id} not found");
            }
            var autor = mapper.Map<AutorLibroDto>(autorDB);
            log.LogInformation("Finish GetById");
            return autor;
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<AutorDto>>> GetByName([FromRoute] string nombre)
        {
            log.LogInformation("Init GetByName");
            var autoresDB = await context.Autores.Where(autor => autor.Nombre.Contains(nombre)).ToListAsync();
            if (autoresDB == null)
            {
                log.LogError($"Error in GetByName Controller: name: {nombre} not found");
                return NotFound($"Author with name: {nombre} not found");
            }
            var autores = mapper.Map<List<AutorDto>>(autoresDB);
            log.LogInformation("Finish GetByName");
            return autores;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AddAutorDto AutorDto)
        {
            log.LogInformation("Init Post");
            var existAuthor = await context.Autores.AnyAsync(a => a.Nombre == AutorDto.Nombre);
            if (existAuthor)
            {
                log.LogError($"Error in Post Controller: autor with name: {AutorDto.Nombre} not found");
                return BadRequest($"Author with name: {AutorDto.Nombre} already exists");
            }
            var autor = mapper.Map<Autor>(AutorDto);
            context.Add(autor);
            await context.SaveChangesAsync();
            var autorDto = mapper.Map<AutorDto>(autor);
            log.LogInformation("Finish Post");
            return CreatedAtRoute("GetAutorById", new { id = autor.ID }, autorDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(AddAutorDto addAutorDto, int id)
        {
            log.LogInformation("Init Put");
            var existAuthor = await context.Autores.AnyAsync(a => a.ID == id);
            if (!existAuthor)
            {
                log.LogError($"Error in Put Controller: id {id} not found");
                return NotFound($"Author with id: {id} not found");
            }
            var autor = mapper.Map<Autor>(addAutorDto);
            autor.ID = id;
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