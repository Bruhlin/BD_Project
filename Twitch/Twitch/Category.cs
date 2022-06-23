using System;

namespace Twitch
{

	[Serializable()]
	public class Category
	{
		private int id;
		private String name;
		private int numLiveViewers, numLiveChannels;
		private float avgViewers7Days, avgChannels7Days;
		private int rank;

		public Category() : base()
		{
		}

		public Category(int id) : base()
		{
			this.id = id;
		}

		public override String ToString()
		{
			return name;
		}

		public int Id { get => id; set => id = value; }

		public string Name
		{
			get => name;
			set
			{
				if (value == null | String.IsNullOrEmpty(value))
					throw new Exception("Channel Name cannot be empty");
				name = value;
			}
		}
		public int NumLiveViewers { get => numLiveViewers; set => numLiveViewers = value; }
		public int NumLiveChannels { get => numLiveChannels; set => numLiveChannels = value; }
		public float AvgViewers7Days { get => avgViewers7Days; set => avgViewers7Days = value; }
		public float AvgChannels7Days { get => avgChannels7Days; set => avgChannels7Days = value; }
		public int Rank { get => rank; set => rank = value; }

	}
}

