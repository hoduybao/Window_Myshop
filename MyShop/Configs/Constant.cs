
using System.Configuration;

namespace MyShop.Configs
{
    public class Constant
    {
        public static string BACKEND_URL = ConfigurationManager.AppSettings["BACKEND_URL"];
    }
}
