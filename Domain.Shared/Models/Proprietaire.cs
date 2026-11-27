namespace Domain.Shared.Models;

public class Proprietaire
{
    public Proprietaire(int id, string prenom, string nom, string email, bool actif)
    {
        Id = id;
        Prenom = prenom;
        Nom = nom;
        Email = email;
        Actif = actif;
    }

    public int Id { get; set; }

    public string Prenom { get; set;  }

    public string Nom { get; set; }

    public string Email { get; set; }

    public bool Actif { get; set; }
}