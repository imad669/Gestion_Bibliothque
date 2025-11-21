using BibliothequeTP2.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BibliothequeTP2.DAL
{
    public class EmpruntRepository
    {
        // CREATE
        public void Emprunter(int idUsager, int idLivre, DateTime dateRetourPrevue)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    INSERT INTO Emprunts (IdUsager, IdLivre, DateEmprunt, DateRetourPrevue) 
                    VALUES (@u, @l, GETDATE(), @p);
                    UPDATE Livres SET QuantiteDisponible = QuantiteDisponible - 1 
                    WHERE IdLivre = @l AND QuantiteDisponible > 0", conn);

                cmd.Parameters.AddWithValue("@u", idUsager);
                cmd.Parameters.AddWithValue("@l", idLivre);
                cmd.Parameters.AddWithValue("@p", dateRetourPrevue);

                cmd.ExecuteNonQuery();
            }
        }

        // UPDATE (Retourner un livre)
        public void Retourner(int idLivre)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    UPDATE Emprunts SET DateRetourReel = GETDATE() 
                    WHERE IdLivre = @l AND DateRetourReel IS NULL;
                    UPDATE Livres SET QuantiteDisponible = QuantiteDisponible + 1 
                    WHERE IdLivre = @l", conn);

                cmd.Parameters.AddWithValue("@l", idLivre);
                cmd.ExecuteNonQuery();
            }
        }

        // READ (emprunts en cours pour un usager)
        public List<Emprunt> GetEmpruntsEnCours(int idUsager)
        {
            var liste = new List<Emprunt>();
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = @"
                    SELECT e.IdEmprunt, e.IdLivre, l.Titre, e.DateEmprunt, e.DateRetourPrevue
                    FROM Emprunts e
                    JOIN Livres l ON e.IdLivre = l.IdLivre
                    WHERE e.IdUsager = @id AND e.DateRetourReel IS NULL";

                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", idUsager);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        liste.Add(new Emprunt
                        {
                            IdEmprunt = reader.GetInt32(0),
                            IdLivre = reader.GetInt32(1),
                            TitreLivre = reader.GetString(2),
                            DateEmprunt = reader.GetDateTime(3),
                            DateRetourPrevue = reader.GetDateTime(4),
                            DateRetourReel = null
                        });
                    }
                }
            }
            return liste;
        }

        // READ (all)
        public List<Emprunt> GetAll()
        {
            var liste = new List<Emprunt>();
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    SELECT e.IdEmprunt, e.IdUsager, e.IdLivre, l.Titre, 
                           e.DateEmprunt, e.DateRetourPrevue, e.DateRetourReel
                    FROM Emprunts e
                    JOIN Livres l ON e.IdLivre = l.IdLivre", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        liste.Add(new Emprunt
                        {
                            IdEmprunt = reader.GetInt32(0),
                            IdUsager = reader.GetInt32(1),
                            IdLivre = reader.GetInt32(2),
                            TitreLivre = reader.GetString(3),
                            DateEmprunt = reader.GetDateTime(4),
                            DateRetourPrevue = reader.GetDateTime(5),
                            DateRetourReel = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6)
                        });
                    }
                }
            }
            return liste;
        }

        // READ (by Id)
        public Emprunt GetById(int id)
        {
            Emprunt e = null;
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    SELECT e.IdEmprunt, e.IdUsager, e.IdLivre, l.Titre, 
                           e.DateEmprunt, e.DateRetourPrevue, e.DateRetourReel
                    FROM Emprunts e
                    JOIN Livres l ON e.IdLivre = l.IdLivre
                    WHERE e.IdEmprunt = @id", conn);

                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        e = new Emprunt
                        {
                            IdEmprunt = reader.GetInt32(0),
                            IdUsager = reader.GetInt32(1),
                            IdLivre = reader.GetInt32(2),
                            TitreLivre = reader.GetString(3),
                            DateEmprunt = reader.GetDateTime(4),
                            DateRetourPrevue = reader.GetDateTime(5),
                            DateRetourReel = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6)
                        };
                    }
                }
            }
            return e;
        }

        // UPDATE (modifier un emprunt)
        public void Update(Emprunt e)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    UPDATE Emprunts 
                    SET IdUsager=@u, IdLivre=@l, DateEmprunt=@de, 
                        DateRetourPrevue=@drp, DateRetourReel=@drr
                    WHERE IdEmprunt=@id", conn);

                cmd.Parameters.AddWithValue("@id", e.IdEmprunt);
                cmd.Parameters.AddWithValue("@u", e.IdUsager);
                cmd.Parameters.AddWithValue("@l", e.IdLivre);
                cmd.Parameters.AddWithValue("@de", e.DateEmprunt);
                cmd.Parameters.AddWithValue("@drp", e.DateRetourPrevue);
                cmd.Parameters.AddWithValue("@drr", (object)e.DateRetourReel ?? DBNull.Value);

                cmd.ExecuteNonQuery();
            }
        }

        // DELETE
        public void Delete(int id)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM Emprunts WHERE IdEmprunt=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
