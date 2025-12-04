using Dominio.Entidad.Abstraccion;
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
    public class imcDAO: IImc
    {
        public string Add(Imc registro) {

            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cadena"].ConnectionString)) {
                try { 
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("sp_registrarIMC", cn);
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idUsuario", registro.idUsuario);
                        cmd.Parameters.AddWithValue("@peso", registro.peso);
                        cmd.Parameters.AddWithValue("@altura", registro.altura);
                        cmd.ExecuteNonQuery();
                    }
                    mensaje ="IMC registrado correctamente.";
                }
                catch (SqlException ex) { mensaje = ex.Message; }
                finally { cn.Close(); }
            }
            return mensaje;
        }

        public string Update(Imc registro)
        {
            return "";
        }

        public IEnumerable<Imc> HistorialPorUsuario(int idUsuario)
        {
            var lista = new List<Imc>();
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_historialIMC", cn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Imc
                        {
                            peso = reader.GetDecimal(0),
                            altura = reader.GetDecimal(1),
                            valorIMC = reader.GetDecimal(2),
                            fechaRegistro = reader.GetDateTime(3)
                        });
                    }
                }
            }
            return lista;
        }

    }
}
