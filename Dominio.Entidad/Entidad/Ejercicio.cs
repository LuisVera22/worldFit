using System;

namespace Dominio.Entidad.Entidad
{
    public class Ejercicio
    {
        public int idEjercicio { get; set; }
        public int? idUsuario { get; set; }
        public int? idRutina { get; set; }
        public string nombreEjercicio { get; set; }
        public int series { get; set; }
        public string repeticiones { get; set; }
        public DateTime fechaRegistro { get; set; }

        // ⭐ NUEVO — necesario para borrar los ejercicios del usuario
        public int? idEjercicioUsuario { get; set; }
    }
}

