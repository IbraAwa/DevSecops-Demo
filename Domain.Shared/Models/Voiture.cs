namespace Domain.Shared.Models;

// Ma classe voiture est la copie conforme de la table Voiture en Base de donnée 
// doit au moin posséder ce que possède la table
// mais peux posséder + de propriété
// comme des propriété calculable
public class Voiture
{
    public Voiture(int id, string marque, string modele, decimal prix, int proprietaireId)
    {
        Id = id;
        Marque = marque;
        Modele = modele;
        Prix = prix;
        ProprietaireId = proprietaireId;
    }

    public int Id { get; set; }

    public string Marque { get; set; }
    public string Modele { get; set; }

    public decimal Prix { get; set; }

    // propriété calculable
    // public bool EstChere {get; set;} => Prix > 50 000

    public int ProprietaireId { get; set; }
}
