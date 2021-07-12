using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/Error")]
    public class ErrorController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Error error = new Error();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo,CodigoUsuario,Fecha,Fuente,Numero,Descripcion,Vista,Accion
                                                            FROM Error
                                                            Where Codigo= @Codigo", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Codigo", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        error.Codigo = sqlDataReader.GetInt32(0);
                        error.CodigoUsuario = sqlDataReader.GetInt32(1);
                        error.Fecha = sqlDataReader.GetDateTime(2);
                        error.Fuente = sqlDataReader.GetString(3);
                        error.Numero = sqlDataReader.GetString(4);
                        error.Descripcion = sqlDataReader.GetString(5);
                        error.Vista = sqlDataReader.GetString(6);
                        error.Accion = sqlDataReader.GetString(7);

                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Ok(error);

        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Error> errores = new List<Error>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo,CodigoUsuario,Fecha,Fuente,Numero,Descripcion,Vista,Accion
                                                            FROM Error", sqlConnection);



                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Error error = new Error();
                        error.Codigo = sqlDataReader.GetInt32(0);
                        error.CodigoUsuario = sqlDataReader.GetInt32(1);
                        error.Fecha = sqlDataReader.GetDateTime(2);
                        error.Fuente = sqlDataReader.GetString(3);
                        error.Numero = sqlDataReader.GetString(4);
                        error.Descripcion = sqlDataReader.GetString(5);
                        error.Vista = sqlDataReader.GetString(6);
                        error.Accion = sqlDataReader.GetString(7);

                        errores.Add(error);


                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Ok(errores);

        }



        [HttpPost]
        public IHttpActionResult Ingresar(Error error)
        {
            if (error == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO Error (CodigoUsuario,Fecha,Fuente,Numero,Descripcion,Vista,Accion) 
                                                                VALUES 
                                                                (@CodigoUsuario,@Fecha,@Fuente,@Numero,@Descripcion,@Vista,@Accion)", sqlConnection);


                    //sqlCommand.Parameters.AddWithValue("@Codigo", error.Codigo);
                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", error.CodigoUsuario);
                    sqlCommand.Parameters.AddWithValue("@Fecha", error.Fecha);
                    sqlCommand.Parameters.AddWithValue("@Fuente", error.Fuente);
                    sqlCommand.Parameters.AddWithValue("@Numero", error.Numero);
                    sqlCommand.Parameters.AddWithValue("@Descripcion", error.Descripcion);
                    sqlCommand.Parameters.AddWithValue("@Vista", error.Vista);
                    sqlCommand.Parameters.AddWithValue("@Accion", error.Accion);


                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }
            return Ok(error);
        }

        [HttpPut]
        public IHttpActionResult Actualizar(Error error)
        {
            if (error == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"UPDATE Error set 
                                                                CodigoUsuario = @CodigoUsuario,
                                                                Fecha = @Fecha,
                                                                Fuente = @Fuente,
                                                                Numero = @Numero,
                                                                Descripcion = @Descripcion,
                                                                Vista = @Vista,
                                                                Accion = @Accion
                                                            WHERE Codigo = @Codigo", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Codigo", error.Codigo);
                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", error.CodigoUsuario);
                    sqlCommand.Parameters.AddWithValue("@Fecha", error.Fecha);
                    sqlCommand.Parameters.AddWithValue("@Fuente", error.Fuente);
                    sqlCommand.Parameters.AddWithValue("@Numero", error.Numero);
                    sqlCommand.Parameters.AddWithValue("@Descripcion", error.Descripcion);
                    sqlCommand.Parameters.AddWithValue("@Vista", error.Vista);
                    sqlCommand.Parameters.AddWithValue("@Accion", error.Accion);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }

            return Ok(error);
        }

        [HttpDelete]
        public IHttpActionResult Eliminar(int id)
        {
            if (id < 1)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"DELETE Error 
                                                            WHERE Codigo = @Codigo", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Codigo", id);


                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }
            return Ok(id);
        }
    }
}
