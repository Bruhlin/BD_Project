DROP TRIGGER IF EXISTS Twitch.CreateTimeInterval;
DROP TRIGGER IF EXISTS Twitch.CreateDateTimeInterval;

GO

CREATE TRIGGER Twitch.CreateTimeInterval on Twitch.CLIP
AFTER INSERT, UPDATE
AS
	SET NOCOUNT ON;
	DECLARE @StartTime as TIME;
	DECLARE @DurationSeconds as INT;
	DECLARE @EndTime as TIME;
	SELECT @StartTime = Start_time FROM INSERTED;
	SELECT @DurationSeconds = Duration_seconds FROM INSERTED;
	SELECT @EndTime = DATEADD(SECOND, @DurationSeconds, @StartTime);

	IF EXISTS (SELECT * FROM Twitch.TIME_INTERVAL WHERE @StartTime = Start_time AND @DurationSeconds = Duration_seconds AND @Endtime = End_time)
	BEGIN
		RAISERROR ('This clip already exists!', 16, 1);
		ROLLBACK TRAN;
	END

	BEGIN
		INSERT INTO Twitch.TIME_INTERVAL(Start_time, Duration_seconds, End_Time)
		VALUES(@StartTime, @DurationSeconds, @EndTime)
	END
GO

CREATE TRIGGER Twitch.CreateDateTimeInterval on Twitch.STREAM
AFTER INSERT, UPDATE
AS
	SET NOCOUNT ON;
	DECLARE @StartDateTime as DATETIME;
	DECLARE @DurationSeconds as INT;
	DECLARE @EndDateTime as DATETIME;
	DECLARE @live as BIT;
	SELECT @StartDateTime = Start_date_Time FROM INSERTED;
	SELECT @DurationSeconds = Duration_seconds FROM INSERTED;
	SELECT @EndDateTime = DATEADD(SECOND, @DurationSeconds, @StartDateTime)
	SELECT @live = Has_ended FROM INSERTED;

	IF EXISTS (SELECT * FROM Twitch.DATETIME_INTERVAL WHERE @StartDateTime = Start_date_time AND @DurationSeconds = Duration_seconds AND @EndDateTime = End_date_time)
	BEGIN
		RAISERROR ('This stream already exists!', 16, 1);
		ROLLBACK TRAN;
	END

	BEGIN
		IF @live = 1
		BEGIN
			INSERT INTO Twitch.DATETIME_INTERVAL(Start_date_time, Duration_seconds, End_date_time)
			VALUES (@StartDateTime, @DurationSeconds, @EndDateTime)
		END
	END
GO
