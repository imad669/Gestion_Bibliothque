using BibliothequeTP2.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BibliothequeTP2.DAL
{
    public class UsagerRepository
    {
        public List<Usager> GetAll()
        {
            var liste = new List<Usager>();
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT IdUsager, Nom, Email, Telephone FROM Usagers", conn); // ← AJOUT Telephone
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        liste.Add(new Usager
                        {
                            IdUsager = reader.GetInt32(0),
                            Nom = reader.GetString(1),
                            Email = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            Telephone = reader.IsDBNull(3) ? "" : reader.GetString(3) // ← Lecture du téléphone
                        });
                    }
                }
            }
            return liste;
        }

        public void Add(Usager u)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO Usagers (Nom, Email, Telephone) VALUES (@n, @e, @t)", conn); // ← AJOUT Telephone

                cmd.Parameters.AddWithValue("@n", u.Nom);
                cmd.Parameters.AddWithValue("@e", (object)u.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@t", (object)u.Telephone ?? DBNull.Value); // ← Insertion du téléphone

                cmd.ExecuteNonQuery();
            }
        }
    }
}