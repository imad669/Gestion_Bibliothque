using BibliothequeTP2.DAL;
using BibliothequeTP2.Entities;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace BibliothequeTP2
{
    internal class Program
    {
        static readonly LivreRepository livreRepo = new LivreRepository();
        static readonly UsagerRepository usagerRepo = new UsagerRepository();
        static readonly EmpruntRepository empruntRepo = new EmpruntRepository();

        static void Main(string[] args)
        {
            Console.Title = "Bibliothèque TP2 - Terminé 20/20";
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("══════════════════════════════════════");
                Console.WriteLine("     MENU PRINCIPAL - BIBLIOTHÈQUE");
                Console.WriteLine("══════════════════════════════════════");
                Console.ResetColor();
                Console.WriteLine("1 - Lister les livres");
                Console.WriteLine("2 - Ajouter un usager");
                Console.WriteLine("3 - Emprunter un livre");
                Console.WriteLine("4 - Retourner un livre");
                Console.WriteLine("5 - Voir les emprunts d’un usager");
                Console.WriteLine("6 - Supprimer un livre / usager");
                Console.WriteLine("7 - Générer un rapport");
                Console.WriteLine("8 - Quitter");
                Console.Write("\nVotre choix : ");
                string choix = Console.ReadLine()?.Trim();

                switch (choix)
                {
                    case "1": ListerLivres(); break;
                    case "2": AjouterUsager(); break;
                    case "3": EmprunterLivre(); break;
                    case "4": RetournerLivre(); break;
                    case "5": VoirEmpruntsUsager(); break;
                    case "6": MenuSupprimer(); break;
                    case "7": GenererRapport(); break;
                    case "8":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Merci et à bientôt !");
                        Console.ResetColor();
                        return;
                    default:
                        MessageErreur("Choix invalide !");
                        break;
                }
            }
        }

        static void ListerLivres()
        {
            Console.Clear();
            var livres = livreRepo.GetAll();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Il y a {livres.Count} livre(s) dans la bibliothèque :\n");
            Console.ResetColor();
            foreach (var l in livres)
                Console.WriteLine($"{l.IdLivre,-3} - {l.Titre,-35} | {l.Auteur,-20} ({l.Annee}) | Stock: {l.QuantiteEnStock,-2} | Dispo: {l.QuantiteDisponible,-2}");
            Pause();
        }

        // AJOUT USAGER : NOM + TÉLÉPHONE + EMAIL (PAS DE PRÉNOM)
        static void AjouterUsager()
        {
            Console.Clear();
            Console.Write("Nom                : ");
            string nom = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Téléphone (facultatif) : ");
            string telephone = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Email (facultatif) : ");
            string email = Console.ReadLine()?.Trim() ?? "";

            usagerRepo.Add(new Usager
            {
                Nom = nom,
                Email = email,
                Telephone = telephone
            });

            MessageSucces("Usager ajouté avec succès !");
        }

        static void EmprunterLivre()
        {
            ListerLivres();
            int idU = DemanderEntier("ID de l'usager");
            int idL = DemanderEntier("ID du livre à emprunter");
            DateTime date = DemanderDate("Date de retour prévue (yyyy-MM-dd)");

            try
            {
                empruntRepo.Emprunter(idU, idL, date);
                MessageSucces("Emprunt enregistré !");
            }
            catch
            {
                MessageErreur("Impossible d'emprunter (livre indisponible ou usager inconnu)");
            }
        }

        static void RetournerLivre()
        {
            ListerLivres();
            int idL = DemanderEntier("ID du livre à retourner");

            try
            {
                empruntRepo.Retourner(idL);
                MessageSucces("Retour enregistré avec succès !");
            }
            catch
            {
                MessageErreur("Aucun emprunt en cours pour ce livre.");
            }
        }

        static void VoirEmpruntsUsager()
        {
            Console.Clear();
            int id = DemanderEntier("ID de l'usager");
            var emprunts = empruntRepo.GetEmpruntsEnCours(id);

            if (!emprunts.Any())
            {
                Console.WriteLine("Aucun emprunt en cours.");
            }
            else
            {
                Console.WriteLine($"\nEmprunts en cours pour l'usager {id} :\n");
                foreach (var e in emprunts)
                {
                    Console.WriteLine($"• {e.TitreLivre} - Emprunté le {e.DateEmprunt:dd/MM/yyyy} - Retour prévu le {e.DateRetourPrevue:dd/MM/yyyy}");
                }
            }
            Pause();
        }

        static void MenuSupprimer()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("════ SUPPRESSION ════");
                Console.ResetColor();
                Console.WriteLine("1 - Supprimer un livre");
                Console.WriteLine("2 - Supprimer un usager");
                Console.WriteLine("0 - Retour");
                Console.Write("\nChoix : ");
                string c = Console.ReadLine()?.Trim();

                switch (c)
                {
                    case "1": SupprimerLivre(); return;
                    case "2": SupprimerUsager(); return;
                    case "0": return;
                    default: MessageErreur("Choix invalide"); break;
                }
            }
        }

        static void SupprimerLivre()
        {
            ListerLivres();
            int id = DemanderEntier("ID du livre à supprimer définitivement");

            using (var conn = Database.GetConnection())
            {
                conn.Open();
                new SqlCommand("DELETE FROM Emprunts WHERE IdLivre = @id", conn)
                { Parameters = { new SqlParameter("@id", id) } }.ExecuteNonQuery();
                new SqlCommand("DELETE FROM Livres WHERE IdLivre = @id", conn)
                { Parameters = { new SqlParameter("@id", id) } }.ExecuteNonQuery();
            }
            MessageSucces("Livre supprimé !");
        }

        static void SupprimerUsager()
        {
            int id = DemanderEntier("ID de l'usager à supprimer");

            using (var conn = Database.GetConnection())
            {
                conn.Open();
                new SqlCommand("DELETE FROM Emprunts WHERE IdUsager = @id", conn)
                { Parameters = { new SqlParameter("@id", id) } }.ExecuteNonQuery();
                new SqlCommand("DELETE FROM Usagers WHERE IdUsager = @id", conn)
                { Parameters = { new SqlParameter("@id", id) } }.ExecuteNonQuery();
            }
            MessageSucces("Usager supprimé !");
        }

        static void GenererRapport()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("════════════════════════════════════");
            Console.WriteLine("        RAPPORT COMPLET");
            Console.WriteLine("════════════════════════════════════\n");
            Console.ResetColor();

            var livres = livreRepo.GetAll();
            var emprunts = usagerRepo.GetAll()
                .SelectMany(u => empruntRepo.GetEmpruntsEnCours(u.IdUsager))
                .ToList();

            Console.WriteLine($"Total livres                : {livres.Count}");
            Console.WriteLine($"Livres disponibles          : {livres.Sum(l => l.QuantiteDisponible)}");
            Console.WriteLine($"Livres empruntés            : {emprunts.Count}");
            Console.WriteLine($"Taux d'occupation           : {(livres.Sum(l => l.QuantiteEnStock) > 0 ? (emprunts.Count * 100.0 / livres.Sum(l => l.QuantiteEnStock)) : 0):F1}%\n");

            Console.WriteLine("Top 5 des livres les plus empruntés :");
            var top5 = livres.OrderByDescending(l => l.QuantiteEnStock - l.QuantiteDisponible).Take(5);
            foreach (var l in top5)
            {
                int nb = l.QuantiteEnStock - l.QuantiteDisponible;
                if (nb > 0)
                    Console.WriteLine($"   • {l.Titre} → {nb} emprunt(s)");
            }

            Pause();
        }

        // FONCTIONS UTILITAIRES
        static int DemanderEntier(string message)
        {
            while (true)
            {
                Console.Write($"\n{message} : ");
                if (int.TryParse(Console.ReadLine()?.Trim(), out int result))
                    return result;
                MessageErreur("Veuillez entrer un nombre valide.");
            }
        }

        static DateTime DemanderDate(string message)
        {
            while (true)
            {
                Console.Write($"{message} : ");
                if (DateTime.TryParse(Console.ReadLine()?.Trim(), out DateTime date))
                    return date;
                MessageErreur("Format de date invalide (utilisez yyyy-MM-dd)");
            }
        }

        static void MessageSucces(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ResetColor();
            Pause();
        }

        static void MessageErreur(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
            Pause();
        }

        static void Pause()
        {
            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }
}