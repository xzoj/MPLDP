using LandingPage.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace LandingPage.Controllers
{
    public class LandingPagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: LandingPage
        
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Index([Bind(Include = "Id,Name,Email,Phone,Message")] Information info)
        {
            info.Status = Information.EnumStatus.New;
            info.CreateAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Information.Add(info);
                db.SaveChanges();

                TempData["message"] = "Success";
                ModelState.Clear();

                return PartialView("_ModalContent");
            }
            

            return PartialView("_ModalContent", info);
        }
        [Throttle(TimeUnit = TimeUnit.Seconds, Count = 1)]
        [Throttle(TimeUnit = TimeUnit.Minute, Count = 10)]
        [Throttle(TimeUnit = TimeUnit.Hour, Count = 20)]
        [Throttle(TimeUnit = TimeUnit.Day, Count = 100)]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage([Bind(Include = "Id,Name,Email,Phone,Message")]Information info)
        {
            info.Status = Information.EnumStatus.New;
            info.CreateAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Information.Add(info);
                db.SaveChanges();
                return Json("Chúng tôi đã nhận được thông tin của bạn. Nhân viên sẽ liên lạc lại với bạn trong thời gian sớm nhất. Xin cảm ơn!");
            }

            return Json("Vui lòng kiểm tra lại các thông tin trước khi gửi!");
        }
        
        public enum TimeUnit
        {
            Seconds = 1,
            Minute = 60,
            Hour = 3600,
            Day = 86400
        }

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
        public class ThrottleAttribute : ActionFilterAttribute
        {
            public TimeUnit TimeUnit { get; set; }
            public int Count { get; set; }

            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var seconds = Convert.ToInt32(TimeUnit);

                var key = string.Join(
                    "-",
                    seconds,
                    filterContext.HttpContext.Request.HttpMethod,
                    filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                    filterContext.ActionDescriptor.ActionName,
                    filterContext.HttpContext.Request.UserHostAddress
                );

                // increment the cache value
                var cnt = 1;
                if (HttpRuntime.Cache[key] != null)
                {
                    cnt = (int)HttpRuntime.Cache[key] + 1;
                }
                HttpRuntime.Cache.Insert(
                    key,
                    cnt,
                    null,
                    DateTime.UtcNow.AddSeconds(seconds),
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.Low,
                    null
                );

                if (cnt > Count)
                {
                    filterContext.Result = new ContentResult
                    {
                        Content = "You are allowed to make only " + Count + " requests per " + TimeUnit.ToString().ToLower()
                    };
                    filterContext.HttpContext.Response.StatusCode = 429;
                }
            }
        }
    }
}