using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLabEntity.BussinessObject
{
    public partial class TlQuestionObj
    {
        public int Id { get; set; }

        public string QuestionText { get; set; } = null!;
        [NotMapped]
        public string EquationQuestionTextToDisplay { get; set; } = null!;
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

        public virtual TlChapterObj Chapter { get; set; } = null!;

        public virtual TlCourseObj Course { get; set; } = null!;

        public virtual TlAdminObj? CreateByNavigation { get; set; }

        public virtual ICollection<TlAnswerObj> TlAnswers { get; } = new List<TlAnswerObj>();

        public virtual ICollection<TlQuestionPaperObj> TlQuestionPapers { get; } = new List<TlQuestionPaperObj>();

        public bool IsSelected { get; set; } = false;
    }
}
