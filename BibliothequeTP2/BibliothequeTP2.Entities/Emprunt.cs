using System;

namespace BibliothequeTP2.Entities
{
    public class Emprunt
    {
        public int IdEmprunt { get; set; }
        public DateTime DateEmprunt { get; set; }
        public DateTime DateRetourPrevue { get; set; }
        public DateTime? DateRetourReel { get; set; }

        // Relations
        public int IdLivre { get; set; }
        public int IdUsager { get; set; }

        // Informations complémentaires pour affichage/rapports
        public string TitreLivre { get; set; } = string.Empty;
        public string NomUsager { get; set; } = string.Empty;
        public string EmailUsager { get; set; } = string.Empty;
        public string TelephoneUsager { get; set; } = string.Empty;
    }
}
