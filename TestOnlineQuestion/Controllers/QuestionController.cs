using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestOnlineQuestion.Models;

namespace TestOnlineQuestion.Controllers
{
    public class QuestionController : Controller
    {
        private WebDbContext db = new WebDbContext();

        // Phương thức hành động để hiển thị danh sách câu hỏi
        public ActionResult Index()
        {
            var questions = db.Questions.ToList();
            return View(questions);
        }

        public ActionResult Create()
        {
           //Tạo một topic lưu trữ thông tin
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


        // Phương thức hành động xử lý yêu cầu POST để tạo câu hỏi mới
        [HttpPost]
        public ActionResult Create(Question question)
        {
            if (ModelState.IsValid)
            {
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(question);
        }

        // Phương thức hành động để hiển thị form chỉnh sửa câu hỏi
        public ActionResult Edit(int id)
        {
            // Lấy câu hỏi theo id
            var question = db.Questions.Find(id);

            // Lấy danh sách các chủ đề có trạng thái state = true
            var topics = db.Topics
                .Where(t => (bool)t.State)  // Lọc theo trạng thái
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(), // Giá trị để lưu
                    Text = t.Name // Hiển thị trong danh sách
                });

            // Tạo một SelectList từ danh sách chủ đề và chọn giá trị cho câu hỏi hiện tại
            ViewBag.TopicId = new SelectList(topics, "Value", "Text", question.TopicId);

            if (question == null)
            {
                return HttpNotFound();
            }

            return View(question);
        }


        // Phương thức hành động xử lý yêu cầu POST để cập nhật câu hỏi
        [HttpPost]
        public ActionResult Edit(Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(question);
        }

        // Phương thức hành động hiển thị form xóa câu hỏi
        public ActionResult Delete(int id)
        {
            var question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // Phương thức hành động xử lý yêu cầu POST để xóa câu hỏi
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var question = db.Questions.Find(id);
            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}