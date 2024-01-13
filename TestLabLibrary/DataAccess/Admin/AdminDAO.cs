using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestLabEntity.AutoDB;

namespace TestLabLibrary.DataAccess.Admin
{
    public class AdminDAO
    {
        private TestLabContext context;

        private static AdminDAO instance;
        private static readonly object padlock = new object();

        public static AdminDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AdminDAO();
                }
                return instance;
            }
        }

        private AdminDAO()
        {
            context = new TestLabContext();
            // Khởi tạo context, admin, các entities ban đầu
        }

        public TlAdmin Login(string username, string password)
        {
            var admin = context.TlAdmins.Where(a => a.Username == username && a.Password == password).FirstOrDefault();
            return admin;
        }
    }
}
