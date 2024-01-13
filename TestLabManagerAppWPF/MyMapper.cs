using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestLabEntity.AutoDB;
using TestLabEntity.BussinessObject;
namespace TestLabManagerAppWPF
{
    public static class MyMapper
    {
        private static bool _isInitialized;
        public static Mapper mapper { get; set; } = null!;
        public static void Initialize()
        {
            if (_isInitialized)
                return;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TlPaper, TlPaperObj>().ReverseMap();
                cfg.CreateMap<TlQuestion, TlQuestionObj>().ReverseMap();
                cfg.CreateMap<TlAdmin, TlAdminObj>().ReverseMap();
                cfg.CreateMap<TlCourse, TlCourseObj>().ReverseMap();
                cfg.CreateMap<TlChapter, TlChapterObj>().ReverseMap();
                cfg.CreateMap<TlAnswer, TlAnswerObj>().ReverseMap();
                cfg.CreateMap<TlQuestionPaper, TlQuestionPaperObj>().ReverseMap();
            });
            mapper = new Mapper(config);
            _isInitialized = true;
        }
    }
}
