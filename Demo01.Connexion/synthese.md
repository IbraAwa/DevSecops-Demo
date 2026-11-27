# Synthèse Demo01 – Connexion

## La chaîne de connexion
Une suite de `clé=valeur;` qui dit **où** et **comment** se connecter.

| Clé | Rôle |
|---|---|
| `Server=` (ou `Data Source=`) | l'adresse du serveur SQL |
| `Database=` (ou `Initial Catalog=`) | la base ciblée sur ce serveur |
| `Integrated Security=True` | connexion avec le compte Windows (pas de login / mot de passe) |
| `Trust Server Certificate=True` | accepte le certificat sans le vérifier (OK en local, **pas en prod**) |

- `SqlConnectionStringBuilder` découpe la chaîne en propriétés C# (`DataSource`, `InitialCatalog`...).
- Les clés non écrites prennent leur valeur par défaut (`Pooling`, `ConnectTimeout`, `Encrypt`...).
- **Sécurité** : la chaîne ne doit pas finir sur GitHub → user secrets ou fichier dans le `.gitignore`.

## Ouvrir / fermer une connexion
```csharp
SqlConnection connection = new SqlConnection(ConnectionString.Value); // State = Closed
await connection.OpenAsync();   // State = Open
await connection.CloseAsync();  // State = Closed
await connection.DisposeAsync(); // libère la ressource
```
- Le **Garbage Collector** libère la mémoire tout seul...
- ... **sauf** les ressources externes (connexion DB, fichiers) → il faut les libérer nous-mêmes avec `Dispose`.

## `using` = fermeture automatique
```csharp
await using (SqlConnection connection = new SqlConnection(...))
{
    await connection.OpenAsync();
} // ici la connexion est fermée + Dispose automatiquement

await using SqlConnection connection = new(...); // sans accolades : fermé à la fin du bloc (méthode, if...)
```
- Plus besoin d'appeler `Close` / `Dispose`, même si une exception est levée.
- **Toujours** mettre un `using` sur `SqlConnection`, `SqlCommand`, `SqlDataReader`.

## Créer une commande
```csharp
await using SqlCommand command = connection.CreateCommand(); // commande liée à la connexion
command.CommandText = "SELECT COUNT(*) FROM Voiture";
object? resultat = await command.ExecuteScalarAsync();
```
- `SqlCommand` = la requête SQL à envoyer.
- `CreateCommand()` = crée la commande avec les infos de connexion déjà dedans.

## Async / await (le cuisinier)
- **Synchrone** : le cuisinier attend devant la casserole que l'eau boue → lent.
- **Plusieurs threads** : engager plusieurs cuisiniers → cher (~1 Mo de mémoire par thread).
- **Asynchrone** : il met l'eau à bouillir et prépare la sauce pendant ce temps → optimisé.
- On l'utilise surtout pour les **ressources externes** (DB, fichiers, réseau) : c'est ce qui prend le plus de temps.

| Synchrone | Asynchrone |
|---|---|
| `void` | `async Task` |
| `Voiture` | `async Task<Voiture>` |
| `Open()` | `await OpenAsync()` |

- `await` = "va faire autre chose, je te préviens quand c'est fini".

## À retenir
1. Chaîne de connexion = où + comment se connecter, jamais dans un repo public.
2. Une connexion ouverte doit **toujours** être fermée → `using`.
3. `CreateCommand()` pour créer la requête depuis la connexion.
4. Accès DB = `async` / `await` partout.
