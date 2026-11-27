using Microsoft.Data.SqlClient;

namespace Tools.Shared.Mapping;

// une classe d'extensions
// ça permet de rajouter des méthodes à un objet
// par exemple on va rajouter des méthode à notre objet SqlDataReader
public static class SqlDataReaderExtensions
{

    // en tant que utilisateur je regarde j'insère le nom de la colonne dans nomColonne
    // et ma méthode passe par toute les colonnes et pour chaque colonne
    // il regarde si le nom donner par l'utilisateur correspond à un nom dans mon reader
    public static bool ColumnExist(this SqlDataReader reader, string nomColonne)
    {
        // avec le for je vais boucler sur toute les colonnes que me renvoie
        // ma requête sql
        // si ma requête sql renvoie SELECT Marque, Modele, Prix
        // mon reader aura 3 colonne donc il va boucler 3 fois
        for (int i = 0; i < reader.FieldCount; i++)
        {
            // mon equals() va comparer la valeur de deux chaine de caractère
            // mon GetName() va récupérer le nom de ma colonne via l'index
            // donc si mon select c'est SELECT Marque, Modele, prix FROM Voiture;
            // Qu'est-ce que va me donner GetName(2) ? => Prix

            // StringComparison.OrdinalIgnoreCase ?
            // il retire la sensibilité à la casse
            // ça veut dire que pour lui Prix ==  PRIX ou PriX == pRix il s'en fiche 
            // de la majuscuke ou miniscule
            if (string.Equals(reader.GetName(i), nomColonne, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }


    // T? me permet de transformer ma colonne qui est un object en type plus précis
    // si Colonne est en int j'ai juste à faire Get<int>
    // si Colonne est en string j'ai juste à faire Get<string> etc...
    // Je choisis le tyep de retour selon le type de ma colonne en db

    // this SqlDataReader reader permet 
    // d'appeler mon get directement via l'objet reader 
    // exemple : reader.Get(=
    public static T? Get<T>(this SqlDataReader reader, string nomColonne, T? valueAbsentOrDbNull = default)
    {

        // on doit regarder si le nom de la colonne existe bien 
        // si la colonne donné en paramètre nomColonne existe 
        if(!reader.ColumnExist(nomColonne) || reader[nomColonne] is DBNull || reader[nomColonne] is null)
        {
            return valueAbsentOrDbNull;
        }

        // je retourne ma colonne caster implicitement
        // (int)reader["Id"]
        return (T)reader[nomColonne];
    }
}
