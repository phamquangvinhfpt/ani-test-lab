using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLabEntity.AutoDB
{
    public partial class TlChapter
    {
        public int Id { get; set; }

        public string ChapterName { get; set; } = null!;

        public int CourseId { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        public DateTime? DeteleAt { get; set; }

        public DateTime? UpdateAt { get; set; }

        public int CreateBy { get; set; }

        public virtual TlCourse Course { get; set; } = null!;

        public virtual TlAdmin CreateByNavigation { get; set; } = null!;

        public virtual ICollection<TlQuestion> TlQuestions { get; } = new List<TlQuestion>();
    }
}
