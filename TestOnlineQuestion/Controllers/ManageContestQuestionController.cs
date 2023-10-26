using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestOnlineQuestion.Models;

namespace TestOnlineQuestion.Controllers
{
    public class ManageContestQuestionController : Controller
    {
        private WebDbContext db = new WebDbContext();

        public ActionResult Index(int? id)
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

            ViewBag.ContestId = id;
            ViewBag.ContestName = contest.Name;

            // Số câu hỏi còn lại
            int remainingQuestions = (int)(contest.QuestionCount - db.ContestQuestions.Where(cq => cq.Idcontest  == id).Count());
            ViewBag.RemainingQuestions = remainingQuestions;

            return View(contest);
        }

        public ActionResult AddQuestion(int? contestId)
        {
            if (contestId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Contest contest = db.Contests.Find(contestId);

            if (contest == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách câu hỏi phù hợp với cuộc thi
            var questions = db.Questions.Where(q => q.DifficultyLevel == contest.DifficultyLevel).ToList();

            ViewBag.ContestId = contestId;
            ViewBag.ContestName = contest.Name;

            return View(questions);
        }

        [HttpPost]
        public ActionResult AddQuestion(int? contestId, int? questionId)
        {
            if (contestId == null || questionId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Contest contest = db.Contests.Find(contestId);

            if (contest == null)
            {
                return HttpNotFound();
            }

            // Kiểm tra xem cuộc thi đã đủ số câu hỏi chưa
            int remainingQuestions = (int)(contest.QuestionCount - db.ContestQuestions.Where(cq => cq.Idcontest == contestId).Count());
            if (remainingQuestions <= 0)
            {
                ModelState.AddModelError("", "Cuộc thi đã đủ số lượng câu hỏi.");
            }
            else
            {
                var existingQuestion = db.ContestQuestions.FirstOrDefault(cq => cq.Idcontest == contestId && cq.IdQuestion == questionId);

                if (existingQuestion != null)
                {
                    ModelState.AddModelError("", "Câu hỏi đã tồn tại trong cuộc thi.");
                }
                else
                {
                    var contestQuestion = new ContestQuestion
                    {
                        Idcontest = (int)contestId,
                        IdQuestion = (int)questionId,
                        DifficultyLevel = contest.DifficultyLevel
                    };

                    db.ContestQuestions.Add(contestQuestion);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index", new { id = contestId });
        }
    }
}