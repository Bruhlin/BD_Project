DROP PROCEDURE IF EXISTS Twitch.GetStreamEndDateTime
DROP PROCEDURE IF EXISTS Twitch.GetClipEndTime
DROP PROCEDURE IF EXISTS Twitch.RemoveStream
DROP PROCEDURE IF EXISTS Twitch.RemoveCategory
DROP PROCEDURE IF EXISTS Twitch.RemoveChannel

GO

CREATE PROCEDURE Twitch.GetStreamEndDateTime @Stream_ID INT, @EndDateTime DATETIME OUTPUT
AS
	BEGIN
		DECLARE @StartDateTime AS DATETIME;
		DECLARE @DurationSeconds AS INT;
		SELECT @StartDateTime = Start_date_time FROM Twitch.STREAM WHERE Stream_id = @Stream_ID;
		SELECT @DurationSeconds = Duration_seconds FROM Twitch.STREAM WHERE Stream_id = @Stream_ID;
		SELECT @EndDateTime = End_date_time FROM Twitch.DATETIME_INTERVAL WHERE @StartDateTime = Start_date_time AND @DurationSeconds = Duration_seconds;
	END
GO

CREATE PROCEDURE Twitch.GetClipEndTime @Clip_ID INT, @EndTime TIME OUTPUT
AS
	BEGIN
		DECLARE @StartTime AS TIME;
		DECLARE @DurationSeconds AS INT;
		SELECT @StartTime = Start_time FROM Twitch.CLIP WHERE Clip_id = @Clip_ID;
		SELECT @DurationSeconds = Duration_seconds FROM Twitch.CLIP WHERE Clip_id = @Clip_ID;
		SELECT @EndTime = End_time FROM Twitch.TIME_INTERVAL WHERE @StartTime = Start_Time AND @DurationSeconds = Duration_seconds;
	END
GO


CREATE PROCEDURE Twitch.RemoveStream @Stream_ID INT
As
	BEGIN
		DELETE FROM Twitch.CLIP WHERE Stream_ID = @Stream_ID
		DELETE FROM Twitch.STREAM_TAG WHERE Stream_ID = @Stream_ID
		DELETE FROM Twitch.STREAM WHERE Stream_ID = @Stream_ID
	END
GO

CREATE PROCEDURE Twitch.RemoveCategory @Category_ID INT
AS
	BEGIN
		DECLARE @Stream_ID as int
		SELECT @Stream_ID = Stream_id FROM Twitch.STREAM WHERE Cat_id = @Category_ID
		EXEC Twitch.RemoveStream @Stream_ID
		DELETE FROM Twitch.IS_ACTIVE_IN WHERE Cat_ID = @Category_ID
		DELETE FROM Twitch.CATEGORY WHERE Category_id = @Category_ID
	END
GO

CREATE PROCEDURE Twitch.RemoveChannel @Channel_ID INT
AS
	BEGIN
		DECLARE @Stream_ID as int
		SELECT @Stream_ID = Stream_id FROM Twitch.STREAM WHERE Channel_id = @Channel_ID
		EXEC Twitch.RemoveStream @Stream_ID
		DELETE FROM Twitch.SUBSCRIPTION WHERE From_channel_id = @Channel_ID
		DELETE FROM Twitch.SUBSCRIPTION WHERE To_channel_id = @Channel_ID
		DELETE FROM Twitch.CLIP WHERE Creator_channel_id = @Channel_ID
		DELETE FROM Twitch.FOLLOWS WHERE Follower_channel_id = @Channel_ID
		DELETE FROM Twitch.FOLLOWS WHERE Followed_channel_id = @Channel_ID
		DELETE FROM Twitch.IS_ACTIVE_IN WHERE Channel_id = @Channel_ID
		DELETE FROM Twitch.CHANNEL WHERE Channel_id = @Channel_ID
	END
GO
