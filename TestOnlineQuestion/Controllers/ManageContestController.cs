using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestOnlineQuestion.Models;
using TestOnlineQuestion.ViewModels;

namespace TestOnlineQuestion.Controllers
{
    public class ManageContestController : Controller
    {
        private WebDbContext db = new WebDbContext();

        // GET: ManageContest
        public ActionResult Index()
        {
            var contests = db.Contests.Include(c => c.Topic);
            return View(contests.ToList());
        }

        // GET: ManageContest/Create
        public ActionResult Create()
        {
            // Tạo một topic lưu trữ thông tin
            var topics = db.Topics
                .Where(t => t.State == true)  // tìm theo trạng thái
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(), // sủ dụng để lấy gí trị
                    Text = t.Name // sử dụng để hiển thị
                });

            ViewBag.TopicId = new SelectList(topics, "Value", "Text");
            return View();
        }

        // POST: ManageContest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,TopicId,DifficultyLevel,QuestionCount")] Contest contest)
        {
            if (ModelState.IsValid)
            {
                db.Contests.Add(contest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TopicId = new SelectList(db.Topics, "Id", "Name", contest.TopicId);
            return View(contest);
        }

        // GET: ManageContest/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contest contest = db.Contests.Find(id);
            if (contest == null)
            {
                return HttpNotFound();
            }
            // Lấy danh sách các chủ đề có trạng thái state = true
            var topics = db.Topics
                .Where(t => (bool)t.State)  // Lọc theo trạng thái
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(), // Giá trị để lưu
                    Text = t.Name // Hiển thị trong danh sách
                });
            ViewBag.TopicId = new SelectList(topics, "Value", "text", contest.TopicId);
            return View(contest);
        }

        // POST: ManageContest/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,TopicId,DifficultyLevel,QuestionCount")] Contest contest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TopicId = new SelectList(db.Topics, "Id", "Name", contest.TopicId);
            return View(contest);
        }

        // GET: ManageContest/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contest contest = db.Contests.Find(id);
            if (contest == null)
            {
                return HttpNotFound();
            }
            return View(contest);
        }

        // POST: ManageContest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contest contest = db.Contests.Find(id);
            db.Contests.Remove(contest);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ManageContestQuestions(int id)
        {
            Contest contest = db.Contests.Find(id);
            if (contest == null)
            {
                return HttpNotFound();
            }

            ViewBag.QuestionCount = contest.QuestionCount;

            var relatedQuestions = db.Questions
                .Where(q => q.TopicId == contest.TopicId && q.DifficultyLevel == contest.DifficultyLevel)
                .ToList();

            ViewBag.ContestId = contest.Id;

            return View(relatedQuestions);
        }



        [HttpPost]
        public ActionResult AddQuestionsToContest(int ContestId, List<int> selectedQuestions)
        {
            Contest contest = db.Contests.Find(ContestId);

            if (contest == null)
            {
                return HttpNotFound();
            }

            try
            {
                // Xóa các câu hỏi đang tồn tại trong cuộc thi
                var existingContestQuestions = db.ContestQuestions
                    .Where(cq => cq.Idcontest == ContestId)
                    .ToList();

                db.ContestQuestions.RemoveRange(existingContestQuestions);

                // Tạo danh sách các ContestQuestion cần thêm vào cơ sở dữ liệu
                var contestQuestionsToAdd = new List<ContestQuestion>();

                foreach (int questionId in selectedQuestions)
                {
                    var question = db.Questions.Find(questionId);
                    if (question != null)
                    {
                        var contestQuestion = new ContestQuestion
                        {
                            Idcontest = ContestId,
                            IdQuestion = questionId,
                            DifficultyLevel = question.DifficultyLevel
                        };
                        contestQuestionsToAdd.Add(contestQuestion);
                    }
                }

                // Thêm tất cả các ContestQuestion đã chọn vào cơ sở dữ liệu
                db.ContestQuestions.AddRange(contestQuestionsToAdd);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Các câu hỏi đã được thêm vào cuộc thi thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi thêm câu hỏi vào cuộc thi: " + ex.Message;
            }

            return RedirectToAction("Index");
        }


        public ActionResult UserIndex(int? selectedTopicId)
        {
            // Tạo danh sách chủ đề
            var topics = db.Topics
                .Where(t => t.State == true)
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                });

            ViewBag.TopicList = new SelectList(topics, "Value", "Text");

            // Lấy danh sách cuộc thi
            var contests = db.Contests.Include(c => c.Topic);

            // Lọc cuộc thi theo chủ đề được chọn (nếu có)
            if (selectedTopicId.HasValue && selectedTopicId > 0)
            {
                contests = contests.Where(c => c.TopicId == selectedTopicId.Value);
            }

            return View(contests.ToList());
        }

        public ActionResult ContestQuestions(int id)
        {
            var contestQuestions = db.ContestQuestions
                .Where(cq => cq.Idcontest == id)
                .Select(cq => new QuestionDTO
                {
                    Id = cq.IdQuestion,
                    QuestionText = cq.Question.QuestionText,
                    Answer1 = cq.Question.Answer1,
                    Answer2 = cq.Question.Answer2,
                    Answer3 = cq.Question.Answer3,
                    Answer4 = cq.Question.Answer4,
                    SelectedAnswer = 0 // Khởi tạo lựa chọn mặc định là 0 (chưa chọn)
                })
                .ToList();

            return View(contestQuestions);
        }
        private int CalculateCorrectAnswers(List<QuestionDTO> userAnswers, int contestId)
        {
            int correctAnswersCount = 0;

            var correctAnswers = db.ContestQuestions
                .Where(cq => cq.Idcontest == contestId)
                .Select(cq => new { QuestionId = cq.IdQuestion, CorrectAnswer = cq.Question.CorrectAnswer })
                .ToList();

            foreach (var answer in userAnswers)
            {
                var correctAnswer = correctAnswers.FirstOrDefault(ca => ca.QuestionId == answer.Id);
                if (correctAnswer != null && correctAnswer.CorrectAnswer == answer.SelectedAnswer)
                {
                    correctAnswersCount++;
                }
            }

            return correctAnswersCount;
        }

        [HttpPost]
        public ActionResult SubmitContestAnswers(int ContestId, List<QuestionDTO> userAnswers)
        {
            int correctAnswersCount = CalculateCorrectAnswers(userAnswers, ContestId);
            ViewBag.CorrectAnswersCount = correctAnswersCount;

            // Retrieve contest questions to display the full details in the "ContestResults" view
            var contestQuestions = db.ContestQuestions
                .Where(cq => cq.Idcontest == ContestId)
                .Select(cq => new QuestionDTO
                {
                    Id = cq.IdQuestion,
                    QuestionText = cq.Question.QuestionText,
                    Answer1 = cq.Question.Answer1,
                    Answer2 = cq.Question.Answer2,
                    Answer3 = cq.Question.Answer3,
                    Answer4 = cq.Question.Answer4,
                    SelectedAnswer = 0 // Khởi tạo lựa chọn mặc định là 0 (chưa chọn)
                })
                .ToList();

            return View("ContestResults", contestQuestions);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}