﻿using System;
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

        public int Duration { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool IsOpen { get; set; }

        public byte[] CreateAt { get; set; } = null!;

        public DateTime? DeteleAt { get; set; }

        public DateTime? UpdateAt { get; set; }

        public int CreateBy { get; set; }

        public int? CourseId { get; set; }

        public virtual TlCourse? Course { get; set; }

        public virtual TlAdmin CreateByNavigation { get; set; } = null!;

        public virtual ICollection<TlQuestionPaper> TlQuestionPapers { get; } = new List<TlQuestionPaper>();

    }
}
