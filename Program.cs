using System;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace radioSheetBased
{
	class MainClass
	{
		//this is set to read only!
		static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
		static string ApplicationName = "radioStation";



		public static void Main(string[] args)
		{
			UserCredential credential;

			using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
			{
				string credPath = System.Environment.GetFolderPath(
					System.Environment.SpecialFolder.Personal);
				credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

				credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.Load(stream).Secrets,
					Scopes,
					"user",
					CancellationToken.None,
					new FileDataStore(credPath, true)).Result;
				//Console.WriteLine("Credential file saved to: " + credPath);
			}

			// Create Google Sheets API service.
			var service = new SheetsService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = ApplicationName,
			});

			//this part here is the 'get request'
			// Define request parameters.
			String spreadsheetId = "1LKG7f-wjjEfDwZWXKDl_KiSEV35PNLDHaQpsPd-D5Jg";
			String range = "A2:G";
			SpreadsheetsResource.ValuesResource.GetRequest request =
					service.Spreadsheets.Values.Get(spreadsheetId, range);

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Getting the data just a sec...");
			//boom make the query go get the data
			ValueRange response = request.Execute();
			IList<IList<Object>> songs = response.Values;

			List<Song> myMusic = new List<Song>();

			if (songs != null && songs.Count > 0)
			{

				foreach (var row in songs)
				{
					//row[3] is an image and I have to use for that data...
					myMusic.Add(new Song(row[0],row[1],row[2],row[4],row[5],row[6]));
				}
			}
			else
			{
				Console.WriteLine("Unknown.");
			}
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Ok you're ready to Rock!");


			//start the menu loop
			string userInput = "";
			while (userInput != "quit")
			{
				Console.ForegroundColor = ConsoleColor.White;
				Menu();
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.WriteLine("Type a selection");
				userInput = Console.ReadLine().ToLower();
				MenuLogic(userInput, myMusic);
			}
		}


		/// <summary>
		/// Display the Menu
		/// </summary>
		static public void Menu()
		{
			Console.WriteLine();
			Console.WriteLine("---------------------- MENU -----------------------");
			Console.WriteLine("See All       : Show all songs in the database");
			Console.WriteLine("Song Artist   : Search for the artist of a song");
			Console.WriteLine("Song Album    : Find the album a song is on");
			Console.WriteLine("Song Details  : Find all info about a song");
			Console.WriteLine("Artist Songs  : Find a list of songs by a artist");
			Console.WriteLine("Who Added     : See a who added a song");
			Console.WriteLine("Songs by user : Find a list of a songs someone added");
			Console.WriteLine("Quit          : Exit the program");
			Console.WriteLine("---------------------------------------------------");
		}

		/// <summary>
		/// handles menu logic
		/// </summary>
		/// <param name="input">Input from menu</param>
		static public void MenuLogic(string input, List<Song>myMusic)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			switch (input)
			{
				case "see all":
					ShowAllSongs(myMusic);
					break;

				case "song artist":
					FindArtist(myMusic);
					break;

				case "song album":
					FindAlbum(myMusic);
					break;

				case "song details":
					SongDetail(myMusic);
					break;

				case "artist songs":
					ArtistSongs(myMusic);
					break;

				case "who added":
					WhoDoneIt(myMusic);
					break;

					case "songs by user":
					WhatDidTheyAdd(myMusic);
					break;

				case "quit":
					Console.WriteLine("So Long");
					break;

				default:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("unknown command");
					break;
			}
		}//end menu Logic


		/// <summary>
		/// Shows all songs.
		/// </summary>
		public static void ShowAllSongs(List<Song> myMusic)
		{
			foreach (Song s in myMusic)
			{
				s.basicData();
			}

		}

		/// <summary>
		/// Finds the artist for a given song
		/// </summary>
		/// <param name="myMusic">My music.</param>
		static public void FindArtist(List<Song> myMusic)
		{
			Console.WriteLine("Enter a song title");
			string userInput = Console.ReadLine().ToLower();
			Console.WriteLine("Checking for possible matches");
			bool found = false;
			for (int i = 0; i < myMusic.Count; i++)
			{
				if (myMusic[i].Title.ToLower() == userInput)
				{
					Console.WriteLine("Found : " + myMusic[i].Title + " by " + myMusic[i].Artist);
					found = true;
				}
			}
			if (!found)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("No Artists were found for " + userInput);
			}
		}


		/// <summary>
		/// Finds the album for a given song
		/// </summary>
		/// <param name="myMusic">My music.</param>
		static public void FindAlbum(List<Song> myMusic)
		{
			Console.WriteLine("Enter a song title");
			string userInput = Console.ReadLine().ToLower();
			Console.WriteLine("Checking for possible matches");
			bool found = false;
			for (int i = 0; i < myMusic.Count; i++)
			{
				if (myMusic[i].Title.ToLower() == userInput)
				{
					Console.WriteLine("Found : " + myMusic[i].Title + " by " + myMusic[i].Artist + " on " + myMusic[i].Album);
					found = true;
				}
			}
			if (!found)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("No Albums were found for " + userInput);
			}
		}

		/// <summary>
		/// Finds all the data for a given song
		/// </summary>
		/// <param name="myMusic">My music.</param>
		static public void SongDetail(List<Song> myMusic)
		{
			Console.WriteLine("Enter a song title");
			string userInput = Console.ReadLine().ToLower();
			Console.WriteLine("Checking for possible matches");
			bool found = false;
			for (int i = 0; i < myMusic.Count; i++)
			{
				if (myMusic[i].Title.ToLower() == userInput)
				{
					myMusic[i].AllData();
					found = true;
				}
			}
			if (!found)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("No data was found for " + userInput);
			}
		}



		/// <summary>
		/// Finds all songs by an artist
		/// </summary>
		/// <param name="myMusic">My music.</param>
		static public void ArtistSongs(List<Song> myMusic)
		{
			Console.WriteLine("Enter an artist");
			string userInput = Console.ReadLine().ToLower();
			Console.WriteLine("Checking for tracks from " + userInput);
			bool found = false;
			for (int i = 0; i < myMusic.Count; i++)
			{
				if (myMusic[i].Artist.ToLower() == userInput)
				{
					Console.WriteLine("{0} by {1} appears on {2}",myMusic[i].Title,myMusic[i].Artist,myMusic[i].Album);
					found = true;
				}
			}
			if (!found)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("No data was found for " + userInput);
			}
		}

		/// <summary>
		/// Finds who added a song
		/// </summary>
		/// <param name="myMusic">My music.</param>
		static public void WhoDoneIt(List<Song> myMusic)
		{
			Console.WriteLine("Enter a song");
			string userInput = Console.ReadLine().ToLower();
			Console.WriteLine("Checking who added " + userInput);
			bool found = false;
			for (int i = 0; i < myMusic.Count; i++)
			{
				if (myMusic[i].Title.ToLower() == userInput)
				{
					myMusic[i].WhoAdded();
					found = true;
				}
			}
			if (!found)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("No data was found for " + userInput);
			}
		}


		/// <summary>
		///	Shows all the songs that a user added
		/// </summary>
		/// <param name="myMusic">My music.</param>
		static public void WhatDidTheyAdd(List<Song> myMusic)
		{
			Console.WriteLine("Enter a contributor name");
			string userInput = Console.ReadLine().ToLower();
			Console.WriteLine("Checking to see what songs " + userInput + " added.");
			bool found = false;
			for (int i = 0; i < myMusic.Count; i++)
			{
				if (myMusic[i].Contributor.ToLower() == userInput)
				{
					myMusic[i].basicData();
					found = true;
				}
			}
			if (!found)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Unable to find a user with the name " + userInput);
			}
		}
	}
}
