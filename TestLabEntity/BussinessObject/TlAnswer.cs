using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLabEntity.BussinessObject
{
    public partial class TlAnswerObj
    {
        public int Id { get; set; }

        public string AnswerText { get; set; } = null!;

        public int QuestionId { get; set; }

        public bool IsCorrect { get; set; }

        public byte[] CreateAt { get; set; } = null!;

        public DateTime? DeteleAt { get; set; }

        public DateTime? UpdateAt { get; set; }

        public int CreateBy { get; set; }

        public virtual TlAdminObj CreateByNavigation { get; set; } = null!;

        public virtual TlQuestionObj Question { get; set; } = null!;

        public bool IsSelected { get; set; } = false;
    }

}
