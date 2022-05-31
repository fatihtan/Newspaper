using Sky.News.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Web.Infrastructure
{
    public class NewsHelper
    {
        private static int TIMEZONE = 3;

        public void Setup(List<News> newsList)
        {
            this.SetNullImages(newsList);
            this.SetTimeZone(newsList);
        }

        public void Setup(News news)
        {
            this.SetNullImages(news);
            this.SetTimeZone(news);
        }

        private void SetNullImages(List<News> newsList)
        {
            newsList
                    .Where(n => string.IsNullOrEmpty(n.ImageURL))
                    .All(n =>
                    {
                        n.ImageURL = string.Format("https://www.bihaberver.com/assets/content/img/categories/category-{0}.PNG", n.CategoryID);
                        return true;
                    });
        }

        private void SetNullImages(News news)
        {
            if (string.IsNullOrEmpty(news.ImageURL))
            {
                news.ImageURL = string.Format("https://www.bihaberver.com/assets/content/img/categories/category-{0}.PNG", news.CategoryID);
            }
        }

        private void SetTimeZone(List<News> newsList)
        {
            newsList.ForEach(n =>
            {
                n.CreatedAt = n.CreatedAt.AddHours(TIMEZONE);
            });
        }

        private void SetTimeZone(News news)
        {
            news.CreatedAt = news.CreatedAt.AddHours(TIMEZONE);
        }
    }
}