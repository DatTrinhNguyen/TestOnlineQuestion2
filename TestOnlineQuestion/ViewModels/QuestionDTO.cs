using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestOnlineQuestion.ViewModels
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public int? TopicId { get; set; }
        public int? DifficultyLevel { get; set; }
        public string QuestionText { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }

        public string Answer3 { get; set; }

        public string Answer4 { get; set; }
        public int CorrectAnswer { get; set; }
        public int SelectedAnswer { get; set; }
    }
}