using Domain.Shared;
using Microsoft.Data.SqlClient;

namespace Demo03.RequeteParametrer.Steps;

internal class SansParametre
{
    public static async Task ExecuteAsync(string voitureId, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new(ConnectionString.Value);
        await connection.OpenAsync();
        using SqlCommand command = connection.CreateCommand();

        // DISCLAMAIRE
        // Injection SQL possible
        // Pourquoi ?
        // Car je prend la valeur reçu en paramètre et le concatène directement dans la requete
        // donc votre db le considère comme une requête a part entière au lieu d'un
        // Problème de l'interpolation je peux faire
        // ' OR '1' = '1' UNION SELECT etc...
        // Donc j'injecte des requête SQL non voulu
        // et j'ai donc accès à d'autre donnée qui peuvent être confidentiel
        command.CommandText = $"SELECT Marque FROM Voiture Where Id = '{voitureId}'";
        
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
