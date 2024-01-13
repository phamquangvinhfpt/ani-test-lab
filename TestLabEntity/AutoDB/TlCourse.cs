using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLabEntity.AutoDB
{
    public partial class TlCourse
    {
        public int Id { get; set; }

        public string CourseName { get; set; } = null!;

        public byte[] CreateAt { get; set; } = null!;

        public DateTime? DeteleAt { get; set; }

        public DateTime? UpdateAt { get; set; }

        public int CreateBy { get; set; }

        public virtual TlAdmin CreateByNavigation { get; set; } = null!;

        public virtual ICollection<TlChapter> TlChapters { get; } = new List<TlChapter>();

        public virtual ICollection<TlPaper> TlPapers { get; } = new List<TlPaper>();

        public virtual ICollection<TlQuestion> TlQuestions { get; } = new List<TlQuestion>();
    }
}
