--
-- File generated with SQLiteStudio v3.3.3 on Fri May 6 19:50:48 2022
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: AspNetRoleClaims
CREATE TABLE AspNetRoleClaims (Id TEXT PRIMARY KEY NOT NULL, ClaimType TEXT, ClaimValue TEXT, RoleId TEXT NOT NULL);

-- Table: AspNetRoles
CREATE TABLE AspNetRoles (Id TEXT PRIMARY KEY NOT NULL, ConcurrencyStamp TEXT, Name TEXT, NormalizedName TEXT);

-- Table: AspNetUserClaims
CREATE TABLE AspNetUserClaims (Id TEXT PRIMARY KEY NOT NULL, ClaimType TEXT, ClaimValue TEXT, UserId TEXT NOT NULL);

-- Table: AspNetUserLogins
CREATE TABLE AspNetUserLogins (LoginProvider TEXT PRIMARY KEY NOT NULL, ProviderKey TEXT NOT NULL, ProviderDisplayName TEXT, UserId TEXT NOT NULL);

-- Table: AspNetUserRoles
CREATE TABLE AspNetUserRoles (UserId TEXT NOT NULL, RoleId TEXT NOT NULL);

-- Table: AspNetUsers
CREATE TABLE AspNetUsers (Id TEXT PRIMARY KEY NOT NULL, AccessFailedCount INTEGER NOT NULL, ConcurrencyStamp TEXT, Email TEXT, EmailConfirmed BOOLEAN NOT NULL, LockoutEnabled BOOLEAN NOT NULL, LockoutEnd TIME, NormalizedEmail TEXT, NormalizedUserName TEXT, PasswordHash TEXT, PhoneNumber TEXT, PhoneNumberConfirmed BOOLEAN NOT NULL, SecurityStamp TEXT, TwoFactorEnabled BOOLEAN NOT NULL, UserName TEXT);

-- Table: AspNetUserTokens
CREATE TABLE AspNetUserTokens (UserId TEXT NOT NULL PRIMARY KEY, LoginProvider TEXT NOT NULL, Name TEXT NOT NULL, Value TEXT);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
