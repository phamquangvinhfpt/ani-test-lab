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
using iTextSharp.text;
using iTextSharp.text.pdf;
using Document = iTextSharp.text.Document;
using Paragraph = iTextSharp.text.Paragraph;
using Image = iTextSharp.text.Image;
using WpfMath.Parsers;
using TestLabManagerAppWPF.ChildWindow.Question;
using WpfMath.Rendering;
using XamlMath;
using System.Windows.Media.Imaging;

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
                GeneratePDFContent(selectedPapers, path);

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
                paragraph.AppendText("\nSample Test Paper " + DateTime.Now.Year);
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
                table.ResetCells(size_answer + 1, collumn);

                for (int i = 1; i < collumn; i++)
                {
                    table[0, i].AddParagraph().AppendText((i).ToString());
                    table[0, i].CellFormat.HorizontalMerge = CellMerge.Start;
                    table[0, i].CellFormat.VerticalAlignment = Syncfusion.DocIO.DLS.VerticalAlignment.Middle;
                    table[0, i].CellFormat.Paddings.All = 5;
                    table[0, i].Paragraphs[0].ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                }

                for (int i = 1; i <= size_answer; i++)
                {
                    for (int j = 0; j < collumn; j++)
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

                    // Render question text to image
                    var parser = WpfTeXFormulaParser.Instance;
                    var formula = parser.Parse(q.QuestionText);
                    var environment = WpfTeXEnvironment.Create(TexStyle.Display, 20.0, "Arial");
                    var bitmapSource = formula.RenderToBitmap(environment);
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                    // Question text paragraph
                    IWParagraph questionParagraph = section.AddParagraph();
                    questionParagraph.AppendText("Question " + (j + 1) + ": ");

                    // Question image paragraph
                    IWParagraph imageParagraph = section.AddParagraph();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        byte[] imageBytes = ms.ToArray();
                        using (MemoryStream ms2 = new MemoryStream(imageBytes))
                        {
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms2);
                            WPicture picture = imageParagraph.AppendPicture(image) as WPicture;
                            picture.HorizontalAlignment = ShapeHorizontalAlignment.Center;
                            picture.VerticalAlignment = ShapeVerticalAlignment.Center;
                        }
                    }

                    if (q.QuestionImage != null)
                    {
                        byte[] imageBytes = Convert.FromBase64String(Convert.ToBase64String(q.QuestionImage));
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                            WPicture picture = section.AddParagraph().AppendPicture(image) as WPicture;
                            picture.HorizontalAlignment = ShapeHorizontalAlignment.Center;
                            picture.VerticalAlignment = ShapeVerticalAlignment.Center;
                        }
                    }

                    char index = 'A';
                    for (int i = 0; i < answers.Count; i++)
                    {
                        var answer = answers[i];

                        // Render answer text to image
                        formula = parser.Parse(answer.AnswerText);
                        environment = WpfTeXEnvironment.Create(TexStyle.Display, 10.0, "Arial");
                        bitmapSource = formula.RenderToBitmap(environment);
                        encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                        // Answer text paragraph
                        IWParagraph answerParagraph = section.AddParagraph();
                        answerParagraph.AppendText(index + ". ");

                        // Answer image paragraph
                        imageParagraph = section.AddParagraph();
                        using (MemoryStream ms = new MemoryStream())
                        {
                            encoder.Save(ms);
                            byte[] imageBytes = ms.ToArray();
                            using (MemoryStream ms2 = new MemoryStream(imageBytes))
                            {
                                System.Drawing.Image image = System.Drawing.Image.FromStream(ms2);
                                WPicture picture = imageParagraph.AppendPicture(image) as WPicture;
                                picture.HorizontalAlignment = ShapeHorizontalAlignment.Center;
                                picture.VerticalAlignment = ShapeVerticalAlignment.Center;
                            }
                        }

                        index = (char)(index + 1);
                        if (i == (answers.Count - 1))
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



        private void GeneratePDFContent(List<TlPaperObj> selectedPapers, string path)
        {
            var paperRepository = MyService.serviceProvider.GetService<IPaperRepository>();
            var parser = WpfTeXFormulaParser.Instance;

            foreach (var paperObj in selectedPapers)
            {
                var paperDetail = paperRepository.getPaperdetails(paperObj.Id);
                var course = paperDetail.Course;
                var qp = paperDetail.TlQuestionPapers.ToList();
                string fileName = paperDetail.PaperName + ".pdf";
                string fullPath = System.IO.Path.Combine(path, fileName);
                string[] alphabetArray = new string[]
                {
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
            "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
                };

                using (FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (Document document = new Document())
                    {
                        iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, fs);
                        document.Open();

                        // Add title
                        Paragraph title = new Paragraph($"FPT University\nSubject: {course.CourseName}\nSample Test Paper {DateTime.Now.Year}\nOfficial\nQuestion Number: {paperDetail.QuestionNum}");
                        title.Alignment = Element.ALIGN_LEFT;
                        document.Add(title);

                        // Add name and code
                        Paragraph nameAndCode = new Paragraph($"\n\nName: ...............................................................\n Code: .......................... Class: ..............................\n\n");
                        document.Add(nameAndCode);

                        // Add table
                        PdfPTable table = new PdfPTable(qp.Count + 1);
                        for (int i = 0; i <= qp.Count; i++)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(i == 0 ? "" : i.ToString()));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell);
                        }

                        for (int i = 1; i <= qp.Count; i++)
                        {
                            for (int j = 0; j <= GetMaxAnswer(qp); j++)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(j == 0 ? alphabetArray[i - 1] : "o"));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                table.AddCell(cell);
                            }
                        }
                        document.Add(table);

                        // Add questions and answers
                        foreach (var q in qp)
                        {
                            // Render QuestionText as image
                            var formula = parser.Parse(q.Question.QuestionText);
                            var environment = WpfTeXEnvironment.Create(TexStyle.Display, 20.0, "Arial");
                            var bitmapSource = formula.RenderToBitmap(environment);
                            var encoder = new PngBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                            using (MemoryStream ms = new MemoryStream())
                            {
                                encoder.Save(ms);
                                byte[] imageBytes = ms.ToArray();
                                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageBytes);
                                document.Add(new Paragraph("Question " + (qp.IndexOf(q) + 1) + ":"));
                                document.Add(image);
                            }

                            // Add image if exists
                            if (q.Question.QuestionImage != null)
                            {
                                byte[] imageBytes = Convert.FromBase64String(Convert.ToBase64String(q.Question.QuestionImage));
                                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageBytes);
                                document.Add(image);
                            }

                            char index = 'A';
                            foreach (var answer in q.Question.TlAnswers)
                            {
                                // Render AnswerText as image
                                formula = parser.Parse(answer.AnswerText);
                                environment = WpfTeXEnvironment.Create(TexStyle.Display, 10.0, "Arial");
                                bitmapSource = formula.RenderToBitmap(environment);
                                encoder = new PngBitmapEncoder();
                                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    encoder.Save(ms);
                                    byte[] imageBytes = ms.ToArray();
                                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageBytes);
                                    Paragraph answerParagraph = new Paragraph(index + ". ");
                                    answerParagraph.Add(image);
                                    document.Add(answerParagraph);
                                }
                                index = (char)(index + 1);
                                if (index > 'Z') index = 'A';
                            }

                            document.Add(new Paragraph());
                        }

                        // Add end note
                        Paragraph endNote = new Paragraph($"\n===End===\n \"Candidates may not use the document. The exam invigilator did not explain further.\"");
                        endNote.Alignment = Element.ALIGN_CENTER;
                        document.Add(endNote);
                    }
                }
            }
        }
    }
}
