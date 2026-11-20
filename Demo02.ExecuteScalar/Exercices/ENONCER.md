# Consignes du projet

## 1. Cloner le projet
- Cloner le projet afin de travailler sur une copie locale.

## 2. Configurer les chaînes de connexion
- Modifier la chaîne de connexion utilisée lors de la publication (*Publish*).
- Modifier également les variables d'environnement contenant les chaînes de connexion.
- Respecter les bonnes pratiques concernant la gestion des chaînes de connexion.
- Demander conseil à une IA afin de vérifier que la configuration respecte les bonnes pratiques en matière de sécurité.

## 3. Réaliser les exercices

Pour chaque exercice présent dans le dossier `Exercices`, vous devez créer une classe dédiée.

### Structure attendue

Pour chaque exercice :

1. Créer un fichier portant un nom explicite qui reprend le nom de l'exercice.
2. Créer une connexion à la base de données.
3. Ouvrir la connexion.
4. Créer une commande SQL.
5. Insérer la requête SQL dans la propriété `command.CommandText`.
6. Afficher dans la console, à l'aide de `Console.WriteLine()` :
   - La requête SQL exécutée.
   - Le résultat obtenu à partir de cette requête.

### Bonnes pratiques de développement

- Toutes les méthodes doivent être asynchrones (`async`).
- Utiliser `tryParse` lorsque cela est pertinent pour convertir des valeurs.
- Choisir des noms de variables explicites et cohérents.
- Respecter les bonnes pratiques de programmation.
- Organiser le code proprement et éviter les répétitions inutiles.

## 4. Liste des exercices à réaliser

Vous devez réaliser les quatre exercices suivants :

1. **Calculer le prix moyen des voitures.**
2. **Afficher la marque de la voiture ayant l'identifiant 5.**
3. **Afficher le propriétaire ayant l'identifiant 2.**
4. **Afficher le propriétaire de la voiture ayant l'identifiant 14.**

## 5. Consigne importante concernant les classes

**Attention : chaque exercice doit être réalisé dans une classe `static` distincte, contenant une méthode `static` dédiée.**

Chaque exercice doit donc être indépendant des autres et respecter cette organisation.