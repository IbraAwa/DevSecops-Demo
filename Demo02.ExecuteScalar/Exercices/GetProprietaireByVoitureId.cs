using Domain.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Demo02.ExecuteScalar.Exercices;

internal class GetProprietaireByVoitureId
{
    // Montrer la puissance des paramètrage en SQL
    // meme si un string en paramètre de la méthode
    public static async Task<string?> ExecuteAsync(string voitureId)
    {
        using SqlConnection connection = new(ConnectionString.Value);
        await connection.OpenAsync();
        using SqlCommand command = connection.CreateCommand();
        command.CommandText =
            "SELECT p.Nom FROM Proprietaire AS p LEFT JOIN Voiture AS v " +
            "ON v.proprietaireId = p.Id WHERE v.Id = @voitureId";

        command.Parameters.Add("@voitureId", SqlDbType.Int).Value = voitureId;

        object? prenomObj = await command.ExecuteScalarAsync();

        return prenomObj switch
        {
            null or DBNull => null,
            string prenom => prenom,
            _ => throw new InvalidCastException($"string attendu comme type mais à la place" +
            $"{prenomObj.GetType().Name}")
        };
    }

    public static async Task<string?> ExecuteAsync1(string voitureId)
    {
        using SqlConnection connection = new(ConnectionString.Value);
        using SqlCommand command = connection.CreateCommand();
        await connection.OpenAsync();

        command.CommandText = "SELECT ProprietaireId FROM Voiture WHERE Id = @voitureId";
        command.Parameters.Add("@voitureId", SqlDbType.Int).Value = voitureId;

        object? proprietaireIdObj = await command.ExecuteScalarAsync();
        int proprietaireId = proprietaireIdObj switch
        {
            null or DBNull => 0,
            int proprietaireIdConversion => proprietaireIdConversion,
            _ => throw new InvalidCastException($"Probleme de conversion de proprietaireId")
        };

        command.CommandText = "SELECT Nom FROM Proprietaire WHERE Id = @proprietaireId";
        // ATTENTION CLEAR LES PARAMETERS DE LA PREMIERE REQUETE
        command.Parameters.Clear();
        command.Parameters.Add("@proprietaireId", SqlDbType.Int).Value = proprietaireId;

        object? nomProprietaireObj = await command.ExecuteScalarAsync();
        return nomProprietaireObj switch
        {
            null or DBNull => null,
            string nomProprietaire => nomProprietaire,
            _ => throw new InvalidCastException($"string attendu comme type mais à la place" +
            $"{nomProprietaireObj.GetType().Name}")
        };
    }
}
