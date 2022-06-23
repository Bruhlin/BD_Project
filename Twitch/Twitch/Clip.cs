using System;

namespace Twitch
{

	[Serializable()]
	public class Clip
	{
		private int id, streamId, creatorChannelId;
		private String title, startTime;
		private int numViews, duration;

		public Clip() : base()
		{
		}
		public Clip(int id) : base()
		{
			this.id = id;
		}

		public override String ToString()
		{
			return title;
		}

		public int Id { get => id; set => id = value; }
        public int StreamId { get => streamId; set => streamId = value; }
        public int CreatorChannelId { get => creatorChannelId; set => creatorChannelId = value; }
        public string Title { get => title; set => title = value; }
        public string StartTime { get => startTime; set => startTime = value; }
        public int NumViews { get => numViews; set => numViews = value; }
        public int Duration { get => duration; set => duration = value; }
    }
}

