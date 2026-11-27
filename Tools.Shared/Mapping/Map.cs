
using Domain.Shared.Models;
using Microsoft.Data.SqlClient;
using Tools.Shared.Mapping;

namespace Domain.Shared.Mappers;

public static class Map
{
    public static Voiture ToVoiture(SqlDataReader reader)
    {
        int id = reader.Get<int>("Id"); // SI null = 0
        string? marque = reader.Get<string>("Marque") ?? ""; // si null = chaine caractère vide
        string? modele = reader.Get<string>("Modele") ?? ""; // si null = chaine caractère vide
        decimal prix = reader.Get<decimal>("prix"); // si null = 0.00
        int proprietaireId = reader.Get<int>("ProprietaireId"); // SI null = 0

        return new Voiture(id, marque, modele, prix, proprietaireId);
    }

    public static Proprietaire ToProprietaire(SqlDataReader reader)
    {
        int id = reader.Get<int>("Id");

        // COALESCE (??) il regarde si a gauche c'est null
        // si c'est le cas il affecte la valeur à ça droite 
        // sinon il affecte la valeur à ça gauche
        string prenom = reader.Get<string>("Prenom") ?? "";
        string nom = reader.Get<string>("Nom") ?? "";
        string email = reader.Get<string>("Email") ?? "";
        bool actif = reader.Get<bool>("actif");

        return new Proprietaire(id, prenom, nom, email, actif);

    }
}
