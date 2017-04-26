using APIMetods;
using Moq;
using Newtonsoft.Json.Linq;
using serverpayer;
using System.Collections.Generic;
using Xunit;

namespace GatewayTest
{
    public class GatewayTest
    {
        //https://azhidkov.wordpress.com/2011/09/08/moq-quick-start/
        [Theory]
        [InlineData("Pay?order_id=2&card_number=2222222222222222&expiry_month=6&expiry_year=2018&cvv=999&amount_kop=50000&cardholder_name=IVANOV%20IVAN")]
        public void PayResponseGatewayMoq(string strUrl)
        {
            JObject chekObjnew = new JObject
            {
                { "id", 1 },
                { "mesasage", "Test" }
            };
            var fakeResponseGateway = new Mock<IResponseGateway>();
            fakeResponseGateway.Setup(ld => ld.ResponseGateway(It.IsAny<Dictionary<string, string>>())).Returns(chekObjnew);
            var facade = new APIFacade();
            facade.AddAPI("Pay", fakeResponseGateway.Object);
            var val = facade.GetResult(strUrl);

            Assert.Equal(chekObjnew.ToString(), val);
        }

        [Theory]
        [InlineData("Pay?order_id=2")]
        public void RefundResponseGatewayMoq(string strUrl)
        {
            JObject chekObjnew = new JObject
            {
                { "id", 1 },
                { "mesasage", "Test" }
            };
            var fakeResponseGateway = new Mock<IResponseGateway>();
            fakeResponseGateway.Setup(ld => ld.ResponseGateway(It.IsAny<Dictionary<string, string>>())).Returns(chekObjnew);
            var facade = new APIFacade();
            facade.AddAPI("Pay", fakeResponseGateway.Object);
            var val = facade.GetResult(strUrl);

            Assert.Equal(chekObjnew.ToString(), val);
        }

        [Theory]
        [InlineData("Pay?order_id=2")]
        public void GetStatusResponseGatewayMoq(string strUrl)
        {
            JObject chekObjnew = new JObject
            {
                { "id", 1 },
                { "mesasage", "Test" }
            };
            var fakeResponseGateway = new Mock<IResponseGateway>();
            fakeResponseGateway.Setup(ld => ld.ResponseGateway(It.IsAny<Dictionary<string, string>>())).Returns(chekObjnew);
            var facade = new APIFacade();
            facade.AddAPI("Pay", fakeResponseGateway.Object);
            var val = facade.GetResult(strUrl);

            Assert.Equal(chekObjnew.ToString(), val);
        }


        [Theory]
        [InlineData("Pay?order_id=2&card_number=2222222222222222&expiry_month=6&expiry_year=2018&cvv=999&amount_kop=50000&cardholder_name=IVANOV IVAN")]
        public void PayResponseGatewayInegrat(string strUrl)
        {
            JObject chekObjnew = new JObject
            {
                { "id", 1 },
                { "mesasage", "Ok" }
            };
            APIFacade facade = new APIFacade();
            facade.AddAPI("Pay", new PayGateway());
            var val = facade.GetResult(strUrl);

            Assert.Equal(chekObjnew.ToString(), val);
        }

        [Theory]
        [InlineData("Pay?order_id=3")]
        public void RefundResponseGatewayInegrat(string strUrl)
        {
            JObject chekObjnew = new JObject
            {
                { "id", 1 },
                { "mesasage", "Ok" }
            };
            APIFacade facade = new APIFacade();
            facade.AddAPI("Pay", new RefundGateway());
            var val = facade.GetResult(strUrl);

            Assert.Equal(chekObjnew.ToString(), val);
        }

        [Theory]
        [InlineData("Pay?order_id=3")]
        public void GetStatusResponseGatewayInegrat(string strUrl)
        {
            JObject chekObjnew = new JObject
            {
                { "id", 1 },
                { "mesasage", "Ok" }
            };
            APIFacade facade = new APIFacade();
            facade.AddAPI("Pay", new GetStatusGateway());
            var val = facade.GetResult(strUrl);

            Assert.Equal(chekObjnew.ToString(), val);
        }

    }
}
