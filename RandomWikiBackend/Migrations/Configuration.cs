namespace RandomWikiBackend.Migrations
{
	using System.Data.Entity.Migrations;

	internal sealed class Configuration : DbMigrationsConfiguration<Models.RandomWikiBackendContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        /// Initializes an empty database
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(Models.RandomWikiBackendContext context)
        {
        }
    }
}
