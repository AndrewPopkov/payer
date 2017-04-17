using APIMetods;
using Moq;
using serverpayer;
using System;
using Xunit;

namespace UnitTestServer
{
    public class GatewayTest
    {
        [Theory]
        [InlineData(20, 180, 80, "good")]
        [InlineData(20, 180, 50, "bad")]
        public void ShouldDeclineUnderAgeApplicant()
        {
            // use Moq to create a fake version of an ICreditCheckerGateway
            var fakeGateway = new Mock<IResponseGateway>();

            //// create the sut and pass the fake version to it's constructor
            //var sut = APIFacade.getResult(fakeGateway);

            //// create some test data
            //var application = new CreditCardApplication
            //{
            //    ApplicantAgeInYears = 20
            //};

            //// execute the behaviour
            //var result = sut.ScoreApplication(application);

            //// check expected result
            //Assert.That(result, Is.False);
        }
    }
}
