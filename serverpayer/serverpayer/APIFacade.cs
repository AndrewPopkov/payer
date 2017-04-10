using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using APIMetods;
//using Newtonsoft.Json.Linq;

namespace serverpayer
{
    public class APIFacade
    {
        private string urlAPI;
        private string curentMethod;
        private List<string> paramsMethod;
        Dictionary<string, MulticastDelegate> listMethods;
        //добавляем нужные фукции апи
        private void  InitializationAPI()
        {
            this.listMethods = new Dictionary<string, MulticastDelegate>();
            InitializationPay();
        }
        private void  InitializationPay()
        {
            Func<int, string, int, int, int, decimal, string, int> Pay = PaymentFun.Pay;
            this.listMethods.Add("Pay", Pay);
        }

            

        private string getMethod()
        {
            string buf=string.Empty;
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
                this.paramsMethod = new List<string>();
                foreach (Match match in re.Matches(urlAPI))
                {
                    Regex reInside = new Regex(@"([=]\w*$)", RegexOptions.IgnoreCase);
                    this.paramsMethod.Add(reInside.Match(match.Value).Value.Replace("=", string.Empty));

                }
            }
            return paramsMethod;
        }

        APIFacade(string url)
        {
            this.urlAPI = url.Replace(" ", string.Empty);
            InitializationAPI();
            this.curentMethod = getMethod();
            this.paramsMethod=getParams();
        }

        public static string  /*JObject*/ getResult(string url)
        {
            APIFacade curentFacade = new APIFacade(url);

            /*return new JObject();*/
            return string.Empty;

        }
    }
}
