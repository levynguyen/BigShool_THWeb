namespace BigShool_ThWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateCategoryTable : DbMigration
    {
        public override void Up()
        {
            Sql("insert into Categories (ID,NAME) values (1,'Development')");
            Sql("insert into Categories (ID,NAME) values (2,'Business')");
            Sql("insert into Categories (ID,NAME) values (3,'Marketing')");
        }
        
        public override void Down()
        {
        }
    }
}
