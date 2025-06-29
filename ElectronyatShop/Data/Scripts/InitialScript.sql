﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE "AspNetRoles" (
    "Id" text NOT NULL,
    "Name" character varying(256),
    "NormalizedName" character varying(256),
    "ConcurrencyStamp" text,
    CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id")
);

CREATE TABLE "AspNetUsers" (
    "Id" text NOT NULL,
    "FirstName" text NOT NULL,
    "LastName" text NOT NULL,
    "Address" text NOT NULL,
    "UserName" character varying(256),
    "NormalizedUserName" character varying(256),
    "Email" character varying(256),
    "NormalizedEmail" character varying(256),
    "EmailConfirmed" boolean NOT NULL,
    "PasswordHash" text,
    "SecurityStamp" text,
    "ConcurrencyStamp" text,
    "PhoneNumber" text,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamp with time zone,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" integer NOT NULL,
    CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id")
);

CREATE TABLE "Products" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Type" integer NOT NULL,
    "Name" text NOT NULL,
    "Image" text NOT NULL,
    "Description" text NOT NULL,
    "Price" numeric NOT NULL,
    "AvailableQuantity" integer NOT NULL,
    "DiscountPercentage" integer NOT NULL,
    "Status" boolean NOT NULL,
    CONSTRAINT "PK_Products" PRIMARY KEY ("Id")
);

CREATE TABLE "AspNetRoleClaims" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "RoleId" text NOT NULL,
    "ClaimType" text,
    "ClaimValue" text,
    CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserClaims" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "UserId" text NOT NULL,
    "ClaimType" text,
    "ClaimValue" text,
    CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserLogins" (
    "LoginProvider" character varying(128) NOT NULL,
    "ProviderKey" character varying(128) NOT NULL,
    "ProviderDisplayName" text,
    "UserId" text NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserRoles" (
    "UserId" text NOT NULL,
    "RoleId" text NOT NULL,
    CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserTokens" (
    "UserId" text NOT NULL,
    "LoginProvider" character varying(128) NOT NULL,
    "Name" character varying(128) NOT NULL,
    "Value" text,
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Carts" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "TotalPrice" numeric NOT NULL,
    "UserId" text,
    CONSTRAINT "PK_Carts" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Carts_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id")
);

CREATE TABLE "Orders" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "CreateDate" timestamp with time zone NOT NULL,
    "TotalPrice" numeric NOT NULL,
    "Address" text NOT NULL,
    "Status" integer NOT NULL,
    "UserId" text,
    CONSTRAINT "PK_Orders" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Orders_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id")
);

CREATE TABLE "CartItems" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Quantity" integer NOT NULL,
    "CartId" integer,
    "ProductId" integer,
    CONSTRAINT "PK_CartItems" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_CartItems_Carts_CartId" FOREIGN KEY ("CartId") REFERENCES "Carts" ("Id"),
    CONSTRAINT "FK_CartItems_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE
);

CREATE TABLE "OrderItems" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "OrderId" integer,
    "ProductId" integer,
    "Quantity" integer NOT NULL,
    CONSTRAINT "PK_OrderItems" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_OrderItems_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders" ("Id"),
    CONSTRAINT "FK_OrderItems_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");

CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");

CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");

CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");

CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");

CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");

CREATE INDEX "IX_CartItems_CartId" ON "CartItems" ("CartId");

CREATE INDEX "IX_CartItems_ProductId" ON "CartItems" ("ProductId");

CREATE INDEX "IX_Carts_UserId" ON "Carts" ("UserId");

CREATE INDEX "IX_OrderItems_OrderId" ON "OrderItems" ("OrderId");

CREATE INDEX "IX_OrderItems_ProductId" ON "OrderItems" ("ProductId");

CREATE INDEX "IX_Orders_UserId" ON "Orders" ("UserId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250609203435_Initial', '9.0.5');

COMMIT;

