USE BibliothequeTP2;
GO

-- =============================================
-- DONNÉES DE TEST - 100% IDENTIQUES À TA BASE ACTUELLE
-- =============================================

-- Usagers (exactement comme dans ta capture)
INSERT INTO Usagers (Nom, Email, Telephone) VALUES
('Martin Jean',      'jean.martin@email.fr',      '01-23-45-67-89'),
('Bernard Marie',    'marie.bernard@email.fr',    '02-34-56-78-90'),
('Dubois Pierre',    'pierre.dubois@email.fr',    '03-45-67-89-01'),
('Robert Luc',       'luc.robert@email.fr',       '05-67-89-01-23'),
('Richard Claire',   'claire.richard@email.fr',   '06-78-90-12-34'),
('imad',             NULL,                        NULL),
('oussama',          'oussama2004@gmail.com',     NULL),
('alex',             'alex123@gmail.com',         NULL),
('riyade',           'riyade@gmail.com',          NULL),
('david',            'david147@gmail.com',        NULL),
('amin',             'amin789@gmail.com',         '43845621346'),
('alex',             'alex456@gmail.com',         '4383999312');
GO

-- Livres (les 8 livres qui existent dans tes emprunts + titres réalistes)
INSERT INTO Livres (Titre, Auteur, Annee, ISBN, Categorie, QuantiteEnStock, QuantiteDisponible) VALUES
('Le Petit Prince',           'Antoine de Saint-Exupéry', 1943, '978-2070408504', 'Jeunesse',      10, 9),
('1984',                      'George Orwell',            1949, '978-0451524935', 'Dystopie',      8,  8),
('Harry Potter T1',           'J.K. Rowling',             1997, '978-2070584628', 'Fantasy',       12, 10),
('Da Vinci Code',             'Dan Brown',                2003, '978-2709624933', 'Thriller',      10, 9),
('L''Alchimiste',             'Paulo Coelho',             1988, '978-2290313411', 'Roman',         9,  9),
('Dune',                      'Frank Herbert',            1965, '978-2221132388', 'Science-fiction',11, 11),
('Le Seigneur des Anneaux',   'J.R.R. Tolkien',           1954, '978-2075134040', 'Fantasy',       10, 7),
('Germinal',                  'Émile Zola',               1885, '978-2253004226', 'Classique',     6,  6);
GO

-- Emprunts (exactement comme dans ta capture)
INSERT INTO Emprunts (DateEmprunt, DateRetourPrev, DateRetourReel, IdLivre, IdUsager) VALUES
('2025-11-01', '2025-11-15', NULL, 1, 1),   -- Martin Jean emprunte Le Petit Prince
('2025-11-10', '2025-11-24', NULL, 3, 3),   -- Dubois Pierre emprunte Harry Potter
('2025-11-19', '2025-12-01', NULL, 8, 1),   -- Martin Jean emprunte Le Seigneur des Anneaux
('2025-11-19', '2025-12-01', NULL, 8, 3),   -- Dubois Pierre emprunte aussi Le Seigneur des Anneaux
('2025-11-20', '2025-11-27', NULL, 4, 3);   -- Dubois Pierre emprunte Da Vinci Code
GO

-- Mise à jour du stock disponible (pour coller exactement à ta base)
UPDATE Livres SET QuantiteDisponible = 9  WHERE IdLivre = 1;  -- Petit Prince
UPDATE Livres SET QuantiteDisponible = 10 WHERE IdLivre = 3;  -- Harry Potter
UPDATE Livres SET QuantiteDisponible = 7  WHERE IdLivre = 8;  -- Seigneur des Anneaux
UPDATE Livres SET QuantiteDisponible = 9  WHERE IdLivre = 4;  -- Da Vinci Code
GO

PRINT 'Données de test insérées - 100% identiques à ta base actuelle !';
