using System.Data.Entity;
using System.Data.SqlClient;

namespace RandomWikiBackend.Models
{
    public class RandomWikiBackendContext : DbContext
	{
		// You can add custom code to this file. Changes will not be overwritten.
		// 
		// If you want Entity Framework to drop and regenerate your database
		// automatically whenever you change your model schema, please use data migrations.
		// For more information refer to the documentation:
		// http://msdn.microsoft.com/en-us/data/jj591621.aspx

		SqlConnection conn { get; }
		public RandomWikiBackendContext() : base("name=RandomWikiBackendContext")
		{
		}

		public DbSet<WikiLinks> WikiLinks { get; set; }
	}

    /// <summary>
    /// Class to represent a wiki page
    /// </summary>
	public class WikiLinks
	{
		public int id { get; set; }
		public string uri { get; set; }
		public string category { get; set; }
	}
}
