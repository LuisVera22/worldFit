using Dominio.Entidad.Entidad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.SQL
{
    public class habitoDAO
    {
        public string Add(Habito registro)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("sp_registrarHabito", cn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@idUsuario", registro.idUsuario);
                    cmd.Parameters.AddWithValue("@fecha", registro.fechaRegistro);
                    cmd.Parameters.AddWithValue("@pasos", registro.pasos);
                    cmd.Parameters.AddWithValue("@horasSueno", registro.horasSueno);
                    cmd.Parameters.AddWithValue("@vasosAgua", registro.vasosAgua);
                    cmd.Parameters.AddWithValue("@notas", registro.notas ?? (object)DBNull.Value);

                    cmd.ExecuteNonQuery();
                    mensaje = "✅ Hábito registrado correctamente.";
                }
                catch (SqlException ex)
                {
                    mensaje = "⚠ Error al registrar el hábito: " + ex.Message;
                }
                finally
                {
                    cn.Close();
                }
            }
            return mensaje;
        }


        public string Update(Habito registro)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("sp_actualizarHabito", cn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@idHabito", registro.idHabito);
                    cmd.Parameters.AddWithValue("@fecha", registro.fechaRegistro);
                    cmd.Parameters.AddWithValue("@pasos", registro.pasos);
                    cmd.Parameters.AddWithValue("@horasSueno", registro.horasSueno);
                    cmd.Parameters.AddWithValue("@vasosAgua", registro.vasosAgua);
                    cmd.Parameters.AddWithValue("@notas", registro.notas ?? (object)DBNull.Value);

                    cmd.ExecuteNonQuery();
                    mensaje = "✅ Hábito actualizado correctamente.";
                }
                catch (SqlException ex)
                {
                    mensaje = "⚠ Error al actualizar el hábito: " + ex.Message;
                }
                finally
                {
                    cn.Close();
                }
            }
            return mensaje;
        }


        public IEnumerable<Habito> ListarPorUsuario(int idUsuario)
        {
            var lista = new List<Habito>();
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_listarHabitosPorUsuario", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Habito
                        {
                            idHabito = reader.GetInt32(0),
                            idUsuario = reader.GetInt32(1),
                            fechaRegistro = reader.GetDateTime(2),
                            pasos = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                            horasSueno = reader["horasSueno"] != DBNull.Value ? Convert.ToDouble(reader["horasSueno"]) : 0.0,
                            vasosAgua = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                            notas = reader.IsDBNull(6) ? "" : reader.GetString(6)
                        });
                    }
                }
            }
            return lista;
        }


        public IEnumerable<Habito> GetAll()
        {
            var lista = new List<Habito>();
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_listarHabitos", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Habito
                        {
                            idHabito = reader.GetInt32(0),
                            idUsuario = reader.GetInt32(1),
                            fechaRegistro = reader.GetDateTime(2),
                            pasos = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                            horasSueno = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                            vasosAgua = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                            notas = reader.IsDBNull(6) ? "" : reader.GetString(6)
                        });
                    }
                }
            }
            return lista;
        }

        public Habito BuscarPorId(int id)
        {
            Habito habito = null;
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_buscarHabitoPorId", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@idHabito", id);

                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        habito = new Habito
                        {
                            idHabito = Convert.ToInt32(dr["idHabito"]),
                            idUsuario = Convert.ToInt32(dr["idUsuario"]),
                            fechaRegistro = Convert.ToDateTime(dr["fecha"]),
                            pasos = dr["pasos"] != DBNull.Value ? Convert.ToInt32(dr["pasos"]) : 0,
                            horasSueno = dr["horasSueno"] != DBNull.Value ? Convert.ToDouble(dr["horasSueno"]) : 0.0,
                            vasosAgua = dr["vasosAgua"] != DBNull.Value ? Convert.ToInt32(dr["vasosAgua"]) : 0,
                            notas = dr["notas"] != DBNull.Value ? dr["notas"].ToString() : ""
                        };
                }   }
                
                return habito;
            }
        }
    }
}


