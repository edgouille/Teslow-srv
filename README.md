# Teslow API

API pour gérer des **matchs de baby-foot**, des **scores**, des **utilisateurs** et des **réservations**.  
Stack : **ASP.NET Core**, **Entity Framework Core**, **MariaDB** (Docker), **JWT**, **Swagger**.

---

## 🚀 Fonctionnalités

- **Utilisateurs** : création, consultation, authentification (JWT).
- **Matchs (Games)** : création rapide via **AddScore** (team1, team2, duration), listing, détails.
- **Réservations** : créer un créneau, vérifier la disponibilité, lier une table de jeu.
- **Classement** : leaderboard des joueurs (matchs joués, etc.).
- **Swagger** : documentation 

---

## 📦 Prérequis

- [.NET 8 SDK](https://dotnet.microsoft.com/)
- [Docker](https://www.docker.com/) / Docker Compose
- (Optionnel) `dotnet-ef` pour les migrations
  ```bash
  dotnet tool install --global dotnet-ef
  ```

---

## 🔐 Configuration (appsettings)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=teslow_db;user=teslow_user;password=teslow_pass"
  },
  "Jwt": {
    "Issuer": "TeslowServer",
    "Audience": "TeslowClients",
    "Secret": "1f4e2d587c8b4b2ab5d8b8c176b1b051",
    "ExpiryMinutes": 120
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

---

## 🐳 Lancer avec Docker file (DB)

### Build l'image 
```bash
docker build -f Dockerfile.db -t teslow-db .
```

### Lancer le container
```bash
docker run -d --name mariadb-teslow -e MARIADB_ROOT_PASSWORD=rootpass -e MARIADB_DATABASE=teslow_db -e MARIADB_USER=teslow_user -e MARIADB_PASSWORD=teslow_pass -p 3306:3306 teslow-db
```
### Lancer l'API
Lancer via visual stuido 2022 sur le profil https

Swagger : https://localhost:8080/swagger/index.html

---

## 🧰 Aides

- **`UseMySql` introuvable** → installe `Pomelo.EntityFrameworkCore.MySql`.
- **Erreur “Unknown column 'PasswordHash'”** → schéma DB pas à jour.

---
