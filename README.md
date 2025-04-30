# StageManager

Application de gestion de stages pour les étudiants et les entreprises.

## Fonctionnalités

- **Authentification** : Connexion et inscription des utilisateurs
- **Tableau de bord** : Interface moderne avec menu de navigation
- **Liste des étudiants** : Visualisation et recherche d'étudiants disponibles pour un stage
- **Importation Excel** : Ajout d'étudiants à partir d'un fichier Excel
- **Contact** : Possibilité de contacter directement les étudiants depuis l'application

## Technologies utilisées

- C# / .NET Framework
- Windows Forms
- Firebase (base de données)
- EPPlus (manipulation de fichiers Excel)
- BCrypt.NET (sécurisation des mots de passe)

## Structure du projet

Le projet est organisé selon une architecture claire et modulaire. Voici les principaux fichiers et leur rôle :

### Fichiers principaux

| Fichier | Description |
|---------|-------------|
| `Program.cs` | Point d'entrée de l'application. Initialise l'environnement et lance le formulaire de connexion. |
| `StageManager.csproj` | Fichier de configuration du projet .NET. |
| `App.config` | Configuration de l'application. |

### Modèles de données

| Fichier | Description |
|---------|-------------|
| `User.cs` | Définit la classe `User` qui représente un utilisateur (entreprise ou administrateur). |
| `Stage.cs` | Définit la classe `Stage` qui représente un stage avec toutes ses informations. |
| `Models.cs` | Contient les classes `Student` et `School` pour la gestion des étudiants et des écoles. |

### Gestion de la base de données

| Fichier | Description |
|---------|-------------|
| `FirebaseConfig.cs` | Configuration de la connexion à Firebase avec le pattern Singleton. |
| `FirebaseAuthManager.cs` | Gestion de l'authentification des utilisateurs avec Firebase. |

### Formulaires (Interface utilisateur)

| Fichier | Description |
|---------|-------------|
| `LoginForm.cs` | Formulaire de connexion, premier écran de l'application. |
| `RegisterForm.cs` | Formulaire d'inscription pour créer un nouveau compte. |
| `DashboardForm.cs` | Tableau de bord principal avec menu de navigation. |
| `StageListForm.cs` | Affichage et recherche de la liste des étudiants disponibles. |
| `StageDetailsForm.cs` | Détails d'un stage sélectionné. |
| `ContactForm.cs` | Formulaire pour contacter un étudiant. |

### Utilitaires

| Fichier | Description |
|---------|-------------|
| `ExcelImporter.cs` | Gestion de l'importation de données depuis des fichiers Excel. |
| `ModernTheme.cs` | Définition du thème visuel moderne de l'application. |

## Installation

1. Clonez ce dépôt
2. Ouvrez la solution dans Visual Studio
3. Restaurez les packages NuGet
4. Compilez et exécutez l'application

## Flux de travail de l'application

1. L'utilisateur se connecte via le formulaire de connexion (`LoginForm.cs`)
2. Après authentification, il accède au tableau de bord (`DashboardForm.cs`)
3. Depuis le tableau de bord, il peut :
   - Consulter la liste des étudiants (`StageListForm.cs`)
   - Importer des données depuis Excel (`ExcelImporter.cs`)
   - Contacter un étudiant (`ContactForm.cs`)
   - Se déconnecter

## Identifiants de test

- **Admin**
  - Email: admin@stagemanager.com
  - Mot de passe: admin123

## Dépendances externes

Le projet utilise plusieurs bibliothèques externes :

- **FireSharp** : Client .NET pour Firebase
- **EPPlus** : Manipulation de fichiers Excel
- **BCrypt.Net-Next** : Hashage sécurisé des mots de passe
- **Newtonsoft.Json** : Sérialisation/désérialisation JSON

## Auteurs

- Rayan
- Tadj (backend et gestion des données)
