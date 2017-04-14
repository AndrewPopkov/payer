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

        public GetStatusGateway()
        {

        }

        private JObject GetResponseStatus(statusEnum stat)
        {
            JObject response = new JObject();
            using (GatewayContext db = new GatewayContext())
            {
                status statusObj = db.statuses.Find((int)stat);                
                result.Add("id", statusObj.status_id);
                result.Add("mesasage", statusObj.mesasage);
            }
            return response;
        }

        private bool GetStatusValidationParams(string str_order_id)
        {
            bool ValidationData = false;
            if (Regex.Match(str_order_id, @"(\d+)").Success)
            {
                ValidationData = int.TryParse(str_order_id, out this.order_id);
            }
            return ValidationData;
        }

        public void GetStatus()
        {
            using (GatewayContext db = new GatewayContext())
            {
                try
                {
                    order order = db.orders.Find(this.order_id);
                    if (order != null)
                    {
                        result = GetResponseStatus((statusEnum)order.status_id);
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }
            }
        }

        public JObject ResponseGateway(Dictionary<string, string> param)
        {
            if (GetStatusValidationParams(param["order_id"]))
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
