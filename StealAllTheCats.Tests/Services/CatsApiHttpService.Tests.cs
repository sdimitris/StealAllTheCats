using System.Net;
using System.Text;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using StealAllTheCats.Application.Services;
using StealAllTheCats.Domain.Common.Enums;
using StealAllTheCats.Domain.Configuration;
using StealAllTheCats.Domain.Entities;

namespace StealAllTheCats.Tests.Services
{
    public class CatsApiHttpServiceTests
    {
        [Fact]
        public async Task GetCatsAsync_ShouldReturnSuccess_WhenApiIsUp()
        {
            // Arrange
            var options = new Mock<IOptions<CatsApiSettings>>();
            options.Setup(x => x.Value)
                .Returns(new CatsApiSettings
                {
                    BaseUrl = "http://someurl.com",  
                    ApiKey = "someapikey"
                });

            var mockHttpResponse = new List<CatApiResponse>
            {
                new CatApiResponse
                {
                    Id = "1",
                    Width = 223,
                    Height = 223,
                    Url = "http://someurl.com",
                },
                new CatApiResponse(){
                    Id = "12",
                    Width = 222,
                    Height = 222,
                    Url = "http://someurl.com",
                }
            };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(), 
                    ItExpr.IsAny<CancellationToken>() 
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(mockHttpResponse), Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var sut = new CatsApiHttpService(options.Object, mockHttpClientFactory.Object);
            
            // Act
            var result = await sut.GetCatsAsync(1, 1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(JsonConvert.SerializeObject(mockHttpResponse), JsonConvert.SerializeObject(result.Value));
        }
        
        [Fact]
        public async Task GetCatsAsync_ShouldReturnCatsApiError_WhenApiReturningFailure()
        {
            // Arrange
            var options = new Mock<IOptions<CatsApiSettings>>();
            options.Setup(x => x.Value)
                .Returns(new CatsApiSettings
                {
                    BaseUrl = "http://someurl.com",  
                    ApiKey = "someapikey"
                });
            

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(), 
                    ItExpr.IsAny<CancellationToken>() 
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var sut = new CatsApiHttpService(options.Object, mockHttpClientFactory.Object);
            
            // Act
            var result = await sut.GetCatsAsync(1, 1);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(KnownApplicationErrorEnum.CatsApiError, result.Error.ApplicationError);
        }
    }
}
