using Dominio.Entidad.Entidad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

public class ejercicioDAO
{
    string cnx = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

    // Ejercicios globales por rutina
    public List<Ejercicio> ListarPorRutina(int idRutina)
    {
        var lista = new List<Ejercicio>();

        using (SqlConnection cn = new SqlConnection(cnx))
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM Ejercicios WHERE idRutina = @idRutina", cn);

            cmd.Parameters.AddWithValue("@idRutina", idRutina);

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    lista.Add(new Ejercicio
                    {
                        idEjercicio = dr.GetInt32(dr.GetOrdinal("idEjercicio")),
                        idRutina = dr.GetInt32(dr.GetOrdinal("idRutina")),
                        nombreEjercicio = dr["nombreEjercicio"].ToString(),
                        series = dr["series"] != DBNull.Value ? dr.GetInt32(dr.GetOrdinal("series")) : 0,
                        repeticiones = dr["repeticiones"].ToString()
                    });
                }
            }
        }

        return lista;
    }


    // Ejercicios que el usuario agregó de esa rutina
    public List<Ejercicio> ListarEjerciciosUsuarioPorRutina(int idUsuario, int idRutina)
    {
        var lista = new List<Ejercicio>();

        using (SqlConnection cn = new SqlConnection(cnx))
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand("sp_listarEjerciciosUsuarioPorRutina", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
            cmd.Parameters.AddWithValue("@idRutina", idRutina);

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    lista.Add(new Ejercicio
                    {
                        idEjercicio = dr.GetInt32(0),
                        idRutina = dr.GetInt32(1),
                        nombreEjercicio = dr.GetString(2),
                        series = dr.GetInt32(3),
                        repeticiones = dr.GetString(4),
                        fechaRegistro = dr.GetDateTime(5),
                        idEjercicioUsuario = dr.GetInt32(6)  // IMPORTANTE para eliminar
                    });
                }
            }
        }
        return lista;
    }

    // Agregar ejercicio al usuario
    public string AgregarEjercicioUsuario(int idUsuario, int idEjercicio)
    {
        using (SqlConnection cn = new SqlConnection(cnx))
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand("sp_agregarEjercicioUsuario", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
            cmd.Parameters.AddWithValue("@idEjercicio", idEjercicio);

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

    // Eliminar ejercicio del usuario
    public bool EliminarEjercicioUsuario(int idEjercicioUsuario)
    {
        using (SqlConnection cn = new SqlConnection(cnx))
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand(
                "DELETE FROM EjerciciosUsuario WHERE idEjercicioUsuario = @id", cn);

            cmd.Parameters.AddWithValue("@id", idEjercicioUsuario);
            return cmd.ExecuteNonQuery() > 0;
        }
    }


    public List<Ejercicio> ListarEjerciciosDelUsuario(int idUsuario)
    {
        List<Ejercicio> lista = new List<Ejercicio>();

        using (SqlConnection cn = new SqlConnection(cnx))
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand(@"
            SELECT eu.idEjercicioUsuario, e.idEjercicio, e.nombreEjercicio, 
                   e.series, e.repeticiones
            FROM EjerciciosUsuario eu
            INNER JOIN Ejercicios e ON eu.idEjercicio = e.idEjercicio
            WHERE eu.idUsuario = @idUsuario", cn);

            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new Ejercicio
                {
                    idEjercicioUsuario = dr.GetInt32(0),
                    idEjercicio = dr.GetInt32(1),
                    nombreEjercicio = dr.GetString(2),
                    series = dr.GetInt32(3),
                    repeticiones = dr.GetString(4)
                });
            }
        }

        return lista;
    }

    public List<Ejercicio> ListarTodos()
    {
        List<Ejercicio> lista = new List<Ejercicio>();

        using (SqlConnection cn = new SqlConnection(cnx))
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand(@"
            SELECT idEjercicio, nombreEjercicio, series, repeticiones, idRutina
            FROM Ejercicios
            ORDER BY idEjercicio ASC
        ", cn);

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new Ejercicio
                {
                    idEjercicio = dr.GetInt32(0),
                    nombreEjercicio = dr.GetString(1),
                    series = dr.GetInt32(2),
                    repeticiones = dr.GetString(3),
                    idRutina = dr.IsDBNull(4) ? null : (int?)dr.GetInt32(4)
                });
            }
        }

        return lista;
    }


}
