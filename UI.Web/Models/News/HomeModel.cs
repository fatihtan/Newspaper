using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Web.Models.News
{
    public class HomeModel
    {
        public List<Sky.News.DTO.News> HeadlineList { get; set; }

        public List<Sky.News.DTO.News> FlashList { get; set; }

        public List<Sky.News.DTO.News> HeadlineTriple { get; set; }

        public List<Sky.News.DTO.Category> CategoryList { get; set; }

        public List<Sky.News.DTO.Channel> ChannelList { get; set; }

        public Dictionary<Sky.News.DTO.Category, List<Sky.News.DTO.News>> TabList { get; set; }

        public List<Sky.News.DTO.News> LatestRightSmallList { get; set; }

        public List<Sky.News.DTO.News> SliderList { get; set; }

        public List<Sky.News.DTO.News> TeknolojiNewsList { get; set; }

        public List<Sky.News.DTO.News> SeyahatNewsList { get; set; }

        public List<Sky.News.DTO.News> OtomobilNewsList { get; set; }

        public List<Sky.News.DTO.News> SaglikNewsList { get; set; }

        public List<Sky.News.DTO.News> EkonomiFinansNewsList { get; set; }

        public List<Sky.News.DTO.News> EgitimNewsList { get; set; }

        public List<Sky.News.DTO.News> KulturSanatNewsList { get; set; }

        public List<Sky.News.DTO.News> MagazinNewsList { get; set; }

        public List<Sky.News.DTO.News> YasamNewsList { get; set; }

        public List<Sky.News.DTO.News> SporNewsList { get; set; }
    }
}