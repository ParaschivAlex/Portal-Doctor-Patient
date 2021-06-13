namespace PortalDocPat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        ArticleId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        DoctorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ArticleId)
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: true)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        DoctorId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsAvailable = c.Boolean(nullable: false),
                        Phone = c.String(),
                        PriceRate = c.Single(nullable: false),
                        Rating = c.Single(nullable: false),
                        SpecializationId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.DoctorId)
                .ForeignKey("dbo.Specializations", t => t.SpecializationId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.SpecializationId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Consultations",
                c => new
                    {
                        ConsultationId = c.Int(nullable: false, identity: true),
                        DoctorId = c.Int(nullable: false),
                        PatientId = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        ConsultationName = c.DateTime(nullable: false),
                        CancelStatus = c.Boolean(nullable: false),
                        Patient_PatiendId = c.Int(),
                    })
                .PrimaryKey(t => t.ConsultationId)
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: true)
                .ForeignKey("dbo.Patients", t => t.Patient_PatiendId)
                .Index(t => t.DoctorId)
                .Index(t => t.Patient_PatiendId);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        PatiendId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Sex = c.String(nullable: false),
                        BirthDay = c.DateTime(nullable: false),
                        Kg = c.Single(nullable: false),
                        Allergies = c.String(),
                        UserId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PatiendId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewId = c.Int(nullable: false, identity: true),
                        Grade = c.Int(nullable: false),
                        Comment = c.String(maxLength: 150),
                        Date = c.DateTime(nullable: false),
                        DoctorId = c.Int(nullable: false),
                        PatientId = c.Int(nullable: false),
                        Patient_PatiendId = c.Int(),
                    })
                .PrimaryKey(t => t.ReviewId)
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: true)
                .ForeignKey("dbo.Patients", t => t.Patient_PatiendId)
                .Index(t => t.DoctorId)
                .Index(t => t.Patient_PatiendId);
            
            CreateTable(
                "dbo.Specializations",
                c => new
                    {
                        SpecializationId = c.Int(nullable: false, identity: true),
                        SpecializationName = c.String(nullable: false),
                        Price = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.SpecializationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Doctors", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Doctors", "SpecializationId", "dbo.Specializations");
            DropForeignKey("dbo.Reviews", "Patient_PatiendId", "dbo.Patients");
            DropForeignKey("dbo.Reviews", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.Patients", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Consultations", "Patient_PatiendId", "dbo.Patients");
            DropForeignKey("dbo.Consultations", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.Articles", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.Reviews", new[] { "Patient_PatiendId" });
            DropIndex("dbo.Reviews", new[] { "DoctorId" });
            DropIndex("dbo.Patients", new[] { "User_Id" });
            DropIndex("dbo.Consultations", new[] { "Patient_PatiendId" });
            DropIndex("dbo.Consultations", new[] { "DoctorId" });
            DropIndex("dbo.Doctors", new[] { "User_Id" });
            DropIndex("dbo.Doctors", new[] { "SpecializationId" });
            DropIndex("dbo.Articles", new[] { "DoctorId" });
            DropTable("dbo.Specializations");
            DropTable("dbo.Reviews");
            DropTable("dbo.Patients");
            DropTable("dbo.Consultations");
            DropTable("dbo.Doctors");
            DropTable("dbo.Articles");
        }
    }
}
