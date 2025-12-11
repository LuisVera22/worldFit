using Dominio.Entidad.Entidad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class usuarioRutinaDAO
{
    string cnx = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

    // Listar rutinas asignadas a un usuario
    public List<UsuarioRutina> ListarRutinasPorUsuario(int idUsuario)
    {
        var lista = new List<UsuarioRutina>();

        using (SqlConnection cn = new SqlConnection(cnx))
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand("sp_listarRutinasPorUsuario", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    lista.Add(new UsuarioRutina
                    {
                        idUsuarioRutina = dr.GetInt32(0),
                        idUsuario = dr.GetInt32(1),
                        idRutina = dr.GetInt32(2),
                        nombreRutina = dr.GetString(3),
                        dificultad = dr.GetString(5),
                        metaSemanal = dr.GetInt32(6),
                        diasCumplidos = dr.GetInt32(7),
                        fechaAsignacion = dr.GetDateTime(8)
                    });
                }
            }
        }
        return lista;
    }

    // Agregar rutina al usuario
    public string AgregarUsuarioRutina(int idUsuario, int idRutina)
    {
        using (SqlConnection cn = new SqlConnection(cnx))
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand("sp_agregarUsuarioRutina", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
            cmd.Parameters.AddWithValue("@idRutina", idRutina);

            try
            {
                cmd.ExecuteNonQuery();
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }

    // Eliminar rutina del usuario
    public void EliminarUsuarioRutina(int idUsuarioRutina)
    {
        using (SqlConnection cn = new SqlConnection(cnx))
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand("sp_eliminarUsuarioRutina", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idUsuarioRutina", idUsuarioRutina);

            cmd.ExecuteNonQuery();
        }
    }

    // Marcar día cumplido
    public void ActualizarProgreso(int idUsuarioRutina)
    {
        using (SqlConnection cn = new SqlConnection(cnx))
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand("sp_actualizarProgresoRutina", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idUsuarioRutina", idUsuarioRutina);

            cmd.ExecuteNonQuery();
        }
    }
}
