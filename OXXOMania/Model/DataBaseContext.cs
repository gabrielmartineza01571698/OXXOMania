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

        public Usuario GetUsuarioLogin(string usr)
        {
            Usuario myUsuario = new Usuario();
            MySqlConnection conexion = new MySqlConnection(ConnectionString);
            conexion.Open();
            MySqlCommand cmd = new MySqlCommand("call GetUsuarioExistente(@usr)", conexion);
            cmd.Parameters.AddWithValue("@usr", usr);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    myUsuario.id_usuario = Convert.ToInt32(reader["id_usuario"]);
                    myUsuario.nombre = reader["nombre"].ToString();
                    myUsuario.apellido = reader["apellido"].ToString();
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
        public void AgregarUsuario(string nombre, string apellido, string user, string sucursal, string password)
        {
            MySqlConnection conexion = new MySqlConnection(ConnectionString);
            conexion.Open();
            MySqlCommand cmd = new MySqlCommand("call CrearUsuario(@nombre, @apellido, @user, @sucursal, @password)", conexion); // "" query con parametro
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@apellido", apellido);
            cmd.Parameters.AddWithValue("@user", user);
            cmd.Parameters.AddWithValue("@sucursal", sucursal);
            cmd.Parameters.AddWithValue("@password", password);

            cmd.ExecuteNonQuery();

            conexion.Close();
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

                MySqlCommand cmd = new MySqlCommand("call showPodium;", conexion);

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

    	// Vista Asesor (Mariangel)
        public List<Empleado> AgarrarHorarios(int id_lider)
        {
            List<Empleado> listaEmpleados = new List<Empleado>();

            using (MySqlConnection conexion = new MySqlConnection(ConnectionString))
            {
                conexion.Open();
                MySqlCommand cmd = new MySqlCommand("call MostrarEmpleadosPorLider(@id_lider);", conexion);
                cmd.Parameters.AddWithValue("@id_lider", id_lider);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Empleado e = new Empleado
                        {
                            nombre_lider = reader["nombre_lider"].ToString(),
                            nombre_empleado = reader["nombre"].ToString(),
                            apellido_empleado = reader["apellido"].ToString(),
                            horario_entrada = reader.GetTimeSpan(reader.GetOrdinal("horario_entrada")),
                            horario_salida = reader.GetTimeSpan(reader.GetOrdinal("horario_salida")),
                            dias_trabajo = reader["dias_trabajo"].ToString(),
                        };

                        listaEmpleados.Add(e);
                    }
                }
            }

            return listaEmpleados;
        }

        public List<Empleado> AgarrarTodosHorarios()
        {
            List<Empleado> listaEmpleados = new List<Empleado>();

            using (MySqlConnection conexion = new MySqlConnection(ConnectionString))
            {
                conexion.Open();
                MySqlCommand cmd = new MySqlCommand("select e.*, u.nombre as nombre_lider from empleados e inner join usuario u on u.id_usuario = e.id_lider_tienda;", conexion);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Empleado e = new Empleado
                        {
                            nombre_lider = reader["nombre_lider"].ToString(),
                            nombre_empleado = reader["nombre"].ToString(),
                            apellido_empleado = reader["apellido"].ToString(),
                            horario_entrada = reader.GetTimeSpan(reader.GetOrdinal("horario_entrada")),
                            horario_salida = reader.GetTimeSpan(reader.GetOrdinal("horario_salida")),
                            dias_trabajo = reader["dias_trabajo"].ToString(),
                        };

                        listaEmpleados.Add(e);
                    }
                }
            }

            return listaEmpleados;
        }
            
        public List<LideresAsesor> AgarrarLideresdeAsesor(int id_asesor)
        {
            List<LideresAsesor> listaLideres = new List<LideresAsesor>();

            using (MySqlConnection conexion = new MySqlConnection(ConnectionString))
            {
                conexion.Open();

                MySqlCommand cmd = new MySqlCommand("call LideresPorAsesor(@idAsesor);", conexion);
                cmd.Parameters.AddWithValue("@idAsesor", id_asesor);


                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        LideresAsesor u = new LideresAsesor
                        {
                            id_usuario = Convert.ToInt32(reader["id_usuario"]),
                            nombre = reader["nombre"].ToString(),
                            apellido = reader["apellido"].ToString(),
                            sucursal = reader["sucursal"].ToString(),
                            foto = Convert.ToInt32(reader["id_objeto"])
                        };

                        listaLideres.Add(u);
                    }
                }
            }

            return listaLideres;
        }
    }
}
