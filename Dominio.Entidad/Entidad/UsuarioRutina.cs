using System;

namespace Dominio.Entidad.Entidad
{
    public class UsuarioRutina
    {
        public int idUsuarioRutina { get; set; }
        public int idUsuario { get; set; }
        public int idRutina { get; set; }

        public string nombreRutina { get; set; }
        public int metaSemanal { get; set; }
        public int diasCumplidos { get; set; }
        public DateTime fechaAsignacion { get; set; }

        // Propiedades adicionales útiles
        public string dificultad { get; set; }

        // Relaciones opcionales
        public Usuario usuario { get; set; }
        public Rutina rutina { get; set; }
    }
}
