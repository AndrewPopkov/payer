using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using APIMetods;
using Newtonsoft.Json.Linq;

namespace serverpayer
{
    public class APIFacade
    {
        private string urlAPI;
        private string curentMethod;
        private Dictionary<string,string> paramsMethod;
        private Dictionary<string, IResponseGateway> listMethods;
        //добавляем нужные фукции апи
        private void  InitializationAPI()
        {
            this.listMethods =new  Dictionary<string, IResponseGateway>();
            InitializationPay();
            InitializationGetStatus();
            InitializationRefund();
        }
        private void InitializationGetStatus()
        {
            this.listMethods.Add("GetStatus", new GetStatusGateway());
        }
        private void InitializationPay()
        {
            this.listMethods.Add("Pay", new PayGateway());
        }
        private void InitializationRefund()
        {
            this.listMethods.Add("Refund", new RefundGateway());
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
        private Dictionary<string, string> getParams()
        {
            
            Regex re = new Regex(@"([?&]\w*=\w*)", RegexOptions.IgnoreCase);
            if (re.Matches(this.urlAPI).Count > 0)
            {
                this.paramsMethod = new Dictionary<string,string>();
                foreach (Match match in re.Matches(urlAPI))
                {
                    Regex namePar = new Regex(@"(\w*[=])", RegexOptions.IgnoreCase);
                    Regex valPar = new Regex(@"([=]\w*$)", RegexOptions.IgnoreCase);
                    this.paramsMethod.Add(namePar.Match(match.Value).Value.Replace("=", string.Empty),
                                          valPar.Match(match.Value).Value.Replace("=", string.Empty));

                }
            }
            return paramsMethod;
        }

        APIFacade(string url)
        {
            this.urlAPI = url.Replace(" ", string.Empty);         
            this.curentMethod = getMethod();
            this.paramsMethod=getParams();
            InitializationAPI();
        }

        public static string  getResult(string url)
        {
            APIFacade curentFacade = new APIFacade(url);
            JObject obj = curentFacade.listMethods[curentFacade.curentMethod].ResponseGateway(curentFacade.paramsMethod);
            return obj.ToString();

        }
    }
}
