﻿using DimensionTest.viewmodel;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace payerClient.viewmodel
{
    class RefundViewModel : NotifyPropertyChanged
    {
        public RefundViewModel()
        {
            setupCommands();
        }
        private string _order_id;
        public string order_id
        {
            get { return _order_id; }
            set
            {
                _order_id = value;
                RaisePropertyChanged(() => order_id);
            }
        }

        private SimpleCommand _RefundCommand;
        public SimpleCommand RefundCommand
        {
            get { return _RefundCommand; }
            set
            {
                _RefundCommand = value;
                RaisePropertyChanged(() => RefundCommand);
            }
        }


        /// <summary>
        /// Создать команды.
        /// </summary>
        public void setupCommands()
        {
            this.RefundCommand = new SimpleCommand(
                p =>
                {
                    setResult();
                },
                p =>
                {
                    return true;
                });
        }


        /// <summary>
        /// Обновить состояние команд.
        /// </summary>
        public void updateCommandsState()
        {
            this.RefundCommand.Update();
        }

        private async void setResult()
        {
            JObject response;
            try
            {
                response = await Task<JObject>.Run(() =>
                {
                    return GetRefund();
                });
                MessageBox.Show(response.ToString(), "Ответ сервера", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка ответа сервера", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private JObject GetRefund()
        {
            var request = new RestRequest("api/{method}");
            request.AddParameter("method", "Refund", ParameterType.UrlSegment);
            request.AddParameter("order_id", order_id);

            var restClient = new RestClient();
            restClient.BaseUrl = new System.Uri("http://127.0.0.1:81");
            restClient.Encoding = Encoding.UTF8;
            var response = restClient.Execute(request);
            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, response.ErrorException);
                throw twilioException;
            }
            var obj = JObject.Parse(response.Content);
            return obj;
        }
    }
}
