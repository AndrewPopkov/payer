using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace APIMetods
{
    public interface IResponseGateway
    {
         JObject ResponseGateway(Dictionary<string, string> param);
    }
}
