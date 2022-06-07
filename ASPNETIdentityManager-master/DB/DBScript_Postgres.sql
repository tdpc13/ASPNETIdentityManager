CREATE TABLE "AspNetUserTokens"(
	"UserId" text NOT NULL, /*era chiave primaria in sql server*/
	"LoginProvider" text NOT NULL, /*era chiave primaria in sql server*/
	"Name" text NOT NULL, /*era chiave primaria in sql server*/
	"Value" text NULL);

CREATE TABLE "AspNetUsers"(
	"Id" text primary key NOT NULL,
	"AccessFailedCount" integer NOT NULL,
	"ConcurrencyStamp" text NULL,
	"Email" text NULL,
	"EmailConfirmed" boolean NOT NULL,
	"LockoutEnabled" boolean NOT NULL,
	"LockoutEnd" timestamp NULL,
	"NormalizedEmail" text NULL,
	"NormalizedUserName" text NULL,
	"PasswordHash" text NULL,
	"PhoneNumber" text NULL,
	"PhoneNumberConfirmed" boolean NOT NULL,
	"SecurityStamp" text NULL,
	"TwoFactorEnabled" boolean NOT NULL,
	"UserName" text NULL);

CREATE TABLE "AspNetUserRoles"(
	"UserId" text NOT NULL, /*era chiave primaria in sql server*/
	"RoleId" text NOT NULL); /*era chiave primaria in sql server*/

CREATE TABLE "AspNetUserClaims"(
	"Id" serial PRIMARY KEY NOT NULL,
	"ClaimType" text NULL,
	"ClaimValue" text NULL,
	"UserId" text NOT NULL);

CREATE TABLE "AspNetUserLogins"(
	"LoginProvider" text primary key NOT NULL,
	"ProviderKey" text NOT NULL, /*anche questa era primary key in sql server*/
	"ProviderDisplayName" text NULL,
	"UserId" text NOT NULL);

CREATE TABLE "AspNetRoles"(
	"Id" text primary key NOT NULL,
	"ConcurrencyStamp" text NULL,
	"Name" text NULL,
	"NormalizedName" text NULL);

CREATE TABLE "AspNetRoleClaims"(
	"Id" serial PRIMARY KEY NOT NULL,
	"ClaimType" text NULL,
	"ClaimValue" text NULL,
	"RoleId" text NOT NULL);