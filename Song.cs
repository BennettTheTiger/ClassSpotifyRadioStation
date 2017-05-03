using System;
namespace radioSheetBased
{
	public class Song
	{
		//fields
		private string title;
		private string artist;
		private string album;
		private string trackLink;
		private string contributor;
		private string date;


		//properties
		public string Title
		{
			get { return title; }
		}

		public string Artist
		{
			get { return artist; }
		}

		public string Album
		{
			get { return album; }
		}

		public string TrackLink
		{
			get { return trackLink; }
		}

		public string Contributor
		{
			get { return contributor; }
		}

		public string Date
		{
			get { return date; }
		}


		//constructors;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:radioSheetBased.Song"/> class.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="artist">Artist.</param>
		/// <param name="album">Album.</param>
		/// <param name="trackLink">Track link.</param>
		/// <param name="contributor">Contributor.</param>
		/// <param name="date">Date.</param>
		public Song(object title, object artist,object album,object trackLink,object contributor, object date)
		{
			this.title = title.ToString();
			this.artist = artist.ToString();
			this.album = album.ToString();
			this.trackLink = trackLink.ToString();
			this.contributor = contributor.ToString();
			this.date = date.ToString();
		}




		/// <summary>
		/// Pritns who added a song
		/// </summary>
		public void WhoAdded()
		{
			Console.WriteLine("{0} added the song {1} on {2}", contributor,title,date);
		}

		public void basicData()
		{
			Console.WriteLine(title + " by " + artist);
		}

		public void AllData()
		{
			Console.WriteLine("{0} by {1} apeared on {2} . It was added by {3} on {4}. Here is a link to the song {5}",title,artist,album,contributor,date,trackLink);
		}

	}
}
