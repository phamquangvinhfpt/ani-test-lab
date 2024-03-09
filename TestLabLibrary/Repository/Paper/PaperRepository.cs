using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestLabEntity.AutoDB;
using TestLabEntity.Object;
using TestLabLibrary.DataAccess.Paper;

namespace TestLabLibrary.Repository
{
    public class PaperRepository : IPaperRepository
    {
        // Paper
        List<TlPaper> IPaperRepository.GetPapers() => PaperDAO.Instance.GetPapers();
        List<TlPaper> IPaperRepository.GetPapers(int offset, int limit, string search) => PaperDAO.Instance.GetPapers(offset, limit, search);
        TlPaper? IPaperRepository.GetPaper(int id) => PaperDAO.Instance.GetPaper(id);
        TlPaper? IPaperRepository.GetPaper(string code) => PaperDAO.Instance.GetPaper(code);
        int IPaperRepository.AddPaper(TlPaper paper) => PaperDAO.Instance.AddPaper(paper);
        bool IPaperRepository.UpdatePaper(TlPaper paper) => PaperDAO.Instance.UpdatePaper(paper);
        bool IPaperRepository.DeletePaper(int id) => PaperDAO.Instance.DeletePaper(id);
        bool IPaperRepository.DeletePaper(string code) => PaperDAO.Instance.DeletePaper(code);
        // Paper Question
        List<TlQuestionPaper> IPaperRepository.GetQuestionPapers(int paper_id) => QuestionPaperDAO.Instance.GetQuestionPapers(paper_id);
        List<TlQuestionPaper> IPaperRepository.GetQuestionPapers(int offset, int limit, int qid, int pid) => QuestionPaperDAO.Instance.GetQuestionPapers(offset, limit, qid, pid);
        TlQuestionPaper? IPaperRepository.GetQuestionPapersByPaperId(int pid) => QuestionPaperDAO.Instance.GetQuestionPapersByPaperId(pid);
        TlQuestionPaper? IPaperRepository.GetQuestionPaper(int id) => QuestionPaperDAO.Instance.GetQuestionPaper(id);
        bool IPaperRepository.AddQuestionPaper(TlQuestionPaper questionPaper) => QuestionPaperDAO.Instance.AddQuestionPaper(questionPaper);
        bool IPaperRepository.UpdateQuestionPaper(TlQuestionPaper questionPaper) => QuestionPaperDAO.Instance.UpdateQuestionPaper(questionPaper);
        bool IPaperRepository.DeleteQuestionPaper(int id) => QuestionPaperDAO.Instance.DeleteQuestionPaper(id);

        int IPaperRepository.CountAll() => PaperDAO.Instance.CountAll();

        List<TlPaper> IPaperRepository.GetPapersByCourseId(int idCourseSelected, string SearchValue) => PaperDAO.Instance.GetPapersByCourseId(idCourseSelected, SearchValue);
       
        TlPaper IPaperRepository.getPaperdetails(int paperID)
        {
            var qp = PaperDAO.Instance.GetPaperForPrint(paperID);
            return qp;
        }
    }
}
