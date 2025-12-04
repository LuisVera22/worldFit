using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidad.Entidad
{
    public class Habito
    {
        
        public int idHabito { get; set; }
        public int idUsuario { get; set; }

        [Display(Name = "Pasos")]
        public int pasos { get; set; }

        [Display(Name = "Horas de Sueño")]
        public double horasSueno { get; set; }

        [Display(Name = "Vasos de Agua")]
        public int vasosAgua { get; set; }

        [Display(Name = "Notas")]
        public string notas { get; set; }

        [Display(Name = "Fecha de registro")]
        public DateTime fechaRegistro { get; set; }
    }
}
