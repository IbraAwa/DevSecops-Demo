using Domain.Shared;
using Domain.Shared.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo04.DataReader.Exercices;

internal class GetProprietaireByFirstLetter
{
    public static async Task ExecuteAsync(char letter, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new(ConnectionString.Value);
        await connection.OpenAsync(cancellationToken);
        using SqlCommand command = connection.CreateCommand();

        // Les colonnes soft delete comme Actif ou IsDelete etc..
        // sont toujours pris dans un select
        // pour les besoin du DEV c'est comme l'id 99% du temps on le récupère
        command.CommandText = "SELECT Id, Prenom, Nom, Actif FROM Proprietaire WHERE Nom LIKE @letter";
        // Nom NVARCHAR(100) en DB => même taille
        command.Parameters.Add("@letter", System.Data.SqlDbType.NVarChar, 100).Value = $"{letter}%";

        using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        List<Proprietaire> proprietaires = new();
        while (await reader.ReadAsync(cancellationToken)) 
        {
            proprietaires.Add(MapperAmeliorer.ToProprietaire(reader));
        }


        foreach (Proprietaire proprietaire in proprietaires)
        {
            Console.WriteLine($"Le propriétaire numéro {proprietaire.Id}");
            Console.WriteLine($"Ce nomme : {proprietaire.Nom} {proprietaire.Prenom}");
            Console.WriteLine($"Actif ? {proprietaire.Actif}");
        }
    }
}