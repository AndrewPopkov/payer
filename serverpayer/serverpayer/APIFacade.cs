using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace serverpayer
{
    public class APIFacade
    {
        private string UrlAPI;
        public APIFacade(string url)
        {
            this.UrlAPI = url;
        }
    }
}
