using Dominio.Entidad.Abstraccion;
using Dominio.Entidad.Entidad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Infraestructura.SQL
{
    public class imcDAO : Imc
    {
        public string Add(Imc registro)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("sp_registrarIMC", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@idUsuario", registro.idUsuario);
                    cmd.Parameters.AddWithValue("@peso", registro.peso);
                    cmd.Parameters.AddWithValue("@altura", registro.altura);

                    cmd.ExecuteNonQuery();
                    mensaje = "IMC registrado correctamente.";
                }
                catch (SqlException ex)
                {
                    mensaje = ex.Message;
                }
            }
            return mensaje;
        }


        public IEnumerable<Imc> HistorialPorUsuario(int idUsuario)
        {
            var lista = new List<Imc>();

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_historialIMC", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Imc
                        {
                            peso = dr.GetDecimal(0),
                            altura = dr.GetDecimal(1),
                            imcCalculado = dr.GetDecimal(2),
                            fechaRegistro = dr.GetDateTime(3)
                        });
                    }
                }
            }

            return lista;
        }
    }
}
