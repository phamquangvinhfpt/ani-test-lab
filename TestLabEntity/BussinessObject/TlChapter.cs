using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLabEntity.BussinessObject
{
    public partial class TlChapterObj
    {
        public int Id { get; set; }

        public string ChapterName { get; set; } = null!;

        public int CourseId { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        public DateTime? DeteleAt { get; set; }

        public DateTime? UpdateAt { get; set; }

        public int CreateBy { get; set; }

        public virtual TlCourseObj Course { get; set; } = null!;

        public virtual TlAdminObj CreateByNavigation { get; set; } = null!;

        public virtual ICollection<TlQuestionObj> TlQuestions { get; } = new List<TlQuestionObj>();
        public bool IsSelected { get; set; } = false;
    }
}
