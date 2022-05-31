using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Web.Models.News
{
    public class NewsListModel
    {
        public List<Sky.News.DTO.News> NewsList { get; set; }

        public List<Sky.News.DTO.News> RelatedNewsRightSmall { get; set; }

        public List<Sky.News.DTO.News> RelatedNewsRightBig { get; set; }

        public bool IsSearched { get; set; }

        public string SearchQuery { get; set; }

        public Sky.News.DTO.Category Category { get; set; }

        public Sky.News.DTO.Channel Channel { get; set; }

        public string PageTitle { get; set; }

        public string BreadCrumbTitle { get; set; }

        public List<Sky.Common.Provider.Pagination.Pagina> LasPaginas { get; set; }
    }
}