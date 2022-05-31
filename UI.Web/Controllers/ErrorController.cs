using Sky.CMS.IServices;
using Sky.News.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.Web.Infrastructure;

namespace UI.Web.Controllers
{
    public class ErrorController : BaseController
    {
        private readonly IRedirectService _iRedirectService;

        public ErrorController(
            IRedirectService iRedirectService,
            INavigationService iNavigationService,
            IAppService iAppService)
            : base(iNavigationService, iAppService)
        {
            this._iRedirectService = iRedirectService;
        }

        [Route("Error/Oops")]
        public ActionResult Oops()
        {
            Response.StatusCode = 404;

            string sourceURL = Request.RawUrl;
            if (string.IsNullOrEmpty(sourceURL))
            {
                return View();
            }

            sourceURL = sourceURL.IndexOf('?') > -1 ? sourceURL.Substring(0, sourceURL.IndexOf('?')) : sourceURL;
            var anyRedirectItem = this._iRedirectService.GetRedirectURLBySourceURLSMALL(sourceURL, Settings.WebsiteID);
            if (anyRedirectItem == null)
            {
                return View();
            }

            return Redirect(string.Format("{0}?redirected=true&source={1}", anyRedirectItem.TargetURL, sourceURL));
        }

        [Route("Error/DeletedPage")]
        public ActionResult DeletedPage()
        {
            Response.StatusCode = 410;
            return View();
        }
    }
}