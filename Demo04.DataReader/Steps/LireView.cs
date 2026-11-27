
using Domain.Shared;
using Domain.Shared.Models;
using Microsoft.Data.SqlClient;

namespace Demo04.DataReader.Steps;

internal class LireView
{

    public static async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new SqlConnection(ConnectionString.Value);
        await connection.OpenAsync(cancellationToken);

        await using SqlCommand command = connection.CreateCommand();
        // Sans view a chaque besoin écrire une grande requête chiant au bout d'un moment
        //  command.CommandText = "SELECT [Id], [Prenom], [Nom], [Email] FROM  [dbo].[Proprietaire] WHERE Actif = 1;";
        command.CommandText = "SELECT * FROM V_Proprietaire";

        using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);


        // ReadAsync lit ligne par ligne
        // et renvoie false quand il n'y a plus de ligne
        // donc parfait pour notre boucle while

        List<Proprietaire> proprietaires = new();
        while (await reader.ReadAsync(cancellationToken))
        {
           proprietaires.Add(ProprietaireMapper.ViewToProprietaire(reader));
        }

        Console.WriteLine();
        Console.WriteLine("Les propriétaires encore actifs sont : ");
        foreach (Proprietaire proprietaire in proprietaires)
        {
            Console.WriteLine($"Le propriétaire numéro {proprietaire.Id}");
            Console.WriteLine($"Se nomme : {proprietaire.Nom} {proprietaire.Prenom}");
            Console.WriteLine($"Son email est {proprietaire.Email}");
            Console.WriteLine();
        }
    }
}
