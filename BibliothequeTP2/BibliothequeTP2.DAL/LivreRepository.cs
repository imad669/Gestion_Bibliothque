using BibliothequeTP2.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BibliothequeTP2.DAL
{
    public class LivreRepository
    {
        // CREATE
        public void Add(Livre l)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(
                    @"INSERT INTO Livres (Titre, Auteur, Annee, ISBN, Categorie, QuantiteEnStock, QuantiteDisponible) 
                      VALUES (@t, @a, @an, @isbn, @c, @qs, @qd)", conn);

                cmd.Parameters.AddWithValue("@t", l.Titre);
                cmd.Parameters.AddWithValue("@a", l.Auteur);
                cmd.Parameters.AddWithValue("@an", (object)l.Annee ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@isbn", (object)l.ISBN ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@c", (object)l.Categorie ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@qs", l.QuantiteEnStock);
                cmd.Parameters.AddWithValue("@qd", l.QuantiteDisponible);

                cmd.ExecuteNonQuery();
            }
        }

        // READ (all)
        public List<Livre> GetAll()
        {
            var livres = new List<Livre>();
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT IdLivre, Titre, Auteur, Annee, ISBN, Categorie, QuantiteEnStock, QuantiteDisponible FROM Livres", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        livres.Add(new Livre
                        {
                            IdLivre = reader.GetInt32(0),
                            Titre = reader.GetString(1),
                            Auteur = reader.GetString(2),
                            Annee = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                            ISBN = reader.IsDBNull(4) ? "" : reader.GetString(4),
                            Categorie = reader.IsDBNull(5) ? "" : reader.GetString(5),
                            QuantiteEnStock = reader.GetInt32(6),
                            QuantiteDisponible = reader.GetInt32(7)
                        });
                    }
                }
            }
            return livres;
        }

        // READ (by Id)
        public Livre GetById(int id)
        {
            Livre l = null;
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT IdLivre, Titre, Auteur, Annee, ISBN, Categorie, QuantiteEnStock, QuantiteDisponible FROM Livres WHERE IdLivre=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        l = new Livre
                        {
                            IdLivre = reader.GetInt32(0),
                            Titre = reader.GetString(1),
                            Auteur = reader.GetString(2),
                            Annee = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                            ISBN = reader.IsDBNull(4) ? "" : reader.GetString(4),
                            Categorie = reader.IsDBNull(5) ? "" : reader.GetString(5),
                            QuantiteEnStock = reader.GetInt32(6),
                            QuantiteDisponible = reader.GetInt32(7)
                        };
                    }
                }
            }
            return l;
        }

        // UPDATE
        public void Update(Livre l)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(
                    @"UPDATE Livres SET Titre=@t, Auteur=@a, Annee=@an, ISBN=@isbn, 
                      Categorie=@c, QuantiteEnStock=@qs, QuantiteDisponible=@qd 
                      WHERE IdLivre=@id", conn);

                cmd.Parameters.AddWithValue("@id", l.IdLivre);
                cmd.Parameters.AddWithValue("@t", l.Titre);
                cmd.Parameters.AddWithValue("@a", l.Auteur);
                cmd.Parameters.AddWithValue("@an", (object)l.Annee ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@isbn", (object)l.ISBN ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@c", (object)l.Categorie ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@qs", l.QuantiteEnStock);
                cmd.Parameters.AddWithValue("@qd", l.QuantiteDisponible);

                cmd.ExecuteNonQuery();
            }
        }

        // DELETE
        public void Delete(int id)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM Livres WHERE IdLivre=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
