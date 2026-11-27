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
    // await reader.readAsync() (dans le while) avance d'une ligne et renvoie false quand il y'en a plus
    // awant le 1er ReadAsync(), le reader n'est positionné sur aucune ligne
    public static async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new SqlConnection(ConnectionString.Value);
        await connection.OpenAsync(cancellationToken);

        await using SqlCommand command = connection.CreateCommand();

        // Toujours récuperer l'id de la ligne dans une table visé 
        // Pourquoi ?
        // Car c'est avec lui que vous allez faire d'autre requête SQL
        // Imaginer une interface web ou vous allez la liste de vos propriétaire
        // vous voulez cliqué sur un propriétaire grâce à l'id vous pourrez faire
        // SELECT * FROM Proprietaire Where Id = @Id

        // Même si l'utilisateur n'a pas besoin de l'id
        // vous en tant que DEVC oui
        command.CommandText = "SELECT Id, Nom, Prenom FROM Proprietaire";

        // Le SqlDataReader comme son nom l'indique
        // Sert à lire votre table ligne par ligne
        await using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        // vous faite une boucle car comme dans une liste en C#
        // pour lire chaque élément d'une liste vous avez besoin d'une boucle
        while(await reader.ReadAsync(cancellationToken)) // Lire chaque ligne une par une 
        {
            // ATTENTION j'ai accès que aux colonnes que ma requête renvoie
            //Console.WriteLine($"Accéder via la position des colonnes");
            //// List<string> colonnes = [Id, Nom, Prenom]
            //Console.WriteLine($"{reader[0]}, {reader[1]}, {reader[2]}");
            //Console.WriteLine();

            // RECOMMANDER
            Console.WriteLine("Accéder via les nom des colonnes");
            Console.WriteLine();
            Console.WriteLine($"{reader["Id"]}, {reader["Nom"]}, {reader["Prenom"]}");
        }


    }
}
