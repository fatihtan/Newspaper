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
    public class BaseController : Controller
    {
        private readonly IAppService _iAppService;
        private readonly INavigationService _iNavigationService;

        public static readonly List<string> ExcludedPathList = new List<string>()
        {
            "/News/GetNewsListPrevNextAjax",
            "/News/RedirectAjax",
            "/News/SubscribeSaveAjax",
            "/News/CommentSaveAjax"
        };

        public BaseController(
            INavigationService iNavigationService,
            IAppService iAppService)
        {
            this._iNavigationService = iNavigationService;
            this._iAppService = iAppService;
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext = null)
        {
            base.Initialize(requestContext);
            try
            {
                if (ExcludedPathList.Contains(Request.Url.AbsolutePath))
                {
                    return;
                }
                var navigationList = this._iNavigationService.GetNavigationListByWebsiteIDSMALL(Settings.WebsiteID);
                foreach (var item in navigationList)
                {
                    item.ChildrenList = navigationList.Where(n => n.ParentNavigationID == item.ID).ToList();
                }

                ViewBag.NavigationList = navigationList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
