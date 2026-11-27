# 🚗 ADO.NET : requêtes SQL, paramètres et DataReader

> **Base utilisée** (à adapter à votre script SQL)
>
> | Table | Colonnes |
> |---|---|
> | `Proprietaire` | `Id`, `Nom`, `Prenom`, `EstActif` (bit) |
> | `Voiture` | `Id`, `Marque`, `Modele`, `Prix` (decimal), `EnStock` (bit), `ProprietaireId` |

---

## 📌 Le cours en 5 lignes

1. **Requête paramétrée** : on écrit la requête avec des **marqueurs** (`@prix`, `@marque`) au lieu de coller dedans ce que l'utilisateur a tapé.
2. La valeur est envoyée **à part** via `command.Parameters` : SQL Server la traite comme une donnée, jamais comme du code.
3. Résultat : **pas d'injection SQL** possible, et les types (nombres, dates) sont respectés.
4. **DataReader** (`SqlDataReader`) : il lit le résultat d'un `SELECT` **ligne par ligne**, en lecture seule et uniquement vers l'avant.
5. On avance avec `reader.Read()` (renvoie `false` à la fin) et la connexion doit rester **ouverte** pendant toute la lecture.

> ⚠️ **À ne jamais faire** :
> ```csharp
> command.CommandText = "SELECT * FROM Voiture WHERE Marque = '" + saisie + "'";
> ```
> Si l'utilisateur tape `' OR 1=1 --`, il récupère toute la table.

---

## 🧱 Le squelette à réutiliser

```csharp
using Microsoft.Data.SqlClient;

string connectionString = "Server=.;Database=Garage;Integrated Security=True;TrustServerCertificate=True";

using SqlConnection connection = new SqlConnection(connectionString);
using SqlCommand command = connection.CreateCommand();

command.CommandText = "/* votre requête ici */";
// command.Parameters.Add(...)   ← si la requête contient un @parametre

connection.Open();
using SqlDataReader reader = command.ExecuteReader();

while (reader.Read())
{
    // reader["NomColonne"] pour lire une colonne de la ligne en cours
}
```

---

## 🟢 Niveau 1 : requêtes sans paramètre

### Exercice 1 : tous les propriétaires inactifs

<details>
<summary>💡 Solution</summary>

```csharp
command.CommandText = "SELECT Id, Nom, Prenom FROM Proprietaire WHERE EstActif = 0";

connection.Open();
using SqlDataReader reader = command.ExecuteReader();

while (reader.Read())
{
    Console.WriteLine($"{reader["Id"]} - {reader["Nom"]} {reader["Prenom"]}");
}
```
</details>

### Exercice 2 : toutes les BMW

<details>
<summary>💡 Solution</summary>

```csharp
command.CommandText = "SELECT Id, Marque, Modele, Prix FROM Voiture WHERE Marque = 'BMW'";

connection.Open();
using SqlDataReader reader = command.ExecuteReader();

while (reader.Read())
{
    Console.WriteLine($"{reader["Marque"]} {reader["Modele"]} : {reader["Prix"]} €");
}
```

> Ici, pas besoin de paramètre : la valeur `'BMW'` est écrite par **le développeur**, pas par l'utilisateur.
</details>

---

## 🟡 Niveau 2 : requêtes paramétrées (saisie utilisateur)

### Exercice 3 : toutes les voitures à partir d'un prix

L'utilisateur tape un prix. On affiche les voitures dont le prix est **supérieur ou égal** à ce prix.

<details>
<summary>💡 Solution</summary>

```csharp
Console.Write("Prix minimum : ");
decimal prix = decimal.Parse(Console.ReadLine()!);

command.CommandText = "SELECT Id, Marque, Modele, Prix FROM Voiture WHERE Prix >= @prix";
command.Parameters.Add("@prix", SqlDbType.Decimal).Value = prix;

connection.Open();
using SqlDataReader reader = command.ExecuteReader();

while (reader.Read())
{
    Console.WriteLine($"{reader["Marque"]} {reader["Modele"]} : {reader["Prix"]} €");
}
```

> `SqlDbType` se trouve dans `using System.Data;`
>
> Variante plus courte : `command.Parameters.AddWithValue("@prix", prix);`
</details>

### Exercice 4 : les propriétaires dont le nom commence par une lettre

L'utilisateur tape une lettre (ex. `n`). On affiche les propriétaires dont le nom commence par cette lettre.

<details>
<summary>💡 Solution</summary>

```csharp
Console.Write("Première lettre du nom : ");
string lettre = Console.ReadLine()!;

command.CommandText = "SELECT Id, Nom, Prenom FROM Proprietaire WHERE Nom LIKE @debut";
command.Parameters.AddWithValue("@debut", lettre + "%");

connection.Open();
using SqlDataReader reader = command.ExecuteReader();

while (reader.Read())
{
    Console.WriteLine($"{reader["Nom"]} {reader["Prenom"]}");
}
```

> 🔑 Le `%` se met **dans la valeur du paramètre**, pas dans la requête.
> `LIKE '@debut%'` ne fonctionne pas : SQL cherche alors littéralement le texte « @debut ».
</details>

### Exercice 5 : toutes les voitures en stock d'une marque choisie

L'utilisateur choisit une marque. S'il tape `Mercedes`, il obtient **toutes les Mercedes en stock**.

<details>
<summary>💡 Solution</summary>

```csharp
Console.Write("Marque : ");
string marque = Console.ReadLine()!;

command.CommandText = @"SELECT Id, Marque, Modele, Prix
                        FROM Voiture
                        WHERE Marque = @marque AND EnStock = 1";
command.Parameters.AddWithValue("@marque", marque);

connection.Open();
using SqlDataReader reader = command.ExecuteReader();

if (!reader.HasRows)
{
    Console.WriteLine($"Aucune {marque} en stock.");
}

while (reader.Read())
{
    Console.WriteLine($"{reader["Marque"]} {reader["Modele"]} : {reader["Prix"]} €");
}
```
</details>

---

## ✅ Récap

| Situation | Que faire ? |
|---|---|
| Valeur fixe écrite par le dev | Directement dans la requête |
| Valeur tapée par l'utilisateur | **Toujours** un `@parametre` |
| Recherche « commence par » | `LIKE @param` et `"valeur%"` dans le paramètre |
| Lire les lignes d'un `SELECT` | `ExecuteReader()` + `while (reader.Read())` |
| Lire une colonne | `reader["NomColonne"]` |