using Sky.CMS.IServices;
using Sky.Common.DTO;
using Sky.Common.Provider;
using Sky.Common.Utilities;
using Sky.Generic.DTO;
using Sky.Generic.IServices;
using Sky.News.DTO;
using Sky.News.IServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using UI.Web.Infrastructure;
using UI.Web.Models.App;
using UI.Web.Models.News;

namespace UI.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly INewsService _iNewsService;
        private readonly IGenericService _iGenericService;
        private readonly IAppService _iAppService;
        private readonly NewsHelper _newsHelper;

        public HomeController(
            NewsHelper newsHelper,
            IGenericService iGenericService,
            INewsService iNewsService,
            INavigationService iNavigationService,
            IAppService iAppService)
            : base(iNavigationService, iAppService)
        {
            this._newsHelper = newsHelper;
            this._iAppService = iAppService;
            this._iGenericService = iGenericService;
            this._iNewsService = iNewsService;
        }

        [HttpGet]
        [Route("")]
        [Route("Anasayfa")]
        public ActionResult Index()
        {
            var model = new HomeModel();

            var newsList = this._iNewsService.GetNewsListForEachCategoryID(Settings.NewsCountForeachCategoryIDForHome);
            var categoryList = this._iNewsService.GetCategoryListMETADATA();
            var channelList = this._iNewsService.GetChannelListMETADATA();

            this._newsHelper.Setup(newsList);

            #region Headlines
            var headLineList = new List<News>();
            headLineList.AddRange(newsList.Where(n => n.CategoryID == Category.TURKIYE).Take(3).ToList());
            headLineList.AddRange(newsList.Where(n => n.CategoryID == Category.DUNYA).Take(3).ToList());
            headLineList.AddRange(newsList.Where(n => n.CategoryID == Category.GUNDEM).Take(3).ToList());
            headLineList.AddRange(newsList.Where(n => n.CategoryID == Category.POLITIKA).Take(2).ToList());
            headLineList.AddRange(newsList.Where(n => n.CategoryID == Category.EKONOMI_FINANS).Take(1).ToList());
            headLineList.AddRange(newsList.Where(n => n.CategoryID == Category.SAGLIK).Take(1).ToList());
            headLineList.AddRange(newsList.Where(n => n.CategoryID == Category.SPOR).Take(1).ToList());
            headLineList.AddRange(newsList.Where(n => n.CategoryID == Category.MAGAZIN).Take(1).ToList());

            //headLineList = headLineList.OrderBy(n => Category.CategoryIDList.IndexOf(n.CategoryID)).ToList();

            newsList = newsList.Where(n => !headLineList.Contains(n)).ToList();
            #endregion

            #region Headline Triple
            var headlineTriple = newsList.Where(n => n.CategoryID == Category.TURKIYE).Take(3).ToList();

            newsList = newsList.Where(n => !headlineTriple.Contains(n)).ToList();
            #endregion

            #region Flash
            var flashList = new List<News>();
            flashList.AddRange(newsList.Where(n => n.CategoryID == Category.TURKIYE).Take(3).ToList());
            flashList.AddRange(newsList.Where(n => n.CategoryID == Category.DUNYA).Take(3).ToList());
            flashList.AddRange(newsList.Where(n => n.CategoryID == Category.GUNDEM).Take(3).ToList());
            flashList.AddRange(newsList.Where(n => n.CategoryID == Category.POLITIKA).Take(3).ToList());

            //flashList = flashList.OrderBy(n => Category.CategoryIDList.IndexOf(n.CategoryID)).ToList();

            newsList = newsList.Where(n => !flashList.Contains(n)).ToList();
            #endregion

            #region TabList
            var tabListForNews = new Dictionary<Category, List<News>>();
            var newsListForTabs = new List<News>();
            newsListForTabs.AddRange(newsList.Where(n => n.CategoryID == Category.TURKIYE).Take(4).ToList());
            newsListForTabs.AddRange(newsList.Where(n => n.CategoryID == Category.DUNYA).Take(4).ToList());
            newsListForTabs.AddRange(newsList.Where(n => n.CategoryID == Category.GUNDEM).Take(4).ToList());
            newsListForTabs.AddRange(newsList.Where(n => n.CategoryID == Category.EKONOMI_FINANS).Take(4).ToList());
            newsListForTabs.AddRange(newsList.Where(n => n.CategoryID == Category.POLITIKA).Take(4).ToList());

            tabListForNews.Add(categoryList.Where(c => c.ID == Category.TURKIYE).FirstOrDefault(), newsList.Where(n => n.CategoryID == Category.TURKIYE).Take(4).ToList());
            tabListForNews.Add(categoryList.Where(c => c.ID == Category.DUNYA).FirstOrDefault(), newsList.Where(n => n.CategoryID == Category.DUNYA).Take(4).ToList());
            tabListForNews.Add(categoryList.Where(c => c.ID == Category.GUNDEM).FirstOrDefault(), newsList.Where(n => n.CategoryID == Category.GUNDEM).Take(4).ToList());
            tabListForNews.Add(categoryList.Where(c => c.ID == Category.EKONOMI_FINANS).FirstOrDefault(), newsList.Where(n => n.CategoryID == Category.EKONOMI_FINANS).Take(4).ToList());
            tabListForNews.Add(categoryList.Where(c => c.ID == Category.POLITIKA).FirstOrDefault(), newsList.Where(n => n.CategoryID == Category.POLITIKA).Take(4).ToList());

            newsList = newsList.Where(n => !newsListForTabs.Contains(n)).ToList();
            #endregion

            #region Latest Right Small News
            var latestRightSmallNews = new List<News>();
            latestRightSmallNews.AddRange(newsList.Where(n => n.CategoryID == Category.TURKIYE).Take(2).ToList());
            latestRightSmallNews.AddRange(newsList.Where(n => n.CategoryID == Category.DUNYA).Take(1).ToList());
            latestRightSmallNews.AddRange(newsList.Where(n => n.CategoryID == Category.GUNDEM).Take(1).ToList());
            latestRightSmallNews.AddRange(newsList.Where(n => n.CategoryID == Category.POLITIKA).Take(1).ToList());
            latestRightSmallNews.AddRange(newsList.Where(n => n.CategoryID == Category.EKONOMI_FINANS).Take(1).ToList());

            newsList = newsList.Where(n => !latestRightSmallNews.Contains(n)).ToList();
            #endregion

            #region Slider News
            var sliderNews = new List<News>();
            sliderNews.AddRange(newsList.Where(n => n.CategoryID == Category.TURKIYE).Take(2).ToList());
            sliderNews.AddRange(newsList.Where(n => n.CategoryID == Category.DUNYA).Take(2).ToList());
            sliderNews.AddRange(newsList.Where(n => n.CategoryID == Category.GUNDEM).Take(2).ToList());
            sliderNews.AddRange(newsList.Where(n => n.CategoryID == Category.POLITIKA).Take(1).ToList());
            sliderNews.AddRange(newsList.Where(n => n.CategoryID == Category.EKONOMI_FINANS).Take(1).ToList());
            sliderNews.AddRange(newsList.Where(n => n.CategoryID == Category.TEKNOLOJI).Take(2).ToList());
            sliderNews.AddRange(newsList.Where(n => n.CategoryID == Category.SAGLIK).Take(1).ToList());
            sliderNews.AddRange(newsList.Where(n => n.CategoryID == Category.SPOR).Take(1).ToList());
            sliderNews.AddRange(newsList.Where(n => n.CategoryID == Category.MAGAZIN).Take(1).ToList());

            newsList = newsList.Where(n => !sliderNews.Contains(n)).ToList();
            #endregion

            #region 5 Li Kategoriler
            var teknolojiNewsList = newsList.Where(n => n.CategoryID == Category.TEKNOLOJI).Take(5).ToList();
            var seyahatNewsList = newsList.Where(n => n.CategoryID == Category.SEYAHAT).Take(5).ToList();

            var otomobilNewsList = newsList.Where(n => n.CategoryID == Category.OTOMOBIL).Take(5).ToList();
            var saglikNewsList = newsList.Where(n => n.CategoryID == Category.SAGLIK).Take(5).ToList();

            var egitimNewsList = newsList.Where(n => n.CategoryID == Category.EGITIM).Take(5).ToList();
            var kulturSanatNewsList = newsList.Where(n => n.CategoryID == Category.KULTUR_SANAT).Take(5).ToList();

            var magazinNewsList = newsList.Where(n => n.CategoryID == Category.MAGAZIN).Take(5).ToList();
            var yasamNewsList = newsList.Where(n => n.CategoryID == Category.YASAM).Take(5).ToList();
            var ekonomiVeFinansNewsList = newsList.Where(n => n.CategoryID == Category.EKONOMI_FINANS).Take(4).ToList();
            var sporNewsList = newsList.Where(n => n.CategoryID == Category.SPOR).Take(6).ToList();

            newsList = newsList.Where(n => !teknolojiNewsList.Contains(n)).ToList();
            newsList = newsList.Where(n => !seyahatNewsList.Contains(n)).ToList();
            newsList = newsList.Where(n => !otomobilNewsList.Contains(n)).ToList();
            newsList = newsList.Where(n => !saglikNewsList.Contains(n)).ToList();
            newsList = newsList.Where(n => !egitimNewsList.Contains(n)).ToList();
            newsList = newsList.Where(n => !kulturSanatNewsList.Contains(n)).ToList();
            newsList = newsList.Where(n => !magazinNewsList.Contains(n)).ToList();
            newsList = newsList.Where(n => !yasamNewsList.Contains(n)).ToList();
            newsList = newsList.Where(n => !ekonomiVeFinansNewsList.Contains(n)).ToList();
            newsList = newsList.Where(n => !sporNewsList.Contains(n)).ToList();
            #endregion

            #region Slider News 2
            sliderNews.AddRange(newsList.Where(n => n.CategoryID == Category.TURKIYE).Take(3).ToList());
            sliderNews.AddRange(newsList.Where(n => n.CategoryID == Category.DUNYA).Take(3).ToList());
            sliderNews.AddRange(newsList.Where(n => n.CategoryID == Category.GUNDEM).Take(3).ToList());
            sliderNews.AddRange(newsList.Where(n => n.CategoryID == Category.POLITIKA).Take(3).ToList());

            newsList = newsList.Where(n => !sliderNews.Contains(n)).ToList();
            #endregion

            model.TabList = tabListForNews;
            model.HeadlineList = headLineList;
            model.HeadlineTriple = headlineTriple;
            model.FlashList = flashList;
            model.LatestRightSmallList = latestRightSmallNews;
            model.SliderList = sliderNews;

            model.TeknolojiNewsList = teknolojiNewsList;
            model.SeyahatNewsList = seyahatNewsList;
            model.OtomobilNewsList = otomobilNewsList;
            model.SaglikNewsList = saglikNewsList;

            model.EkonomiFinansNewsList = ekonomiVeFinansNewsList;

            model.EgitimNewsList = egitimNewsList;
            model.KulturSanatNewsList = kulturSanatNewsList;
            model.MagazinNewsList = magazinNewsList;
            model.YasamNewsList = yasamNewsList;
            model.SporNewsList = sporNewsList;

            model.CategoryList = categoryList;
            model.ChannelList = channelList;

            return View(model);
        }

        [HttpGet]
        public ActionResult AppPage(int? ID, string title)
        {
            Sky.CMS.DTO.AppPage appPageItem = null;

            if (ID.HasValue)
            {
                appPageItem = this._iAppService.GetAppPageByIDSMALL(ID.Value);
            }
            else if (!string.IsNullOrEmpty(Request.Url.AbsolutePath))
            {
                appPageItem = this._iAppService.GetAppPageByURLSMALL(Request.Url.AbsolutePath, Settings.WebsiteID);
            }

            if (appPageItem == null || appPageItem.WebsiteID != Settings.WebsiteID)
            {
                throw new Exception();
            }

            var model = new AppPageModel()
            {
                AppPage = appPageItem
            };

            return View(model);
        }

        [Route("Iletisim")]
        public ActionResult Contact()
        {
            var keysList = AppContentProvider.Keys["Contact"];

            var appContentList = this._iAppService.GetAppContentListByACKeys(keysList.ToArray(), Settings.WebsiteID);

            var model = new AppContentListModel()
            {
                AppContentList = appContentList
            };

            return View(model);
        }

        [HttpPost]
        [Route("Home/ContactMessageSaveAjax")]
        public JsonResult ContactMessageSaveAjax(ContactMessage contactMessage, string CaptchaCode)
        {
            Result result = new Result(false, "Bir hata oluştu.");

            if (string.IsNullOrEmpty(CaptchaCode))
            {
                result.Message = "Güvenlik kodunu girerek yeniden deneyiniz.";
                return Json(result, JsonRequestBehavior.DenyGet);
            }

            if (Request.Form["Terms"] == null || !Request.Form["Terms"].Equals(ContactMessage.TERMS_CHECKED_VALUE))
            {
                result.Message = "Aydınlatma metinini onaylamanız gerekiyor.";
                return Json(result, JsonRequestBehavior.DenyGet);
            }

            result = this.Resolve(CaptchaCode);

            if (!result.IsSuccess)
            {
                return Json(result, JsonRequestBehavior.DenyGet);
            }

            contactMessage.Description = BrowserUtility.GetDescriptionAsJSON();
            contactMessage.WebsiteID = Settings.WebsiteID;
            contactMessage.TKPlatform = TK.TKPlatform.Web;
            contactMessage.TKLanguage = TK.TKLanguage.Turkish;
            contactMessage.IsShown = true;
            contactMessage.IsArchived = false;
            contactMessage.IsRead = false;
            contactMessage.IsDeleted = false;
            contactMessage.IPAddress = IPProvider.GetIPAddress(this.HttpContext);
            contactMessage.CreatedAt = DateTime.UtcNow;

            int id;
            if (!this._iGenericService.SaveContactMessage(contactMessage, out id))
            {
                result.Message = "Mesajınız gönderilemedi bir hata oluştu. Lütfen tekrar deneyiniz.";
                return Json(result, JsonRequestBehavior.DenyGet);
            }

            result.Message = "Mesajınız başarıyla gönderildi. En kısa zamanda size geri dönüş yapacağız.";
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Home/SubscribeSaveAjax")]
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

        [Route("sitemap.xml")]
        public ActionResult Sitemap()
        {
            var siteURL = "https://www.bihaberver.com";
            string dateFormat = "yyyy-MM-ddTHH:mm:ss+03:00";

            var categoryList = this._iNewsService.GetCategoryListMETADATA();
            var defaultAppPageList = Settings.DefaultAppPageList(categoryList);
            var appPageTotalCount = this._iAppService.GetAppPageTotalCount(Settings.WebsiteID);
            var newsTotalCount = this._iNewsService.GetNewsCountSMALL();

            DateTime utc = DateTime.UtcNow;
            DateTime today = new DateTime(utc.Year, utc.Month, utc.Day, 9, 0, 0);
            DateTime yesterday = today.AddDays(-1);

            int totalContentCount =
                appPageTotalCount +
                newsTotalCount +
                defaultAppPageList.Count;

            Response.Clear();
            Response.ContentType = "text/xml";
            XmlTextWriter xmlWriter = new XmlTextWriter(Response.OutputStream, Encoding.UTF8);
            xmlWriter.WriteStartDocument();

            //  sitemapindex
            xmlWriter.WriteStartElement("sitemapindex");
            xmlWriter.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
            xmlWriter.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            xmlWriter.WriteAttributeString("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/siteindex.xsd");

            int totalSitemap = totalContentCount / Settings.SITEMAP_LIMIT;
            if(totalContentCount % Settings.SITEMAP_LIMIT > 0)
            {
                totalSitemap++;
            }

            int halfOfSitemap = totalSitemap / 2;

            for (int i = 1; i <= totalSitemap; i++)
			{
                xmlWriter.WriteStartElement("sitemap");
                xmlWriter.WriteElementString("loc", string.Format("{0}/sitemaps/sitemap/{1}", siteURL, i));

                if (i < halfOfSitemap)
                {
                    xmlWriter.WriteElementString("lastmod", today.ToString(dateFormat));
                }
                else
                {
                    xmlWriter.WriteElementString("lastmod", yesterday.ToString(dateFormat));
                }
                
                xmlWriter.WriteEndElement();
			}

            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            xmlWriter.Close();
            Response.End();

            return View();
        }

        [Route("sitemaps/sitemap/{id}")]
        public ActionResult SitemapURL(int id)
        {
            var siteURL = "https://www.bihaberver.com";
            string dateFormat = "yyyy-MM-ddTHH:mm:ss+03:00";

            int offset = (id - 1) * Settings.SITEMAP_LIMIT;

            var newsList = this._iNewsService.GetNewsListSITEMAP_SMALL(offset, Settings.SITEMAP_LIMIT);

            DateTime utc = DateTime.UtcNow;
            DateTime today = new DateTime(utc.Year, utc.Month, utc.Day, 9, 0, 0);


            Response.Clear();
            Response.ContentType = "text/xml";
            XmlTextWriter xmlWriter = new XmlTextWriter(Response.OutputStream, Encoding.UTF8);
            xmlWriter.WriteStartDocument();

            //  sitemapindex
            xmlWriter.WriteStartElement("urlset");
            xmlWriter.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
            xmlWriter.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            xmlWriter.WriteAttributeString("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/siteindex.xsd");

            double priorityNumeric = 1 - ((double)(id) / 10);

            if (priorityNumeric < 0.4)
                priorityNumeric = 0.4;
            
            string priority = priorityNumeric.ToString("0.0", new CultureInfo("en-US"));

            if (id == 1)
            {
                var categoryList = this._iNewsService.GetCategoryListMETADATA();
                var defaultAppPageList = Settings.DefaultAppPageList(categoryList);
                var appPageList = this._iAppService.GetAppPageListByWebsiteIDSMALL(Settings.WebsiteID);

                foreach (var item in defaultAppPageList)
                {
                    xmlWriter.WriteStartElement("url");
                    xmlWriter.WriteElementString("loc", string.Format("{0}{1}", siteURL, item.URL));
                    xmlWriter.WriteElementString("lastmod", today.ToString(dateFormat));
                    xmlWriter.WriteElementString("priority", priority);
                    xmlWriter.WriteEndElement();
                }

                foreach (var item in appPageList)
                {
                    xmlWriter.WriteStartElement("url");
                    xmlWriter.WriteElementString("loc", string.Format("{0}{1}", siteURL, item.URL));
                    xmlWriter.WriteElementString("lastmod", today.ToString(dateFormat));
                    xmlWriter.WriteElementString("priority", priority);
                    xmlWriter.WriteEndElement();
                }
            }

            foreach (var item in newsList)
            {
                xmlWriter.WriteStartElement("url");
                xmlWriter.WriteElementString("loc", string.Format("{0}/Haber/{1}", siteURL, SeoURLUtility.Convert(item.ID, item.Title)));
                xmlWriter.WriteElementString("lastmod", item.CreatedAt.ToString(dateFormat));
                xmlWriter.WriteElementString("priority", priority);
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            xmlWriter.Close();
            Response.End();

            return View();
        }

        #region Old Sitemap
        //[Route("sitemap.xml")]
        //public ActionResult Sitemap()
        //{
        //    var siteURL = Request.Url.GetLeftPart(UriPartial.Authority);
        //    string dateFormat = "yyyy-MM-ddTHH:mm:ss+03:00";

        //    var appPageList = this._iAppService.GetAppPageListByWebsiteIDSMALL(Settings.WebsiteID);

        //    int limit = 50000 - (appPageList.Count + 2);

        //    var newsList = this._iNewsService.GetNewsListLIMIT(limit);
        //    var categoryList = this._iNewsService.GetCategoryListMETADATA();

        //    var firstNewsItem = newsList.FirstOrDefault() ?? new News();
        //    var lastNewsItem = newsList.LastOrDefault() ?? new News();

        //    Response.Clear();
        //    Response.ContentType = "text/xml";
        //    XmlTextWriter xmlWriter = new XmlTextWriter(Response.OutputStream, Encoding.UTF8);
        //    xmlWriter.WriteStartDocument();

        //    //  urlset
        //    xmlWriter.WriteStartElement("urlset");
        //    xmlWriter.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
        //    xmlWriter.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
        //    xmlWriter.WriteAttributeString("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");

        //    //  first url -> BaseURL
        //    xmlWriter.WriteStartElement("url");
        //    xmlWriter.WriteElementString("loc", string.Format("{0}/", siteURL));
        //    xmlWriter.WriteElementString("lastmod", firstNewsItem.CreatedAt.ToString(dateFormat));
        //    xmlWriter.WriteElementString("priority", "1.0");
        //    xmlWriter.WriteEndElement();

        //    //  second url ->  /Haber/Son-Dakika
        //    xmlWriter.WriteStartElement("url");
        //    xmlWriter.WriteElementString("loc", string.Format("{0}/Haber/Son-Dakika", siteURL));
        //    xmlWriter.WriteElementString("lastmod", firstNewsItem.CreatedAt.ToString(dateFormat));
        //    xmlWriter.WriteElementString("priority", "0.9");
        //    xmlWriter.WriteEndElement();

        //    //  all categories
        //    foreach (Category item in categoryList)
        //    {
        //        xmlWriter.WriteStartElement("url");
        //        xmlWriter.WriteElementString("loc", string.Format("{0}/Haber/Kategori/{1}", siteURL, SeoURLUtility.Convert(item.ID, item.Name)));
        //        xmlWriter.WriteElementString("lastmod", firstNewsItem.CreatedAt.ToString(dateFormat));
        //        xmlWriter.WriteElementString("priority", "0.9");
        //        xmlWriter.WriteEndElement();
        //    }

        //    //  news list
        //    string priority = null;
        //    for (int i = 0; i < newsList.Count; i++)
        //    {
        //        double percentage = (i * 100) / newsList.Count;

        //        if (percentage <= 5)
        //        {
        //            priority = "0.9";
        //        }
        //        else if (percentage <= 15)
        //        {
        //            priority = "0.8";
        //        }
        //        else if (percentage <= 30)
        //        {
        //            priority = "0.7";
        //        }
        //        else if (percentage <= 50)
        //        {
        //            priority = "0.6";
        //        }
        //        else if (percentage <= 75)
        //        {
        //            priority = "0.5";
        //        }
        //        else if (percentage <= 90)
        //        {
        //            priority = "0.4";
        //        }
        //        else
        //        {
        //            priority = "0.3";
        //        }

        //        xmlWriter.WriteStartElement("url");
        //        xmlWriter.WriteElementString("loc", string.Format("{0}/Haber/{1}", siteURL, SeoURLUtility.Convert(newsList[i].ID, newsList[i].Title)));
        //        xmlWriter.WriteElementString("lastmod", newsList[i].CreatedAt.ToString(dateFormat));
        //        xmlWriter.WriteElementString("priority", priority);
        //        xmlWriter.WriteEndElement();
        //    }

        //    foreach (var item in appPageList)
        //    {
        //        xmlWriter.WriteStartElement("url");
        //        xmlWriter.WriteElementString("loc", string.Format("{0}{1}", siteURL, item.URL));
        //        xmlWriter.WriteElementString("lastmod", lastNewsItem.CreatedAt.ToString(dateFormat));
        //        xmlWriter.WriteElementString("priority", "0.7");
        //        xmlWriter.WriteEndElement();
        //    }

        //    xmlWriter.WriteEndDocument();
        //    xmlWriter.Flush();
        //    xmlWriter.Close();
        //    Response.End();

        //    return View();
        //}
        #endregion
    }
}