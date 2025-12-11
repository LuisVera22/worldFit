using System;

namespace Dominio.Entidad.Entidad
{
    public class Rutina
    {
        public int idRutina { get; set; }
        public string nombreRutina { get; set; }
        public string descripcion { get; set; }
        public string dificultad { get; set; }
        public DateTime fechaCreacion { get; set; }
    }
}