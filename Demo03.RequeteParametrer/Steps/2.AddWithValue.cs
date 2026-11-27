using Domain.Shared;
using Microsoft.Data.SqlClient;

namespace Demo03.RequeteParametrer.Steps;

internal class AddWithValue
{
    // Pour éviter les injection sql
    // 1er chose paramètrer sa requête 
    // 2eme chose de bien typer les paramètre de la méthode

    // AVANTAGE SUPER RAPIDE A ECRIRE
    public static async Task ExecuteAsync(int voitureId, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new(ConnectionString.Value);
        await connection.OpenAsync();
        using SqlCommand command = connection.CreateCommand();


        // Paramètrer ma requête au lieu de faire 
        // $"SELECT Marque FROM Voiture Where Id = '{voitureId}'";
        command.CommandText = $"SELECT Marque FROM Voiture Where Id = @voitureId";

        // DISCLAMAIRE
        // !! PROBLEME !!
        // cela peut causer un bug invisible qui sera vu que en PROD et compliqué à retrouver
        // AddWithValue infère le type SQL en regardant le type .net
        // string -> NVARCHAR
        // decimal -> Decimal (si nombre 3,12 il aurait fait decimal(3, 2))
        // Si votre colonne est de type decimal(15, 2)
        // problème AddWithValue va regarder le nombre et va lui decider de ce qu'il y'a entre parenthese dans decimal()
        // Si votre nombre c'est 1005,334 il va transformer de decimal => decimal(7, 3) alors que dans votre DB
        // c'est decimal(15,2)

        // Dans votre DB la colonne est de type decimal(15,2)
        // AddWithValue va regarder le 3.14m et va décider de le transformer en decimal(3, 2)
        // Petit soucis de cohésion entre le type dans la DB et le type passer passer en paramètre
        // Aucun bug mais si votre colonne est indexé il perd son index
        // un index permet quoi de filtrer/rechercher plus vite une information de table
        // si vous perdez votre index vous aller petit à petit perdre en eprformance
        // jusqu'a avor un appel client pas content qui va vous demander de régler le soucis
        // Et bonne chance pour trouver le soucis car BUG SILENCIEUX
        // command.Parameters.AddWithValue("@prix", 3.14m)

        // Dans votre DB la colonne est de type VARCHAR
        // AddWithValue va regarder "BMW" et va décider de le transformer en NVARCHAR
        // petit soucis de cohésion entre le type de la DB et le type passer en paramètre
        // Aucun bug tout va fonctionner
        // Maisi si y'a un index sur la colonne marque dans votre DB
        // l'index va sauter et au fur et a mesure du temps vous allez perdre en performance
        // jusqu'a la plainte du client
        //command.Parameters.AddWithValue("@marque", "BMW");

        command.Parameters.AddWithValue("@voitureId", voitureId);

        // possibilité d'avoir autant de parametère qu'on a besoin
        //int prix = 50;
        //command.Parameters.AddWithValue("@Prix", prix);

        // a parti de maintenant utilisation de cancellationToken
        // comme il faut
        object? marqueObj = await command.ExecuteScalarAsync(cancellationToken);

        string message = marqueObj switch
        {
            null or DBNull => $"Aucune marque trouver avec l'id {voitureId}",
            string marque => $"la marque de la voiture {voitureId} est {marque}",
            _ => "Marque non trouver"
        };

        Console.WriteLine(message);
    }
}
