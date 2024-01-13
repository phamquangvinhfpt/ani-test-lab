using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLabEntity.BussinessObject
{
    public partial class TlQuestionPaperObj
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public int PaperId { get; set; }

        public int Mark { get; set; }

        public virtual TlPaperObj Paper { get; set; } = null!;

        public virtual TlQuestionObj Question { get; set; } = null!;
        public bool IsSelected { get; set; } = false;
    }
}
