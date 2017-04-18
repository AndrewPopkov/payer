using APIMetods;
using Moq;
using serverpayer;
using System;
using Xunit;

namespace UnitTestServer
{
    public class GatewayTest
    {
        // добавим в начале файла с тестами
        using Moq.Protected;
 
        // в тесте
        var mock = new Mock<CommandBase>();
        mock.Protected()
            .Setup<int>("Execute")
            .Return(5);
 
        // если вам нужно совпадение аргументов вы ДОЛЖНЫ использовать ItExpr вместо It
        mock.Protected()
            .Setup<string>("Execute", ItExpr.IsAny<string>())
            .Returns(true);

        https://azhidkov.wordpress.com/2011/09/08/moq-quick-start/
        [Theory]
        [InlineData(20, 180, 80, "good")]
        [InlineData(20, 180, 50, "bad")]
        public void PayResponseGateway(string strRequest)
        {
            var fakeResponseGateway = new Mock<IResponseGateway>();
            fakeResponseGateway.Setup(ld => ld.ResponseGateway(It.IsAny<Dictionary<string,string>>())).Returns(new JObject());

            fakeGateway.Setup(ld => ld.GetDirectoryByLoggerName(It.IsAny<string>())).Returns("C:\\Temp");
            fakeGateway.SetupGet(ld => ld.DefaultLogger).Returns("DefaultLogger");
            var facade = new APIFacade(strRequest);
            facade.AddAPI("Pay", fakeResponseGateway.object);
            //// create the sut and pass the fake version to it's constructor
            //var facade = APIFacade.getResult(fakeGateway);

            //// create some test data
            //var application = new CreditCardApplication
            //{
            //    ApplicantAgeInYears = 20
            //};

            //// execute the behaviour
            //var result = sut.ScoreApplication(application);

            //// check expected result
            //Assert.That(result, Is.False);

            //string directory = logger.GetDirectoryByLoggerName(loggerName);
 
            //Assert.That(directory, Is.EqualTo("C:\\" + loggerName));
        }

          [Theory]
        [InlineData(20, 180, 80, "good")]
        [InlineData(20, 180, 50, "bad")]
        public void PayResponseGateway(string strRequest)
        {
            var fakeResponseGateway = new Mock<IResponseGateway>();
            fakeResponseGateway.Setup(ld => ld.ResponseGateway(It.IsAny<Dictionary<string,string>>())).Returns(new JObject());

            fakeGateway.Setup(ld => ld.GetDirectoryByLoggerName(It.IsAny<string>())).Returns("C:\\Temp");
            fakeGateway.SetupGet(ld => ld.DefaultLogger).Returns("DefaultLogger");
            var facade = new APIFacade(strRequest);
            facade.AddAPI("Pay", fakeResponseGateway.object);
            //// create the sut and pass the fake version to it's constructor
            //var facade = APIFacade.getResult(fakeGateway);

            //// create some test data
            //var application = new CreditCardApplication
            //{
            //    ApplicantAgeInYears = 20
            //};

            //// execute the behaviour
            //var result = sut.ScoreApplication(application);

            //// check expected result
            //Assert.That(result, Is.False);

            //string directory = logger.GetDirectoryByLoggerName(loggerName);
 
            //Assert.That(directory, Is.EqualTo("C:\\" + loggerName));
        }

          [Theory]
        [InlineData(20, 180, 80, "good")]
        [InlineData(20, 180, 50, "bad")]
        public void PayResponseGateway(string strRequest)
        {
            var fakeResponseGateway = new Mock<IResponseGateway>();
            fakeResponseGateway.Setup(ld => ld.ResponseGateway(It.IsAny<Dictionary<string,string>>())).Returns(new JObject());

            fakeGateway.Setup(ld => ld.GetDirectoryByLoggerName(It.IsAny<string>())).Returns("C:\\Temp");
            fakeGateway.SetupGet(ld => ld.DefaultLogger).Returns("DefaultLogger");
            var facade = new APIFacade(strRequest);
            facade.AddAPI("Pay", fakeResponseGateway.object);
            //// create the sut and pass the fake version to it's constructor
            //var facade = APIFacade.getResult(fakeGateway);

            //// create some test data
            //var application = new CreditCardApplication
            //{
            //    ApplicantAgeInYears = 20
            //};

            //// execute the behaviour
            //var result = sut.ScoreApplication(application);

            //// check expected result
            //Assert.That(result, Is.False);

            //string directory = logger.GetDirectoryByLoggerName(loggerName);
 
            //Assert.That(directory, Is.EqualTo("C:\\" + loggerName));
        }
    }
}
