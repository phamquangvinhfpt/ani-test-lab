using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestLabEntity.AutoDB;

namespace TestLabEntity.Object
{
    public class QuestionDetails
    {
        public TlQuestion Question { get; set; }
        public List<TlAnswer> Answers { get; set; }
    }
}
