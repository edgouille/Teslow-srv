-- Crée la DB si besoin et utilise-la
CREATE DATABASE IF NOT EXISTS `teslow_db` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE `teslow_db`;

-- USERS
CREATE TABLE IF NOT EXISTS `Users` (
  `Id` CHAR(36) NOT NULL,
  `UserName` VARCHAR(100) NOT NULL,
  `Role` VARCHAR(50) NULL,
  `CreatedAt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UX_Users_UserName` (`UserName`)
) ENGINE=InnoDB;

-- GAMES
CREATE TABLE IF NOT EXISTS `Games` (
  `Id` CHAR(36) NOT NULL,
  `Score1` INT NOT NULL,
  `Score2` INT NOT NULL,
  `DurationSeconds` INT NOT NULL,     -- équivalent TimeSpan (en secondes)
  `Date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB;

-- TEAM MEMBERSHIP (appartenance d’un user à une équipe “club”)
CREATE TABLE IF NOT EXISTS `TeamMemberships` (
  `Id` CHAR(36) NOT NULL,
  `TeamId` CHAR(36) NOT NULL,
  `UserId` CHAR(36) NOT NULL,
  `JoinedAt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `IX_TeamMemberships_TeamId` (`TeamId`),
  KEY `IX_TeamMemberships_UserId` (`UserId`),
  CONSTRAINT `FK_TeamMemberships_Users_UserId`
    FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- GAME TABLES (les tables de babyfoot physiques)
CREATE TABLE IF NOT EXISTS `GameTables` (
  `Id` CHAR(36) NOT NULL,
  `Name` VARCHAR(100) NOT NULL,
  `Location` VARCHAR(200) NULL,
  `IsActive` TINYINT(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UX_GameTables_Name` (`Name`)
) ENGINE=InnoDB;

-- RESERVATIONS (créneau de réservation)
CREATE TABLE IF NOT EXISTS `Reservations` (
  `Id` CHAR(36) NOT NULL,
  `StartUtc` DATETIME NOT NULL,
  `DurationSeconds` INT NOT NULL,
  `Mode` TINYINT NOT NULL,           -- 1 = 1v1, 2 = 2v2
  `CreatedAtUtc` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `RowVersion` BINARY(8) NULL,       -- si besoin de concurrence optimiste (timestamp MySQL différent d’EF)
  PRIMARY KEY (`Id`),
  KEY `IX_Reservations_StartUtc` (`StartUtc`)
) ENGINE=InnoDB;

-- AFFECTATION D’UNE RÉSERVATION À UNE TABLE
CREATE TABLE IF NOT EXISTS `GameTableAssignments` (
  `Id` CHAR(36) NOT NULL,
  `ReservationId` CHAR(36) NOT NULL,
  `GameTableId` CHAR(36) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_GameTableAssignments_ReservationId` (`ReservationId`),
  KEY `IX_GameTableAssignments_GameTableId` (`GameTableId`),
  CONSTRAINT `FK_GameTableAssignments_Reservations_ReservationId`
    FOREIGN KEY (`ReservationId`) REFERENCES `Reservations` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_GameTableAssignments_GameTables_GameTableId`
    FOREIGN KEY (`GameTableId`) REFERENCES `GameTables` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB;

-- GAME TEAMS (équipes dans un match)
CREATE TABLE IF NOT EXISTS `GameTeams` (
  `Id` CHAR(36) NOT NULL,
  `GameId` CHAR(36) NOT NULL,
  `TeamNumber` INT NOT NULL,           -- 1 ou 2
  PRIMARY KEY (`Id`),
  KEY `IX_GameTeams_GameId` (`GameId`),
  CONSTRAINT `FK_GameTeams_Games_GameId`
    FOREIGN KEY (`GameId`) REFERENCES `Games` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- TEAM PLAYERS (composition des équipes d’un match)
CREATE TABLE IF NOT EXISTS `TeamPlayers` (
  `Id` CHAR(36) NOT NULL,
  `TeamId` CHAR(36) NOT NULL,          -- réf GameTeams.Id
  `UserId` CHAR(36) NOT NULL,          -- réf Users.Id
  PRIMARY KEY (`Id`),
  KEY `IX_TeamPlayers_TeamId` (`TeamId`),
  KEY `IX_TeamPlayers_UserId` (`UserId`),
  CONSTRAINT `FK_TeamPlayers_GameTeams_TeamId`
    FOREIGN KEY (`TeamId`) REFERENCES `GameTeams` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_TeamPlayers_Users_UserId`
    FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- Optionnel: seeds de base
-- INSERT INTO `Users` (`Id`, `UserName`, `Role`) VALUES ('00000000-0000-0000-0000-000000000001', 'alice', 'User');
-- INSERT INTO `Users` (`Id`, `UserName`, `Role`) VALUES ('00000000-0000-0000-0000-000000000002', 'bob', 'User');
