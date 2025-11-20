using BibliothequeTP2.Entities;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BibliothequeTP2.DAL
{
    public class LivreRepository
    {
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
                            Annee = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                            ISBN = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Categorie = reader.IsDBNull(5) ? null : reader.GetString(5),
                            QuantiteEnStock = reader.GetInt32(6),
                            QuantiteDisponible = reader.GetInt32(7)
                        });
                    }
                }
            }

            return livres;
        }
    }
}