CREATE OR REPLACE PROCEDURE "friend_invite_proc"(
    "FromUserUuid" uuid,
    "ToUserUuid" uuid
) AS
$$
DECLARE
    "requestUuid" uuid;
BEGIN
    IF EXISTS(
             SELECT NULL
             FROM "FriendRequests"
             WHERE "User1Uuid" = "FromUserUuid"
               AND "User2Uuid" = "ToUserUuid"
             )
    THEN
        RAISE USING ERRCODE = '23505';
    ELSIF
        EXISTS(
              SELECT "requestUuid" = "Uuid"
              FROM "FriendRequests"
              WHERE "User1Uuid" = "ToUserUuid"
                AND "User2Uuid" = "FromUserUuid"
              )
    THEN
        DELETE FROM "FriendRequests" WHERE "Uuid" = "requestUuid";
        INSERT INTO "UsersFriends" ("User1Uuid", "User2Uuid") VALUES ("ToUserUuid", "FromUserUuid");
    END IF;
    INSERT INTO "FriendRequests" ("User1Uuid", "User2Uuid") VALUES ("FromUserUuid", "ToUserUuid");
END;
$$ LANGUAGE "plpgsql";
