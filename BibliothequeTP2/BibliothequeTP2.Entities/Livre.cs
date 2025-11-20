using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliothequeTP2.Entities
{
    public class Livre
    {
        public int IdLivre { get; set; }
        public string Titre { get; set; } = string.Empty;
        public string Auteur { get; set; } = string.Empty;
        public int? Annee { get; set; }
        public string ISBN { get; set; }
        public string Categorie { get; set; }
        public int QuantiteEnStock { get; set; }
        public int QuantiteDisponible { get; set; }
    }
}