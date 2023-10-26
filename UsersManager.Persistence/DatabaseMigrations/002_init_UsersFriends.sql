-- init UsersFriends table
CREATE TABLE IF NOT EXISTS "UsersFriends"
(
    "Uuid"             uuid                     NOT NULL DEFAULT "gen_random_uuid"()
        CONSTRAINT "PK_UsersFriends"
            PRIMARY KEY,
    "User1Uuid"        uuid                     NOT NULL
        CONSTRAINT "FK_UsersFriends_Users_User1Guid"
            REFERENCES "Users"
            ON DELETE CASCADE,
    "User2Uuid"        uuid                     NOT NULL
        CONSTRAINT "FK_UsersFriends_Users_User2Guid"
            REFERENCES "Users"
            ON DELETE CASCADE,
    "CreationDateTime" timestamp WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "LastModDateTime"  timestamp WITH TIME ZONE NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS "IX_UsersFriends_User1Uuid"
    ON "UsersFriends" ("User1Uuid");

CREATE INDEX IF NOT EXISTS "IX_UsersFriends_User2Uuid"
    ON "UsersFriends" ("User2Uuid");
