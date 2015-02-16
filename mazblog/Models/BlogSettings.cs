using System.Configuration;

namespace mazblog.Models
{
    public class BlogSettings
    {
        private static string _blogTitle;
        private static string _postsCollectionName;
        private static string _imagesCollectionName;
        private static string _author;
        private static string _feedTitle;
        private static string _username;
        private static string _password;

        public static string BlogTitle
        {
            get
            {
                return _blogTitle ?? (_blogTitle = ConfigurationManager.AppSettings["BlogTitle"]);
            }
        }

        public static string PostsCollectionName
        {
            get
            {
                return _postsCollectionName ?? (_postsCollectionName = ConfigurationManager.AppSettings["PostsCollectionName"]);
            }
        }

        public static string ImagesCollectionName
        {
            get
            {
                return _imagesCollectionName ?? (_imagesCollectionName = ConfigurationManager.AppSettings["ImagesCollectionName"]);
            }
            
        }

        public static string Author
        {
            get
            {
                return _author ?? (_author = ConfigurationManager.AppSettings["Author"]);
            }
            
        }

        public static string FeedTitle
        {
            get
            {
                return _feedTitle ?? (_feedTitle = ConfigurationManager.AppSettings["FeedTitle"]);
            }
        }

        public static string Username
        {
            get
            {
                return _username ?? (_username = ConfigurationManager.AppSettings["Username"]);
            }
            
        }

        public static string Password
        {
            get
            {
                return _password ?? (_password = ConfigurationManager.AppSettings["Password"]);
            }
            
        }
    }
}