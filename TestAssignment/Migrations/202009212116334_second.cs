namespace TestAssignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Elevators", "Building_BuildingId", "dbo.Buildings");
            DropIndex("dbo.Elevators", new[] { "Building_BuildingId" });
            RenameColumn(table: "dbo.Elevators", name: "Building_BuildingId", newName: "BuildingId");
            AlterColumn("dbo.Elevators", "BuildingId", c => c.Int(nullable: false));
            CreateIndex("dbo.Elevators", "BuildingId");
            AddForeignKey("dbo.Elevators", "BuildingId", "dbo.Buildings", "BuildingId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Elevators", "BuildingId", "dbo.Buildings");
            DropIndex("dbo.Elevators", new[] { "BuildingId" });
            AlterColumn("dbo.Elevators", "BuildingId", c => c.Int());
            RenameColumn(table: "dbo.Elevators", name: "BuildingId", newName: "Building_BuildingId");
            CreateIndex("dbo.Elevators", "Building_BuildingId");
            AddForeignKey("dbo.Elevators", "Building_BuildingId", "dbo.Buildings", "BuildingId");
        }
    }
}
