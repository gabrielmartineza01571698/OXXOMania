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


        // Karla

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
            MySqlCommand cmd = new MySqlCommand("call CrearUsuario(@nombre, @apellido, @user, @sucursal, @password)", conexion);
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@apellido", apellido);
            cmd.Parameters.AddWithValue("@user", user);
            cmd.Parameters.AddWithValue("@sucursal", sucursal);
            cmd.Parameters.AddWithValue("@password", password);

            cmd.ExecuteNonQuery();

            conexion.Close();
        }

        // Podium (Kevin)
        public int AgarrarCabeza(int id_usuario)
        {
            MySqlConnection conexion = new MySqlConnection(ConnectionString);
            conexion.Open();
            MySqlCommand cmd = new MySqlCommand("call agarrarCabeza(@id);", conexion);
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

        public List<Reconocimientos> GetReconocimientos(int id_destinatario)
        {
            List<Reconocimientos> listaReconocimientos = new List<Reconocimientos>();

            using (MySqlConnection conexion = new MySqlConnection(ConnectionString))
            {
                conexion.Open();

                MySqlCommand cmd = new MySqlCommand("call getReconocimientos(@id_destinatario);", conexion);
                cmd.Parameters.AddWithValue("@id_destinatario", id_destinatario);


                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Reconocimientos r = new Reconocimientos
                        {
                            id_reconocimiento = Convert.ToInt32(reader["id_reconocimiento"]),
                            id_destinatario = Convert.ToInt32(reader["id_destinatario"]),
                            id_remitiente = Convert.ToInt32(reader["id_remitiente"]),
                            nombre_remitiente = reader["nombre"].ToString(),
                            apellido_remitiente = reader["apellido"].ToString(),
                            tipo = reader["tipo"].ToString(),
                            descripcion = reader["descripcion"].ToString(),
                            fecha = reader["fecha"].ToString()
                        };

                        listaReconocimientos.Add(r);
                    }
                }
            }

            return listaReconocimientos;
        }

        public List<Usuario> GetDestinatarios(int id_remitiente)
        {
            List<Usuario> listaDestinatarios = new List<Usuario>();

            using (MySqlConnection conexion = new MySqlConnection(ConnectionString))
            {
                conexion.Open();

                MySqlCommand cmd = new MySqlCommand("call getDestinatarios(@id_remitiente);", conexion);
                cmd.Parameters.AddWithValue("@id_remitiente", id_remitiente);


                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Usuario u = new Usuario
                        {
                            id_usuario = Convert.ToInt32(reader["id_usuario"]),
                            nombre = reader["nombre"].ToString(),
                            apellido = reader["apellido"].ToString(),
                            usuario = reader["usuario"].ToString()
                        };

                        listaDestinatarios.Add(u);
                    }
                }
            }

            return listaDestinatarios;
        }
        
        public void agregarReconocimiento(int id_destinatario, int id_remitiente, string tipo, string descripcion)
        {
            MySqlConnection conexion = new MySqlConnection(ConnectionString);
            conexion.Open();
            MySqlCommand cmd = new MySqlCommand("call crearReconocimiento(@id_destinatario, @id_remitiente, @tipo, @descripcion)", conexion);
            cmd.Parameters.AddWithValue("@id_destinatario", id_destinatario);
            cmd.Parameters.AddWithValue("@id_remitiente", id_remitiente);
            cmd.Parameters.AddWithValue("@tipo", tipo);
            cmd.Parameters.AddWithValue("@descripcion", descripcion);

            cmd.ExecuteNonQuery();

            conexion.Close();
        }

    }
}
