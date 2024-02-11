using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLabEntity.AutoDB
{
    public partial class TlQuestion
    {
        public int Id { get; set; }

        public string QuestionText { get; set; } = null!;

        public byte[]? QuestionImage { get; set; }
        public string QuestionImageBase64
        {
            get
            {
                if (QuestionImage != null)
                {
                    return "data:image/png;base64," + Convert.ToBase64String(QuestionImage);
                }
                return "";
            }
        }
        public int CourseId { get; set; }

        public int ChapterId { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        public DateTime? DeteleAt { get; set; }

        public DateTime? UpdateAt { get; set; }

        public int? CreateBy { get; set; }

        public virtual TlChapter Chapter { get; set; } = null!;

        public virtual TlCourse Course { get; set; } = null!;

        public virtual TlAdmin? CreateByNavigation { get; set; }

        public virtual ICollection<TlAnswer> TlAnswers { get; } = new List<TlAnswer>();

        public virtual ICollection<TlQuestionPaper> TlQuestionPapers { get; } = new List<TlQuestionPaper>();

    }
}
