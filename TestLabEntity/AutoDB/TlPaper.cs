using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLabEntity.AutoDB
{
    public partial class TlPaper
    {
        public int Id { get; set; }

        public string PaperName { get; set; } = null!;

        public string PaperCode { get; set; } = null!;

        public int QuestionNum { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        public DateTime? DeteleAt { get; set; }

        public DateTime? UpdateAt { get; set; }

        public int CreateBy { get; set; }

        public int? CourseId { get; set; }

        public virtual TlCourse? Course { get; set; }

        public virtual TlAdmin CreateByNavigation { get; set; } = null!;

        public virtual ICollection<TlQuestionPaper> TlQuestionPapers { get; } = new List<TlQuestionPaper>();

    }
}
