using Domain.Shared;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo03.RequeteParametrer.Steps
{
    internal class Moderne
    {
        public static async Task ExecuteAsync(string voitureId, CancellationToken cancellationToken = default)
        {
            using SqlConnection connection = new(ConnectionString.Value);
            await connection.OpenAsync();
            using SqlCommand command = connection.CreateCommand();

            command.CommandText = $"SELECT Marque FROM Voiture Where Id = @voitureId";
            SqlParameter voitureIdParameter = new SqlParameter("@voitureId", System.Data.SqlDbType.Int)
            {
                Value = voitureId
            };
            command.Parameters.Add(voitureIdParameter);
            //command.Parameters.Add(new SqlParameter("@voitureId", System.Data.SqlDbType.Int)
            //{
            //    Value = voitureId
            //});


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
}
