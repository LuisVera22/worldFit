using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidad.Entidad
{
    public class Imc
    {
        public int idIMC { get; set; }
        public int idUsuario { get; set; }
        public decimal peso { get; set; }
        public decimal altura { get; set; }
        public decimal valorIMC { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}
