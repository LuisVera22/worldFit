using Dominio.Entidad.Entidad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.SQL
{
    public class ejercicioDAO
    {
        public IEnumerable<Ejercicio> GetAll()
        {
            var lista = new List<Ejercicio>();
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_listarEjercicios", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Ejercicio 
                        {
                            idEjercicio = Convert.ToInt32(dr["idEjercicio"]),
                            idRutina = Convert.ToInt32(dr["idRutina"]),
                            nombreEjercicio = dr["nombreEjercicio"].ToString(),
                            series = Convert.ToInt32(dr["series"]),
                            repeticiones = dr["repeticiones"].ToString()
                        });
                    }
                }
                return lista;
            }
        }
    }
}

