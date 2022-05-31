using Sky.CMS.IServices;
using Sky.Common.DTO;
using Sky.Common.Provider;
using Sky.Common.Provider.Pagination;
using Sky.Common.Utilities;
using Sky.Generic.DTO;
using Sky.Generic.IServices;
using Sky.Log.DTO;
using Sky.Log.IServices;
using Sky.News.DTO;
using Sky.News.IServices;
using Sky.Newspaper.DTO;
using Sky.Newspaper.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.Web.Infrastructure;
using UI.Web.Models.News;

namespace UI.Web.Controllers
{
    public class NewsController : BaseController
    {
        private readonly INewsService _iNewsService;
        private readonly INewspaperService _iNewspaperService;
        private readonly IPaginationProvider _iPaginationProvider;
        private readonly IGenericService _iGenericService;
        private readonly ILogService _iLogService;
        private readonly NewsHelper _newsHelper;

        public NewsController(
            NewsHelper newsHelper,
            ILogService iLogService,
            IGenericService iGenericService,
            IPaginationProvider iPaginationProvider,
            INewsService iNewsService,
            INewspaperService iNewspaperService,
            INavigationService iNavigationService,
            IAppService iAppService)
            : base(iNavigationService, iAppService)
        {
            this._newsHelper = newsHelper;
            this._iLogService = iLogService;
            this._iPaginationProvider = iPaginationProvider;
            this._iNewsService = iNewsService;
            this._iNewspaperService = iNewspaperService;
            this._iGenericService = iGenericService;
        }

        [Route("Haber/{title}/{id}")]
        public ActionResult Detail(int ID)
        {
            var newsItem = this._iNewsService.GetNewsByIDMEDIUM(ID);
            if (newsItem == null)
            {
                return Redirect("/Error/DeletedPage?source="+Request.Url.AbsolutePath);
            }

            var newsListRelated = this._iNewsService.GetNewsListRelated(ID, newsItem.CategoryID, Settings.NewsLimitForRelated);

            this._newsHelper.Setup(newsItem);
            this._newsHelper.Setup(newsListRelated);

            var model = new NewsModel()
            {
                News = newsItem,
                CommentList = this._iNewspaperService.GetCommentListByNewsIDSMALL(ID),
                TotalViews = this._iNewspaperService.GetNewsViewsRelationTotalViews(ID) + 1
            };

            model.RelatedNewsSlider = newsListRelated.Take(8).ToList();
            newsListRelated = newsListRelated.Where(n => !model.RelatedNewsSlider.Contains(n)).ToList();

            model.RelatedNewsRightSmall = newsListRelated.Take(5).ToList();
            newsListRelated = newsListRelated.Where(n => !model.RelatedNewsRightSmall.Contains(n)).ToList();

            model.RelatedNewsRightBig = newsListRelated.Take(5).ToList();

            return View(model);
        }

        [Route("Haber")]
        [Route("Haber/Son-Dakika")]
        [Route("Haber/Kaynak/{title}/{ChannelID:int}")]
        [Route("Haber/Kategori/{title}/{CategoryID:int}")]
        public ActionResult List(int? CategoryID, int? ChannelID, int? sayfa, string q)
        {
            var model = new NewsListModel();

            sayfa = sayfa.HasValue ? sayfa : 1;
            var limitation = this._iPaginationProvider.OffsetAndFetchCount(sayfa.Value, Settings.ContentCountOnAPage);
            int totalCount = 0;
            List<News> newsList = new List<News>();

            if (!string.IsNullOrEmpty(q))
            {
                if (q.Trim().Length < 5)
                {
                    model.PageTitle = "Arama yapabilmek için en az 5 karakter girmelisiniz.";
                }
                else
                {
                    newsList = this._iNewsService.GetNewsListSEARCH_MEDIUM(q, limitation[0], limitation[1], out totalCount);
                    model.PageTitle = string.Format("\"{0}\" İle İlgili {1} Adet Haber Bulundu", q, totalCount);
                }
                model.IsSearched = true;
                model.BreadCrumbTitle = "Arama Sonuçları";
            }
            else if (CategoryID.HasValue && CategoryID.Value != 0)
            {
                var categoryItem = this._iNewsService.GetCategoryByIDSMALL(CategoryID.Value);
                if (categoryItem == null)
                {
                    throw new Exception();
                }

                model.Category = categoryItem;
                newsList = this._iNewsService.GetNewsListByCategoryIDSMALL(CategoryID.Value, limitation[0], limitation[1], out totalCount);
                model.PageTitle = string.Format("{0} İle İlgili Haberler", categoryItem.Name);
                model.BreadCrumbTitle = categoryItem.Name;
            }
            else if (ChannelID.HasValue && ChannelID.Value != 0)
            {
                var channelItem = this._iNewsService.GetChannelByIDSMALL(ChannelID.Value);
                if (channelItem == null)
                {
                    throw new Exception();
                }

                model.Channel = channelItem;
                newsList = this._iNewsService.GetNewsListByChannelIDSMALL(ChannelID.Value, limitation[0], limitation[1], out totalCount);
                model.PageTitle = string.Format("{0} İle İlgili Haberler", channelItem.Name);
                model.BreadCrumbTitle = channelItem.Name;
            }
            else
            {
                newsList = this._iNewsService.GetNewsListSMALL(limitation[0], limitation[1], out totalCount);
                newsList = newsList.OrderBy(n => n.CategoryID).ToList();

                if (Request.Url.AbsolutePath.Contains("Son-Dakika"))
                {
                    model.PageTitle = "Son Dakika Haberleri";
                    model.BreadCrumbTitle = "Son Dakika Haberleri";
                }
                else
                {
                    model.PageTitle = "Haberler";
                    model.BreadCrumbTitle = "Haberler";
                }
            }

            this._newsHelper.Setup(newsList);

            newsList.Reverse();

            model.RelatedNewsRightBig = newsList.Take(2).ToList();
            newsList = newsList.Where(n => !model.RelatedNewsRightBig.Contains(n)).ToList();

            model.RelatedNewsRightSmall = newsList.Take(5).ToList();
            newsList = newsList.Where(n => !model.RelatedNewsRightSmall.Contains(n)).ToList();

            newsList.Reverse();

            model.LasPaginas = this._iPaginationProvider.Paginate(totalCount, Settings.ContentCountOnAPage, sayfa.Value, 7);
            model.NewsList = newsList;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("News/CommentSaveAjax")]
        public JsonResult CommentSaveAjax(Comment comment, string CaptchaCode)
        {
            Result result = new Result();

            if (string.IsNullOrEmpty(CaptchaCode))
            {
                result.Message = "Güvenlik kodunu girerek yeniden deneyiniz.";
                return Json(result, JsonRequestBehavior.DenyGet);
            }

            result = this.Resolve(CaptchaCode);

            if (!result.IsSuccess)
            {
                return Json(result, JsonRequestBehavior.DenyGet);
            }

            if (string.IsNullOrEmpty(comment.FullName))
            {
                result.Message = "Adınızı ve soyadınızı giriniz.";
                return Json(result, JsonRequestBehavior.DenyGet);
            }

            if (string.IsNullOrEmpty(comment.Email))
            {
                result.Message = "Email adresinizi giriniz.";
                return Json(result, JsonRequestBehavior.DenyGet);
            }

            if (string.IsNullOrEmpty(comment.FullName))
            {
                result.Message = "Yorumunuzu giriniz.";
                return Json(result, JsonRequestBehavior.DenyGet);
            }

            if (comment.NewsID == 0)
            {
                result.Message = "İşleminiz gerçekleştirilemedi. Lütfen tekrar deneyiniz.";
                return Json(result, JsonRequestBehavior.DenyGet);
            }

            comment.Description = BrowserUtility.GetDescriptionAsJSON();
            comment.IPAddress = IPProvider.GetIPAddress(this.HttpContext);
            comment.TKWebsite = Settings.TKWebsite;
            comment.TKPlatform = TK.TKPlatform.Web;
            comment.IsDeleted = false;
            comment.IsVerified = false;
            comment.CreatedAt = DateTime.UtcNow;

            int id;
            if (!this._iNewspaperService.SaveComment(comment, out id))
            {
                result.Message = "Yorumunuz kaydedilemedi. Bir hata oluştu. Lütfen tekrar deneyiniz.";
                return Json(result, JsonRequestBehavior.DenyGet);
            }

            result.IsSuccess = true;
            result.Message = "Yorumunuz başarıyla gönderildi. Yorumunuz onaylandıktan sonra yayınlanacaktır.";

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("News/SubscribeSaveAjax")]
        public JsonResult SubscribeSaveAjax(Subscribe subscribe)
        {
            Result result = new Result();

            if (string.IsNullOrEmpty(subscribe.Email))
            {
                result.Message = "Email adresinizi giriniz.";
                return Json(result, JsonRequestBehavior.DenyGet);
            }

            var anySubscribe = this._iGenericService.GetSubscribeByEmail(subscribe.Email);
            if (anySubscribe != null)
            {
                if (anySubscribe.IsSubscribed)
                {
                    result.Message = "Email adresiniz daha önce kaydedilmiştir.";
                    result.IsSuccess = true;
                    return Json(result, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    bool isSuccess = this._iGenericService.UpdateSubscribeUNSUBSCRIBE(new Subscribe()
                    {
                        ID = anySubscribe.ID,
                        IsSubscribed = true,
                        UpdatedAt = DateTime.UtcNow
                    });

                    if (isSuccess)
                    {
                        result.IsSuccess = true;
                        result.Message = "Email adresiniz başarıyla kaydedildi.";
                        return Json(result, JsonRequestBehavior.DenyGet);
                    }
                    else
                    {
                        result.Message = "Email adresiniz kaydedilemedi. Bir hata oluştu. Lütfen tekrar deneyiniz.";
                        return Json(result, JsonRequestBehavior.DenyGet);
                    }
                }
            }

            subscribe.UnSubscribeKey = Guid.NewGuid().ToString().Replace("-", "");
            subscribe.Description = BrowserUtility.GetDescriptionAsJSON();
            subscribe.WebsiteID = Settings.WebsiteID;
            subscribe.TKPlatform = TK.TKPlatform.Web;
            subscribe.IsSubscribed = true;
            subscribe.IsDeleted = false;
            subscribe.IPAddress = IPProvider.GetIPAddress(this.HttpContext);
            subscribe.CreatedAt = DateTime.UtcNow;

            int id;
            if (!this._iGenericService.SaveSubscribe(subscribe, out id))
            {
                result.Message = "Email adresiniz kaydedilemedi. Bir hata oluştu. Lütfen tekrar deneyiniz.";
                return Json(result, JsonRequestBehavior.DenyGet);
            }

            result.IsSuccess = true;
            result.Message = "Email adresiniz başarıyla kaydedildi.";


            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [Route("News/GetNewsListPrevNextAjax")]
        public JsonResult GetNewsListPrevNextAjax(int ID, int CategoryID)
        {
            Result result = new Result();

            var newsList = this._iNewsService.GetNewsListPrevNext(ID, CategoryID);

            this._newsHelper.Setup(newsList);

            result.IsSuccess = newsList.Any();
            result.Data = new
            {
                Prev = newsList.FirstOrDefault(),
                Next = newsList.LastOrDefault()
            };

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [Route("News/RedirectAjax")]
        public JsonResult RedirectAjax(string surl, string turl, int rid)
        {
            Result result = new Result();

            var newsItem = this._iNewsService.GetNewsByIDSMALL(rid);

            string redirectedURL = null;
            if (newsItem != null)
            {
                redirectedURL = newsItem.Link;
            }

            LogRedirect log = new LogRedirect()
            {
                SourceURL = Server.UrlDecode(surl),
                TargetURL = Server.UrlDecode(turl),
                RedirectedURL = redirectedURL,
                DatabaseID = Constants.Database.News.Value,
                TableID = (int)Constants.Database.News.Table.News,
                RowID = rid,
                TKWebsite = Settings.TKWebsite,
                TKPlatform = TK.TKPlatform.Web,
                IsSuccess = newsItem != null,
                Description = BrowserUtility.GetDescriptionAsJSON(),
                IPAddress = IPProvider.GetIPAddress(this.HttpContext),
                CreatedAt = DateTime.UtcNow
            };

            result.IsSuccess = this._iLogService.SaveLogRedirect(log);

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [Route("News/GetFlashNewsListAjax")]
        public JsonResult GetFlashNewsListAjax()
        {
            Result result = new Result();

            var flashNewsList = this._iNewsService.GetNewsListForEachCategoryID(Settings.NewsCountForeachCategoryIDForFlash);

            result.Data = flashNewsList;
            result.IsSuccess = true;

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        private Result Resolve(string text)
        {
            Result result = new Result();

            if (string.IsNullOrEmpty(text))
            {
                result.Message = "Güvenlik kodunu girerek yeniden deneyiniz.";
                return result;
            }

            if (Session["Captcha"] != null && text == Session["Captcha"].ToString())
            {
                result.Message = "Success";
                result.IsSuccess = true;
            }
            else
            {
                result.Message = "Güvenlik kodunu kontrol ederek yeniden deneyiniz.";
            }

            return result;
        }
    }
}