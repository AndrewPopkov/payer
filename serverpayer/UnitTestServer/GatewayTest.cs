using APIMetods;
using Moq;
using serverpayer;
using System;
using Xunit;
using Moq.Protected;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace GatewayTest
{
    public class GatewayTest
    {
        //// добавим в начале файла с тестами
        
 
        //// в тесте
        //var mock = new Mock<CommandBase>();
        //mock.Protected()
        //    .Setup<int>("Execute")
        //    .Return(5);
 
        //// если вам нужно совпадение аргументов вы ДОЛЖНЫ использовать ItExpr вместо It
        //mock.Protected()
        //    .Setup<string>("Execute", ItExpr.IsAny<string>())
        //    .Returns(true);

        //https://azhidkov.wordpress.com/2011/09/08/moq-quick-start/
        [Theory]
        [InlineData("Pay?order_id=2&card_number=2222222222222222&expiry_month=6&expiry_year=2018&cvv=999&amount_kop=50000&cardholder_name=IVANOV%20IVAN")]
        public void PayResponseGateway(string strUrl)
        {
            JObject chekObjnew = new JObject();
            chekObjnew.Add("id", 1);
            chekObjnew.Add("mesasage", "Test");
            var fakeResponseGateway = new Mock<IResponseGateway>();
            fakeResponseGateway.Setup(ld => ld.ResponseGateway(It.IsAny<Dictionary<string, string>>())).Returns(chekObjnew);
            var facade = new APIFacade();
            facade.AddAPI("Pay", fakeResponseGateway.Object);
            //// create the sut and pass the fake version to it's constructor
            var val = facade.getResult(strUrl);

            Assert.Equal(chekObjnew.ToString(), val);
        }

     
    }
}
