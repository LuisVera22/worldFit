using Dominio.Entidad.Abstraccion;
using Dominio.Entidad.Entidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.SQL
{
    public class usuarioDAO : IUsuario
    {
        public string Add(Usuario registro)
        {

            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("sp_registrarUsuario", cn);
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@correo", registro.correo);
                        cmd.Parameters.AddWithValue("@clave", registro.clave);
                        cmd.Parameters.AddWithValue("@nombre", registro.nombre);
                        cmd.Parameters.AddWithValue("@telefono", (object)registro.telefono ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@fechaNacimiento", (object)registro.fechaNacimiento ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@genero", (object)registro.genero ?? DBNull.Value);

                        int c = cmd.ExecuteNonQuery();
                        mensaje = $"Usuario {c}registrado correctamente.";
                    }

                }
                catch (SqlException ex)
                {
                    mensaje = ex.Message;
                }
                finally
                {
                    cn.Close();
                }

            return mensaje;
        }

        public string Update(Usuario registro)
        {

            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {

                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("sp_actualizarUsuario", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idUsuario", registro.IdUsuario);
                    cmd.Parameters.AddWithValue("@correo", registro.correo);
                    cmd.Parameters.AddWithValue("@clave", registro.clave);
                    cmd.Parameters.AddWithValue("@nombre", registro.nombre);
                    cmd.Parameters.AddWithValue("@telefono", (object)registro.telefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@fechaNacimiento", (object)registro.fechaNacimiento ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@genero", (object)registro.genero ?? DBNull.Value);

                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Usuario actualizado correctamente ({c} fila).";
                }
                catch (SqlException ex) { mensaje = ex.Message; }
                finally { cn.Close(); }
            }
            return mensaje;

        }

        public Usuario Login(string correo, string clave)
        {
            Usuario usuario = null;
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("sp_loginUsuario", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@correo", correo);
                    cmd.Parameters.AddWithValue("@clave", clave);

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        usuario = new Usuario()
                        {
                            IdUsuario = Convert.ToInt32(dr["idUsuario"]),
                            nombre = dr["nombre"].ToString(),
                            correo = correo
                        };
                    }
                }
                catch
                {
                    usuario = null;
                }
            }
            return usuario;
        }

        // Listar todos los usuarios (para pruebas)
        // Listar todos los usuarios (usa dbo.Usuarios)
        public List<Usuario> ListarTodos()
        {
            var lista = new List<Usuario>();

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(@"SELECT IdUsuario, correo, clave, nombre, telefono, fechaNacimiento, genero, fechaRegistro, activo 
                                             FROM dbo.Usuarios", cn))
            {
                cmd.CommandType = CommandType.Text;
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var u = new Usuario
                        {
                            IdUsuario = dr.IsDBNull(dr.GetOrdinal("IdUsuario")) ? 0 : Convert.ToInt32(dr["IdUsuario"]),
                            correo = dr.IsDBNull(dr.GetOrdinal("correo")) ? null : dr["correo"].ToString(),
                            clave = dr.IsDBNull(dr.GetOrdinal("clave")) ? null : dr["clave"].ToString(),
                            nombre = dr.IsDBNull(dr.GetOrdinal("nombre")) ? null : dr["nombre"].ToString(),
                            telefono = dr.IsDBNull(dr.GetOrdinal("telefono")) ? null : dr["telefono"].ToString(),
                            fechaNacimiento = dr.IsDBNull(dr.GetOrdinal("fechaNacimiento")) ? (DateTime?)null : Convert.ToDateTime(dr["fechaNacimiento"]),
                            genero = dr.IsDBNull(dr.GetOrdinal("genero")) ? null : dr["genero"].ToString(),
                            fechaRegistro = dr.IsDBNull(dr.GetOrdinal("fechaRegistro")) ? DateTime.MinValue : Convert.ToDateTime(dr["fechaRegistro"]),
                            activo = !dr.IsDBNull(dr.GetOrdinal("activo")) && Convert.ToBoolean(dr["activo"])
                        };
                        lista.Add(u);
                    }
                }
            }

            return lista;
        }

        public List<Ejercicio> ListarPorUsuario(int idUsuario)
        {
            var lista = new List<Ejercicio>();

            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_listarEjerciciosPorUsuario", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Ejercicio
                        {
                            idEjercicio = Convert.ToInt32(dr["idEjercicio"]),
                            idUsuario = idUsuario,
                            idRutina = dr["idRutina"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["idRutina"]),
                            nombreEjercicio = dr["nombreEjercicio"].ToString(),
                            series = Convert.ToInt32(dr["series"]),
                            repeticiones = dr["repeticiones"].ToString(),
                            fechaRegistro = Convert.ToDateTime(dr["fechaRegistro"])
                        });
                    }
                }
            }

            return lista;
        }

    }
}
