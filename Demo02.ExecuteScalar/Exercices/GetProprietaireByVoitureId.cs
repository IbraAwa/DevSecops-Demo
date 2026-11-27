using Domain.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Demo02.ExecuteScalar.Exercices
{
    internal class GetProprietaireByVoitureId
    {
        // Déjà un INT protège un peu de l'injection SQL
        // pourquoi car ton appli empêche une écriture et donc
        // de concaténer une requete sql non voulu dans la requête de base
        public static async Task<string?> ExecuteAsync(int voitureId, CancellationToken cancellationToken = default)
        {
            using SqlConnection connection = new(ConnectionString.Value);
            await connection.OpenAsync();
            using SqlCommand command = connection.CreateCommand();

            command.CommandText =
                $"SELECT p.Nom FROM Proprietaire AS p LEFT JOIN Voiture AS v " +
                $"ON v.proprietaireId = p.Id WHERE v.Id = @voitureId";

            // Paramètrage de notre Sql pour éviter les injection SQL
            // Nom du paramètre (dans ma requête sql près du commandText)
            // Le type de paramètre attendu
            // .Value = pour lui mettre la valeur qu'on a choisis 
            command.Parameters.Add("@voitureId", SqlDbType.Int).Value = voitureId;

            object? nomObj = await command.ExecuteScalarAsync();

            return nomObj switch
            {
                null or DBNull => null,
                string nom => nom,
                _ => "Pas de propriétaire trouver"
            };
        }

        public static async Task<string?> ExecuteDoubleRequestAsync(int voitureId, CancellationToken cancellationToken = default)
        {
            using SqlConnection connection = new(ConnectionString.Value);
            await connection.OpenAsync();
            using SqlCommand command = connection.CreateCommand();

            // INTERDIT DE FAIRE : $"SELECT ProprietaireId FROM Voiture WHERE Id = {voitureId}";
            // Pourquoi ?
            // Car injection sql
            command.CommandText = "SELECT ProprietaireId FROM Voiture WHERE Id = @voitureId";
            command.Parameters.Add("@voitureId", SqlDbType.Int).Value = voitureId;

            object? proprietaireIdObj = await command.ExecuteScalarAsync();
            int proprietaireId = proprietaireIdObj switch
            {
                null or DBNull => 0,
                int proprietaireIdConversion => proprietaireIdConversion,
                _ => 0
            };


            string? nom = null;
            if (proprietaireId > 0)
            {
                command.CommandText = "SELECT Nom FROM Proprietaire WHERE Id = @proprietaireId";
                command.Parameters.Clear();

                command.Parameters.Add("@proprietaireId", SqlDbType.Int).Value = proprietaireId;

                object? proprietaireNomObj = await command.ExecuteScalarAsync();

                nom = proprietaireNomObj switch
                {
                    null or DBNull => null,
                    string proprietaireNom => proprietaireNom,
                    _ => null
                };
            }

            return nom;
        }
    }
}
