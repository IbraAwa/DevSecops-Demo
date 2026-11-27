using Domain.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Demo06.AppelStoreProcedureAndTrigger.Steps;

internal class AppelProcedureStockerAdd
{
    // Procédure appelée : [dbo].[AddProprietaire]
    /*
        CREATE PROCEDURE [dbo].[AddProprietaire]
            @Prenom NVARCHAR(100),
            @Nom NVARCHAR(100),
            @Email NVARCHAR(100)
        AS
        BEGIN
            SET NOCOUNT ON
            INSERT INTO [dbo].[Proprietaire] ([Prenom], [Nom], [Email])
            VALUES (@Prenom, @Nom, @Email)

            SELECT CAST(SCOPE_IDENTITY() AS INT)
        END
     */

    // Prénom, nom et email arrivent en paramètre (choisis dans Program.cs)
    // La méthode renvoie l'id du propriétaire créé
    // on s'en sert dans Program.cs pour la démo du trigger
    public static async Task<int> ExecuteAsync(string prenom, string nom, string email, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new SqlConnection(ConnectionString.Value);
        await connection.OpenAsync(cancellationToken);

        await using SqlCommand command = connection.CreateCommand();

        // ÉTAPE 1 : dire à ADO.NET qu'on appelle une PROCÉDURE STOCKÉE
        // Par défaut CommandType = CommandType.Text
        // => le CommandText est vu comme une requête SQL
        // Avec StoredProcedure
        // => le CommandText est juste le NOM de la procédure
        //    ADO.NET s'occupe de faire le "EXEC AddProprietaire @Prenom, @Nom, @Email" à notre place
        command.CommandType = CommandType.StoredProcedure;

        // ÉTAPE 2 : le nom de la procédure (PAS de EXEC, PAS de parenthèses, PAS de paramètres)
        // "dbo.AddProprietaire" marche aussi
        command.CommandText = "AddProprietaire";

        // ÉTAPE 3 : les paramètres
        // Le nom du paramètre en C# doit être EXACTEMENT le même que dans la procédure (@Prenom, @Nom, @Email)
        // Sinon SQL Server répond : "Procedure or function 'AddProprietaire' expects parameter '@Prenom', which was not supplied."
        // L'ordre n'a pas d'importance, c'est le NOM qui compte
        //
        // Le 3ème argument (100) = la taille, la même que dans la procédure NVARCHAR(100)
        // Si on ne met pas la taille, ADO.NET prend la longueur du texte envoyé
        // => "Jean" donne NVARCHAR(4), "Jeanne" donne NVARCHAR(6)...
        command.Parameters.Add("@Prenom", SqlDbType.NVarChar, 100).Value = prenom;
        command.Parameters.Add("@Nom", SqlDbType.NVarChar, 100).Value = nom;

        // Email est UNIQUE dans la table Proprietaire
        // Si on envoie un email qui existe déjà => SqlException (violation de la contrainte UNIQUE)
        command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = email;

        // ÉTAPE 4 : choisir la bonne méthode Execute
        // Notre procédure fait un SELECT CAST(SCOPE_IDENTITY() AS INT) à la fin
        // => elle renvoie UNE seule valeur (1 ligne, 1 colonne)
        // => ExecuteScalar
        //
        // Pourquoi PAS ExecuteNonQuery ?
        // 1. ExecuteNonQuery ignore complètement le résultat du SELECT, on perdrait l'id
        // 2. La procédure fait SET NOCOUNT ON
        //    => SQL Server ne renvoie plus le nombre de lignes affectées
        //    => ExecuteNonQuery renverrait -1 alors que l'INSERT a bien marché
        //
        // Pourquoi PAS ExecuteReader ?
        // On pourrait, mais c'est sortir l'artillerie lourde pour lire une seule valeur
        object? idInsert = await command.ExecuteScalarAsync(cancellationToken);

        // On a fait le CAST en INT côté SQL donc on reçoit directement un int
        // (sans le CAST, SCOPE_IDENTITY() renvoie un decimal et le (int) planterait)
        if (idInsert is not int nouvelId)
        {
            throw new InvalidOperationException("La procédure n'a pas renvoyé d'id");
        }

        Console.WriteLine($"[ADD] Nouveau propriétaire inséré avec l'id {nouvelId}");

        return nouvelId;
    }
}
