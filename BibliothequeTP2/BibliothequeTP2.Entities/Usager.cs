namespace BibliothequeTP2.Entities
{
    public class Usager
    {
        public int IdUsager { get; set; }

        // Informations principales
        public string Nom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;

        // Propriétés complémentaires pour enrichir les rapports
        public int NbEmprunts { get; set; }   // Nombre d’emprunts en cours ou historiques
        public bool Actif => NbEmprunts > 0; // Indique si l’usager a des emprunts actifs
    }
}
