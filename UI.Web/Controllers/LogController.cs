using Sky.Common.DTO;
using Sky.Common.Provider;
using Sky.Common.Utilities;
using Sky.Log.DTO;
using Sky.Log.IServices;
using Sky.Newspaper.DTO;
using Sky.Newspaper.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using UI.Web.Infrastructure;

namespace UI.Web.Controllers
{
    public class LogController : Controller
    {
        private readonly ILogService _iLogService;
        private readonly INewspaperService _iNewspaperService;

        public LogController(
            ILogService iLogService,
            INewspaperService iNewspaperService)
        {
            this._iLogService = iLogService;
            this._iNewspaperService = iNewspaperService;
        }

        [HttpPost]
        [Route("Log/URLError")]
        public JsonResult URLError(string URL)
        {
            Result result = new Result();

            LogURLError lue = new LogURLError()
            {
                URL = URL,
                WebsiteID = Settings.WebsiteID,
                Description = BrowserUtility.GetDescriptionAsJSON(),
                IPAddress = IPProvider.GetIPAddress(this.HttpContext),
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                result.IsSuccess = this._iLogService.SaveLogURLError(lue);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result.Message = ex.Message;
            }
            
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [Route("Log/Navigation")]
        public JsonResult Navigation(string URL)
        {
            Result result = new Result();

            LogNavigation ln = new LogNavigation()
            {
                URL = URL,
                WebsiteID = Settings.WebsiteID,
                Description = BrowserUtility.GetDescriptionAsJSON(),
                IPAddress = IPProvider.GetIPAddress(this.HttpContext),
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                result.IsSuccess = this._iLogService.SaveLogNavigation(ln);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [Route("Log/IncreaseNews")]
        public JsonResult IncreaseNews(int ID)
        {
            Result result = new Result();

            NewsViews nv = new NewsViews()
            {
                NewsID = ID,
                TKWebsite = Settings.TKWebsite,
                TKPlatform = TK.TKPlatform.Web,
                Description = BrowserUtility.GetDescriptionAsJSON(),
                IPAddress = IPProvider.GetIPAddress(this.HttpContext),
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                result.IsSuccess = this._iNewspaperService.SaveNewsViews(nv);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }
    }
}