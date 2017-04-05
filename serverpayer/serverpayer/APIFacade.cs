using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace serverpayer
{
    public class APIFacade
    {
        private string UrlAPI;
        private Match ReqMatch;
        private string curentMethod;
        private List<string> paramsMethod;
        //private T result;
        public APIFacade(string url)
        {
            this.UrlAPI = url;
            this.ReqMatch = Regex.Match(UrlAPI, @"^(/api/)");
        }
        //switchMethod()
        public void GetResult<T>(string url, Action<T> dataLoad)
        {
            dataLoad(result);
        }
    }
}
