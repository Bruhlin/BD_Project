DROP FUNCTION IF EXISTS Twitch.GetFollowers
DROP FUNCTION IF EXISTS Twitch.GetFollowing
DROP FUNCTION IF EXISTS Twitch.GetSubsFrom
DROP FUNCTION IF EXISTS Twitch.GetSubsTo
DROP FUNCTION IF EXISTS Twitch.GetStreamClips

GO
CREATE FUNCTION Twitch.GetFollowers(@Channel_id INT) RETURNS Table
AS
RETURN (
SELECT Follower_channel_id, Channel_name
FROM Twitch.FOLLOWS JOIN Twitch.CHANNEL ON Follower_channel_id=Channel_id
WHERE Followed_channel_id=@Channel_id
)
GO

--SELECT * FROM Twitch.GetFollowers(928033)

GO
CREATE FUNCTION Twitch.GetFollowing(@Channel_id INT) RETURNS Table
AS
RETURN (
SELECT Followed_channel_id, U.Channel_name
FROM Twitch.FOLLOWS JOIN Twitch.CHANNEL AS U ON Followed_channel_id=Channel_id
WHERE Follower_channel_id=@Channel_id
)
GO

--SELECT * FROM Twitch.GetFollowing(3)

GO
-- Returns channels Channel_id is subbed to
CREATE FUNCTION Twitch.GetSubsFrom(@Channel_id INT) RETURNS Table
AS
RETURN (
SELECT To_channel_id, U.Channel_name
FROM Twitch.SUBSCRIPTION JOIN Twitch.CHANNEL AS U ON To_channel_id=Channel_id
WHERE From_channel_id=@Channel_id
)
GO

--SELECT * FROM Twitch.GetSubsFrom(2)

GO
-- Returns channels subbed to Channel_id 
CREATE FUNCTION Twitch.GetSubsTo(@Channel_id INT) RETURNS Table
AS
RETURN (
SELECT From_channel_id, U.Channel_name
FROM Twitch.SUBSCRIPTION JOIN Twitch.CHANNEL AS U ON From_channel_id=Channel_id
WHERE To_channel_id=@Channel_id
)
GO

-- SELECT * FROM Twitch.GetSubsTo(928033)






