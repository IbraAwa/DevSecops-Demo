using Domain.Shared.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo04.DataReader;

public static class ProprietaireMapper
{
    // Colonnes renvoyées par la vue V_Proprietaire : [Id], [Prenom], [Nom], [Email]
    public static Proprietaire ViewToProprietaire(SqlDataReader reader)
    {
        // L'identifiant d'une table ne deviendra jamais null donc ternaire inutile
        // mais mauvaise habitude (ou bonne ?) => explication dans VoitureMapper
        int id = reader["Id"] is DBNull || reader["Id"] is null ? 0 : (int)reader["Id"];
        string prenom = reader["Prenom"] is DBNull || reader["Prenom"] is null ? "" : (string)reader["Prenom"];
        string nom = reader["Nom"] is DBNull || reader["Nom"] is null ? "" : (string)reader["Nom"];
        string email = reader["Email"] is DBNull || reader["Email"] is null ? "" : (string)reader["Email"];

        // true directement pour Actif car ma vue fait un WHERE pour ne renvoyer
        // que les propriétaires actifs
        return new Proprietaire(id, prenom, nom, email, true); 
    }
}
