using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestLabEntity.AutoDB;

namespace TestLabLibrary.Repository
{
    public class Repository : IRepository
    {
        AdminRepository IRepository.AdminRepository => new AdminRepository();

        QuestionRepository IRepository.QuestionRepository => new QuestionRepository();

        TlAdmin IRepository.Admin { get; set; }
    }
}
