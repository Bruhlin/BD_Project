using System;

namespace Twitch
{

	[Serializable()]
	public class Channel
	{
		private int id;
		private String name, dateCreated;
		private int numFollowers, numSubs;
		private float avgViewers;
		private int peakViewers;
		private int daysActive;
		private float hoursStreamed, hoursWatched;

		public Channel() : base()
		{
		}

		public Channel(int id) : base()
		{
			this.id = id;
		}

		public override String ToString()
		{
			return name;
		}

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		public String Name
		{
			get { return name; }
			set
			{
				if (value == null | String.IsNullOrEmpty(value))
					throw new Exception("Channel Name cannot be empty");
				name = value;
			}
		}

		public String DateCreated
		{
			get { return dateCreated; }
			set
			{
				if (value == null | String.IsNullOrEmpty(value))
					throw new Exception("Date Created cannot be empty");
				dateCreated = value;
			}
		}

		public int NumFollowers
		{
			get { return numFollowers; }
			set
			{
				numFollowers = value;
			}
		}

		public int NumSubs
		{
			get { return numSubs; }
			set { numSubs = value; }
		}

		public float AvgViewers
		{
			get { return avgViewers; }
			set { avgViewers = value; }
		}

		public int PeakViewers
		{
			get { return peakViewers; }
			set { peakViewers = value; }
		}

		public int DaysActive
		{
			get { return daysActive; }
			set { daysActive = value; }
		}

		public float HoursStreamed
		{
			get { return hoursStreamed; }
			set { hoursStreamed = value; }
		}

		public float HoursWatched
		{
			get { return hoursWatched; }
			set { hoursWatched = value; }
		}

	}
}

