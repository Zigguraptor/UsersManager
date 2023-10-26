CREATE OR REPLACE FUNCTION "is_friends_func"(
    "@User1Uuid" UUID,
    "@User2Uuid" UUID
) RETURNS boolean AS
$$
DECLARE
    "friends_exists" boolean;
BEGIN
    SELECT EXISTS(
                 SELECT NULL
                 FROM "UsersFriends"
                 WHERE ("User1Uuid" = "@User1Uuid" AND "User2Uuid" = "@User2Uuid")
                    OR ("User1Uuid" = "@User2Uuid" AND "User2Uuid" = "@User1Uuid")
                 )
    INTO "friends_exists";

    RETURN "friends_exists";
END;
$$ LANGUAGE "plpgsql";
