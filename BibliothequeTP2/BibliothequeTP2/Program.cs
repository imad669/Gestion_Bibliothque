using BibliothequeTP2.DAL;
using BibliothequeTP2.Entities;
using System;
using System.Linq;
using System.Collections.Generic;

namespace BibliothequeTP2
{
    internal class Program
    {
        static readonly LivreRepository livreRepo = new LivreRepository();
        static readonly UsagerRepository usagerRepo = new UsagerRepository();
        static readonly EmpruntRepository empruntRepo = new EmpruntRepository();

        static void Main(string[] args)
        {
            Console.Title = "Bibliothèque TP2 - Version CRUD Structurée";
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("══════════════════════════════════════");
                Console.WriteLine("     MENU PRINCIPAL - BIBLIOTHÈQUE");
                Console.WriteLine("══════════════════════════════════════");
                Console.ResetColor();
                Console.WriteLine("1 - Gérer les LIVRES");
                Console.WriteLine("2 - Gérer les USAGERS");
                Console.WriteLine("3 - Gérer les EMPRUNTS");
                Console.WriteLine("0 - Quitter");
                Console.Write("\nVotre choix : ");
                string choix = Console.ReadLine()?.Trim();

                switch (choix)
                {
                    case "1": MenuLivres(); break;
                    case "2": MenuUsagers(); break;
                    case "3": MenuEmprunts(); break;
                    case "0":
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

        // === MENU LIVRES ===
        static void MenuLivres()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("════ GESTION DES LIVRES ════");
                Console.ResetColor();
                Console.WriteLine("1 - Lister les livres");
                Console.WriteLine("2 - Ajouter un livre");
                Console.WriteLine("3 - Modifier un livre");
                Console.WriteLine("4 - Supprimer un livre");
                Console.WriteLine("0 - Retour");
                Console.Write("\nVotre choix : ");
                string c = Console.ReadLine()?.Trim();

                switch (c)
                {
                    case "1": ListerLivres(); break;
                    case "2": AjouterLivre(); break;
                    case "3": ModifierLivre(); break;
                    case "4": SupprimerLivre(); break;
                    case "0": return;
                    default: MessageErreur("Choix invalide"); break;
                }
            }
        }

        static void ListerLivres()
        {
            var livres = livreRepo.GetAll();
            Console.WriteLine($"\nIl y a {livres.Count} livre(s) :");
            foreach (var l in livres)
                Console.WriteLine($"{l.IdLivre} - {l.Titre} | {l.Auteur} ({l.Annee}) | Stock: {l.QuantiteEnStock} | Dispo: {l.QuantiteDisponible}");
            Pause();
        }

        static void AjouterLivre()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("════ AJOUTER UN LIVRE ════");
            Console.ResetColor();

            Console.Write("Titre : "); string titre = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Auteur : "); string auteur = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Année (facultatif) : ");
            string anneeStr = Console.ReadLine()?.Trim();
            int? annee = int.TryParse(anneeStr, out int a) ? a : (int?)null;
            Console.Write("ISBN (facultatif) : "); string isbn = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Catégorie (facultatif) : "); string cat = Console.ReadLine()?.Trim() ?? "";
            int stock = DemanderEntier("Quantité en stock");

            var livre = new Livre
            {
                Titre = titre,
                Auteur = auteur,
                Annee = annee,
                ISBN = isbn,
                Categorie = cat,
                QuantiteEnStock = stock,
                QuantiteDisponible = stock
            };

            livreRepo.Add(livre);
            MessageSucces("Livre ajouté avec succès !");
        }

        static void ModifierLivre()
        {
            int id = DemanderEntier("ID du livre à modifier");
            var livre = livreRepo.GetById(id);
            if (livre == null) { MessageErreur("Livre introuvable"); return; }

            Console.Write("Nouveau titre : "); livre.Titre = Console.ReadLine();
            Console.Write("Nouvel auteur : "); livre.Auteur = Console.ReadLine();
            // Tu peux ajouter ici la modification de Année/ISBN/Catégorie/Stock si nécessaire.
            livreRepo.Update(livre);
            MessageSucces("Livre modifié !");
        }

        static void SupprimerLivre()
        {
            int id = DemanderEntier("ID du livre à supprimer");
            livreRepo.Delete(id);
            MessageSucces("Livre supprimé !");
        }

        // === MENU USAGERS ===
        static void MenuUsagers()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("════ GESTION DES USAGERS ════");
                Console.ResetColor();
                Console.WriteLine("1 - Lister les usagers");
                Console.WriteLine("2 - Ajouter un usager");
                Console.WriteLine("3 - Modifier un usager");
                Console.WriteLine("4 - Supprimer un usager");
                Console.WriteLine("0 - Retour");
                Console.Write("\nVotre choix : ");
                string c = Console.ReadLine()?.Trim();

                switch (c)
                {
                    case "1": ListerUsagers(); break;
                    case "2": AjouterUsager(); break;
                    case "3": ModifierUsager(); break;
                    case "4": SupprimerUsager(); break;
                    case "0": return;
                    default: MessageErreur("Choix invalide"); break;
                }
            }
        }

        static void ListerUsagers()
        {
            var usagers = usagerRepo.GetAll();
            Console.WriteLine($"\nIl y a {usagers.Count} usager(s) :");
            foreach (var u in usagers)
                Console.WriteLine($"{u.IdUsager} - {u.Nom} | {u.Email} | {u.Telephone}");
            Pause();
        }

        static void AjouterUsager()
        {
            Console.Clear();
            Console.WriteLine("=== AJOUTER UN USAGER ===");

            Console.Write("Nom : "); string nom = Console.ReadLine();
            Console.Write("Email : "); string email = Console.ReadLine();
            Console.Write("Téléphone : "); string tel = Console.ReadLine();

            usagerRepo.Add(new Usager { Nom = nom, Email = email, Telephone = tel });
            MessageSucces("Usager ajouté !");
        }

        static void ModifierUsager()
        {
            int id = DemanderEntier("ID de l'usager à modifier");
            var u = usagerRepo.GetById(id);
            if (u == null) { MessageErreur("Usager introuvable"); return; }

            Console.Write("Nouveau nom : "); u.Nom = Console.ReadLine();
            Console.Write("Nouvel email : "); u.Email = Console.ReadLine();
            Console.Write("Nouveau téléphone : "); u.Telephone = Console.ReadLine();
            usagerRepo.Update(u);
            MessageSucces("Usager modifié !");
        }

        static void SupprimerUsager()
        {
            int id = DemanderEntier("ID de l'usager à supprimer");
            usagerRepo.Delete(id);
            MessageSucces("Usager supprimé !");
        }

        // === MENU EMPRUNTS ===
        static void MenuEmprunts()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("════ GESTION DES EMPRUNTS ════");
                Console.ResetColor();
                Console.WriteLine("1 - Lister les emprunts");
                Console.WriteLine("2 - Emprunter un livre");
                Console.WriteLine("3 - Retourner un livre");
                Console.WriteLine("4 - Modifier un emprunt");
                Console.WriteLine("5 - Supprimer un emprunt");
                Console.WriteLine("0 - Retour");
                Console.Write("\nVotre choix : ");
                string c = Console.ReadLine()?.Trim();

                switch (c)
                {
                    case "1": ListerEmprunts(); break;
                    case "2": EmprunterLivre(); break;
                    case "3": RetournerLivre(); break;
                    case "4": ModifierEmprunt(); break;
                    case "5": SupprimerEmprunt(); break;
                    case "0": return;
                    default: MessageErreur("Choix invalide"); break;
                }
            }
        }

        static void ListerEmprunts()
        {
            var emprunts = empruntRepo.GetAll();
            Console.WriteLine($"\nIl y a {emprunts.Count} emprunt(s) :");
            foreach (var e in emprunts)
                Console.WriteLine($"{e.IdEmprunt} - Livre: {e.TitreLivre} | Usager: {e.IdUsager} | Emprunté le {e.DateEmprunt:dd/MM/yyyy} | Retour prévu {e.DateRetourPrevue:dd/MM/yyyy} | Retour réel {(e.DateRetourReel.HasValue ? e.DateRetourReel.Value.ToString("dd/MM/yyyy") : "-")}");
            Pause();
        }

        static void EmprunterLivre()
        {
            int idU = DemanderEntier("ID de l'usager");
            int idL = DemanderEntier("ID du livre");
            DateTime date = DemanderDate("Date de retour prévue (yyyy-MM-dd)");
            try
            {
                empruntRepo.Emprunter(idU, idL, date);
                MessageSucces("Emprunt enregistré !");
            }
            catch
            {
                MessageErreur("Impossible d'emprunter (livre indisponible ou usager inconnu).");
            }
        }

        static void RetournerLivre()
        {
            int idL = DemanderEntier("ID du livre à retourner");
            try
            {
                empruntRepo.Retourner(idL);
                MessageSucces("Retour enregistré !");
            }
            catch
            {
                MessageErreur("Aucun emprunt en cours pour ce livre.");
            }
        }

        static void ModifierEmprunt()
        {
            int id = DemanderEntier("ID de l'emprunt à modifier");
            var e = empruntRepo.GetById(id);
            if (e == null) { MessageErreur("Emprunt introuvable"); return; }

            e.DateRetourPrevue = DemanderDate("Nouvelle date de retour prévue (yyyy-MM-dd)");
            empruntRepo.Update(e);
            MessageSucces("Emprunt modifié !");
        }

        static void SupprimerEmprunt()
        {
            int id = DemanderEntier("ID de l'emprunt à supprimer");
            empruntRepo.Delete(id);
            MessageSucces("Emprunt supprimé !");
        }

        // === UTILITAIRES ===
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
                MessageErreur("Format de date invalide (utilisez yyyy-MM-dd).");
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
