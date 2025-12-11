using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // Relaciones opcionales (si quieres acceder a los datos completos)
        public Usuario Usuario { get; set; }
        public Rutina Rutina { get; set; }
        public string dificultad { get; set; }
    }
}
