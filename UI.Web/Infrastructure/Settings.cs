using Sky.Common.DTO;
using Sky.News.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace UI.Web.Infrastructure
{
    public class Settings
    {
        public static readonly int WebsiteID = (int)TK.TKWebsite.ProductBiHaberVer;

        public static readonly TK.TKWebsite TKWebsite = TK.TKWebsite.ProductBiHaberVer;

        public static readonly int NewsCountForeachCategoryIDForHome = 30;

        public static readonly int NewsCountForeachCategoryIDForFlash = 3;

        public static readonly int NewsLimitForRelated = 15;

        public static readonly CultureInfo TurkishCultureInfo = new CultureInfo("tr-TR");

        public static int ContentCountOnAPage = 57;

        public static int SITEMAP_LIMIT = 40000;

        private static List<Sky.CMS.DTO.AppPage> ConstantAppPageList = new List<Sky.CMS.DTO.AppPage>()
        {
            new Sky.CMS.DTO.AppPage(){
                URL = "/"
            },
            new Sky.CMS.DTO.AppPage(){
                URL = "/Haber/Son-Dakika"
            },
            new Sky.CMS.DTO.AppPage(){
                URL = "/Iletisim"
            }
        };

        public static List<Sky.CMS.DTO.AppPage> DefaultAppPageList(List<Sky.News.DTO.Category> CategoryList)
        {
            List<Sky.CMS.DTO.AppPage> list = new List<Sky.CMS.DTO.AppPage>();

            list.AddRange(ConstantAppPageList);

            foreach (var item in CategoryList)
            {
                list.Add(new Sky.CMS.DTO.AppPage() {
                    URL = string.Format("/Haber/Kategori/{0}", Sky.Common.Utilities.SeoURLUtility.Convert(item.ID, item.Name))
                });
            }

            return list;
        }
    }
}