using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestLabEntity.AutoDB;

namespace TestLabLibrary.Repository
{
    public interface IPaperRepository
    {
        // Paper
        List<TlPaper> GetPapers();
        List<TlPaper> GetPapers(int offset = 0, int limit = 10, string search = "");
        TlPaper? GetPaper(int id);
        TlPaper? GetPaper(string code);
        int AddPaper(TlPaper paper);
        bool UpdatePaper(TlPaper paper);
        bool DeletePaper(int id);
        bool DeletePaper(string code);
        int CountAll();
        // Paper Question
        List<TlQuestionPaper> GetQuestionPapers(int paper_id);
        List<TlQuestionPaper> GetQuestionPapers(int offset = 0, int limit = 10, int qid = 0, int pid = 0);
        TlQuestionPaper? GetQuestionPapersByPaperId(int pid);
        TlQuestionPaper? GetQuestionPaper(int id);
        bool AddQuestionPaper(TlQuestionPaper questionPaper);
        bool UpdateQuestionPaper(TlQuestionPaper questionPaper);
        bool DeleteQuestionPaper(int id);
        List<TlPaper> GetPapersByCourseId(int idCourseSelected, string SearchValue);
    }
}
