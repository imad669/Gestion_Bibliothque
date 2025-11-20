using BibliothequeTP2.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BibliothequeTP2.DAL
{
    public class EmpruntRepository
    {
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
    }
}
