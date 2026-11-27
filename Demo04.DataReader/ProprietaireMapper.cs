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
    // 	[Id], [Prenom], [Nom], [Email] 
    public static Proprietaire ViewToProprietaire(SqlDataReader reader)
    {
        // un identifiant d'une table ne deviendra jamais null donc ternaire inutile
        // mais mauvaise habitude (ou bonne ?)
        int id = reader["Id"] is DBNull || reader["Id"] is null ? 0 : (int)reader["Id"];
        string prenom = reader["Prenom"] is DBNull || reader["Prenom"] is null ? "" : (string)reader["prenom"];
        string nom = reader["Nom"] is DBNull || reader["Nom"] is null ? "" : (string)reader["Nom"];
        string email = reader["Email"] is DBNull || reader["Email"] is null ? "" : (string)reader["Email"];

        // True directement à actif car ma view faire un where pour renvoyer uniquement
        // les proprietaire actif
        return new Proprietaire(id, prenom, nom, email, true); 
    }
}
