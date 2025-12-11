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
    public class usuarioDAO: IUsuario
    {
        public string Add(Usuario registro) {

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
                        mensaje = $"Usuario registrado correctamente.";
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

        public string Update(Usuario registro) {

            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cadena"].ConnectionString)) {

                try {
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

    }
}
