using System;

namespace Twitch
{

	[Serializable()]
	public class Stream
	{
		private int id;
		private String title;
		private int channelId, categoryId;
		private int numCurrentViewers, numPeakViewers;
		private int duration; // seconds
		private bool hasEnded;
		private String startDateAndTime;

		public Stream() : base()
		{
		}
		public Stream(int id) : base()
		{
			this.id = id;
		}

		public override String ToString()
		{
			return title;
		}

		public int Id { get => id; set => id = value; }
		public string Title { get => title; set => title = value; }
		public int ChannelId
		{
			get => channelId;
			set => channelId = value;
		}
		public int CategoryId
		{
			get => categoryId;
			set => categoryId = value;
		}
		public int NumCurrentViewers { get => numCurrentViewers; set => numCurrentViewers = value; }
		public int NumPeakViewers { get => numPeakViewers; set => numPeakViewers = value; }
		public int Duration { get => duration; set => duration = value; }
		public bool HasEnded { get => hasEnded; set => hasEnded = value; }
		public string StartDateAndTime { get => startDateAndTime; set => startDateAndTime = value; }
	}
}

