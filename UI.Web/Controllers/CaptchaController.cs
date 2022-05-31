using Sky.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Web.Controllers
{
    public class CaptchaController : Controller
    {
        [Route("Captcha/Get")]
        public FileResult Get(bool? refresh)
        {
            Random rnd = new Random();

            if (refresh.HasValue && refresh.Value)
            {
                Session["Captcha"] = rnd.Next(10, 99);
            }

            if (Session["Captcha"] == null)
            {
                Session["Captcha"] = rnd.Next(10, 99);
            }

            byte[] bytes = CaptchaUtility.Generate(Session["Captcha"].ToString());

            return File(bytes, "image/png");
        }
    }
}
