using Domain.Shared.Models;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

namespace Demo04.DataReader;

// ça permet de mapper un objet vers un autre type objet
// ça permet de mapper ce qu'on reçoit de la DB en objet C#
public static class VoitureMapper
{
    // SqlDataReader
    // correspond à une ligne dans une table

    // Mapping
    // le mapping doit correspondre à la requete sql faite
    // dans notre cas la requête sql est : 
    // "SELECT Id, Marque, Modele, Prix FROM Voiture";
    // problème on ne prend pas le proprietaireId donc il ne faut pas essayer d'y accéder
    public static Voiture VersVoiture(SqlDataReader reader)
    {
        // reader["Id"] renvoie un objet
        // donc on va faire un cast explicite pour le mettre en int
        // Attention la ternaire est non nécessaire si votre colonne est not null
        // mais recommandé pourquoi ?
        // Car si votre colonne passe de not null à null ce sera un changement à faire moin
        // et aussi impossible d'oublier ce que vous avez déjà fait
        int id = reader["Id"] is DBNull || reader["Id"] is null ? 0 : (int)reader["Id"];
        string marque = reader["Marque"] is DBNull || reader["Marque"] is null ? "" : (string)reader["Marque"];

        // Si le client vient et me dit que maintenant le modele peut être nulle
        // je change la DB en retirant le not null 
        // Mon client va crée des voiture sans modèle
        // et si j'ai oublié de modifier mon mapper ça va throw une exception non gérer
        // voila pourquoi je gère le null directement même si la colonne est non null
        string modele = reader["Modele"] is DBNull || reader["Modele"] is null ? "" : (string)reader["Modele"];

        decimal prix = reader["Prix"] is DBNull || reader["Prix"] is null ? 0.00m : (decimal)reader["Prix"];
        //int proprietaireId = reader["ProprietaireId"] is DBNull || reader["ProprietaireId"] is null ? 0 : (int)reader["ProprietaireId"];

        return new Voiture(id, marque, modele, prix, 0);
    }
}
