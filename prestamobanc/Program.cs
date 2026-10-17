using System;
using Microsoft.Data.SqlClient; // Si usas .NET Core / .NET 6+
// using System.Data.SqlClient; // Descomenta esta línea si usas .NET Framework tradicional

namespace prestamobanc
{
    internal class Program
    {
        // Reemplaza el contenido entre comillas con tu cadena de conexión copiada de las Propiedades
        static string cadenaConexion = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BibliotecaDB;Integrated Security=True;";

        static void Main(string[] args)
        {
            string opcion = "";
            while (opcion != "0")
            {
                Console.Clear();
                MostrarMenu();
                opcion = Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1": CrearLector(); break;
                    case "2": ListarLectores(); break;
                    case "3": Console.WriteLine("\n[Actualizar Lector]"); break;
                    case "4": Console.WriteLine("\n[Eliminar Lector]"); break;
                    case "5": CrearLibro(); break;
                    case "6": ListarLibros(); break;
                    case "7": Console.WriteLine("\n[Actualizar Libro]"); break;
                    case "8": Console.WriteLine("\n[Eliminar Libro]"); break;
                    case "9": RegistrarPrestamo(); break;
                    case "10": ListarPrestamos(); break;
                    case "11": Console.WriteLine("\n[Actualizar Préstamo]"); break;
                    case "12": DevolverLibro(); break;
                    case "13": Console.WriteLine("\n[Eliminar Préstamo]"); break;
                    case "14": DemostrarPolimorfismo(); break;
                    case "0":
                        Console.WriteLine("\n¡Gracias por usar el sistema!");
                        break;
                    default:
                        Console.WriteLine("\nOpción no válida. Presiona cualquier tecla para reintentar...");
                        Console.ReadKey();
                        break;
                }

                if (opcion != "0" && opcion != "1" && opcion != "2" && opcion != "5" && opcion != "6" && opcion != "9" && opcion != "10" && opcion != "12" && opcion != "14")
                {
                    Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        // --- DIBUJO DEL MENÚ PRINCIPAL EN CONSOLA ---
        static void MostrarMenu()
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine("                SISTEMA DE PRESTAMOS DE BIBLIOTECA                 ");
            Console.WriteLine("==================================================================");
            Console.WriteLine("LECTORES:  1 Crear | 2 Listar | 3 Actualizar | 4 Eliminar");
            Console.WriteLine("LIBROS:    5 Crear | 6 Listar | 7 Actualizar | 8 Eliminar");
            Console.WriteLine("PRESTAMOS: 9 Crear | 10 Listar | 11 Actualizar | 12 Devolver | 13 Eliminar");
            Console.WriteLine("POO: 14 Demostrar polimorfismo | 0 Salir");
            Console.Write("Opcion: ");
        }

        // --- MÉTODOS DE BASE DE DATOS ---

        static void CrearLector()
        {
            Console.WriteLine("\n--- CREAR LECTOR ---");
            Console.Write("Nombre: ");
            string nombre = Console.ReadLine() ?? "";
            Console.Write("Email: ");
            string email = Console.ReadLine() ?? "";

            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                try
                {
                    con.Open();
                    string query = "INSERT INTO Usuarios (Nombre, Email) VALUES (@Nombre, @Email)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", nombre);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("\n¡Lector registrado con éxito!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nError: " + ex.Message);
                }
            }
            Console.WriteLine("Presiona cualquier tecla para volver...");
            Console.ReadKey();
        }

        static void ListarLectores()
        {
            Console.WriteLine("\n--- LISTA DE LECTORES ---");
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                try
                {
                    con.Open();
                    string query = "SELECT UsuarioID, Nombre, Email FROM Usuarios";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["UsuarioID"]} | Nombre: {reader["Nombre"]} | Email: {reader["Email"]}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nError: " + ex.Message);
                }
            }
            Console.WriteLine("Presiona cualquier tecla para volver...");
            Console.ReadKey();
        }

        static void CrearLibro()
        {
            Console.WriteLine("\n--- CREAR LIBRO ---");
            Console.Write("Título: ");
            string titulo = Console.ReadLine() ?? "";
            Console.Write("Autor: ");
            string autor = Console.ReadLine() ?? "";

            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                try
                {
                    con.Open();
                    string query = "INSERT INTO Libros (Titulo, Autor, Disponible) VALUES (@Titulo, @Autor, 1)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Titulo", titulo);
                        cmd.Parameters.AddWithValue("@Autor", autor);
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("\n¡Libro registrado con éxito!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nError: " + ex.Message);
                }
            }
            Console.WriteLine("Presiona cualquier tecla para volver...");
            Console.ReadKey();
        }

        static void ListarLibros()
        {
            Console.WriteLine("\n--- LISTA DE LIBROS ---");
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                try
                {
                    con.Open();
                    string query = "SELECT LibroID, Titulo, Autor, Disponible FROM Libros";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bool disp = Convert.ToBoolean(reader["Disponible"]);
                            string estado = disp ? "Disponible" : "Prestado";
                            Console.WriteLine($"ID: {reader["LibroID"]} | Título: {reader["Titulo"]} | Autor: {reader["Autor"]} | Estado: {estado}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nError: " + ex.Message);
                }
            }
            Console.WriteLine("Presiona cualquier tecla para volver...");
            Console.ReadKey();
        }

        static void RegistrarPrestamo()
        {
            Console.WriteLine("\n--- CREAR PRÉSTAMO ---");
            Console.Write("ID del Libro: ");
            int libroId = int.Parse(Console.ReadLine()!);
            Console.Write("ID del Lector: ");
            int usuarioId = int.Parse(Console.ReadLine()!);

            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                try
                {
                    con.Open();
                    SqlTransaction tx = con.BeginTransaction();

                    try
                    {
                        string qDisp = "SELECT Disponible FROM Libros WHERE LibroID = @LibroID";
                        using (SqlCommand cmd = new SqlCommand(qDisp, con, tx))
                        {
                            cmd.Parameters.AddWithValue("@LibroID", libroId);
                            object res = cmd.ExecuteScalar();

                            if (res == null || !Convert.ToBoolean(res))
                            {
                                tx.Rollback();
                                Console.WriteLine("\nEl libro no existe o ya está prestado.");
                                Console.WriteLine("Presiona cualquier tecla para volver...");
                                Console.ReadKey();
                                return;
                            }
                        }

                        string qIns = "INSERT INTO Prestamos (LibroID, UsuarioID, FechaPrestamo) VALUES (@LibroID, @UsuarioID, GETDATE())";
                        using (SqlCommand cmd = new SqlCommand(qIns, con, tx))
                        {
                            cmd.Parameters.AddWithValue("@LibroID", libroId);
                            cmd.Parameters.AddWithValue("@UsuarioID", usuarioId);
                            cmd.ExecuteNonQuery();
                        }

                        string qUpd = "UPDATE Libros SET Disponible = 0 WHERE LibroID = @LibroID";
                        using (SqlCommand cmd = new SqlCommand(qUpd, con, tx))
                        {
                            cmd.Parameters.AddWithValue("@LibroID", libroId);
                            cmd.ExecuteNonQuery();
                        }

                        tx.Commit();
                        Console.WriteLine("\n¡Préstamo realizado exitosamente!");
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nError: " + ex.Message);
                }
            }
            Console.WriteLine("Presiona cualquier tecla para volver...");
            Console.ReadKey();
        }

        static void ListarPrestamos()
        {
            Console.WriteLine("\n--- LISTA DE PRÉSTAMOS ---");
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                try
                {
                    con.Open();
                    string query = "SELECT P.PrestamoID, L.Titulo, U.Nombre, P.FechaPrestamo, P.FechaDevolucion " +
                                   "FROM Prestamos P " +
                                   "JOIN Libros L ON P.LibroID = L.LibroID " +
                                   "JOIN Usuarios U ON P.UsuarioID = U.UsuarioID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string dev = reader["FechaDevolucion"] == DBNull.Value ? "Pendiente" : Convert.ToDateTime(reader["FechaDevolucion"]).ToString("dd/MM/yyyy");
                            Console.WriteLine($"Préstamo #{reader["PrestamoID"]} | Libro: {reader["Titulo"]} | Lector: {reader["Nombre"]} | Dev: {dev}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nError: " + ex.Message);
                }
            }
            Console.WriteLine("Presiona cualquier tecla para volver...");
            Console.ReadKey();
        }

        static void DevolverLibro()
        {
            Console.WriteLine("\n--- DEVOLVER LIBRO ---");
            Console.Write("ID del Préstamo: ");
            int prestamoId = int.Parse(Console.ReadLine()!);
            Console.Write("ID del Libro a liberar: ");
            int libroId = int.Parse(Console.ReadLine()!);

            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                try
                {
                    con.Open();
                    SqlTransaction tx = con.BeginTransaction();

                    try
                    {
                        string qDev = "UPDATE Prestamos SET FechaDevolucion = GETDATE() WHERE PrestamoID = @PrestamoID";
                        using (SqlCommand cmd = new SqlCommand(qDev, con, tx))
                        {
                            cmd.Parameters.AddWithValue("@PrestamoID", prestamoId);
                            cmd.ExecuteNonQuery();
                        }

                        string qLib = "UPDATE Libros SET Disponible = 1 WHERE LibroID = @LibroID";
                        using (SqlCommand cmd = new SqlCommand(qLib, con, tx))
                        {
                            cmd.Parameters.AddWithValue("@LibroID", libroId);
                            cmd.ExecuteNonQuery();
                        }

                        tx.Commit();
                        Console.WriteLine("\n¡Libro devuelto con éxito!");
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nError: " + ex.Message);
                }