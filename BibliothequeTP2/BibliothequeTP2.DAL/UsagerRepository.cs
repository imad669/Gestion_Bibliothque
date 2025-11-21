using BibliothequeTP2.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BibliothequeTP2.DAL
{
    public class UsagerRepository
    {
        // CREATE
        public void Add(Usager u)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO Usagers (Nom, Email, Telephone) VALUES (@n, @e, @t)", conn);

                cmd.Parameters.AddWithValue("@n", u.Nom);
                cmd.Parameters.AddWithValue("@e", (object)u.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@t", (object)u.Telephone ?? DBNull.Value);

                cmd.ExecuteNonQuery();
            }
        }

        // READ (all)
        public List<Usager> GetAll()
        {
            var liste = new List<Usager>();
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT IdUsager, Nom, Email, Telephone FROM Usagers", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        liste.Add(new Usager
                        {
                            IdUsager = reader.GetInt32(0),
                            Nom = reader.GetString(1),
                            Email = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            Telephone = reader.IsDBNull(3) ? "" : reader.GetString(3)
                        });
                    }
                }
            }
            return liste;
        }

        // READ (by Id)
        public Usager GetById(int id)
        {
            Usager u = null;
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT IdUsager, Nom, Email, Telephone FROM Usagers WHERE IdUsager=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        u = new Usager
                        {
                            IdUsager = reader.GetInt32(0),
                            Nom = reader.GetString(1),
                            Email = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            Telephone = reader.IsDBNull(3) ? "" : reader.GetString(3)
                        };
                    }
                }
            }
            return u;
        }

        // UPDATE
        public void Update(Usager u)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "UPDATE Usagers SET Nom=@n, Email=@e, Telephone=@t WHERE IdUsager=@id", conn);

                cmd.Parameters.AddWithValue("@id", u.IdUsager);
                cmd.Parameters.AddWithValue("@n", u.Nom);
                cmd.Parameters.AddWithValue("@e", (object)u.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@t", (object)u.Telephone ?? DBNull.Value);

                cmd.ExecuteNonQuery();
            }
        }

        // DELETE
        public void Delete(int id)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM Usagers WHERE IdUsager=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
