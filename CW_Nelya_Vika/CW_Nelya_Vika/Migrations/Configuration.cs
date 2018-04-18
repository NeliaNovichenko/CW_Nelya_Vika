namespace CW_Nelya_Vika.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CW_Nelya_Vika.Models.DB.GraphProblemDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "CW_Nelya_Vika.Models.DB.GraphProblemDBContext";
        }

        protected override void Seed(CW_Nelya_Vika.Models.DB.GraphProblemDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
