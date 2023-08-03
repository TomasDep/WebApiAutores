using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIAutores.Entities
{
    public class Autor
    { 
      public int ID { get; set; }
      public string Nombre { get; set; }
      public List<Libro> Libro { get; set; }
    }
}