CREATE OR REPLACE PROCEDURE "friend_invite_proc"(
    "FromUserUuid" uuid,
    "ToUserUuid" uuid
) AS
$$
DECLARE
    "requestUuid" uuid;
BEGIN
    IF NOT EXISTS(
                 SELECT NULL
                 FROM "Users"
                 WHERE "Uuid" = "FromUserUuid"
                   AND "IsActive"
                 ) OR
       NOT EXISTS(
                 SELECT NULL
                 FROM "Users"
                 WHERE "Uuid" = "ToUserUuid"
                   AND "IsActive"
                 )
    THEN
        RAISE EXCEPTION 'UserNotFound' ;
    END IF;

    IF EXISTS(
             SELECT NULL
             FROM "FriendRequests"
             WHERE "User1Uuid" = "FromUserUuid"
               AND "User2Uuid" = "ToUserUuid"
             )
    THEN
        RAISE USING ERRCODE = '23505';
    END IF;

    SELECT "Uuid"
    INTO "requestUuid"
    FROM "FriendRequests"
    WHERE "User1Uuid" = "ToUserUuid"
      AND "User2Uuid" = "FromUserUuid";

    IF "requestUuid" IS NOT NULL THEN
        DELETE FROM "FriendRequests" WHERE "Uuid" = "requestUuid";
        INSERT INTO "UsersFriends" ("User1Uuid", "User2Uuid") VALUES ("ToUserUuid", "FromUserUuid");
    ELSE
        INSERT INTO "FriendRequests" ("User1Uuid", "User2Uuid") VALUES ("FromUserUuid", "ToUserUuid");
    END IF;
END;
$$ LANGUAGE "plpgsql";
