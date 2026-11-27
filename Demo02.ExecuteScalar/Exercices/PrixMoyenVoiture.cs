using Domain.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Demo02.ExecuteScalar.Exercices;

internal class PrixMoyenVoiture
{
    // méthode static
    // je peux l'appeler sans crée d'objet PrixMoyenVoiture
    // il me suffit d'écrire PrixMyenVoiture.NomMéthode();
    public static async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        // SqlConnection sert à crée un lien entre mon application C#
        // et ma base de donnée Sql Server
        using SqlConnection connection = new(ConnectionString.Value);
        // vu que ma méthode est async
        // je peux utilisé await 
        await connection.OpenAsync();

        //Maintenant que j'ai fais le lien entre ma DB et mon appli et que je l'ai ouvert
        // il faut crée ma commande sql qui sera envoyé à ma DB

        using SqlCommand command = connection.CreateCommand();

        // CommandText possède ma requête SQL
        // Qui sera envoyé à ma DB
        command.CommandText = "SELECT AVG(Prix) FROM Voiture;";

        // Par défaut le type de command est du texte SQL
        // MAis si vous appelez une procédure stocké il faut précisé le type
        command.CommandType = CommandType.Text; // Non obligatoire si requête sql en forme de texte

        // A chaque fois que j'appel une ressource externe (DB, disque, fichier etc..)
        // utilisation de await pour ne pas attendre la réponse 
        object? prixMoyenStock = await command.ExecuteScalarAsync(); // DBNull

        //Pattern matching 
        // Je regade à l'intérieur de prixMoyenStock
        // Et selon certaine conditon je vais remplir ma variable string message
        // d'une valeur
        string message = prixMoyenStock switch
        {
            null or DBNull => $"La requête SQL a renvoyer une valeur null",
            // Quand y'a un switch et qu'on fait decimal prix ou int prix ou string message
            // => prixMoyenStock is decimal 
            // il regarde si el prixMoyenStock est de type decimal
            decimal prixMoyen => $"le prix moyen de toute les voitures en stock est {prixMoyen}",
            // Underscore si aucune des conditions au-dessus à été respecter
            // Souvent throw une exception moi ce sera un simple message
            _ => $"La requête envoyer à le type suivant: {prixMoyenStock.GetType().Name}, mais decimal attendu"
        };
        
        Console.WriteLine(message);
    }
}
