using Dominio.Entidad.Entidad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class rutinaDAO
{
    string cnx = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

    // Listar rutinas globales
    public List<Rutina> GetAll()
    {
        var lista = new List<Rutina>();

        using (SqlConnection cn = new SqlConnection(cnx))
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand("sp_listarRutinas", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    lista.Add(new Rutina
                    {
                        idRutina = dr.GetInt32(0),
                        nombreRutina = dr.GetString(1),
                        descripcion = dr.GetString(2),
                        dificultad = dr.GetString(3),
                        fechaCreacion = dr.GetDateTime(4)
                    });
                }
            }
        }
        return lista;
    }

    // Ejercicios de una rutina global
    public List<Ejercicio> ListarPorRutina(int idRutina)
    {
        var lista = new List<Ejercicio>();

        using (SqlConnection cn = new SqlConnection(cnx))
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand("sp_listarEjerciciosPorRutina", cn);
            cmd.CommandType = CommandType.StoredProcedure;
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
                        fechaRegistro = dr.GetDateTime(5)
                    });
                }
            }
        }

        return lista;
    }
}
