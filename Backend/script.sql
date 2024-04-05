CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `VideoStats` (
    `VideoId` char(36) COLLATE ascii_general_ci NOT NULL,
    `VideoLength` decimal(65,30) NOT NULL,
    `Category` int NOT NULL,
    CONSTRAINT `PK_VideoStats` PRIMARY KEY (`VideoId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `WatchHistories` (
    `UserId` char(36) COLLATE ascii_general_ci NOT NULL,
    `VideoId` char(36) COLLATE ascii_general_ci NOT NULL,
    `WatchedTime` decimal(18,2) NOT NULL,
    `Liked` int NOT NULL,
    `FullyWatched` tinyint(1) NOT NULL,
    CONSTRAINT `PK_WatchHistories` PRIMARY KEY (`UserId`, `VideoId`),
    CONSTRAINT `FK_WatchHistories_VideoStats_VideoId` FOREIGN KEY (`VideoId`) REFERENCES `VideoStats` (`VideoId`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_WatchHistories_VideoId` ON `WatchHistories` (`VideoId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240309120844_InitialCreate', '8.0.2');

COMMIT;

START TRANSACTION;

ALTER TABLE `VideoStats` ADD `PublishedAt` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240402145546_addedPublishDate', '8.0.2');

COMMIT;

