namespace TestAssignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Buildings",
                c => new
                    {
                        BuildingId = c.Int(nullable: false, identity: true),
                        BuildingNumber = c.String(nullable: false),
                        BuildingName = c.String(nullable: false),
                        BuildingLimitElevatorNo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BuildingId);
            
            CreateTable(
                "dbo.Elevators",
                c => new
                    {
                        ElevatorId = c.Int(nullable: false, identity: true),
                        ElevatorNumber = c.Int(nullable: false),
                        ElevatorLimitPeople = c.Int(nullable: false),
                        ElevatorLimitWeight = c.Int(nullable: false),
                        Building_BuildingId = c.Int(),
                    })
                .PrimaryKey(t => t.ElevatorId)
                .ForeignKey("dbo.Buildings", t => t.Building_BuildingId)
                .Index(t => t.Building_BuildingId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Elevators", "Building_BuildingId", "dbo.Buildings");
            DropIndex("dbo.Elevators", new[] { "Building_BuildingId" });
            DropTable("dbo.Elevators");
            DropTable("dbo.Buildings");
        }
    }
}
