DROP TABLE IF EXISTS Twitch.STREAM_TAGS;
DROP TABLE IF EXISTS Twitch.STREAM_TAG;
DROP TABLE IF EXISTS Twitch.IS_ACTIVE_IN;
DROP TABLE IF EXISTS Twitch.CLIP;
DROP TABLE IF EXISTS Twitch.STREAM;
DROP TABLE IF EXISTS Twitch.SUBSCRIPTION;
DROP TABLE IF EXISTS Twitch.FOLLOWS;
DROP TABLE IF EXISTS Twitch.CHANNEL;
DROP TABLE IF EXISTS Twitch.CATEGORY;
DROP TABLE IF EXISTS Twitch.TIME_INTERVAL;
DROP TABLE IF EXISTS Twitch.DATETIME_INTERVAL;
DROP SCHEMA IF EXISTS Twitch;

GO
CREATE SCHEMA Twitch;
GO

CREATE TABLE Twitch.CHANNEL (
	Channel_id INT UNIQUE NOT NULL,
	Channel_name VARCHAR(30) UNIQUE NOT NULL,
	Date_created DATE NOT NULL,
	Num_followers INT NOT NULL,
	Num_subs INT NOT NULL,
	Days_active INT NOT NULL,
	Hours_streamed INT NOT NULL,
	Hours_watched INT NOT NULL,
	Avg_viewers DECIMAL(9,1) NOT NULL,
	Peak_viewers INT NOT NULL,
	PRIMARY KEY (Channel_id)
)

CREATE TABLE Twitch.CATEGORY (
	Category_id INT UNIQUE NOT NULL,
	Category_name VARCHAR(30) UNIQUE NOT NULL,
	Num_live_viewers INT NOT NULL,
	Num_live_channels INT NOT NULL,
	AvgViewers_7days DECIMAL(9,1),
	AvgChannels_7days DECIMAL(9,1),
	Rank INT NOT NULL,
	PRIMARY KEY (Category_id)
)

CREATE TABLE Twitch.STREAM (
	Stream_id INT UNIQUE NOT NULL,
	Channel_id INT NOT NULL,
	Cat_id INT NOT NULL,
	Title VARCHAR(100),
	Num_current_viewers INT NOT NULL,
	Num_peak_viewers INT,
	Has_ended BIT NOT NULL,
	Start_date_time DATETIME NOT NULL,
	Duration_seconds INT NOT NULL,
	PRIMARY KEY (Stream_id),
	FOREIGN KEY (Channel_id) REFERENCES Twitch.CHANNEL(Channel_id),
	FOREIGN KEY (Cat_id) REFERENCES Twitch.CATEGORY(Category_id)
)

CREATE TABLE Twitch.SUBSCRIPTION (
	From_channel_id INT NOT NULL,
	To_channel_id INT NOT NULL,
	Is_Amazon_prime BIT NOT NULL,
	Duration_months INT NOT NULL,
	PRIMARY KEY (From_channel_id, To_channel_id),
	FOREIGN KEY (From_channel_id) REFERENCES Twitch.CHANNEL(Channel_id),
	FOREIGN KEY (To_channel_id) REFERENCES Twitch.CHANNEL (Channel_id)
)

CREATE TABLE Twitch.CLIP (
	Clip_id INT UNIQUE NOT NULL,
	Stream_id INT NOT NULL,
	Creator_channel_id INT NOT NULL,
	Title VARCHAR(100) NOT NULL,
	Start_time TIME NOT NULL,
	Duration_seconds INT NOT NULL,
	Num_views INT NOT NULL,
	PRIMARY KEY (Clip_id),
	FOREIGN KEY (Stream_id) REFERENCES Twitch.STREAM(Stream_id),
	FOREIGN KEY (Creator_channel_id) REFERENCES Twitch.CHANNEL(Channel_id)
)

CREATE TABLE Twitch.FOLLOWS (
	Date_followed DATE NOT NULL,
	Follower_channel_id INT NOT NULL,
	Followed_channel_id INT NOT NULL,
	PRIMARY KEY (Follower_channel_id, Followed_channel_id),
	FOREIGN KEY (Follower_channel_id) REFERENCES Twitch.CHANNEL(Channel_id),
	FOREIGN KEY (Followed_channel_id) REFERENCES Twitch.CHANNEL(Channel_id)
)

CREATE TABLE Twitch.IS_ACTIVE_IN (
	Avg_viewers DECIMAL(9,1) NOT NULL,
	Hours_streamed INT NOT NULL,
	Channel_id INT NOT NULL,
	Cat_id INT NOT NULL,
	PRIMARY KEY (Channel_id, Cat_id),
	FOREIGN KEY (Channel_id) REFERENCES Twitch.CHANNEL (Channel_id),
	FOREIGN KEY (Cat_id) REFERENCES Twitch.CATEGORY (Category_id)
)

CREATE TABLE Twitch.STREAM_TAG (
	Stream_id INT NOT NULL,
	Tag VARCHAR(30) NOT NULL,
	PRIMARY KEY (Stream_id, Tag),
	FOREIGN KEY (Stream_id) REFERENCES Twitch.STREAM (Stream_id)
)

CREATE TABLE Twitch.TIME_INTERVAL
(
	Start_time TIME,
	Duration_seconds INT,
	End_time TIME,
	PRIMARY KEY (Start_time, Duration_seconds)
)

CREATE TABLE Twitch.DATETIME_INTERVAL
(
	Start_date_time DATETIME,
	Duration_seconds INT,
	End_date_time DATETIME,
	PRIMARY KEY (Start_date_time, Duration_seconds)
)