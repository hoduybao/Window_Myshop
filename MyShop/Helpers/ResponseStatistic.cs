using System.Collections.Generic;

namespace MyShop.Helpers
{
    public class ResponseStatistic
    {
        //public string Status { get; set; }

        //public string Message { get; set; }

        //public JToken Data { get; set; }

        public string Status { get; set; }
        public string Message { get; set; }
        public List<List<object>> Data { get; set; }
    }
}
