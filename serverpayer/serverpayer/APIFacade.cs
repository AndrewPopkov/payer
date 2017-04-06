using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
//using Newtonsoft.Json.Linq;

namespace serverpayer
{
    public class APIFacade
    {
        private string urlAPI;
        private string curentMethod;
        private List<string> paramsMethod;
        Dictionary<string, Delegate> listMethods;
        //добавляем нужные фукции апи
        private void  InitializationAPI()
        {
            this.listMethods = new Dictionary<string, Delegate>();

        }
        private string getMethod()
        {
            string buf=string.Empty;
            Regex re = new Regex(@"(/api/)", RegexOptions.IgnoreCase);
            this.urlAPI = re.Replace(this.urlAPI, "");
            buf = urlAPI.Substring(0, this.urlAPI.IndexOf("?"));
            if (buf == string.Empty)
            {
                throw new ArgumentException("Некоректная строка запроса к функции API, невозможно выбрать используемый метод");
            }
            this.urlAPI=urlAPI.Replace(buf, string.Empty);
            return buf;
        }
        private List<string> getParams()
        {
            Regex re = new Regex(@"([?]\w*=\w*)", RegexOptions.IgnoreCase);
            if (re.Matches(this.urlAPI).Count > 0)
            {
                foreach (Match match in re.Matches(urlAPI))
                {
                    Regex reInside = new Regex(@"([=]\w*$)", RegexOptions.IgnoreCase);
                    match.Value

                }
            }
            else
            {
                Console.WriteLine("Совпадений не найдено");
            }

            //if (buf == string.Empty)
            //{
            //    throw new ArgumentException("Некоректная строка запроса к функции API, невозможно выбрать используемый метод");
            //}
            return paramsMethod;
        }

        APIFacade(string url)
        {
            this.urlAPI = url.Replace(" ", string.Empty);
            InitializationAPI();
            this.curentMethod = getMethod();
            this.paramsMethod=getParams();

        }
        //switchMethod()
        public static string  /*JObject*/ getResult(string url)
        {
            APIFacade curentFacade = new APIFacade(url);

            /*return new JObject();*/
            return string.Empty;

        }
    }
}
