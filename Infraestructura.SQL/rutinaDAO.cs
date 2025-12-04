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
    public class rutinaDAO
    {
        public IEnumerable<Rutina> GetAll()
        {
            var lista = new List<Rutina>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_listarRutinas", cn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Rutina
                    {
                        idRutina = (int)dr["idRutina"],
                        nombreRutina = dr["nombreRutina"].ToString(),
                        descripcion = dr["descripcion"].ToString(),
                        dificultad = dr["dificultad"].ToString()
                    });
                }
            }
            return lista;
        }

        public List<Ejercicio> ListarPorRutina(int idRutina)
        {
            List<Ejercicio> lista = new List<Ejercicio>();
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_listarEjerciciosPorRutina", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idRutina", idRutina);

                SqlDataReader dr = cmd.ExecuteReader();
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
