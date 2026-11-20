/*
Script de post-déploiement
--------------------------------------------------------------------------------------
Ce script s'exécute APRÈS le déploiement de la base de données
(création des tables, vues, procédures stockées et triggers).

ATTENTION :
Ce script est rejoué à CHAQUE déploiement.
Il doit donc être "idempotent" : l'exécuter 1 fois ou 10 fois donne le même résultat.
On vérifie donc l'existence de chaque ligne avant de l'insérer (NOT EXISTS),
pour éviter les doublons.

Syntaxe SQLCMD utile :
    :r .\monfichier.sql          -> inclure un autre fichier
    :setvar TableName MyTable    -> déclarer une variable, utilisée avec $(TableName)
--------------------------------------------------------------------------------------
*/

-- =====================================================
-- INSERTION DES PROPRIÉTAIRES
-- =====================================================
-- On vérifie ligne par ligne grâce à l'Email (identifiant "métier" du propriétaire).
-- Avantage : si un propriétaire manque, il est ajouté sans toucher aux autres.

INSERT INTO [dbo].[Proprietaire] ([Prenom], [Nom], [Email], [Actif])
SELECT src.[Prenom], src.[Nom], src.[Email], src.[Actif]
FROM (VALUES
        (N'John',    N'Doe',     N'johndoe@hotmail.be',         1),
        (N'Marie',   N'Dupont',  N'marie.dupont@gmail.com',     1),
        (N'Lucas',   N'Martin',  N'lucas.martin@hotmail.com',   1),
        (N'Sophie',  N'Bernard', N'sophie.bernard@gmail.com',   1),
        (N'Thomas',  N'Petit',   N'thomas.petit@outlook.com',   1),
        (N'Emma',    N'Laurent', N'emma.laurent@hotmail.be',    1),
        (N'Noah',    N'Moreau',  N'noah.moreau@gmail.com',      0),
        (N'Julie',   N'Simon',   N'julie.simon@hotmail.com',    1),
        (N'Gabriel', N'Michel',  N'gabriel.michel@gmail.com',   1),
        (N'Camille', N'Garcia',  N'camille.garcia@outlook.com', 1)
     ) AS src ([Prenom], [Nom], [Email], [Actif])
WHERE NOT EXISTS (
    SELECT 1
    FROM [dbo].[Proprietaire] p
    WHERE p.[Email] = src.[Email]
);
GO

-- =====================================================
-- INSERTION DES VOITURES
-- =====================================================
-- On NE code PAS l'Id du propriétaire en dur (1, 2, 3...) :
-- avec une colonne IDENTITY, rien ne garantit que John aura l'Id 1
-- (suppression de lignes, reseed, ordre d'insertion différent...).
-- On retrouve donc l'Id via l'Email du propriétaire (JOIN).

INSERT INTO [dbo].[Voiture] ([Marque], [Modele], [Prix], [ProprietaireId])
SELECT src.[Marque], src.[Modele], src.[Prix], p.[Id]
FROM (VALUES
        (N'BMW',        N'Série 3',     35000.00,  N'johndoe@hotmail.be'),
        (N'Audi',       N'A3',          28000.00,  N'marie.dupont@gmail.com'),
        (N'Mercedes',   N'Classe A',    32000.00,  N'lucas.martin@hotmail.com'),
        (N'Volkswagen', N'Golf 8',      26000.00,  N'sophie.bernard@gmail.com'),
        (N'Peugeot',    N'308',         22000.00,  N'thomas.petit@outlook.com'),
        (N'Renault',    N'Clio 5',      18000.00,  N'emma.laurent@hotmail.be'),
        (N'Tesla',      N'Model 3',     42000.00,  N'noah.moreau@gmail.com'),
        (N'Toyota',     N'Corolla',     25000.00,  N'julie.simon@hotmail.com'),
        (N'Ford',       N'Focus',       21000.00,  N'gabriel.michel@gmail.com'),
        (N'Porsche',    N'911 Carrera', 115000.00, N'camille.garcia@outlook.com'),
        (N'BMW',        N'X5',          65000.00,  N'johndoe@hotmail.be'),
        (N'Audi',       N'Q5',          48000.00,  N'marie.dupont@gmail.com'),
        (N'Volkswagen', N'Tiguan',      36000.00,  N'sophie.bernard@gmail.com'),
        (N'Mercedes',   N'GLC',         55000.00,  N'thomas.petit@outlook.com'),
        (N'Peugeot',    N'3008',        31000.00,  N'julie.simon@hotmail.com')
     ) AS src ([Marque], [Modele], [Prix], [EmailProprietaire])
INNER JOIN [dbo].[Proprietaire] p
    ON p.[Email] = src.[EmailProprietaire]
WHERE NOT EXISTS (
    SELECT 1
    FROM [dbo].[Voiture] v
    WHERE v.[Marque]         = src.[Marque]
      AND v.[Modele]         = src.[Modele]
      AND v.[ProprietaireId] = p.[Id]
);
GO