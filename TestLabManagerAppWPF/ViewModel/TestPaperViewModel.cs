using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestLabEntity.AutoDB;
using TestLabEntity.BussinessObject;
using TestLabLibrary.Repository;
using TestLabManagerAppWPF.ChildWindow.TestPaper;
using Microsoft.Office.Interop.Word;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System.Windows.Shapes;
namespace TestLabManagerAppWPF.ViewModel
{
    class TestPaperViewModel : ViewModelBase
    {
        private ObservableCollection<TlPaperObj> _papers;
        private ObservableCollection<TlCourseObj> _courses;
        private TlCourseObj _selectedCourse;
        private string _searchText = "";
        public ObservableCollection<TlPaperObj> Papers
        {
            get
            {
                return _papers;
            }
            set
            {
                _papers = value;
                OnPropertyChanged(nameof(Papers));
            }
        }

        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public ObservableCollection<TlCourseObj> Courses
        {
            get
            {
                return _courses;
            }
            set
            {
                _courses = value;
                OnPropertyChanged(nameof(Courses));
            }
        }

        public TlCourseObj SelectedCourse
        {
            get
            {
                return _selectedCourse;
            }
            set
            {
                // Load papers
                _selectedCourse = value;
                LoadPapers(value.Id);
                OnPropertyChanged(nameof(SelectedCourse));
            }
        }

        // Get Papers from database
        public void LoadPapers(int courseId = 0)
        {
            var paperRepository = MyService.serviceProvider.GetService<IPaperRepository>();
            var papersEf = new ObservableCollection<TlPaper>(paperRepository.GetPapers(0, 9999, SearchText));
            if (courseId != 0)
            {
                papersEf = new ObservableCollection<TlPaper>(papersEf.Where(p => p.CourseId == courseId).ToList());
            }
            Papers = new ObservableCollection<TlPaperObj>(MyMapper.mapper.Map<List<TlPaperObj>>(papersEf));
        }

        // Load Courses
        public void LoadCourses()
        {
            var courseRepository = MyService.serviceProvider.GetService<IQuestionRepository>();
            var coursesEf = courseRepository.GetCourses(0, 9999, SearchText);
            Courses = new ObservableCollection<TlCourseObj>(MyMapper.mapper.Map<List<TlCourseObj>>(coursesEf));
        }

        // Get Selected Papers
        public List<TlPaperObj> GetSelectedPapers()
        {
            List<TlPaperObj> selectedPapers = new List<TlPaperObj>();
            foreach (var paper in Papers)
            {
                if (paper.IsSelected)
                {
                    selectedPapers.Add(paper);
                }
            }
            return selectedPapers;
        }


        // Command
        public ICommand SearchCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand PrintCommand { get; }
        public TestPaperViewModel()
        {
            LoadCourses();
            LoadPapers();

            SearchCommand = new ViewModelCommand(ExuteSearchCommand, null);
            AddCommand = new ViewModelCommand(ExuteAddCommand, null);
            EditCommand = new ViewModelCommand(ExuteEditCommand, null);
            DeleteCommand = new ViewModelCommand(ExuteDeleteCommand, null);
            PrintCommand = new ViewModelCommand(ExutePrintCommand, null);
        }

        private void ExuteDeleteCommand(object obj)
        {
            // Get selected Paper
            var selectedPapers = GetSelectedPapers();
            if (selectedPapers.Count == 0)
            {
                System.Windows.MessageBox.Show("Please select a paper to delete!");
                return;
            }
            var paperRepository = MyService.serviceProvider.GetService<IPaperRepository>();
            try
            {
                foreach (var paper in selectedPapers)
                {
                    paperRepository.DeletePaper(paper.Id);
                }
                System.Windows.MessageBox.Show("Delete paper successfully!");
                LoadPapers();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Delete paper failed with error: " + ex.Message);
            }
        }
        private void ExutePrintCommand(object obj)
        {
            var selectedPapers = GetSelectedPapers();
            if (selectedPapers.Count == 0)
            {
                System.Windows.MessageBox.Show("Please select a paper to export!");
                return;
            }
            string path = "";
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                path = dialog.SelectedPath;
            }
            if(path.Equals(""))
            {
                System.Windows.MessageBox.Show("Please select Path");
                return;
            }
            try
            {
                
                GenerateTestContent(selectedPapers, path);
                
                LoadPapers();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Export paper failed with error: " + ex.Message);
            }
        }
        private void GenerateTestContent(List<TlPaperObj> selectedPapers, string path)
        {
            var paperRepository = MyService.serviceProvider.GetService<IPaperRepository>();
            var paperDetail = paperRepository.getPaperdetails(selectedPapers[0].Id);
            var course = paperDetail.Course;
            var qp = paperDetail.TlQuestionPapers.ToList();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NAaF1cXmhNYVRpR2Nbe05xdV9FZVZRRGYuP1ZhSXxXdkZjUX5fc3FVR2ZfVUY=");
            string fileName = paperDetail.PaperName + ".docx";
            string fullPath = System.IO.Path.Combine(path, fileName);
            string[] alphabetArray = new string[]
{
    "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
    "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
};
            using (WordDocument document = new WordDocument())
            {
                WSection section = (WSection)document.AddSection();

                IWParagraph paragraph = section.AddParagraph();

                paragraph.AppendText("     FPT University\t\t\t\t\t\t\t\t\tSubject: " + course.CourseName);
                paragraph.AppendText("\n          Official\t\t\t\t\t\t\t\t\t\tQuestion Number: " + paperDetail.QuestionNum);
                paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Left;
                IWTextRange textRange = paragraph.ChildEntities[0] as IWTextRange;
                textRange.CharacterFormat.FontSize = 11f;
                textRange = paragraph.ChildEntities[1] as IWTextRange;
                textRange.CharacterFormat.FontSize = 10f;
                textRange = paragraph.ChildEntities[2] as IWTextRange;
                textRange.CharacterFormat.FontSize = 10f;
                section.Body.ChildEntities.Add(paragraph);

                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                paragraph.AppendText("\n\nName: …………………………………………………………………………………………………………");
                paragraph.AppendText("\nCode:………………………………………….. .Class: …………………………………………………….\n\n");
                textRange = paragraph.ChildEntities[0] as IWTextRange;
                textRange.CharacterFormat.Bold = true;
                textRange = paragraph.ChildEntities[1] as IWTextRange;
                textRange.CharacterFormat.Bold = true;
                section.Body.ChildEntities.Add(paragraph);
                
                int size_answer = GetMaxAnswer(qp);
                
                WTable table = (WTable)section.AddTable();
                int collumn = qp.Count + 1;
                table.ResetCells(collumn + 1, size_answer);

                // Thêm dòng tiêu đề
                for (int i = 1; i <= qp.Count; i++)
                {
                    table[0, i].AddParagraph().AppendText((i).ToString());
                    table[0, i].CellFormat.HorizontalMerge = CellMerge.Start;
                    table[0, i].CellFormat.VerticalAlignment = Syncfusion.DocIO.DLS.VerticalAlignment.Middle;
                    table[0, i].CellFormat.Paddings.All = 5;
                    table[0, i].Paragraphs[0].ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                }

                // Thêm dòng câu trả lời

                for(int i = 1; i <= collumn; i++)
{
                    for (int j = 0; j < size_answer; j++)
                    {
                        if (j == 0)
                        {
                            table[i, j].AddParagraph().AppendText(alphabetArray[i - 1]);
                        }
                        else
                        {
                            table[i, j].AddParagraph().AppendText("o");
                        }
                        table[i, j].CellFormat.VerticalAlignment = Syncfusion.DocIO.DLS.VerticalAlignment.Middle;
                        table[i, j].CellFormat.Paddings.All = 5;
                        table[i, j].Paragraphs[0].ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                    }
                }
                section.Body.ChildEntities.Add(table);

                for (int j = 0; j < qp.Count; j++)
                {
                    var q = qp[j].Question;
                    var answers = q.TlAnswers.ToList();

                    section.AddParagraph().AppendText("Question " + (j + 1) + ": " + q.QuestionText);

                    if (q.QuestionImage != null)
                    {
                        byte[] imageBytes = Convert.FromBase64String(Convert.ToBase64String(q.QuestionImage));
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                            WPicture picture = section.AddParagraph().AppendPicture(image) as WPicture;
                            picture.HorizontalAlignment = ShapeHorizontalAlignment.Center;
                            picture.VerticalAlignment = ShapeVerticalAlignment.Center;
                            picture.Width = 300;
                            picture.Height = 200;
                        }
                    }

                    char index = 'A';
                    for (int i = 0; i < answers.Count; i++)
                    {
                        var answer = answers[i];
                        section.AddParagraph().AppendText(index + ". " + answer.AnswerText);
                        index = (char)(index + 1);
                        if(i == (answers.Count - 1))
                        {
                            index = 'A';
                        }
                    }

                    section.AddParagraph();
                }


                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                paragraph.AppendText("\n===End===");
                paragraph.AppendText("\nCandidates may not use the document. The exam invigilator did not explain further.");
                textRange = paragraph.ChildEntities[0] as IWTextRange;
                textRange.CharacterFormat.Bold = true;
                textRange = paragraph.ChildEntities[1] as IWTextRange;
                textRange.CharacterFormat.Bold = true;
                section.Body.ChildEntities.Add(paragraph);

                document.Save(fullPath, FormatType.Docx);
                document.Close();
            }
        }
        private int GetMaxAnswer(List<TlQuestionPaper> qp)
        {
            int maxAnswer = 0;
            foreach (TlQuestionPaper p in qp)
            {
                if(maxAnswer < p.Question.TlAnswers.Count)
                {
                    maxAnswer = p.Question.TlAnswers.Count;
                }
            }
            return maxAnswer;
        }
        private void ExuteEditCommand(object obj)
        {
            // get selected paper
            var selectedPapers = GetSelectedPapers();
            if (selectedPapers.Count == 0)
            {
                System.Windows.MessageBox.Show("Please select a paper to edit!");
                return;
            }
            if (selectedPapers.Count > 1)
            {
                System.Windows.MessageBox.Show("Please select only one paper to edit!");
                return;
            }
            if (SelectedCourse == null)
            {
                System.Windows.MessageBox.Show("Please select a course!");
                return;
            }

            var editTestPaperWindow = new EditTestPaperWindow();
            var paper = MyMapper.mapper.Map<TlPaper>(selectedPapers[0]);
            var editTestPaperViewModel = new EditTestPaperViewModel(paper);
            editTestPaperViewModel.IdCourseSelected = SelectedCourse.Id;
            editTestPaperWindow.DataContext = editTestPaperViewModel;
            if (editTestPaperWindow.ShowDialog() == true)
            {
                // Reload papers
                LoadPapers(SelectedCourse.Id);
            }
        }

        private void ExuteAddCommand(object obj)
        {
            var addTestPaperWindow = new AddTestPaperWindow();
            var addTestPaperViewModel = new AddTestPaperViewModel();
            if (SelectedCourse == null)
            {
                System.Windows.MessageBox.Show("Please select a course!");
                return;
            }
            addTestPaperViewModel.IdCourseSelected = SelectedCourse.Id;
            addTestPaperWindow.DataContext = addTestPaperViewModel;
            if (addTestPaperWindow.ShowDialog() == true)
            {
                // Reload papers
                LoadPapers(SelectedCourse.Id);
            }

        }

        private void ExuteSearchCommand(object obj)
        {
            if (SelectedCourse == null)
            {
                LoadPapers();
            }
            else
            {
                LoadPapers(SelectedCourse.Id);
            }
            // Notify UI to update
            OnPropertyChanged(nameof(Papers));
        }
    }
}
