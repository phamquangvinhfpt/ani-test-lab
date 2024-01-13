using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLabEntity.AutoDB
{
    public partial class TlQuestionPaper
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public int PaperId { get; set; }

        public int Mark { get; set; }

        public virtual TlPaper Paper { get; set; } = null!;

        public virtual TlQuestion Question { get; set; } = null!;
    }
}
