using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RandomWikiBackend.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace RandomWikiBackend.Controllers
{

    /// <summary>
    /// Controller for database
    /// </summary>
    public class WikiLinksController : ApiController
	{
        /// <summary>
        /// Get all wiki pages in database
        /// </summary>
        /// <returns>List of all wiki pages in database</returns>
		public List<WikiLinks> Get()
		{
			return db.WikiLinks.ToList();
		}

        /// <summary>
        /// Get a single wiki page
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Wikipage</returns>
		public WikiLinks Get(int? id)
		{
			WikiLinks wikiLinks = db.WikiLinks.Find(id);
			if (wikiLinks == null)
			{
				return new WikiLinks();
			}
			return wikiLinks;
		}

        /// <summary>
        /// Fetches 10 new random wiki pages and adds to database
        /// </summary>
        /// <returns>List of all wiki pages in database</returns>
		public List<WikiLinks> Post()
		{
			List<JToken> pages = GetRandomPages("https://en.wikipedia.org/", 10);
			foreach (JToken page in pages)
			{
				WikiLinks wl = new WikiLinks();
				wl.uri = "https://en.wikipedia.org/wiki/" + page["title"];
				wl.category = "Default";
				db.WikiLinks.Add(wl);
			}
			db.SaveChanges();
			return Get();
		}

        /// <summary>
        /// Clears all wiki pages from database
        /// </summary>
        /// <returns>List of all wiki pages in database (now an empty list)</returns>
		public List<WikiLinks> Delete()
		{
			List<WikiLinks> wlList = Get();
			db.WikiLinks.RemoveRange(wlList);
			db.SaveChanges();
			return Get();
		}

        /// <summary>
        /// Updates a wiki page category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category">New category name</param>
        /// <returns>List of all wiki pages in database</returns>
		public List<WikiLinks> Patch(int? id, string category)
		{
			WikiLinks wl = Get(id);
			wl.category = category;
			db.Entry(wl).State = EntityState.Modified;
			db.SaveChanges();
			return Get();
		}

        /// <summary>
        /// Generates "n" wiki pages from "url"
        /// Wikimedia API "rnlimit=n" would generate one random and the "n" following pages
        /// </summary>
        /// <param name="url">Wikimedia API address</param>
        /// <param name="n">Number of pages to generate</param>
        /// <returns>List of n wiki pages</returns>
		static List<JToken> GetRandomPages(string url, int n = 10)
		{
			List<JToken> pages = new List<JToken>();
			for (int i = 0; i < n; i++)
			{
				pages.Add(GetRandomPage(url));
			}
			return pages;
		}

        /// <summary>
        /// Generates one random wiki page
        /// </summary>
        /// <param name="url">Wikimedia API address</param>
        /// <returns>Wiki page</returns>
		static JToken GetRandomPage(string url)
		{
			string uri = url + "w/api.php?action=query&format=json&list=random&rnlimit=1";
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
			WebResponse webResponse = request.GetResponse();
			Stream webStream = webResponse.GetResponseStream();
			StreamReader responseReader = new StreamReader(webStream);
			string responseJson = responseReader.ReadToEnd();
			JObject responeseJObject = JsonConvert.DeserializeObject<JObject>(responseJson);
			JToken query = responeseJObject.Property("query").Value;
			JToken page = query["random"][0];
			responseReader.Close();
			webStream.Close();
			return page;
		}
		private RandomWikiBackendContext db = new RandomWikiBackendContext();
	}
}
