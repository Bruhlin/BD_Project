using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq; // for Enumerable.OfType()
using System.Diagnostics; // for Debug.WriteLine()

namespace Twitch
{
    public partial class Form1 : Form
    {
        private SqlConnection cn;
        private int currentListBoxSelectIndex;
        private bool adding;
        List<TextBox> streamTagTxtBoxes = new List<TextBox>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            emptyPanel.BringToFront();
            streamTagTxtBoxes.Add(streamTag1TxtBox);
            streamTagTxtBoxes.Add(streamTag2TxtBox);
            streamTagTxtBoxes.Add(streamTag3TxtBox);
            streamTagTxtBoxes.Add(streamTag4TxtBox);

            panelToolStripMenuItem1.Text = "General info";
            panelToolStripMenuItem2.Text = "Followers and Following";
            panelToolStripMenuItem3.Text = "Subs";
            panelToolStripMenuItem1.Click += ShowChannel;
            panelToolStripMenuItem2.Click += ShowFollowersAndFollowing;
            panelToolStripMenuItem3.Click += ShowSubs;

            cn = getSGBDConnection();
            loadChannelsToolStripMenuItem_Click(null, null);
        }


        private SqlConnection getSGBDConnection()
        {
            return new SqlConnection("data source= mednat.ieeta.pt\\SQLSERVER,8101; initial catalog=p6g9; User id=p6g9; Password=qwrmK35");
            //return new SqlConnection("data source= DESKTOP-FI4THUP\\SQLEXPRESS;integrated security=true;initial catalog=master");
        }

        private bool verifySGBDConnection()
        {
            if (cn == null)
                cn = getSGBDConnection();

            if (cn.State != ConnectionState.Open)
                cn.Open();

            return cn.State == ConnectionState.Open;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                currentListBoxSelectIndex = listBox1.SelectedIndex;
                switch (listSelected)
                {
                    case 0:
                        ShowChannel(null, null);
                        break;
                    case 1:
                        ShowCategory();
                        break;
                    case 2:
                        ShowStream(null, null);
                        break;
                }
            }
        }

        #region ToolStrip (File and Panel) functions
        private void ClearPanelToolStrip()
        {
            panelToolStripMenuItem1.Visible = false;
            panelToolStripMenuItem2.Visible = false;
            panelToolStripMenuItem3.Visible = false;
            panelToolStripMenuItem4.Visible = false;
            panelToolStripMenuItem1.Text = "1";
            panelToolStripMenuItem2.Text = "2";
            panelToolStripMenuItem3.Text = "3";
            panelToolStripMenuItem4.Text = "4";

            switch (listSelected)
            {
                case 0:
                    panelToolStripMenuItem1.Click -= ShowChannel;
                    panelToolStripMenuItem2.Click -= ShowFollowersAndFollowing;
                    panelToolStripMenuItem3.Click -= ShowSubs;
                    break;
                case 1:
                    break;
                case 2:
                    panelToolStripMenuItem1.Click -= ShowStream;
                    panelToolStripMenuItem2.Click -= LoadStreamClips;
                    break;
            }
        }
        private int listSelected = 0; // 0 = channels, 1 = categories, 2 = streams
        private void loadChannelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("SELECT * FROM Twitch.CHANNEL", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            ClearPanelToolStrip();
            listSelected = 0;
            listBox1.Items.Clear();

            /*
            CREATE TABLE Twitch.CHANNEL (
	            Channel_id INT NOT NULL,
	            Channel_name VARCHAR(30) NOT NULL,
	            Date_created DATE NOT NULL,
	            Num_followers INT NOT NULL,
	            Num_subs INT NOT NULL,
	            Days_active INT NOT NULL,
	            Hours_streamed INT NOT NULL,
	            Hours_watched INT NOT NULL,
	            Avg_viewers DECIMAL(9,1) NOT NULL,
	            Peak_viewers INT NOT NULL,
            )
            */

            while (reader.Read())
            {
                Channel channel = new Channel(Int32.Parse(reader["Channel_id"].ToString()));
                channel.Name = reader["Channel_name"].ToString();

                // reader["Date_created"].ToString() returns "30/10/2011 00:00:00"
                // sql date is MM-DD-YYYY
                char[] charArr = reader["Date_created"].ToString().ToCharArray();
                char[] desiredChars = { charArr[3], charArr[4], '-', charArr[0], charArr[1], '-', charArr[6], charArr[7], charArr[8], charArr[9] };
                channel.DateCreated = new string(desiredChars);

                channel.NumFollowers = Int32.Parse(reader["Num_followers"].ToString());
                channel.NumSubs = Int32.Parse(reader["Num_subs"].ToString());
                channel.DaysActive = Int32.Parse(reader["Days_active"].ToString());
                channel.HoursStreamed = float.Parse(reader["Hours_streamed"].ToString());
                channel.HoursWatched = float.Parse(reader["Hours_watched"].ToString());
                channel.AvgViewers = float.Parse(reader["Avg_viewers"].ToString());
                channel.PeakViewers = Int32.Parse(reader["Peak_viewers"].ToString());
                listBox1.Items.Add(channel);
            }

            cn.Close();

            panelToolStripMenuItem1.Text = "General info";
            panelToolStripMenuItem2.Text = "Followers and Following";
            panelToolStripMenuItem3.Text = "Subs";
            panelToolStripMenuItem1.Click += ShowChannel;
            panelToolStripMenuItem2.Click += ShowFollowersAndFollowing;
            panelToolStripMenuItem3.Click += ShowSubs;
            panelToolStripMenuItem1.Visible = true;
            panelToolStripMenuItem2.Visible = true;
            panelToolStripMenuItem3.Visible = true;

            channelIdTxtBox.Text = "";
            channelNameTxtBox.Text = "";
            channelDateCreatedTxtBox.Text = "";
            channelFollowersTxtBox.Text = "";
            channelSubsTxtBox.Text = "";
            channelAvgViewersTxtBox.Text = "";
            channelPeakViewersTxtBox.Text = "";
            channelDaysActiveTxtBox.Text = "";
            channelHoursStreamedTxtBox.Text = "";
            channelHoursWatchedTxtBox.Text = "";
            channelPanel.BringToFront();

            listBox1.SelectedIndex = 0;
            currentListBoxSelectIndex = 0;
            ShowChannel(null, null);
            ShowButtons();

        }

        private void loadCategoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("SELECT * FROM Twitch.CATEGORY", cn);
            SqlDataReader reader = cmd.ExecuteReader();

            ClearPanelToolStrip();
            listSelected = 1;
            listBox1.Items.Clear();

            /*
            CREATE TABLE Twitch.CATEGORY (
	            Category_id INT NOT NULL,
	            Category_name VARCHAR(30) NOT NULL,
	            Num_live_viewers INT NOT NULL,
	            Num_live_channels INT NOT NULL,
	            AvgViewers_7days DECIMAL(9,1),
	            AvgChannels_7days DECIMAL(9,1),
	            Rank INT NOT NULL,
            )
            */

            while (reader.Read())
            {
                Category cat = new Category(Int32.Parse(reader["Category_id"].ToString()));
                cat.Name = reader["Category_name"].ToString();
                cat.NumLiveViewers = Int32.Parse(reader["Num_live_viewers"].ToString());
                cat.NumLiveChannels = Int32.Parse(reader["Num_live_channels"].ToString());
                cat.AvgViewers7Days = float.Parse(reader["AvgViewers_7days"].ToString());
                cat.AvgChannels7Days = float.Parse(reader["AvgChannels_7days"].ToString());
                cat.Rank = Int32.Parse(reader["Rank"].ToString());
                listBox1.Items.Add(cat);
            }

            cn.Close();

            catIdTxtBox.Text = "";
            catNameTxtBox.Text = "";
            catRankTxtBox.Text = "";
            catLiveViewersTxtBox.Text = "";
            catLiveChannelsTxtBox.Text = "";
            catAvgViewers7dTxtBox.Text = "";
            catAvgChannels7dTxtBox.Text = "";
            categoryPanel.BringToFront();

            listBox1.SelectedIndex = 0;
            currentListBoxSelectIndex = 0;
            ShowCategory();
            ShowButtons();

        }

        private void loadStreamsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("SELECT * FROM Twitch.STREAM", cn);
            SqlDataReader reader = cmd.ExecuteReader();

            ClearPanelToolStrip();
            listSelected = 2;
            listBox1.Items.Clear();

            /*
            CREATE TABLE Twitch.STREAM (
	            Stream_id INT NOT NULL,
	            Channel_id INT NOT NULL,
	            Cat_id INT NOT NULL,
	            Title VARCHAR(100),
	            Num_current_viewers INT NOT NULL,
	            Num_peak_viewers INT,
	            Has_ended BIT NOT NULL,
	            Start_date_time DATETIME NOT NULL,
	            Duration_seconds INT NOT NULL,
            )
            */
            while (reader.Read())
            {
                Stream stream = new Stream(Int32.Parse(reader["Stream_id"].ToString()));
                stream.ChannelId = Int32.Parse(reader["Channel_id"].ToString());
                stream.CategoryId = Int32.Parse(reader["Cat_id"].ToString());
                stream.Title = reader["Title"].ToString();
                stream.NumCurrentViewers = Int32.Parse(reader["Num_current_viewers"].ToString());
                stream.NumPeakViewers = Int32.Parse(reader["Num_peak_viewers"].ToString());
                stream.Duration = Int32.Parse(reader["Duration_seconds"].ToString());
                if (reader["Has_ended"].ToString() == "True")
                    stream.HasEnded = true;
                else stream.HasEnded = false;
                stream.StartDateAndTime = ReaderDatetimeToSqlDatetime(reader["Start_date_time"].ToString());

                listBox1.Items.Add(stream);

            }

            cn.Close();

            panelToolStripMenuItem1.Text = "General info";
            panelToolStripMenuItem2.Text = "Stream clips";
            panelToolStripMenuItem1.Click += ShowStream;
            panelToolStripMenuItem2.Click += LoadStreamClips;
            panelToolStripMenuItem1.Visible = true;
            panelToolStripMenuItem2.Visible = true;

            streamIdTxtBox.Text = "";
            streamerChannelNameTxtBox.Text = "";
            streamCatNameTxtBox.Text = "";
            streamTitleTxtBox.Text = "";
            streamCurrentViewersTxtBox.Text = "";
            streamPeakViewersTxtBox.Text = "";
            streamHasEndedTxtBox.Text = "";
            streamDurationTxtBox.Text = "";
            streamStartDatetimeTxtBox.Text = "";
            streamEndDatetimeTxtBox.Text = "";
            streamPanel.BringToFront();

            listBox1.SelectedIndex = 0;
            currentListBoxSelectIndex = 0;
            ShowStream(null, null);
            ShowButtons();
        }
        #endregion

        public String ReaderDatetimeToSqlDatetime(string readerDatetime)
        {
            // reader["Start_date_time"].ToString() returns '18/06/2015 22:34:09' (19 chars)
            // SQL DATETIME: 'YYYYMMDD HH:MM:SS PM'
            // store in stream the SQL way

            if (readerDatetime.Length == 0)
                return "";

            string a = readerDatetime;
            string sqlDatetime = "";
            string year = a.Substring(6, 4);
            string month = a.Substring(3, 2);
            string day = a.Substring(0, 2);
            string hours = a.Substring(11, 2);
            string mins = a.Substring(14, 2);
            string secs = a.Substring(17, 2);
            string amOrPm = "AM";
            if (Int32.Parse(hours) >= 12)
            {
                amOrPm = "PM";
                hours = (Int32.Parse(hours) - 12).ToString();
                if (hours.Length == 1)
                    hours = "0" + hours;
            }
            sqlDatetime = year + month + day + " " + hours + ":" + mins + ":" + secs + " " + amOrPm;
            return sqlDatetime;
        }

        #region Submit, update, remove channel
        private void SubmitChannel(Channel ch)
        {
            if (!verifySGBDConnection())
                return;

            /*
            CREATE TABLE Twitch.CHANNEL (
	            Channel_id INT NOT NULL,
	            Channel_name VARCHAR(30) NOT NULL,
	            Date_created DATE NOT NULL,
	            Num_followers INT NOT NULL,
	            Num_subs INT NOT NULL,
	            Days_active INT NOT NULL,
	            Hours_streamed INT NOT NULL,
	            Hours_watched INT NOT NULL,
	            Avg_viewers DECIMAL(9,1) NOT NULL,
	            Peak_viewers INT NOT NULL,
            )
            */

            SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = "INSERT Customers (CustomerID, CompanyName, ContactName, Address, " + "City, Region, PostalCode, Country) " + "VALUES (@CustomerID, @CompanyName, @ContactName, @Address, " + "@City, @Region, @PostalCode, @Country) ";
            cmd.CommandText = "INSERT INTO Twitch.CHANNEL VALUES (@Channel_id, @Channel_name, @Date_created, @Num_followers, @Num_subs, @Days_active, @Hours_streamed, @Hours_watched, @Avg_viewers, @Peak_viewers)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Channel_id", ch.Id);
            cmd.Parameters.AddWithValue("@Channel_name", ch.Name);
            cmd.Parameters.AddWithValue("@Date_created", ch.DateCreated);
            cmd.Parameters.AddWithValue("@Num_followers", ch.NumFollowers);
            cmd.Parameters.AddWithValue("@Num_subs", ch.NumSubs);
            cmd.Parameters.AddWithValue("@Days_active", ch.DaysActive);
            cmd.Parameters.AddWithValue("@Hours_streamed", ch.HoursStreamed);
            cmd.Parameters.AddWithValue("@Hours_watched", ch.HoursWatched);
            cmd.Parameters.AddWithValue("@Avg_viewers", ch.AvgViewers);
            cmd.Parameters.AddWithValue("@Peak_viewers", ch.PeakViewers);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Channel submit failed. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }


        private void UpdateChannel(Channel ch)
        {
            int rows = 0;

            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            //cmd.CommandText = "UPDATE Customers " + "SET CompanyName = @CompanyName, " + "    ContactName = @ContactName, " + "    Address = @Address, " + "    City = @City, " + "    Region = @Region, " + "    PostalCode = @PostalCode, " + "    Country = @Country " + "WHERE CustomerID = @CustomerID";
            cmd.CommandText = "UPDATE Twitch.CHANNEL SET Channel_name = @Channel_name, Date_created = @Date_created, Num_followers = @Num_followers, Num_subs = @Num_subs, Days_active = @Days_active, Hours_streamed = @Hours_streamed, Hours_watched = @Hours_watched, Avg_viewers = @Avg_viewers, Peak_viewers = @Peak_viewers WHERE Channel_id=@Channel_id";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Channel_id", ch.Id);
            cmd.Parameters.AddWithValue("@Channel_name", ch.Name);
            cmd.Parameters.AddWithValue("@Date_created", ch.DateCreated);
            cmd.Parameters.AddWithValue("@Num_followers", ch.NumFollowers);
            cmd.Parameters.AddWithValue("@Num_subs", ch.NumSubs);
            cmd.Parameters.AddWithValue("@Days_active", ch.DaysActive);
            cmd.Parameters.AddWithValue("@Hours_streamed", ch.HoursStreamed);
            cmd.Parameters.AddWithValue("@Hours_watched", ch.HoursWatched);
            cmd.Parameters.AddWithValue("@Avg_viewers", ch.AvgViewers);
            cmd.Parameters.AddWithValue("@Peak_viewers", ch.PeakViewers);
            cmd.Connection = cn;

            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update contact in database. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                if (rows == 1)
                    MessageBox.Show("Update channel OK");
                else
                    MessageBox.Show("Update channel NOT OK");

                cn.Close();
            }
        }

        private void RemoveChannel(int channelToRemoveId)
        {
            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXEC Twitch.RemoveChannel " + channelToRemoveId.ToString();
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Channel delete failed. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            loadChannelsToolStripMenuItem_Click(null, null);
        }
        #endregion

        #region Submit, update, remove category
        private void SubmitCategory(Category cat)
        {
            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            /*
            CREATE TABLE Twitch.CATEGORY (
	            Category_id INT NOT NULL,
	            Category_name VARCHAR(30) NOT NULL,
	            Num_live_viewers INT NOT NULL,
	            Num_live_channels INT NOT NULL,
	            AvgViewers_7days DECIMAL(9,1),
	            AvgChannels_7days DECIMAL(9,1),
	            Rank INT NOT NULL,
            )
            */

            //cmd.CommandText = "INSERT Customers (CustomerID, CompanyName, ContactName, Address, " + "City, Region, PostalCode, Country) " + "VALUES (@CustomerID, @CompanyName, @ContactName, @Address, " + "@City, @Region, @PostalCode, @Country) ";
            cmd.CommandText = "INSERT INTO Twitch.CATEGORY VALUES (@Category_id, @Category_name, @Num_live_viewers, @Num_live_channels, @AvgViewers_7days, @AvgChannels_7days, @Rank)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Category_id", cat.Id);
            cmd.Parameters.AddWithValue("@Category_name", cat.Name);
            cmd.Parameters.AddWithValue("@Num_live_viewers", cat.NumLiveViewers);
            cmd.Parameters.AddWithValue("@Num_live_channels", cat.NumLiveChannels);
            cmd.Parameters.AddWithValue("@AvgViewers_7days", cat.AvgViewers7Days);
            cmd.Parameters.AddWithValue("@AvgChannels_7days", cat.AvgChannels7Days);
            cmd.Parameters.AddWithValue("@Rank", cat.Rank);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Category submit failed. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }


        private void UpdateCategory(Category cat)
        {
            int rows = 0;

            if (!verifySGBDConnection())
                return;

            /*
            CREATE TABLE Twitch.CATEGORY (
	            Category_id INT NOT NULL,
	            Category_name VARCHAR(30) NOT NULL,
	            Num_live_viewers INT NOT NULL,
	            Num_live_channels INT NOT NULL,
	            AvgViewers_7days DECIMAL(9,1),
	            AvgChannels_7days DECIMAL(9,1),
	            Rank INT NOT NULL,
            )
             */

            SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = "UPDATE Customers " + "SET CompanyName = @CompanyName, " + "    ContactName = @ContactName, " + "    Address = @Address, " + "    City = @City, " + "    Region = @Region, " + "    PostalCode = @PostalCode, " + "    Country = @Country " + "WHERE CustomerID = @CustomerID";
            cmd.CommandText = "UPDATE Twitch.CATEGORY SET Category_name = @Category_name, Num_live_viewers = @Num_live_viewers, Num_live_channels = @Num_live_channels, AvgViewers_7days = @AvgViewers_7days, AvgChannels_7days = @AvgChannels_7days, Rank = @Rank WHERE Category_id=@Category_id";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Category_id", cat.Id);
            cmd.Parameters.AddWithValue("@Category_name", cat.Name);
            cmd.Parameters.AddWithValue("@Num_live_viewers", cat.NumLiveViewers);
            cmd.Parameters.AddWithValue("@Num_live_channels", cat.NumLiveChannels);
            cmd.Parameters.AddWithValue("@AvgViewers_7days", cat.AvgViewers7Days);
            cmd.Parameters.AddWithValue("@AvgChannels_7days", cat.AvgChannels7Days);
            cmd.Parameters.AddWithValue("@Rank", cat.Rank);
            cmd.Connection = cn;

            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Category update failed. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                if (rows == 1)
                    MessageBox.Show("Update category OK");
                else
                    MessageBox.Show("Update category NOT OK");

                cn.Close();
            }
        }


        private void RemoveCategory(int categoryToRemoveId)
        {
            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXEC Twitch.RemoveCategory " + categoryToRemoveId.ToString();
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Cateory delete failed. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            loadCategoriesToolStripMenuItem_Click(null, null);
        }
        #endregion

        #region Submit, update, remove stream
        private void SubmitStream(Stream stream)
        {
            if (!verifySGBDConnection())
                return;

            /*
            CREATE TABLE Twitch.STREAM (
	            Stream_id INT NOT NULL,
	            Channel_id INT NOT NULL,
	            Cat_id INT NOT NULL,
	            Title VARCHAR(100),
	            Num_current_viewers INT NOT NULL,
	            Num_peak_viewers INT,
	            Has_ended BIT NOT NULL,
	            Start_date_time DATETIME NOT NULL,
	            Duration_seconds INT NOT NULL,
            )
            */

            SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = "INSERT Customers (CustomerID, CompanyName, ContactName, Address, " + "City, Region, PostalCode, Country) " + "VALUES (@CustomerID, @CompanyName, @ContactName, @Address, " + "@City, @Region, @PostalCode, @Country) ";
            cmd.CommandText = "INSERT INTO Twitch.STREAM VALUES (@Stream_id, @Channel_id, @Cat_id, @Title, @Num_current_viewers, @Num_peak_viewers, @Has_ended, @Start_date_time, @Duration_seconds)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Stream_id", stream.Id);
            cmd.Parameters.AddWithValue("@Channel_id", stream.ChannelId);
            cmd.Parameters.AddWithValue("@Cat_id", stream.CategoryId);
            cmd.Parameters.AddWithValue("@Title", stream.Title);
            cmd.Parameters.AddWithValue("@Num_current_viewers", stream.NumCurrentViewers);
            cmd.Parameters.AddWithValue("@Num_peak_viewers", stream.NumPeakViewers);
            cmd.Parameters.AddWithValue("@Duration_seconds", stream.Duration);
            if (stream.HasEnded)
                cmd.Parameters.AddWithValue("@Has_ended", 1);
            else cmd.Parameters.AddWithValue("@Has_ended", 0);
            cmd.Parameters.AddWithValue("@Start_date_time", stream.StartDateAndTime);

            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Stream submit failed. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }

            foreach (TextBox streamTagTxtBox in streamTagTxtBoxes)
            {
                if (streamTagTxtBox.Text.ToString().Trim() != "")
                {
                    if (!verifySGBDConnection())
                        return;
                    cmd = new SqlCommand("INSERT INTO Twitch.STREAM_TAG VALUES(@Stream_id, @Tag)", cn);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Stream_id", stream.Id.ToString());
                    cmd.Parameters.AddWithValue("@Tag", streamTagTxtBox.Text.ToString());

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Stream tag submit failed. \n ERROR MESSAGE: \n" + ex.Message);
                    }
                    finally
                    {
                        cn.Close();
                    }
                }
            }

        }

        private void UpdateStream(Stream stream)
        {
            int rows = 0;

            if (!verifySGBDConnection())
                return;

            /*
            CREATE TABLE Twitch.STREAM (
	            Stream_id INT NOT NULL,
	            Channel_id INT NOT NULL,
	            Cat_id INT NOT NULL,
	            Title VARCHAR(100),
	            Num_current_viewers INT NOT NULL,
	            Num_peak_viewers INT,
	            Has_ended BIT NOT NULL,
	            Start_date_time DATETIME NOT NULL,
	            Duration_seconds INT NOT NULL,
            )
            */

            SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = "UPDATE Customers " + "SET CompanyName = @CompanyName, " + "    ContactName = @ContactName, " + "    Address = @Address, " + "    City = @City, " + "    Region = @Region, " + "    PostalCode = @PostalCode, " + "    Country = @Country " + "WHERE CustomerID = @CustomerID";
            cmd.CommandText = "UPDATE Twitch.STREAM SET Cat_id = @Cat_id, Title = @Title, Num_current_viewers = @Num_current_viewers, Num_peak_viewers = @Num_peak_viewers, Has_ended = @Has_ended, Start_date_time = @Start_date_time, Duration_seconds = @Duration_seconds WHERE Stream_id=@Stream_id";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Stream_id", stream.Id);
            cmd.Parameters.AddWithValue("@Cat_id", stream.CategoryId);
            cmd.Parameters.AddWithValue("@Title", stream.Title);
            cmd.Parameters.AddWithValue("@Num_current_viewers", stream.NumCurrentViewers);
            cmd.Parameters.AddWithValue("@Num_peak_viewers", stream.NumPeakViewers);
            cmd.Parameters.AddWithValue("@Duration_seconds", stream.Duration);
            if (stream.HasEnded)
                cmd.Parameters.AddWithValue("@Has_ended", 1);
            else cmd.Parameters.AddWithValue("@Has_ended", 0);
            cmd.Parameters.AddWithValue("@Start_date_time", stream.StartDateAndTime);

            cmd.Connection = cn;

            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Stream update failed. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                if (rows == 1)
                    MessageBox.Show("Update sream OK");
                else
                    MessageBox.Show("Update stream NOT OK");

                cn.Close();
            }

            if (!verifySGBDConnection())
                return;
            cmd = new SqlCommand("DELETE Twitch.STREAM_TAG WHERE Stream_id = @Stream_id", cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Stream_id", stream.Id.ToString());

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Stream tag delete failed. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }

            foreach (TextBox streamTagTxtBox in streamTagTxtBoxes)
            {
                if (streamTagTxtBox.Text.ToString().Trim() != "")
                {
                    if (!verifySGBDConnection())
                        return;
                    cmd = new SqlCommand("INSERT INTO Twitch.STREAM_TAG VALUES(@Stream_id, @Tag)", cn);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Stream_id", stream.Id.ToString());
                    cmd.Parameters.AddWithValue("@Tag", streamTagTxtBox.Text.ToString());

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Stream tag insert (while updating stream) failed. \n ERROR MESSAGE: \n" + ex.Message);
                    }
                    finally
                    {
                        cn.Close();
                    }
                }
            }

        }


        private void RemoveStream(int streamToRemoveId)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXEC Twitch.RemoveStream " + streamToRemoveId.ToString();
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Stream delete failed. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            loadStreamsToolStripMenuItem_Click(null, null);
        }

        private void SubmitStreamClip()
        {
            if (!verifySGBDConnection())
                return;

            /*
            CREATE TABLE Twitch.CLIP (
	            Clip_id INT NOT NULL,
	            Stream_id INT NOT NULL,
	            Creator_channel_id INT NOT NULL,
	            Title VARCHAR(100) NOT NULL,
	            Start_time TIME NOT NULL,
	            Duration_seconds INT NOT NULL,
	            Num_views INT NOT NULL,
            )
            */

            SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = "INSERT Customers (CustomerID, CompanyName, ContactName, Address, " + "City, Region, PostalCode, Country) " + "VALUES (@CustomerID, @CompanyName, @ContactName, @Address, " + "@City, @Region, @PostalCode, @Country) ";
            cmd.CommandText = "INSERT INTO Twitch.CLIP VALUES (@Clip_id, @Stream_id, @Creator_channel_id, @Title, @Start_time, @Duration_seconds, @Num_views)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Clip_id", clipIdTxtBox.Text);
            Stream stream = (Stream)listBox1.Items[listBox1.SelectedIndex];
            cmd.Parameters.AddWithValue("@Stream_id", stream.Id.ToString());
            cmd.Parameters.AddWithValue("@Creator_channel_id", GetChannelId(clipCreatorChannelNameTxtBox.Text.ToString()));
            cmd.Parameters.AddWithValue("@Title", clipTitleTxtBox.Text);
            cmd.Parameters.AddWithValue("@Start_time", clipStartTimeTxtBox.Text);
            cmd.Parameters.AddWithValue("@Duration_seconds", clipDurationTxtBox.Text);
            cmd.Parameters.AddWithValue("@Num_views", clipNumViewsTxtBox.Text);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Stream clip submit failed. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }

        }

        private bool UpdateStreamClip()
        {
            if (clips.Count == 0)
                return false;
            int rows = 0;

            if (!verifySGBDConnection())
                return false;

            /*
            CREATE TABLE Twitch.CLIP (
	            Clip_id INT NOT NULL,
	            Stream_id INT NOT NULL,
	            Creator_channel_id INT NOT NULL,
	            Title VARCHAR(100) NOT NULL,
	            Start_time TIME NOT NULL,
	            Duration_seconds INT NOT NULL,
	            Num_views INT NOT NULL,
            )
            */

            SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = "UPDATE Customers " + "SET CompanyName = @CompanyName, " + "    ContactName = @ContactName, " + "    Address = @Address, " + "    City = @City, " + "    Region = @Region, " + "    PostalCode = @PostalCode, " + "    Country = @Country " + "WHERE CustomerID = @CustomerID";
            cmd.CommandText = "UPDATE Twitch.CLIP SET Title=@Title, Start_time=@Start_time, Duration_seconds = @Duration_seconds WHERE Clip_id=@Clip_id";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Clip_id", clipIdTxtBox.Text);
            cmd.Parameters.AddWithValue("@Title", clipTitleTxtBox.Text);
            cmd.Parameters.AddWithValue("@Start_time", clipStartTimeTxtBox.Text);
            cmd.Parameters.AddWithValue("@Duration_seconds", clipDurationTxtBox.Text);
            cmd.Connection = cn;

            bool ret = true;
            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Stream clip update failed. \n ERROR MESSAGE: \n" + ex.Message);
                ret = false;
            }
            finally
            {
                if (rows == 1)
                    MessageBox.Show("Update sream clip OK");
                else
                {
                    MessageBox.Show("Update stream clip NOT OK");
                    ret = false;
                }

                cn.Close();
            }

            return ret;
        }

        private void RemoveStreamClip()
        {
            if (clips.Count == 0)
                return;
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "DELETE Twitch.CLIP WHERE Clip_id = @Clip_id";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Clip_id", clipIdTxtBox.Text);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Stream clip delete failed. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }
        #endregion

        #region Form control
        public void ClearFields()
        {
            Panel panelInFront = null;
            switch (listSelected)
            {
                case 0:
                    panelInFront = channelPanel;
                    break;
                case 1:
                    panelInFront = categoryPanel;
                    break;
                case 2:
                    panelInFront = streamPanel;
                    break;
            }

            IEnumerable<TextBox> textBoxes = panelInFront.Controls.OfType<TextBox>();
            foreach (TextBox txtBox in textBoxes)
                txtBox.Text = "";
        }

        public void ClearFieldsStreamClip()
        {
            IEnumerable<TextBox> textBoxes = streamClipPanel.Controls.OfType<TextBox>();
            foreach (TextBox txtBox in textBoxes)
                txtBox.Text = "";
        }
        public void LockControls()
        {
            Panel panelInFront = null;
            switch (listSelected)
            {
                case 0:
                    panelInFront = channelPanel;
                    break;
                case 1:
                    panelInFront = categoryPanel;
                    break;
                case 2:
                    panelInFront = streamPanel;
                    break;
            }

            IEnumerable<TextBox> textBoxes = panelInFront.Controls.OfType<TextBox>();
            foreach (TextBox txtBox in textBoxes)
                txtBox.ReadOnly = true;

            streamEndDatetimeLabel.Visible = true;
            streamEndDatetimeTxtBox.Visible = true;
        }

        public void UnlockControls()
        {
            Panel panelInFront = null;
            switch (listSelected)
            {
                case 0:
                    panelInFront = channelPanel;
                    break;
                case 1:
                    panelInFront = categoryPanel;
                    break;
                case 2:
                    panelInFront = streamPanel;
                    break;
            }

            IEnumerable<TextBox> textBoxes = panelInFront.Controls.OfType<TextBox>();
            foreach (TextBox txtBox in textBoxes)
                txtBox.ReadOnly = false;

            if (!adding) // if editing
            {
                channelIdTxtBox.ReadOnly = true;
                catIdTxtBox.ReadOnly = true;
                streamIdTxtBox.ReadOnly = true;
                streamerChannelNameTxtBox.ReadOnly = true;
                streamEndDatetimeTxtBox.ReadOnly = true;
                streamEndDatetimeLabel.Visible = true;
                streamEndDatetimeTxtBox.Visible = true;

            }
            else
            {
                // adding
                streamEndDatetimeLabel.Visible = false;
                streamEndDatetimeTxtBox.Visible = false;
            }
        }

        public void ShowButtons()
        {
            LockControls();
            bttnAdd.Visible = true;
            bttnDelete.Visible = true;
            bttnEdit.Visible = true;
            bttnOK.Visible = false;
            bttnCancel.Visible = false;
            bttnAdd.BringToFront();
            bttnDelete.BringToFront();
            bttnEdit.BringToFront();
        }

        public void HideButtons()
        {
            UnlockControls();
            bttnAdd.Visible = false;
            bttnDelete.Visible = false;
            bttnEdit.Visible = false;
            bttnOK.Visible = true;
            bttnCancel.Visible = true;
            bttnOK.BringToFront();
            bttnCancel.BringToFront();

        }

        public void HideButtonsStreamClip()
        {
            IEnumerable<TextBox> textBoxes = streamClipPanel.Controls.OfType<TextBox>();
            foreach (TextBox txtBox in textBoxes)
                txtBox.ReadOnly = false;

            if (!adding)
            {
                clipIdTxtBox.ReadOnly = true;
                clipCreatorChannelNameTxtBox.ReadOnly = true;
                clipEndTimeTxtBox.ReadOnly = true;
                clipEndTimeTxtBox.Visible = true;
                clipEndTimeLabel.Visible = true;
            }
            else
            {
                // adding
                clipEndTimeTxtBox.Visible = false;
                clipEndTimeLabel.Visible = false;
            }

            bttnAddStreamClip.Visible = false;
            bttnDeleteStreamClip.Visible = false;
            bttnEditStreamClip.Visible = false;
            bttnPreviousClip.Visible = false;
            bttnNextClip.Visible = false;
            bttnOKStreamClip.Visible = true;
            bttnCancelStreamClip.Visible = true;
        }

        public void ShowButtonsStreamClip()
        {
            IEnumerable<TextBox> textBoxes = streamClipPanel.Controls.OfType<TextBox>();
            foreach (TextBox txtBox in textBoxes)
                txtBox.ReadOnly = true;

            bttnAddStreamClip.Visible = true;
            bttnDeleteStreamClip.Visible = true;
            bttnEditStreamClip.Visible = true;
            bttnPreviousClip.Visible = true;
            bttnNextClip.Visible = true;
            bttnOKStreamClip.Visible = false;
            bttnCancelStreamClip.Visible = false;
        }

        public void HideAllButtons()
        {
            bttnAdd.Visible = false;
            bttnDelete.Visible = false;
            bttnEdit.Visible = false;
            bttnOK.Visible = false;
            bttnCancel.Visible = false;
        }

        #endregion

        #region Show selected list element functions

        public void ShowChannel(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0 || currentListBoxSelectIndex < 0)
                return;

            Channel channel = new Channel();
            channel = (Channel)listBox1.Items[currentListBoxSelectIndex];
            channelIdTxtBox.Text = channel.Id.ToString();
            channelNameTxtBox.Text = channel.Name;
            channelDateCreatedTxtBox.Text = channel.DateCreated;
            channelFollowersTxtBox.Text = channel.NumFollowers.ToString();
            channelSubsTxtBox.Text = channel.NumSubs.ToString();
            channelAvgViewersTxtBox.Text = channel.AvgViewers.ToString();
            channelPeakViewersTxtBox.Text = channel.PeakViewers.ToString();
            channelDaysActiveTxtBox.Text = channel.DaysActive.ToString();
            channelHoursStreamedTxtBox.Text = String.Format("{0:F0}", channel.HoursStreamed);
            channelHoursWatchedTxtBox.Text = String.Format("{0:F0}", channel.HoursWatched);

            channelPanel.BringToFront();
            ShowButtons();

        }

        public void ShowFollowersAndFollowing(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0 || currentListBoxSelectIndex < 0)
                return;

            if (!verifySGBDConnection())
                return;

            Channel channel = new Channel();
            channel = (Channel)listBox1.Items[currentListBoxSelectIndex];

            SqlCommand cmd = new SqlCommand("SELECT * FROM Twitch.GetFollowers(" + channel.Id.ToString() + ")", cn);
            // returns Table(Follower_channel_id, Channel_name)
            SqlDataReader reader = cmd.ExecuteReader();

            followersListLabel.Text = "";
            while (reader.Read())
            {
                String followerChannelName = reader["Channel_name"].ToString();
                followersListLabel.Text += followerChannelName + "\n";

            }
            reader.Close();


            cmd = new SqlCommand("SELECT * FROM Twitch.GetFollowing(" + channel.Id.ToString() + ")", cn);
            // returns Table(Followed_channel_id, Channel_name)
            reader = cmd.ExecuteReader();

            followingListLabel.Text = "";
            while (reader.Read())
            {
                String followingChannelName = reader["Channel_name"].ToString();
                followingListLabel.Text += followingChannelName + "\n";

            }
            reader.Close();

            cn.Close();

            followersAndFollowingPanel.BringToFront();

        }

        public void ShowSubs(object sender, EventArgs e)
        {
            if (!verifySGBDConnection())
                return;

            Channel channel = new Channel();
            channel = (Channel)listBox1.Items[currentListBoxSelectIndex];

            SqlCommand cmd = new SqlCommand("SELECT * FROM Twitch.GetSubsTo(" + channel.Id.ToString() + ")", cn);
            // returns Table(To_channel_id, Channel_name)
            SqlDataReader reader = cmd.ExecuteReader();

            subbedToListLabel.Text = "";
            while (reader.Read())
            {
                String subbedToName = reader["Channel_name"].ToString();
                subbedToListLabel.Text += subbedToName + "\n";

            }
            reader.Close();

            cmd = new SqlCommand("SELECT * FROM Twitch.GetSubsFrom(" + channel.Id.ToString() + ")", cn);
            // returns Table(From_channel_id, Channel_name)
            reader = cmd.ExecuteReader();

            subbedFromListLabel.Text = "";
            while (reader.Read())
            {
                String subbedFromName = reader["Channel_name"].ToString();
                subbedFromListLabel.Text += subbedFromName + "\n";

            }
            reader.Close();

            subsPanel.BringToFront();

            cn.Close();

        }

        public void ShowCategory()
        {
            if (listBox1.Items.Count == 0 | currentListBoxSelectIndex < 0)
                return;

            Category category = new Category();
            category = (Category)listBox1.Items[currentListBoxSelectIndex];
            catIdTxtBox.Text = category.Id.ToString();
            catNameTxtBox.Text = category.Name;
            catRankTxtBox.Text = category.Rank.ToString();
            catLiveViewersTxtBox.Text = category.NumLiveViewers.ToString();
            catLiveChannelsTxtBox.Text = category.NumLiveChannels.ToString();
            catAvgViewers7dTxtBox.Text = category.AvgViewers7Days.ToString();
            catAvgChannels7dTxtBox.Text = category.AvgChannels7Days.ToString();

        }

        public void ShowStream(object sender, EventArgs e)
        {
            ClearFields();

            if (listBox1.Items.Count == 0 | currentListBoxSelectIndex < 0)
                return;

            Stream stream = new Stream();
            stream = (Stream)listBox1.Items[currentListBoxSelectIndex];

            streamIdTxtBox.Text = stream.Id.ToString();
            streamerChannelNameTxtBox.Text = GetChannelName(stream.ChannelId);
            streamTitleTxtBox.Text = stream.Title;
            streamCatNameTxtBox.Text = GetCategoryName(stream.CategoryId);
            streamCurrentViewersTxtBox.Text = stream.NumCurrentViewers.ToString();
            streamPeakViewersTxtBox.Text = stream.NumCurrentViewers.ToString();

            // stream.StartDateAndTime returns "09/07/2013 18:01:00"
            // sql datetime is 'YYYYMMDD HH:MM:SS PM'
            // form should show sql datetime
            // UPDATE: stream.StartDateAndTime should return sql datetime now
            streamStartDatetimeTxtBox.Text = stream.StartDateAndTime;
            streamDurationTxtBox.Text = stream.Duration.ToString();
            if (stream.HasEnded)
            {
                streamHasEndedTxtBox.Text = "True";
                if (!verifySGBDConnection())
                    return;
                SqlCommand cmd2 = new SqlCommand("SELECT * FROM Twitch.DATETIME_INTERVAL WHERE Start_date_time=@Start_date_time AND Duration_seconds=@Duration_seconds", cn);
                cmd2.Parameters.Clear();
                // Twitch.DATETIME:INTERVAL.Start_datetime: '2019-02-22 13:54:00.000'
                string param = stream.StartDateAndTime;
                if (stream.StartDateAndTime.Substring(18, 1) == "P") // if PM
                {
                    string hours = stream.StartDateAndTime.Substring(9, 2);
                    string newHours = (Int32.Parse(hours) + 12).ToString();
                    if (newHours.Length == 1)
                        newHours = "0" + newHours;

                    param = param.Substring(0, 4) + "-" + param.Substring(4, 2) + "-" + param.Substring(6, 2) + " " + newHours + param.Substring(11, 6) + ".000";
                }
                else param = param.Substring(0, 4) + "-" + param.Substring(4, 2) + "-" + param.Substring(6, 2) + param.Substring(8, 9) + ".000";
                Debug.WriteLine("param: " + param);
                cmd2.Parameters.AddWithValue("@Start_date_time", param);
                cmd2.Parameters.AddWithValue("@Duration_seconds", stream.Duration.ToString());
                SqlDataReader reader2 = cmd2.ExecuteReader();
                reader2.Read();
                streamEndDatetimeTxtBox.Text = ReaderDatetimeToSqlDatetime(reader2["End_date_time"].ToString());
                reader2.Close();

            }
            else streamHasEndedTxtBox.Text = "False";

            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand("SELECT * FROM Twitch.STREAM_TAG WHERE Stream_id=" + stream.Id.ToString(), cn);
            SqlDataReader reader = cmd.ExecuteReader();
            foreach (TextBox streamTagTxtBox in streamTagTxtBoxes)
            {
                if (reader.Read())
                    streamTagTxtBox.Text = reader["Tag"].ToString();
                else break;
            }
            reader.Close();

            streamPanel.BringToFront();
            LockControls();
            ShowButtons();

        }

        private List<Clip> clips = new List<Clip>();
        private int currentClipIndex = 0;
        public void LoadStreamClips(object sender, EventArgs e)
        {
            clips.Clear();
            currentClipIndex = 0;

            if (listBox1.Items.Count == 0 || currentListBoxSelectIndex < 0)
                return;

            if (!verifySGBDConnection())
                return;

            Stream stream = (Stream)listBox1.Items[currentListBoxSelectIndex];
            SqlCommand cmd = new SqlCommand("SELECT * FROM Twitch.CLIP WHERE Stream_id=" + stream.Id.ToString(), cn);
            SqlDataReader reader = cmd.ExecuteReader();

            /*
            CREATE TABLE Twitch.CLIP (
	            Clip_id INT NOT NULL,
	            Stream_id INT NOT NULL,
	            Creator_channel_id INT NOT NULL,
	            Title VARCHAR(100) NOT NULL,
	            Start_time TIME NOT NULL,
	            Duration_seconds INT NOT NULL,
	            Num_Views INT NOT NULL,
            )
             */

            while (reader.Read())
            {
                Clip clip = new Clip();
                clip.Id = Int32.Parse(reader["Clip_id"].ToString());
                clip.StreamId = Int32.Parse(reader["Stream_id"].ToString());
                clip.CreatorChannelId = Int32.Parse(reader["Creator_channel_id"].ToString());
                clip.Title = reader["Title"].ToString();
                clip.NumViews = Int32.Parse(reader["Num_Views"].ToString());
                clip.StartTime = reader["Start_time"].ToString();
                clip.Duration = Int32.Parse(reader["Duration_seconds"].ToString());
                clips.Add(clip);
            }

            cn.Close();

            clipNumberLabel.Text = "Clip 0/0";
            ClearFieldsStreamClip();
            HideAllButtons();
            ShowButtonsStreamClip();
            streamClipPanel.BringToFront();

            currentClipIndex = 0;
            if (clips.Count > 0)
                ShowStreamClip(null, null);
        }

        public void ShowStreamClip(object sender, EventArgs e)
        {
            ClearFieldsStreamClip();

            if (listBox1.Items.Count == 0 | currentListBoxSelectIndex < 0)
                return;

            if (clips.Count == 0)
            {
                clipNumberLabel.Text = "Clip 0/0";
                ClearFieldsStreamClip();
            }
            else
            {
                if (!verifySGBDConnection())
                    return;
                Clip clip = clips[currentClipIndex];

                SqlCommand cmd = new SqlCommand("SELECT * FROM Twitch.TIME_INTERVAL WHERE Start_time=@Start_time AND Duration_seconds=@Duration_seconds", cn);
                cmd.Parameters.Clear();
                // Twitch.TIME_INTERVAL.Start_time: 00:59:41.0000000
                // clip.StartTime: 00:59:41
                cmd.Parameters.AddWithValue("@Start_time", clip.StartTime + ".0000000");
                cmd.Parameters.AddWithValue("@Duration_seconds", clip.Duration.ToString());
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                clipEndTimeTxtBox.Text = reader["End_time"].ToString();
                reader.Close();

                clipNumberLabel.Text = "Clip " + (currentClipIndex + 1).ToString() + "/" + clips.Count;
                clipIdTxtBox.Text = clip.Id.ToString();
                clipCreatorChannelNameTxtBox.Text = GetChannelName(clip.CreatorChannelId);
                clipTitleTxtBox.Text = clip.Title;
                clipNumViewsTxtBox.Text = clip.NumViews.ToString();
                clipStartTimeTxtBox.Text = clip.StartTime;
                clipDurationTxtBox.Text = clip.Duration.ToString();
            }

            clipEndTimeLabel.Visible = true;
            clipEndTimeTxtBox.Visible = true;

        }

        #endregion

        #region Save functions
        private bool SaveChannel()
        {
            Channel channel = new Channel();
            try
            {
                channel.Id = Int32.Parse(channelIdTxtBox.Text);
                channel.Name = channelNameTxtBox.Text;
                channel.DateCreated = channelDateCreatedTxtBox.Text;
                channel.NumFollowers = Int32.Parse(channelFollowersTxtBox.Text);
                channel.NumSubs = Int32.Parse(channelSubsTxtBox.Text);
                channel.AvgViewers = float.Parse(channelAvgViewersTxtBox.Text);
                channel.PeakViewers = Int32.Parse(channelPeakViewersTxtBox.Text);
                channel.DaysActive = Int32.Parse(channelDaysActiveTxtBox.Text);
                channel.HoursStreamed = Int32.Parse(channelHoursStreamedTxtBox.Text);
                channel.HoursWatched = Int32.Parse(channelHoursWatchedTxtBox.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            if (adding)
            {
                SubmitChannel(channel);
                listBox1.Items.Add(channel);
            }
            else
            {
                UpdateChannel(channel);
                listBox1.Items[currentListBoxSelectIndex] = channel;
            }
            return true;

        }

        private bool SaveCategory()
        {
            Category cat = new Category();
            try
            {
                cat.Id = Int32.Parse(catIdTxtBox.Text.ToString());
                cat.Name = catNameTxtBox.Text;
                cat.Rank = Int32.Parse(catRankTxtBox.Text);
                cat.NumLiveViewers = Int32.Parse(catLiveViewersTxtBox.Text);
                cat.NumLiveChannels = Int32.Parse(catLiveChannelsTxtBox.Text);
                cat.AvgViewers7Days = float.Parse(catAvgViewers7dTxtBox.Text);
                cat.AvgChannels7Days = float.Parse(catAvgChannels7dTxtBox.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            if (adding)
            {
                SubmitCategory(cat);
                listBox1.Items.Add(cat);
            }
            else
            {
                UpdateCategory(cat);
                listBox1.Items[currentListBoxSelectIndex] = cat;
            }
            return true;

        }

        private bool SaveStream()
        {
            Stream stream = new Stream();
            try
            {
                stream.Id = Int32.Parse(streamIdTxtBox.Text);
                stream.ChannelId = GetChannelId(streamerChannelNameTxtBox.Text.ToString());
                stream.Title = streamTitleTxtBox.Text;
                stream.CategoryId = GetCategoryId(streamCatNameTxtBox.Text.ToString());
                stream.NumCurrentViewers = Int32.Parse(streamCurrentViewersTxtBox.Text);
                stream.NumPeakViewers = Int32.Parse(streamPeakViewersTxtBox.Text);
                stream.Duration = Int32.Parse(streamDurationTxtBox.Text);
                if (streamHasEndedTxtBox.Text.Trim() == "True" || streamHasEndedTxtBox.Text.Trim() == "true" || streamHasEndedTxtBox.Text.Trim() == "1")
                    stream.HasEnded = true;
                else stream.HasEnded = false;
                stream.StartDateAndTime = streamStartDatetimeTxtBox.Text;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            if (adding)
            {
                SubmitStream(stream);
                listBox1.Items.Add(stream);
            }
            else
            {
                UpdateStream(stream);
                listBox1.Items[currentListBoxSelectIndex] = stream;
            }
            return true;

        }
        #endregion

        #region On buttons click
        private void bttnAdd_Click(object sender, EventArgs e)
        {
            adding = true;
            ClearFields();
            HideButtons();
            listBox1.Enabled = false;
        }

        private void bttnCancel_Click(object sender, EventArgs e)
        {
            listBox1.Enabled = true;
            if (listBox1.Items.Count > 0)
            {
                currentListBoxSelectIndex = listBox1.SelectedIndex;
                if (currentListBoxSelectIndex < 0)
                    currentListBoxSelectIndex = 0;
                switch (listSelected)
                {
                    case 0:
                        ShowChannel(null, null);
                        break;
                    case 1:
                        ShowCategory();
                        break;
                    case 2:
                        ShowStream(null, null);
                        break;
                }
            }
            else
            {
                ClearFields();
                LockControls();
            }
            ShowButtons();
        }

        private void bttnEdit_Click(object sender, EventArgs e)
        {
            currentListBoxSelectIndex = listBox1.SelectedIndex;
            if (currentListBoxSelectIndex < 0)
            {
                MessageBox.Show("Please select an element to edit");
                return;
            }
            adding = false;
            listBox1.Enabled = false;
            HideButtons();
            UnlockControls();
        }

        private void bttnOK_Click(object sender, EventArgs e)
        {
            try
            {
                switch (listSelected)
                {
                    case 0:
                        SaveChannel();
                        break;
                    case 1:
                        SaveCategory();
                        break;
                    case 2:
                        SaveStream();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            listBox1.Enabled = true;

            int idx = 0;
            switch (listSelected)
            {
                case 0:
                    idx = listBox1.FindString(channelNameTxtBox.Text); // Ch name
                    break;
                case 1:
                    idx = listBox1.FindString(catNameTxtBox.Text); // Cat name
                    break;
                case 2:
                    idx = listBox1.FindString(streamTitleTxtBox.Text); // Stream title
                    break;
            }
            //int idx = listBox1.FindString(txt2.Text);
            listBox1.SelectedIndex = idx;
            ShowButtons();

        }

        private void bttnDelete_Click(object sender, EventArgs e)
        {

            if (listBox1.SelectedIndex > -1)
            {
                try
                {
                    //RemoveContact(((Contact)listBox1.SelectedItem).CustomerID);
                    switch (listSelected)
                    {
                        case 0:
                            RemoveChannel(((Channel)listBox1.SelectedItem).Id);
                            break;
                        case 1:
                            RemoveCategory(((Category)listBox1.SelectedItem).Id);
                            break;
                        case 2:
                            RemoveStream(((Stream)listBox1.SelectedItem).Id);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                //listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                //if (currentListBoxSelectIndex == listBox1.Items.Count)
                //    currentListBoxSelectIndex = listBox1.Items.Count - 1;
                if (currentListBoxSelectIndex == -1)
                {
                    ClearFields();
                    MessageBox.Show("There are no more elements");
                }
                else
                {
                    listBox1.SelectedIndex = 0;
                    currentListBoxSelectIndex = 0;
                    switch (listSelected)
                    {
                        case 0:
                            ShowChannel(null, null);
                            break;
                        case 1:
                            ShowCategory();
                            break;
                        case 2:
                            ShowStream(null, null);
                            break;
                    }
                }
            }

        }
        private void bttnAddStreamClip_Click(object sender, EventArgs e)
        {
            listBox1.Enabled = false;
            adding = true;
            ClearFieldsStreamClip();
            HideButtonsStreamClip();
            clipEndTimeTxtBox.Visible = false;
        }
        private void bttnEditStreamClip_Click(object sender, EventArgs e)
        {
            if (clips.Count == 0)
                return;

            currentListBoxSelectIndex = listBox1.SelectedIndex;
            if (currentListBoxSelectIndex < 0)
            {
                MessageBox.Show("Please select an element to edit");
                return;
            }
            adding = false;
            listBox1.Enabled = false;
            HideAllButtons();
            HideButtonsStreamClip();
            clipEndTimeTxtBox.Visible = true;
            clipEndTimeTxtBox.ReadOnly = true;
            clipEndTimeLabel.Visible = true;

        }
        private void bttnDeleteStreamClip_Click(object sender, EventArgs e)
        {
            if (clips.Count == 0)
                return;

            if (listBox1.SelectedIndex > -1)
            {
                try
                {
                    RemoveStreamClip();
                    LoadStreamClips(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }

        }
        private void bttnOKStreamClip_Click(object sender, EventArgs e)
        {
            if (adding)
            {
                SubmitStreamClip();
                LoadStreamClips(null, null);

            }
            else
            {
                bool updated = UpdateStreamClip();
                if (updated)
                {
                    clips[currentClipIndex].Title = clipTitleTxtBox.Text;
                    clips[currentClipIndex].NumViews = Int32.Parse(clipNumViewsTxtBox.Text);
                    clips[currentClipIndex].StartTime = clipStartTimeTxtBox.Text;
                    clips[currentClipIndex].Duration = Int32.Parse(clipDurationTxtBox.Text);
                }

            }
            clipEndTimeTxtBox.Visible = true;
            clipEndTimeLabel.Visible = true;
            listBox1.Enabled = true;
            ShowButtonsStreamClip();

        }
        private void bttnCancelStreamClip_Click(object sender, EventArgs e)
        {
            listBox1.Enabled = true;
            ShowStreamClip(null, null);
            LockControls();
            clipEndTimeTxtBox.Visible = true;
            clipEndTimeLabel.Visible = true;
            ShowButtonsStreamClip();
        }
        private void bttnPreviousClip_Click(object sender, EventArgs e)
        {
            if (clips.Count == 0)
                return;
            currentClipIndex--;
            if (currentClipIndex < 0)
                currentClipIndex = clips.Count - 1;
            ShowStreamClip(null, null);
        }
        private void bttnNextClip_Click(object sender, EventArgs e)
        {
            if (clips.Count == 0)
                return;
            currentClipIndex++;
            currentClipIndex %= clips.Count;
            ShowStreamClip(null, null);
        }
        #endregion

        /*
        // SQL DATE: 'MM-DD-YYYY'
        CREATE TABLE Twitch.FOLLOWS (
        Date_followed DATE NOT NULL,
        Follower_channel_id INT NOT NULL,
        Followed_channel_id INT NOT NULL,
        )
        */

        #region Buttons add and remove followers and subs

        private void bttnAddFollower_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0 || listBox1.SelectedIndex < 0)
                return;

            if (!verifySGBDConnection())
                return;

            int followerChannelId = GetChannelId(followerNameTxtBox.Text.ToString());

            SqlCommand cmd = new SqlCommand("INSERT INTO Twitch.FOLLOWS VALUES(@Date_followed, @Follower_channel_id, @Followed_channel_id)", cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Date_followed", "06-23-2022"); // today
            cmd.Parameters.AddWithValue("@Follower_channel_id", followerChannelId.ToString());
            Channel channel = (Channel)listBox1.Items[listBox1.SelectedIndex];
            cmd.Parameters.AddWithValue("@Followed_channel_id", channel.Id.ToString());

            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Follower add failed. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            ShowFollowersAndFollowing(null, null);

        }

        private void bttnRemoveFollower_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0 || listBox1.SelectedIndex < 0)
                return;

            if (!verifySGBDConnection())
                return;

            int followerChannelId = GetChannelId(followerNameTxtBox.Text.ToString());

            SqlCommand cmd = new SqlCommand("DELETE Twitch.FOLLOWS WHERE Follower_channel_id=@Follower_channel_id AND Followed_channel_id=@Followed_channel_id", cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Date_followed", "06-23-2022"); // today
            cmd.Parameters.AddWithValue("@Follower_channel_id", followerChannelId.ToString());
            Channel channel = (Channel)listBox1.Items[listBox1.SelectedIndex];
            cmd.Parameters.AddWithValue("@Followed_channel_id", channel.Id.ToString());

            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Follower remove failed. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            ShowFollowersAndFollowing(null, null);

        }

        private void bttnAddFollowing_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0 || listBox1.SelectedIndex < 0)
                return;

            if (!verifySGBDConnection())
                return;

            int followingChannelId = GetChannelId(followingNameTxtBox.Text.ToString());

            SqlCommand cmd = new SqlCommand("INSERT INTO Twitch.FOLLOWS VALUES(@Date_followed, @Follower_channel_id, @Followed_channel_id)", cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Date_followed", "06-23-2022"); // today
            cmd.Parameters.AddWithValue("@Followed_channel_id", followingChannelId.ToString());
            Channel channel = (Channel)listBox1.Items[listBox1.SelectedIndex];
            cmd.Parameters.AddWithValue("@Follower_channel_id", channel.Id.ToString());

            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Following add failed. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            ShowFollowersAndFollowing(null, null);

        }

        private void bttnRemoveFollowing_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0 || listBox1.SelectedIndex < 0)
                return;

            if (!verifySGBDConnection())
                return;

            int followingChannelId = GetChannelId(followingNameTxtBox.Text.ToString());

            SqlCommand cmd = new SqlCommand("DELETE Twitch.FOLLOWS WHERE Follower_channel_id=@Follower_channel_id AND Followed_channel_id=@Followed_channel_id", cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Date_followed", "06-23-2022"); // today
            cmd.Parameters.AddWithValue("@Followed_channel_id", followingChannelId.ToString());
            Channel channel = (Channel)listBox1.Items[listBox1.SelectedIndex];
            cmd.Parameters.AddWithValue("@Follower_channel_id", channel.Id.ToString());

            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Following remove failed. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }

            ShowFollowersAndFollowing(null, null);
        }

        private void bttnAddSubTo_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0 || listBox1.SelectedIndex < 0)
                return;

            if (!verifySGBDConnection())
                return;

            int channelThatSubbedId = GetChannelId(subToNameTxtBox.Text.ToString());

            SqlCommand cmd = new SqlCommand("INSERT INTO Twitch.SUBSCRIPTION VALUES(@From_channel_id, @To_channel_id, @Is_Amazon_prime, @Duration_months)", cn);
            cmd.Parameters.Clear();
            Channel channel = (Channel)listBox1.Items[listBox1.SelectedIndex];
            cmd.Parameters.AddWithValue("@To_channel_id", channel.Id.ToString());
            cmd.Parameters.AddWithValue("@From_channel_id", channelThatSubbedId.ToString());
            System.Random random = new System.Random();
            int zeroOrOne = random.Next(0, 2);
            int durationMonths = random.Next(1, 51);
            cmd.Parameters.AddWithValue("@Is_Amazon_prime", zeroOrOne.ToString());
            cmd.Parameters.AddWithValue("@Duration_months", durationMonths.ToString());

            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Adding sub to this channel failed. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            ShowSubs(null, null);
        }

        private void bttnRemoveSubTo_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0 || listBox1.SelectedIndex < 0)
                return;

            if (!verifySGBDConnection())
                return;

            int channelSubbedToRemoveId = GetChannelId(subToNameTxtBox.Text.ToString());

            SqlCommand cmd = new SqlCommand("DELETE Twitch.SUBSCRIPTION WHERE From_channel_id=@From_channel_id AND To_channel_id=@To_channel_id", cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@From_channel_id", channelSubbedToRemoveId.ToString());
            Channel channel = (Channel)listBox1.Items[listBox1.SelectedIndex];
            cmd.Parameters.AddWithValue("@To_channel_id", channel.Id.ToString());

            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed removing channel subbed to this channel. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            ShowSubs(null, null);
        }

        private void bttnAddSubFrom_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0 || listBox1.SelectedIndex < 0)
                return;

            if (!verifySGBDConnection())
                return;

            int channelToSubToId = GetChannelId(subFromNameTxtBox.Text.ToString());

            SqlCommand cmd = new SqlCommand("INSERT INTO Twitch.SUBSCRIPTION VALUES(@From_channel_id, @To_channel_id, @Is_Amazon_prime, @Duration_months)", cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@To_channel_id", channelToSubToId.ToString());
            Channel channel = (Channel)listBox1.Items[listBox1.SelectedIndex];
            cmd.Parameters.AddWithValue("@From_channel_id", channel.Id.ToString());
            System.Random random = new System.Random();
            int zeroOrOne = random.Next(0, 2);
            int durationMonths = random.Next(1, 51);
            cmd.Parameters.AddWithValue("@Is_Amazon_prime", zeroOrOne.ToString());
            cmd.Parameters.AddWithValue("@Duration_months", durationMonths.ToString());

            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Adding sub from this channel failed. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            ShowSubs(null, null);
        }

        private void bttnRemoveSubFrom_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0 || listBox1.SelectedIndex < 0)
                return;

            if (!verifySGBDConnection())
                return;

            int channelIAmSubbedToToRemoveId = GetChannelId(subFromNameTxtBox.Text.ToString());

            SqlCommand cmd = new SqlCommand("DELETE Twitch.SUBSCRIPTION WHERE From_channel_id=@From_channel_id AND To_channel_id=@To_channel_id", cn);
            cmd.Parameters.Clear();
            Channel channel = (Channel)listBox1.Items[listBox1.SelectedIndex];
            cmd.Parameters.AddWithValue("@From_channel_id", channel.Id.ToString());
            cmd.Parameters.AddWithValue("@To_channel_id", channelIAmSubbedToToRemoveId);

            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed removing sub from this channel. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            ShowSubs(null, null);
        }

        #endregion

        public string GetChannelName(int channelId)
        {
            if (!verifySGBDConnection())
                return "";

            SqlCommand cmd = new SqlCommand("SELECT Channel_name FROM Twitch.CHANNEL WHERE Channel_id=@Channel_id", cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Channel_id", channelId);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            string channelName = reader["Channel_name"].ToString();
            reader.Close();
            return channelName;
        }

        public int GetChannelId(string channelName)
        {
            if (!verifySGBDConnection())
                return -1;

            SqlCommand cmd = new SqlCommand("SELECT Channel_id FROM Twitch.CHANNEL WHERE Channel_name=@Channel_name", cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Channel_name", channelName);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            int channelId = Int32.Parse(reader["Channel_id"].ToString());
            reader.Close();
            return channelId;
        }

        public string GetCategoryName(int categoryId)
        {
            if (!verifySGBDConnection())
                return "";

            SqlCommand cmd = new SqlCommand("SELECT Category_name FROM Twitch.CATEGORY WHERE Category_id=@Category_id", cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Category_id", categoryId);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            string categoryName = reader["Category_name"].ToString();
            reader.Close();
            return categoryName;
        }

        public int GetCategoryId(string categoryName)
        {
            if (!verifySGBDConnection())
                return -1;

            SqlCommand cmd = new SqlCommand("SELECT Category_id FROM Twitch.CATEGORY WHERE Category_name=@Category_name", cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Category_name", categoryName);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            int categoryId = Int32.Parse(reader["Category_id"].ToString());
            reader.Close();
            return categoryId;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #region Unused functions (do not delete)
        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtZIP_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripSeparator1_Click(object sender, EventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void label5_Click(object sender, EventArgs e)
        {

        }
        private void label6_Click(object sender, EventArgs e)
        {

        }
        private void label7_Click(object sender, EventArgs e)
        {

        }
        private void label8_Click(object sender, EventArgs e)
        {

        }
        private void label9_Click(object sender, EventArgs e)
        {

        }
        private void label10_Click(object sender, EventArgs e)
        {

        }
        private void label11_Click(object sender, EventArgs e)
        {

        }
        private void Label1_Click(object sender, EventArgs e)
        {

        }
        private void Label2_Click(object sender, EventArgs e)
        {

        }
        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }
        private void Label5_Click(object sender, EventArgs e)
        {

        }
        private void Label6_Click(object sender, EventArgs e)
        {

        }
        private void Label7_Click(object sender, EventArgs e)
        {

        }
        private void Label8_Click(object sender, EventArgs e)
        {

        }
        private void Label9_Click(object sender, EventArgs e)
        {

        }
        private void Label10_Click(object sender, EventArgs e)
        {

        }
        private void Label11_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click_1(object sender, EventArgs e)
        {

        }

        private void label9_Click_1(object sender, EventArgs e)
        {

        }

        private void txt9_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt10_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt11_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt12_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripSeparator2_Click(object sender, EventArgs e)
        {

        }

        private void panelToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label37_Click(object sender, EventArgs e)
        {

        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        private void label34_Click(object sender, EventArgs e)
        {

        }

        private void subsPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void streamClipPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void clipCreatorChannelNameTxtBox_TextChanged(object sender, EventArgs e)
        {

        }
        #endregion

    }
}
