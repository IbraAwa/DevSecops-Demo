using Domain.Shared.Models;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using Tools.Shared.Mapping;

namespace Demo04.DataReader;

// Un mapper permet de transformer un objet en un autre type d'objet
// Ici : transformer ce qu'on reçoit de la DB (une ligne du SqlDataReader) en objet C# (Voiture)
public static class VoitureMapper
{
    // SqlDataReader
    // correspond à la ligne en cours du résultat de la requête (pas à toute la table)

    // Mapping
    // le mapping doit correspondre à la requête SQL faite
    // dans notre cas la requête SQL est :
    // "SELECT Id, Marque, Modele, Prix FROM Voiture";
    // problème : on ne prend pas le ProprietaireId donc il ne faut pas essayer d'y accéder
    // (sinon IndexOutOfRangeException), d'où le 0 passé au constructeur de Voiture
    public static Voiture VersVoiture(SqlDataReader reader)
    {
        // reader["Id"] renvoie un object
        // donc on fait un cast explicite pour le mettre en int
        // Un NULL SQL arrive en C# sous forme de DBNull.Value (pas null) :
        // c'est le "is DBNull" qui compte, le "is null" est juste une sécurité en plus
        //
        // Attention la ternaire n'est pas nécessaire si votre colonne est NOT NULL, mais recommandée. Pourquoi ?
        // Ex : le client veut maintenant pouvoir créer des voitures sans modèle
        // => je retire le NOT NULL en DB, et si j'ai oublié de modifier mon mapper
        // le cast (string)DBNull.Value va throw une exception non gérée
        // En gérant le null dès le départ : un changement en moins à faire et impossible d'oublier
        int id = reader["Id"] is DBNull || reader["Id"] is null ? 0 : (int)reader["Id"];
        string marque = reader["Marque"] is DBNull || reader["Marque"] is null ? "" : (string)reader["Marque"];
        string modele = reader["Modele"] is DBNull || reader["Modele"] is null ? "" : (string)reader["Modele"];

        decimal prix = reader["Prix"] is DBNull || reader["Prix"] is null ? 0.00m : (decimal)reader["Prix"];
        //int proprietaireId = reader["ProprietaireId"] is DBNull || reader["ProprietaireId"] is null ? 0 : (int)reader["ProprietaireId"];

        return new Voiture(id, marque, modele, prix, 0);
    }
}
