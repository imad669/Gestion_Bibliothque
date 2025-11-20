using BibliothequeTP2.DAL;
using BibliothequeTP2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliothequeTP2.BLL
{
    public class RapportServices
    {
        private readonly LivreRepository _livreRepo = new LivreRepository();
        private readonly UsagerRepository _usagerRepo = new UsagerRepository();
        private readonly EmpruntRepository _empruntRepo = new EmpruntRepository();

        public void AfficherRapportGlobal()
        {
            var livres = _livreRepo.GetAll();
            var tousEmprunts = _usagerRepo.GetAll()
                .SelectMany(u => _empruntRepo.GetEmpruntsEnCours(u.IdUsager))
                .ToList();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    RAPPORT COMPLET - BIBLIOTHÈQUE        ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════╝\n");
            Console.ResetColor();

            Console.WriteLine($"Date du rapport        : {System.DateTime.Now:dddd dd MMMM yyyy HH:mm}");
            Console.WriteLine($"Total livres           : {livres.Count}");
            Console.WriteLine($"Livres disponibles     : {livres.Sum(l => l.QuantiteDisponible)}");
            Console.WriteLine($"Livres empruntés       : {tousEmprunts.Count}");
            Console.WriteLine($"Taux d'occupation      : {(livres.Count > 0 ? (tousEmprunts.Count * 100.0 / livres.Sum(l => l.QuantiteEnStock)) : 0):F1} %\n");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("TOP 5 DES LIVRES LES PLUS EMPRUNTÉS");
            Console.ResetColor();
            var top5 = livres.OrderByDescending(l => l.QuantiteEnStock - l.QuantiteDisponible).Take(5);
            foreach (var l in top5)
            {
                int emprunte = l.QuantiteEnStock - l.QuantiteDisponible;
                if (emprunte > 0)
                    Console.WriteLine($"   • {l.Titre} - {emprunte} emprunt(s)");
            }

            if (top5.All(l => l.QuantiteEnStock - l.QuantiteDisponible == 0))
                Console.WriteLine("   Aucun emprunt enregistré pour le moment.");

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        public void AfficherRapportUsager(int idUsager)
        {
            var emprunts = _empruntRepo.GetEmpruntsEnCours(idUsager);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($" RAPPORT DES EMPRUNTS - USAGER ID {idUsager}");
            Console.WriteLine(new string('═', 60));
            Console.ResetColor();

            if (!emprunts.Any())
            {
                Console.WriteLine("Aucun emprunt en cours.");
            }
            else
            {
                Console.WriteLine($"{"LIVRE",-35} {"EMPRUNTÉ LE",-12} {"RETOUR PRÉVU",-12}");
                Console.WriteLine(new string('─', 60));
                foreach (var e in emprunts)
                {
                    string titre = e.TitreLivre.Length > 33 ? e.TitreLivre.Substring(0, 30) + "..." : e.TitreLivre;
                    Console.WriteLine($"{titre,-35} {e.DateEmprunt:dd/MM/yyyy}   {e.DateRetourPrevue:dd/MM/yyyy}");
                }
            }
            Console.WriteLine("\nAppuyez sur une touche...");
            Console.ReadKey();
        }
    }
}