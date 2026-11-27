using Domain.Shared;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo04.DataReader.Steps;

internal class LireAvecIndexeurs
{
    // await reader.ReadAsync() (dans le while) avance d'une ligne et renvoie false quand il n'y en a plus
    // Avant le 1er ReadAsync(), le reader n'est positionné sur aucune ligne
    public static async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new SqlConnection(ConnectionString.Value);
        await connection.OpenAsync(cancellationToken);

        await using SqlCommand command = connection.CreateCommand();

        // Toujours récupérer l'id des lignes de la table visée
        // Pourquoi ? Car c'est avec lui que vous allez faire vos autres requêtes SQL
        // Ex : une interface web qui affiche la liste de vos propriétaires,
        // vous cliquez sur un propriétaire et grâce à l'id vous pourrez faire
        // SELECT * FROM Proprietaire WHERE Id = @Id
        // Même si l'utilisateur n'a pas besoin de l'id, vous en tant que DEV oui
        command.CommandText = "SELECT Id, Nom, Prenom FROM Proprietaire";

        // Le SqlDataReader, comme son nom l'indique,
        // sert à lire le résultat de votre requête ligne par ligne
        // (en lecture seule et uniquement vers l'avant : impossible de revenir sur une ligne déjà lue)
        // La connexion doit rester ouverte pendant toute la lecture
        await using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        // Vous faites une boucle car, comme pour une liste en C#,
        // pour lire chaque élément vous avez besoin d'une boucle
        while(await reader.ReadAsync(cancellationToken)) // Lit chaque ligne une par une
        {
            // ATTENTION je n'ai accès qu'aux colonnes que ma requête renvoie
            // (sinon IndexOutOfRangeException)
            // reader[...] renvoie un object => cast nécessaire pour l'utiliser comme int, string... (cf. VoitureMapper)
            //Console.WriteLine($"Accéder via la position des colonnes");
            //// List<string> colonnes = [Id, Nom, Prenom]
            Console.WriteLine($"{reader[0]}, {reader[1]}, {reader[2]}");
            //Console.WriteLine();

            // RECOMMANDÉ : si on change l'ordre des colonnes dans le SELECT,
            // l'accès par position lit la mauvaise colonne, l'accès par nom non
            Console.WriteLine("Accéder via les noms des colonnes");
            Console.WriteLine();
            Console.WriteLine($"{reader["Id"]}, {reader["Nom"]}, {reader["Prenom"]}");
        }


    }
}
