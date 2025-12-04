using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entidad.Entidad;

namespace Infraestructura.SQL
{
    public class usuarioRutinaDAO
    {
        public string AgregarRutinaUsuario(int idUsuario, int idRutina, int metaSemanal)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_agregarUsuarioRutina", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                cmd.Parameters.AddWithValue("@idRutina", idRutina);
                cmd.Parameters.AddWithValue("@metaSemanal", metaSemanal);
                cmd.ExecuteNonQuery();
                mensaje = "Rutina agregada correctamente a tu perfil.";
            }
            return mensaje;
        }

        public List<UsuarioRutina> ListarRutinasPorUsuario(int idUsuario)
        {
            List<UsuarioRutina> lista = new List<UsuarioRutina>();

            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_listarRutinasPorUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new UsuarioRutina
                    {
                        idUsuarioRutina = Convert.ToInt32(dr["idUsuarioRutina"]),
                        idUsuario = Convert.ToInt32(dr["idUsuario"]),
                        idRutina = Convert.ToInt32(dr["idRutina"]),
                        nombreRutina = dr["nombreRutina"].ToString(),
                        metaSemanal = Convert.ToInt32(dr["metaSemanal"]),
                        diasCumplidos = Convert.ToInt32(dr["diasCumplidos"]),
                        fechaAsignacion = Convert.ToDateTime(dr["fechaAsignacion"])
                    });
                }
            }

            return lista;
        }

        public void ActualizarProgreso(int idUsuarioRutina)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_actualizarProgresoRutina", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuarioRutina", idUsuarioRutina);
                cmd.ExecuteNonQuery();
            }
        }

        public string EliminarRutinaUsuario(int idUsuarioRutina)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_eliminarUsuarioRutina", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuarioRutina", idUsuarioRutina);
                int filas = cmd.ExecuteNonQuery();

                mensaje = filas > 0 ? "Rutina eliminada correctamente." : "No se pudo eliminar la rutina.";
            }
            return mensaje;
        }

    }
}
