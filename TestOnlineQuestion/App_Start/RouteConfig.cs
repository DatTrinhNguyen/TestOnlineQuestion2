using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TestOnlineQuestion
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
           
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Đăng ký route cho trang đăng nhập
            routes.MapRoute(
                name: "Login",
                url: "Account/Login",
                defaults: new { controller = "Account", action = "Login" }
            );

            // Đăng ký route cho trang đăng ký
            routes.MapRoute(
                name: "Register",
                url: "Account/Register",
                defaults: new { controller = "Account", action = "Register" }
            );
            routes.MapRoute(
                name: "YourAction",
                url: "Account/Register",
                defaults: new { controller = "Account", action = "YourAction" }
            );
            // Đăng ký route cho trang thông tin tài khoản
            routes.MapRoute(
                name: "AccountInfo",
                url: "Account/AccountInfo",
                defaults: new { controller = "Account", action = "AccountInfo" }
            );

            // Đăng ký route cho trang cập nhật tài khoản
            routes.MapRoute(
                name: "UpdateAccount",
                url: "Account/UpdateAccount",
                defaults: new { controller = "Account", action = "UpdateAccount" }
            );

            // Đăng ký route cho trang quản lý người dùng
            routes.MapRoute(
                name: "ManageUsers",
                url: "Account/ManageUsers",
                defaults: new { controller = "Account", action = "ManageUsers" }
            );

            // Đăng ký route cho trang đăng xuất
            routes.MapRoute(
                name: "Logout",
                url: "Account/Logout",
                defaults: new { controller = "Account", action = "Logout" }
            );
            
            routes.MapRoute(
                name: "TopicIndex",
                url: "Topic",
                defaults: new { controller = "Topic", action = "Index" }
            );

            routes.MapRoute(
                name: "TopicCreate",
                url: "Topic/Create",
                defaults: new { controller = "Topic", action = "Create" }
            );

            routes.MapRoute(
                name: "TopicEdit",
                url: "Topic/Edit/{id}",
                defaults: new { controller = "Topic", action = "Edit" }
            );

            routes.MapRoute(
                name: "TopicDelete",
                url: "Topic/Delete/{id}",
                defaults: new { controller = "Topic", action = "Delete" }
            );

            routes.MapRoute(
                name: "QuestionIndex",
                url: "Question/Index",
                defaults: new { controller = "Question", action = "Index" }
            );

            routes.MapRoute(
                name: "QuestionCreate",
                url: "Question/Create",
                defaults: new { controller = "Question", action = "Create" }
            );

            routes.MapRoute(
                name: "QuestionEdit",
                url: "Question/Edit/{id}",
                defaults: new { controller = "Question", action = "Edit" }
            );

            routes.MapRoute(
                name: "QuestionDelete",
                url: "Question/Delete/{id}",
                defaults: new { controller = "Question", action = "Delete" }
            );

            routes.MapRoute(
                name: "ManageContestIndex",
                url: "ManageContest/Index",
                defaults: new { controller = "ManageContest", action = "Index" }
            );

            routes.MapRoute(
                name: "ManageContestCreate",
                url: "ManageContest/Create",
                defaults: new { controller = "ManageContest", action = "Create" }
            );

            routes.MapRoute(
                name: "ManageContestEdit",
                url: "ManageContest/Edit/{id}",
                defaults: new { controller = "ManageContest", action = "Edit" }
            );

            routes.MapRoute(
                name: "ManageContestDelete",
                url: "ManageContest/Delete/{id}",
                defaults: new { controller = "ManageContest", action = "Delete" }
            );
            routes.MapRoute(
                name: "AddQuestionsToContest",
                url: "ManageContest/AddQuestionsToContest/{contestId}", // Đặt tên tham số là contestId
                defaults: new { controller = "ManageContest", action = "AddQuestionsToContest" }
            );

            routes.MapRoute(
                name: "ManageContestQuestions",
                url: "ManageContest/ManageContestQuestions/{Id}",
                defaults: new { controller = "ManageContest", action = "ManageContestQuestions" }
            );
            routes.MapRoute(
                name: "UserIndex",
                url: "ManageContest/UserIndex/{Id}",
                defaults: new { controller = "ManageContest", action = "UserIndex" }
            );
            routes.MapRoute(
                name: "ContestQuestions",
                url: "ManageContest/ContestQuestions/{Id}",
                defaults: new { controller = "ManageContest", action = "ContestQuestions" }
            );
            routes.MapRoute(
                name: "SubmitContestAnswers",
                url: "ManageContest/SubmitContestAnswers/{Id}",
                defaults: new { controller = "ManageContest", action = "SubmitContestAnswers" }
            );
            routes.MapRoute(
                name: "CalculateCorrectAnswers",
                url: "ManageContest/CalculateCorrectAnswers/{Id}",
                defaults: new { controller = "ManageContest", action = "CalculateCorrectAnswers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
