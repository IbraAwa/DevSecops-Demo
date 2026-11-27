using Domain.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Demo05.ExecuteNonQuery.Steps;

internal class InsererUneVoiture
{

    // SGBD SQl Server mes en cache les 1 000 premier Id
    // SI votre Sgbd crash
    // il by pass l'ancine cash et continue vers la millième supérieur
    // Les valeurs arrivent en paramètre de la méthode
    // => c'est Program.cs (ou plus tard un formulaire, une API...) qui choisit quoi insérer
    // => la méthode ne change plus, peu importe la voiture
    public static async Task ExecuteAsync(string marque, string modele, decimal prix, int proprietaireId, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new SqlConnection(ConnectionString.Value);
        await connection.OpenAsync(cancellationToken);

        await using SqlCommand command = connection.CreateCommand();

        command.CommandText = "INSERT INTO Voiture (Marque, Modele, Prix, ProprietaireId) " +
            "VALUES (@Marque, @Modele, @Prix, @ProprietaireId)";
        command.Parameters.Add("@Marque", SqlDbType.NVarChar, 50).Value = marque;
        command.Parameters.Add("@Modele", SqlDbType.NVarChar, 100).Value = modele;
        SqlParameter prixParam = command.Parameters.Add("@Prix", SqlDbType.Decimal);
        prixParam.Precision = 15;
        prixParam.Scale = 2;
        prixParam.Value = prix;

        command.Parameters.Add("@ProprietaireId", SqlDbType.Int).Value = proprietaireId;

        // ExecuteNonQueryAsync()
        // Execute une requête de manière asynchrone
        // et retoure toute les lignes que ça a affectée
        // Update => Retourne toute les lignes qui ont été update
        // delete => retourne toute les lignes on été delete
        // etc...
        //
        // "NonQuery" = pas une requête de LECTURE
        // => on l'utilise pour INSERT / UPDATE / DELETE (et CREATE, DROP...)
        // => on ne récupère AUCUNE donnée, juste un int = le nombre de lignes affectées
        //
        // Précisions sur le int renvoyé :
        // > 0  => nombre de lignes insérées / modifiées / supprimées
        // = 0  => la requête a marché mais aucune ligne ne correspondait (ex: WHERE qui ne trouve rien)
        // = -1 => SQL Server n'a pas envoyé de nombre de lignes :
        //         - pour un SELECT (pas fait pour ça => ExecuteScalar ou ExecuteReader)
        //         - pour un CREATE TABLE, DROP...
        //         - quand il y a un SET NOCOUNT ON (souvent dans les procédures stockées)
        //
        // ATTENTION : -1 ne veut PAS dire "erreur"
        // Si la requête plante (contrainte, mauvaise colonne...) ADO.NET lance une SqlException
        // on n'arrive même pas jusqu'au if
        int ligne = await command.ExecuteNonQueryAsync(cancellationToken);

        if(ligne > 0)
        {
            Console.WriteLine("Succès");
        }

        // -1 => SQL Server n'a pas compté les lignes (voir plus haut), pas forcément un problème
        if(ligne < 0)
        {
            Console.WriteLine("Problème");
        }

        if(ligne == 0 )
        {
            Console.WriteLine("Rien ne s'est passer");
        }
    }
}