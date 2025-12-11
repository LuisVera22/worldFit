using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidad.Entidad
{
    public class Ejercicio
    {
        public int idEjercicio { get; set; }
        public int idRutina { get; set; }
        public string nombreEjercicio { get; set; }
        public int series { get; set; }
        public string repeticiones { get; set; }

    }
}
