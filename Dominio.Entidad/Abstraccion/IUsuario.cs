using Dominio.Entidad.Entidad;
using Dominio.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidad.Abstraccion
{
    public interface IUsuario: IRepositorioCRUD<Usuario>
    {
        Usuario Login(string correo, string clave);
    }
}
