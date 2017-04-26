using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using APIMetods.database;
using APIMetods.database.model;
using System.Text.RegularExpressions;
using LogHelper;

namespace APIMetods
{
    public class GetStatusGateway : IResponseGateway
    {
        enum statusEnum
        {
            Ok=1,
            BackTransaction,
            WrongData,
            CloseLimit,
            ValidationError
        }
        //ид банка
        private JObject result;
        private readonly int vendorCard_id = 1;
        private int order_id;
        private Dictionary<string, string> param;

        public GetStatusGateway()
        {
            
        }

        private JObject GetResponseStatus(statusEnum stat)
        {
            JObject response = new JObject();
            status statusObj = GatewayContext.statuses.Find(s => s.status_id == (int)stat);
            response.Add("id", statusObj.status_id);
            response.Add("mesasage", statusObj.mesasage);
            return response;
        }

        private bool CheckValidationParams()
        {
            bool ValidationData = false;
            if (Regex.Match(param["order_id"], @"(\d+)").Success)
            {
                ValidationData = int.TryParse(param["order_id"], out this.order_id);
            }
            return ValidationData;
        }

        public void GetStatus()
        {

            order order = GatewayContext.orders.Find(o=>o.order_id==this.order_id);
            if (order != null)
            {
                result = GetResponseStatus((statusEnum)order.status_id);
            }

        }

        public JObject ResponseGateway(Dictionary<string, string> _param)
        {
            param = _param;
            if (CheckValidationParams())
            {
                GetStatus();
            }
            else
            {
                result = GetResponseStatus(statusEnum.ValidationError);
            }
            return result;
        }

    }
}
