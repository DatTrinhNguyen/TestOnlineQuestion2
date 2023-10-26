namespace TestOnlineQuestion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Database : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContestQuestions",
                c => new
                    {
                        Idcontest = c.Int(nullable: false),
                        IdQuestion = c.Int(nullable: false),
                        DifficultyLevel = c.Int(),
                    })
                .PrimaryKey(t => new { t.Idcontest, t.IdQuestion })
                .ForeignKey("dbo.Contest", t => t.Idcontest)
                .ForeignKey("dbo.Question", t => t.IdQuestion)
                .Index(t => t.Idcontest)
                .Index(t => t.IdQuestion);
            
            CreateTable(
                "dbo.Contest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 200),
                        TopicId = c.Int(),
                        DifficultyLevel = c.Int(),
                        QuestionCount = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Topic", t => t.TopicId)
                .Index(t => t.TopicId);
            
            CreateTable(
                "dbo.Topic",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        State = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Question",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TopicId = c.Int(),
                        DifficultyLevel = c.Int(),
                        QuestionText = c.String(maxLength: 400),
                        Answer1 = c.String(maxLength: 400),
                        Answer2 = c.String(maxLength: 400),
                        Answer3 = c.String(maxLength: 400),
                        Answer4 = c.String(maxLength: 400),
                        CorrectAnswer = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Topic", t => t.TopicId)
                .Index(t => t.TopicId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 50),
                        HoDem = c.String(maxLength: 50),
                        Ten = c.String(maxLength: 50),
                        Password = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                        State = c.Boolean(),
                        Role = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Question", "TopicId", "dbo.Topic");
            DropForeignKey("dbo.ContestQuestions", "IdQuestion", "dbo.Question");
            DropForeignKey("dbo.Contest", "TopicId", "dbo.Topic");
            DropForeignKey("dbo.ContestQuestions", "Idcontest", "dbo.Contest");
            DropIndex("dbo.Question", new[] { "TopicId" });
            DropIndex("dbo.Contest", new[] { "TopicId" });
            DropIndex("dbo.ContestQuestions", new[] { "IdQuestion" });
            DropIndex("dbo.ContestQuestions", new[] { "Idcontest" });
            DropTable("dbo.User");
            DropTable("dbo.Question");
            DropTable("dbo.Topic");
            DropTable("dbo.Contest");
            DropTable("dbo.ContestQuestions");
        }
    }
}
