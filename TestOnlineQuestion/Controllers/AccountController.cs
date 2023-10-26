using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TestOnlineQuestion.Models;

namespace TestOnlineQuestion.Controllers
{
    public class AccountController : Controller
    {
        private WebDbContext db = new WebDbContext();
        // GET: Account
        public ActionResult UserIndex()
        {
            return View();
        }
        public ActionResult AdminIndex()
        {
            return View();
        }
        //public ActionResult YourAction()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        if (User.IsInRole("Admin"))
        //        {
        //            ViewBag.Layout = "_AdminLayout";
        //        }
        //        else
        //        {
        //            ViewBag.Layout = "_DefaultLayout";
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.Layout = "_Layout";
        //    }

        //    return View();
        //}

        private void CreateUser(User model)
        {
            using (var db = new WebDbContext()) // Thay thế bằng DbContext của bạn
            {
                // Thực hiện thêm tài khoản mới vào cơ sở dữ liệu
                var user = new User
                {
                    Id = model.Id,
                    HoDem = model.HoDem,
                    Ten = model.Ten,
                    Password = model.Password,
                    Email = model.Email,
                    State = true, // Mặc định tài khoản mới được tạo sẽ có trạng thái là "hoạt động"
                    Role = false // Mặc định, bạn có thể thay đổi theo yêu cầu của hệ thống
                };

                db.Users.Add(user);
                db.SaveChanges();
            }
        }
        private bool IsUserExist(string username, string email)
        {
            using (var db = new WebDbContext()) // Thay thế bằng DbContext của bạn
            {
                // Kiểm tra xem có người dùng với tên đăng nhập hoặc email đã cung cấp không
                return db.Users.Any(u => u.Id == username || u.Email == email);
            }
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User model)
        {
            try
            {
                bool testname = db.Users.Any(s => s.Id == model.Id);
                if (testname)
                {
                    ModelState.AddModelError("", "Tên Đăng Nhập Đã Tồn Tại");
                }
                else
                {
                    // Gọi phương thức CreateUser hoặc thực hiện các thao tác lưu tài khoản vào cơ sở dữ liệu
                    CreateUser(model);
                    return RedirectToAction("UserLogin");
                }
            }catch(Exception) 
            {
                ModelState.AddModelError("", "Không Thể Tạo Tài Khoản");
            }

            return View(); // Trả về trang đăng ký với thông báo lỗi nếu có.
        }


        public ActionResult AccountInfoAdmin()
        {
            // Lấy thông tin tài khoản của người dùng đã đăng nhập
            User user = GetUserLoggedIn(); // Thay thế bằng cách lấy thông tin từ phiên đăng nhập hoặc cơ sở dữ liệu

            return View(user);
        }
        public ActionResult AccountInfoUser()
        {
            // Lấy thông tin tài khoản của người dùng đã đăng nhập
            User user = GetUserLoggedIn(); // Thay thế bằng cách lấy thông tin từ phiên đăng nhập hoặc cơ sở dữ liệu

            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAccount(User updatedUser)
        {
            if (ModelState.IsValid)
            {
                // Thực hiện cập nhật thông tin tài khoản trong cơ sở dữ liệu
                // updatedUser chứa thông tin cập nhật từ form

                // Redirect đến trang thông tin tài khoản sau khi cập nhật thành công
                return RedirectToAction("AccountInfo");
            }

            return View(updatedUser);
        }

        // Hàm lấy thông tin người dùng đã đăng nhập
        private User GetUserLoggedIn()
        {
            // Thay thế bằng cách lấy thông tin từ phiên đăng nhập hoặc cơ sở dữ liệu
            // Ví dụ đơn giản, sử dụng Session để lưu thông tin người dùng đăng nhập
            User user = Session["LoggedInUser"] as User;
            return user;
        }
        public ActionResult Logout()
        {
            // Xóa thông tin đăng nhập hiện tại của người dùng (ví dụ: sử dụng Forms Authentication)
            FormsAuthentication.SignOut();

            // Đăng xuất người dùng khỏi ứng dụng (nếu sử dụng Identity)
            // var authenticationManager = HttpContext.GetOwinContext().Authentication;
            // authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            // Chuyển hướng người dùng về trang đăng nhập
            return RedirectToAction("UserLogin", "Account");
        }
        public ActionResult ManageUsers()
        {
            var users = db.Users.ToList(); // Lấy danh sách tài khoản người dùng từ cơ sở dữ liệu
            return View(users);
        }

        // Phương thức để kích hoạt hoặc vô hiệu hóa tài khoản
        public ActionResult ToggleUserState(string userId)
        {
            var user = db.Users.Find(userId);
            if (user != null)
            {
                user.State = !user.State; // Đảo ngược trạng thái (kích hoạt/vô hiệu hóa)
                db.SaveChanges();
            }
            return RedirectToAction("ManageUsers");
        }
        [HttpGet]
        public ActionResult UserLogin()
        {
            return View(); // Trả về trang đăng nhập
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserLogin(User model)
        {
            if (IsValidUser(model.Id, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.Id, false);

                // Kiểm tra xem tài khoản đang đăng nhập có vai trò user hay không
                bool isUser = model.Role.GetValueOrDefault(); // Ngược lại với giá trị bool của Role

                if (isUser==false)
                {
                    // Đặt vai trò "User" cho tài khoản
                    var authTicket = new FormsAuthenticationTicket(
                        1,
                        model.Id,
                        DateTime.Now,
                        DateTime.Now.Add(FormsAuthentication.Timeout),
                        false, // persistent cookie
                        "User" // user data (role)
                    );
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    HttpContext.Response.Cookies.Add(authCookie);
                    return RedirectToAction("UserIndex", "Account");
                }
                else
                {
                    // Xóa bất kỳ cookie "User" nếu có
                    var authCookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                    if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
                    {
                        authCookie.Expires = DateTime.Now.AddYears(-1);
                        HttpContext.Response.Cookies.Add(authCookie);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không hợp lệ.");
            }

            return View();
        }

        [HttpGet]
        public ActionResult AdLogin()
        {
            return View(); // Trả về trang đăng nhập
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Adlogin(User u)
        {
            try
            {
                User ad = db.Users.FirstOrDefault(x => x.Id == u.Id && x.Password == u.Password && x.Role == true);

                if (ad != null)
                {
                    Session["AdminId"] = ad.Id.ToString();
                    return RedirectToAction("AdminIndex","Account");
                }
                else
                {
                    ModelState.AddModelError("", "Tên Đăng Nhập Hoặc Mật Khẩu Không Đúng");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Lỗi Không Thể Đăng Nhập");
            }
            return View();

        }



        private bool IsValidUser(string username, string password)
        {
            using (var db = new WebDbContext()) // Thay thế bằng DbContext thực tế của bạn
            {
                // Kiểm tra xem có người dùng với tên đăng nhập hoặc email đã cung cấp không
                var user = db.Users.SingleOrDefault(u => u.Id == username || u.Email == username);

                if (user != null)
                {
                    // Kiểm tra xem tài khoản của người dùng có đang hoạt động (State = true) không
                    if (user.State == true)
                    {
                        // Bạn nên thực hiện mã hóa mật khẩu và kiểm tra mật khẩu ở đây
                        // Để đơn giản, chúng tôi sẽ so sánh mật khẩu văn bản thuần (không khuyến nghị cho môi trường sản xuất)

                        if (user.Password == password)
                        {
                            // Người dùng hợp lệ, xác thực thành công
                            return true;
                        }
                    }
                }
            }

            // Người dùng không hợp lệ, mật khẩu không đúng hoặc tài khoản không hoạt động
            return false;
        }

    }
}