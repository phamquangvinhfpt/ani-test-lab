using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestLabEntity.AutoDB;

namespace TestLabEntity.Object
{
    public class PaperDetails
    {
        public string PaperName { get; set; }
        public string PaperCode { get; set; }
        public int QuestionNum { get; set; }
        public int Duration { get; set; }
        public List<QuestionDetails> Questions { get; set; }
    }
}
