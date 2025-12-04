using Dominio.Entidad.Entidad;
using Dominio.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidad.Abstraccion
{
    public interface IHabito: IRepositorioGET<Habito>, IRepositorioCRUD<Habito>
    {
        Habito BuscarPorId(int id);
    }
}
