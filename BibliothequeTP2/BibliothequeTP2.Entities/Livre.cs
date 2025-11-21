using System;

namespace BibliothequeTP2.Entities
{
    public class Livre
    {
        public int IdLivre { get; set; }
        public string Titre { get; set; } = string.Empty;
        public string Auteur { get; set; } = string.Empty;
        public int? Annee { get; set; }

        public string ISBN { get; set; } = string.Empty;
        public string Categorie { get; set; } = string.Empty;

        public int QuantiteEnStock { get; set; }
        public int QuantiteDisponible { get; set; }

        // Propriété calculée : nombre d’exemplaires empruntés
        public int NbEmpruntes => QuantiteEnStock - QuantiteDisponible;
    }
}
