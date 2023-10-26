-- init FriendRequests table
CREATE TABLE IF NOT EXISTS "FriendRequests"
(
    "Uuid"             uuid                     NOT NULL DEFAULT "gen_random_uuid"()
        CONSTRAINT "PK_FriendRequests"
            PRIMARY KEY,
    "User1Uuid"        uuid                     NOT NULL
        CONSTRAINT "FK_FriendRequests_Users_User1Guid"
            REFERENCES "Users"
            ON DELETE CASCADE,
    "User2Uuid"        uuid                     NOT NULL
        CONSTRAINT "FK_FriendRequests_Users_User2Guid"
            REFERENCES "Users"
            ON DELETE CASCADE,
    "CreationDateTime" timestamp WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "LastModDateTime"  timestamp WITH TIME ZONE NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS "IX_FriendRequests_User1Uuid"
    ON "FriendRequests" ("User1Uuid");

CREATE INDEX IF NOT EXISTS "IX_FriendRequests_User2Uuid"
    ON "FriendRequests" ("User2Uuid");
