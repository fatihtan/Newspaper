using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Web.Infrastructure
{
    public class AppContentProvider
    {
        public static Dictionary<string, List<string>> Keys = new Dictionary<string, List<string>>()
        {
            {
                "Index", new List<string>{
                    "web.home.main",
                    "web.home.intro.calltoaction",
                    "web.home.rotating.words",
                    "web.home.meta.seo.keywords",
                    "web.home.meta.seo.description"
                }
            },
            {
            
                "Contact", new List<string> {
                    "web.contact.form.aydinlatma.metni"
                }
            }
        };

        public static IHtmlString FindAppContentValue(List<Sky.CMS.DTO.AppContent> AppContentList, string Key)
        {
            if (AppContentList == null)
            {
                return null;
            }
            return new HtmlString((AppContentList.Where(ac => ac.ACKey == Key).FirstOrDefault() ?? new Sky.CMS.DTO.AppContent()).ACValue);
        }
    }
}