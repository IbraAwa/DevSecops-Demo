using Domain.Shared;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo02.ExecuteScalar.Exercices;

internal class GetProprietaireById
{
    // première chose pour éviter sqlInjection
    // utilisé le type int quand on peut
    // ainsi aucun texte peut être introduit uniquement des nombre
    // Mais ce n'est pas suffisant !
    public static async Task<string?> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new(ConnectionString.Value);
        await connection.OpenAsync();
        using SqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT Nom FROM Proprietaire WHERE Id = @id";

        // Trois manière : 
        // Explicite
        // le meilleur (selon moi) 
        command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;

        // Peu recommandé mais rapide
        // Pourquoi peu recommandé ?
        // la méthode infère le type via quel type .net il a
        // string converti en NVARCHAR
        // Si colonne en VARCHAR BUG SILENCIEUX
        // EXPLIQUER APRES EN DETAILS

        // même avec les decimal
        // si DECIMAL = 3,12
        // AdWithValue infere decimal(3,2) (3 chiffre dont un après la virugle)
        // mais si DB de type decimal(18,2) ERREUR
        //command.Parameters.AddWithValue("@id", id);

        // Moderne mais super long
        //command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int) { Value = id });

        object? proprietaireNomObj = await command.ExecuteScalarAsync();

        return proprietaireNomObj switch
        {
            null or DBNull => null,
            string proprietaireNom => proprietaireNom,
            _ => throw new InvalidCastException($"type string attendu mais {proprietaireNomObj.GetType().Name} à la place")
        };
    }
}
