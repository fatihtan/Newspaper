using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Web.Models.News
{
    public class NewsModel
    {
        public Sky.News.DTO.News News { get; set; }

        //public Sky.News.DTO.News PreviousNews { get; set; }

        //public Sky.News.DTO.News NextNews { get; set; }

        public List<Sky.News.DTO.News> RelatedNewsSlider { get; set; }

        public List<Sky.News.DTO.News> RelatedNewsRightSmall { get; set; }

        public List<Sky.News.DTO.News> RelatedNewsRightBig { get; set; }

        public List<Sky.Newspaper.DTO.Comment> CommentList { get; set; }

        public int TotalViews { get; set; }
    }
}