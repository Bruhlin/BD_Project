DELETE FROM Twitch.STREAM_TAG
DELETE FROM Twitch.FOLLOWS
DELETE FROM Twitch.IS_ACTIVE_IN
DELETE FROM TWITCH.CLIP
DELETE FROM Twitch.SUBSCRIPTION
DELETE FROM Twitch.STREAM
DELETE FROM Twitch.CHANNEL
DELETE FROM Twitch.CATEGORY
DELETE FROM Twitch.DATETIME_INTERVAL
DELETE FROM Twitch.TIME_INTERVAL

GO

-- DATE: 'MM-DD-YYYY'
INSERT INTO Twitch.CHANNEL (Channel_id, Channel_name, Date_created, Num_followers, Num_subs, Days_active, Hours_streamed, Hours_watched, Avg_viewers, Peak_viewers)
VALUES (283948, 'xQc', '09-13-2014', 10843374, 91317, 1920, 17898, 645000000, 36036.0, 312158)
INSERT INTO Twitch.CHANNEL (Channel_id, Channel_name, Date_created, Num_followers, Num_subs, Days_active, Hours_streamed, Hours_watched, Avg_viewers, Peak_viewers)
VALUES (152411, 'forsen', '05-19-2011', 1620907, 9033, 1730, 10821, 120000000, 11084.0, 80860)
INSERT INTO Twitch.CHANNEL (Channel_id, Channel_name, Date_created, Num_followers, Num_subs, Days_active, Hours_streamed, Hours_watched, Avg_viewers, Peak_viewers)
VALUES (423591, 'Asmongold', '11-20-2011', 3230631, 39814, 1238, 7807, 254000000, 32559.0, 448161)
INSERT INTO Twitch.CHANNEL (Channel_id, Channel_name, Date_created, Num_followers, Num_subs, Days_active, Hours_streamed, Hours_watched, Avg_viewers, Peak_viewers)
VALUES (928033, 'loltyler1', '11-12-2013', 4989110, 15054, 1413, 11044, 277000000, 25079.0, 368484)
INSERT INTO Twitch.CHANNEL (Channel_id, Channel_name, Date_created, Num_followers, Num_subs, Days_active, Hours_streamed, Hours_watched, Avg_viewers, Peak_viewers)
VALUES (711974, 'shroud', '11-03-2011', 10092644, 4357, 1517, 12224, 348000000, 28434.0, 516289)
INSERT INTO Twitch.CHANNEL (Channel_id, Channel_name, Date_created, Num_followers, Num_subs, Days_active, Hours_streamed, Hours_watched, Avg_viewers, Peak_viewers)
VALUES (000001, 'clipper', '10-10-2010', 0, 0, 1, 24, 5, 3.0, 7)
INSERT INTO Twitch.CHANNEL (Channel_id, Channel_name, Date_created, Num_followers, Num_subs, Days_active, Hours_streamed, Hours_watched, Avg_viewers, Peak_viewers)
VALUES (000002, 'subscriber', '12-20-2020', 100, 16, 7, 168, 500, 321.0, 1659)
INSERT INTO Twitch.CHANNEL (Channel_id, Channel_name, Date_created, Num_followers, Num_subs, Days_active, Hours_streamed, Hours_watched, Avg_viewers, Peak_viewers)
VALUES (000003, 'follower', '03-19-2010', 0, 0, 0, 0, 0, 0.0, 0)


INSERT INTO Twitch.CATEGORY (Category_id, Category_name, Num_live_viewers, Num_live_channels, AvgViewers_7days, AvgChannels_7days, Rank)
VALUES (1,'Just Chatting', 364513, 4405, 347635.0, 4039.0, 1)
INSERT INTO Twitch.CATEGORY (Category_id, Category_name, Num_live_viewers, Num_live_channels, AvgViewers_7days, AvgChannels_7days, Rank)
VALUES (2, 'Grand Theft Auto V', 118153, 2862, 159553.0, 2655.0, 2)
INSERT INTO Twitch.CATEGORY (Category_id, Category_name, Num_live_viewers, Num_live_channels, AvgViewers_7days, AvgChannels_7days, Rank)
VALUES (3, 'League of Legends', 113792, 4005, 147956.0, 3214.0, 3)
INSERT INTO Twitch.CATEGORY (Category_id, Category_name, Num_live_viewers, Num_live_channels, AvgViewers_7days, AvgChannels_7days, Rank)
VALUES (4, 'VALORANT', 123064, 5082, 139390.0, 4914.0, 4)
INSERT INTO Twitch.CATEGORY (Category_id, Category_name, Num_live_viewers, Num_live_channels, AvgViewers_7days, AvgChannels_7days, Rank)
VALUES (5, 'Fortnite', 75314, 7066, 101329.0, 6105.0, 5)

-- DATETIME: 'YYYYMMDD HH:MM:SS PM'
INSERT INTO Twitch.STREAM (Stream_id, Channel_id, Cat_id, Title, Num_current_viewers, Num_peak_viewers, Has_ended, Start_date_time, Duration_seconds)
VALUES (230419, 283948, 1, 'CLICK NOW FAST ITS HERE BIG STRIM ALL DAY ALL NIGHT WARLORD JUICER', 78382, 131335, 1, '20220612 04:55:00 AM', 76457)
INSERT INTO Twitch.STREAM (Stream_id, Channel_id, Cat_id, Title, Num_current_viewers, Num_peak_viewers, Has_ended, Start_date_time, Duration_seconds)
VALUES (838431, 152411, 2, 'Short stream today?', 14153, 18547, 1, '20190222 01:54:00 PM', 31101)
INSERT INTO Twitch.STREAM (Stream_id, Channel_id, Cat_id, Title, Num_current_viewers, Num_peak_viewers, Has_ended, Start_date_time, Duration_seconds)
VALUES (514912, 711974, 4, 'Playing with viewers :)', 9595, 12267, 1, '20220519 01:33:00 AM', 25295)
INSERT INTO Twitch.STREAM (Stream_id, Channel_id, Cat_id, Title, Num_current_viewers, Num_peak_viewers, Has_ended, Start_date_time, Duration_seconds)
VALUES (913794, 423591, 5, 'Solo Fortnite games', 42311, 73034, 1, '20191013 03:00:00 PM', 34000)
INSERT INTO Twitch.STREAM (Stream_id, Channel_id, Cat_id, Title, Num_current_viewers, Num_peak_viewers, Has_ended, Start_date_time, Duration_seconds)
VALUES (183273, 928033, 3, 'The return!', 30275, 35240, 0, '20220612 8:58:00 PM', 30229)


INSERT INTO Twitch.SUBSCRIPTION (From_channel_id, To_channel_id, Is_Amazon_Prime, Duration_months)
VALUES (152411, 283948, 0, 24)
INSERT INTO Twitch.SUBSCRIPTION (From_channel_id, To_channel_id, Is_Amazon_Prime, Duration_months)
VALUES (000002, 423591, 0, 2)
INSERT INTO Twitch.SUBSCRIPTION (From_channel_id, To_channel_id, Is_Amazon_Prime, Duration_months)
VALUES (000002, 152411, 1, 1)
INSERT INTO Twitch.SUBSCRIPTION (From_channel_id, To_channel_id, Is_Amazon_Prime, Duration_months)
VALUES (000002, 928033, 0, 6)
INSERT INTO Twitch.SUBSCRIPTION (From_channel_id, To_channel_id, Is_Amazon_Prime, Duration_months)
VALUES (711974, 928033, 1, 15)


-- TIME: 'HH:MM:SS'
INSERT INTO Twitch.CLIP (Clip_id, Stream_id, Creator_channel_id, Title, Start_time, Duration_seconds, Num_views)
VALUES (73219, 230419, 000001, 'Lovely clip!', '10:32:59', 21, 54371)
--INSERT INTO Twitch.CLIP (Clip_id, Stream_id, Creator_channel_id, Title, Start_time, Duration_seconds, Num_views)
--VALUES (95392, 838431, 000001, 'NO WAY!', '02:16:29', 34, 178)
INSERT INTO Twitch.CLIP (Clip_id, Stream_id, Creator_channel_id, Title, Start_time, Duration_seconds, Num_views)
VALUES (12831, 514912, 000001, 'Cool play', '04:01:42', 18, 12589)
INSERT INTO Twitch.CLIP (Clip_id, Stream_id, Creator_channel_id, Title, Start_time, Duration_seconds, Num_views)
VALUES (68303, 913794, 000001, 'Funny moment', '07:32:09', 42, 8327)
INSERT INTO Twitch.CLIP (Clip_id, Stream_id, Creator_channel_id, Title, Start_time, Duration_seconds, Num_views)
VALUES (19712, 183273, 000001, 'How did that happen', '00:59:41', 50, 624)
INSERT INTO Twitch.CLIP (Clip_id, Stream_id, Creator_channel_id, Title, Start_time, Duration_seconds, Num_views)
VALUES (19713, 183273, 000001, 'v2 How did that happen?', '01:28:33', 45, 3722)
INSERT INTO Twitch.CLIP (Clip_id, Stream_id, Creator_channel_id, Title, Start_time, Duration_seconds, Num_views)
VALUES (19714, 183273, 000001, 'v3 How did that happen...', '02:02:44', 51, 310)


INSERT INTO Twitch.FOLLOWS (Follower_channel_id, Followed_channel_id, Date_followed)
Values (000003, 283948, '10-07-2019')
INSERT INTO Twitch.FOLLOWS (Follower_channel_id, Followed_channel_id, Date_followed)
Values (000003, 711974, '02-17-2018')
INSERT INTO Twitch.FOLLOWS (Follower_channel_id, Followed_channel_id, Date_followed)
Values (283948, 928033, '11-29-2016')
INSERT INTO Twitch.FOLLOWS (Follower_channel_id, Followed_channel_id, Date_followed)
Values (152411, 423591, '09-16-2021')
INSERT INTO Twitch.FOLLOWS (Follower_channel_id, Followed_channel_id, Date_followed)
Values (711974, 928033, '12-30-2017')


INSERT INTO Twitch.IS_ACTIVE_IN (Channel_id, Cat_id, Avg_viewers, Hours_streamed)
Values (283948, 1, 29072.0, 9731)
INSERT INTO Twitch.IS_ACTIVE_IN (Channel_id, Cat_id, Avg_viewers, Hours_streamed)
Values (152411, 2, 8913.0, 1072)
INSERT INTO Twitch.IS_ACTIVE_IN (Channel_id, Cat_id, Avg_viewers, Hours_streamed)
Values (423591, 5, 15981.0, 259)
INSERT INTO Twitch.IS_ACTIVE_IN (Channel_id, Cat_id, Avg_viewers, Hours_streamed)
Values (928033, 3, 19029.0, 1038)
INSERT INTO Twitch.IS_ACTIVE_IN (Channel_id, Cat_id, Avg_viewers, Hours_streamed)
Values (711974, 4, 25986.0, 10391)

INSERT INTO Twitch.STREAM_TAG (Stream_id, Tag)
Values (230419, 'Inglês')
INSERT INTO Twitch.STREAM_TAG (Stream_id, Tag)
Values (838431, 'Inglês')
INSERT INTO Twitch.STREAM_TAG (Stream_id, Tag)
Values (838431, 'Interpretação de personagens')
INSERT INTO Twitch.STREAM_TAG (Stream_id, Tag)
Values (514912, 'Inglês')
INSERT INTO Twitch.STREAM_TAG (Stream_id, Tag)
Values (514912, 'Competitivo')
INSERT INTO Twitch.STREAM_TAG (Stream_id, Tag)
Values (913794, 'Inglês')
INSERT INTO Twitch.STREAM_TAG (Stream_id, Tag)
Values (913794, 'Primeira vez no jogo')
INSERT INTO Twitch.STREAM_TAG (Stream_id, Tag)
Values (913794, 'Squad Stream')
INSERT INTO Twitch.STREAM_TAG (Stream_id, Tag)
Values (183273, 'Inglês')
INSERT INTO Twitch.STREAM_TAG (Stream_id, Tag)
Values (183273, 'PvP')
INSERT INTO Twitch.STREAM_TAG (Stream_id, Tag)
Values (183273, 'ASMR auditivo')
