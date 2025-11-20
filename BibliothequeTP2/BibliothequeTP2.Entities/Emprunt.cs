using System;

namespace BibliothequeTP2.Entities
{
    public class Emprunt
    {
        public int IdEmprunt { get; set; }
        public DateTime DateEmprunt { get; set; }
        public DateTime DateRetourPrevue { get; set; }
        public DateTime? DateRetourReel { get; set; }
        public int IdLivre { get; set; }
        public int IdUsager { get; set; }
        public string TitreLivre { get; set; } = string.Empty;
        public string NomUsager { get; set; } = string.Empty;
    }
}
