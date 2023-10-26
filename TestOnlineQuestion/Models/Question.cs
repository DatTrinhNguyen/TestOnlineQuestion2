namespace TestOnlineQuestion.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Question")]
    public partial class Question
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Question()
        {
            ContestQuestions = new HashSet<ContestQuestion>();
        }

        public int Id { get; set; }

        public int? TopicId { get; set; }

        public int? DifficultyLevel { get; set; }

        [StringLength(400)]
        public string QuestionText { get; set; }

        [StringLength(400)]
        public string Answer1 { get; set; }

        [StringLength(400)]
        public string Answer2 { get; set; }

        [StringLength(400)]
        public string Answer3 { get; set; }

        [StringLength(400)]
        public string Answer4 { get; set; }

        public int? CorrectAnswer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContestQuestion> ContestQuestions { get; set; }

        public virtual Topic Topic { get; set; }
    }
}
