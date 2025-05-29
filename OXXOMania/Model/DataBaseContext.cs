using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace OXXOMania.Model
{
    public class DataBaseContext
    {
        public string ConnectionString { get; set; }

        public DataBaseContext()
        {
            ConnectionString = "Server=mysql-174e7f49-tec-7e55.h.aivencloud.com;Port=11595;Database=OXXOMania;Uid=avnadmin;password=AVNS_fRuRcGKq34nw0MHFPCd;";
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public Usuario GetUsuarioLogin(string usr) //solo recibe un usuario
        {
            Usuario myUsuario = new Usuario();
            MySqlConnection conexion = new MySqlConnection(ConnectionString);
            conexion.Open();
            MySqlCommand cmd = new MySqlCommand("Select * from usuario where usuario = @usr", conexion); // "" query con parametro
            cmd.Parameters.AddWithValue("@usr", usr);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    myUsuario.id_usuario = Convert.ToInt32(reader["id_usuario"]);
                    myUsuario.nombre = reader["nombre"].ToString();
                    myUsuario.apellido = reader["apellido"].ToString();
                    myUsuario.correo = reader["correo"].ToString();
                    myUsuario.usuario = reader["usuario"].ToString();
                    myUsuario.contraseña = reader["contraseña"].ToString();
                    myUsuario.monedas = Convert.ToInt32(reader["monedas"]);
                    myUsuario.score = Convert.ToInt32(reader["score"]);
                    myUsuario.id_asesor = Convert.ToInt32(reader["id_asesor"]);
                    myUsuario.sucursal = reader["sucursal"].ToString();
                    myUsuario.tipo_usuario = reader["tipo_usuario"].ToString();
                }
                else
                {
                    myUsuario.usuario = "";
                }
            }
            conexion.Close();
            return myUsuario;
        }
        public void AgregarUsuario(string nombre, string apellido, string user, string sucursal, string password) //solo recibe un usuario
        {
            MySqlConnection conexion = new MySqlConnection(ConnectionString);
            conexion.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO `usuario` (nombre,apellido,usuario,sucursal,contraseña,monedas,score) VALUES (@nombre, @apellido, @user, @sucursal, @password,0,0)", conexion); // "" query con parametro
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@apellido", apellido);
            //cmd.Parameters.AddWithValue("@correo", correo);
            cmd.Parameters.AddWithValue("@user", user);
            cmd.Parameters.AddWithValue("@sucursal", sucursal);
            cmd.Parameters.AddWithValue("@password", password);

            cmd.ExecuteNonQuery();

            conexion.Close();
            //creo q le falta
        }

        public int AgarrarCabeza(int id_usuario) //solo recibe un usuario
        {
            MySqlConnection conexion = new MySqlConnection(ConnectionString);
            conexion.Open();
            MySqlCommand cmd = new MySqlCommand("call agarrarCabeza(@id);", conexion); // "" query con parametro
            cmd.Parameters.AddWithValue("@id", id_usuario);
            int cabeza = 0;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    cabeza = Convert.ToInt32(reader["id_objeto"]);
                }
                else
                {
                    cabeza = 4;
                }
            }

            conexion.Close();
            return cabeza;
        }


        public List<PodiumUsuario> AgarrarLugares()
            {
                List<PodiumUsuario> listaUsuarios = new List<PodiumUsuario>();

                using (MySqlConnection conexion = new MySqlConnection(ConnectionString))
                {
                    conexion.Open();
                    string query = "call showPodium;";

                    MySqlCommand cmd = new MySqlCommand(query, conexion);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PodiumUsuario u = new PodiumUsuario
                            {
                                id_usuario = Convert.ToInt32(reader["id_usuario"]),
                                nombre = reader["nombre"].ToString(),
                                apellido = reader["apellido"].ToString(),
                                monedas = Convert.ToInt32(reader["monedas"]),
                                score = Convert.ToInt32(reader["score"]),
                                cabeza = Convert.ToInt32(reader["id_objeto"])
                            };

                            listaUsuarios.Add(u);
                        }
                    }
                }

                return listaUsuarios;
            }
    }
}
