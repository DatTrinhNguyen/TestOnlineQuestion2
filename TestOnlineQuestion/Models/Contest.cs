namespace TestOnlineQuestion.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contest")]
    public partial class Contest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Contest()
        {
            ContestQuestions = new HashSet<ContestQuestion>();
        }

        public int Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public int? TopicId { get; set; }

        public int? DifficultyLevel { get; set; }

        public int? QuestionCount { get; set; }

        public virtual Topic Topic { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContestQuestion> ContestQuestions { get; set; }
    }
}
