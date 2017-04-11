using DimensionTest.viewmodel;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace payerClient.vievmodel
{
    class MainViewModel: NotifyPropertyChanged
    {
        public MainViewModel()
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

        private string _card_number;
        public string card_number
        {
            get { return _card_number; }
            set
            {
                _card_number = value;
                RaisePropertyChanged(() => card_number);
            }
        }

        private string _expiry_month;
        public string expiry_month
        {
            get { return _expiry_month; }
            set
            {
                _expiry_month = value;
                RaisePropertyChanged(() => expiry_month);
            }
        }

        private string _expiry_year;
        public string expiry_year
        {
            get { return _expiry_year; }
            set
            {
                _expiry_year = value;
                RaisePropertyChanged(() => expiry_year);
            }
        }

        private string _cardholder_name;
        public string cardholder_name
        {
            get { return _cardholder_name; }
            set
            {
                _cardholder_name = value;
                RaisePropertyChanged(() => cardholder_name);
            }
        }


        private string _cvv;
        public string cvv
        {
            get { return _cvv; }
            set
            {
                _cvv = value;
                RaisePropertyChanged(() => cvv);
            }
        }

        private string _amount_kop;
        public string amount_kop
        {
            get { return _amount_kop; }
            set
            {
                _amount_kop = value;
                RaisePropertyChanged(() => amount_kop);
            }
        }



        private SimpleCommand _PayCommand;
        public SimpleCommand PayCommand
        {
            get { return _PayCommand; }
            set
            {
                _PayCommand = value;
                RaisePropertyChanged(() => PayCommand);
            }
        }


        /// <summary>
        /// Создать команды.
        /// </summary>
        public void setupCommands()
        {
            this.PayCommand = new SimpleCommand(
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
            this.PayCommand.Update();
        }

        private async void setResult()
        {
            JObject response;
            try
            {
                response = await Task < JObject>.Run(() =>
                {
                    return GetPay();
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
        private JObject GetPay()
        {
            var request = new RestRequest();
            request.AddParameter("order_id", order_id);
            request.AddParameter("card_number", card_number);
            request.AddParameter("expiry_month", expiry_month);
            request.AddParameter("expiry_year", expiry_year);
            request.AddParameter("cvv", cvv);
            request.AddParameter("amount_kop", amount_kop);
            request.AddParameter("cardholder_name", cardholder_name);

            var restClient = new RestClient();
            restClient.BaseUrl = new System.Uri("https://127.0.0.1:81");
            var response = restClient.Execute(request);
            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, response.ErrorException);
                throw twilioException;
            }
            var obj  = JObject.Parse(response.Content);
            return obj;
        }
    }
}
