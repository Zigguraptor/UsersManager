-- init Users table
CREATE TABLE IF NOT EXISTS "Users"
(
    "Uuid"             uuid                     NOT NULL DEFAULT "gen_random_uuid"()
        CONSTRAINT "PK_Users"
            PRIMARY KEY,
    "IsActive"         boolean                  NOT NULL DEFAULT TRUE,
    "UserName"         varchar(32) UNIQUE       NOT NULL,
    "DisplayName"      varchar(64)              NOT NULL,
    "EmailAddress"     varchar(254) UNIQUE      NOT NULL,
    "PasswordHash"     varchar(128)             NOT NULL,
    "CreationDateTime" timestamp WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "LastModDateTime"  timestamp WITH TIME ZONE NOT NULL DEFAULT NOW()
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_Users_unique_UserName"
    ON "Users" ("UserName");

-- функциональный индекс по UserName в lowercase
CREATE UNIQUE INDEX IF NOT EXISTS "IX_Users_unique_lower_UserName"
    ON "Users" (LOWER("UserName"));

CREATE UNIQUE INDEX IF NOT EXISTS "IX_Users_unique_EmailAddress"
    ON "Users" ("EmailAddress");

-- функциональный индекс по EmailAddress в lowercase
CREATE UNIQUE INDEX IF NOT EXISTS "IX_Users_unique_lower_EmailAddress"
    ON "Users" (LOWER("EmailAddress"));
