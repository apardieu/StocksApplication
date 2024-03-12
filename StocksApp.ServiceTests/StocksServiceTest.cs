
using AutoFixture;
using Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Moq;
using RepositoryContracts;
using Service;
using ServiceContracts;
using ServiceContracts.DTO;
using System.ComponentModel.DataAnnotations;

namespace StocksTest
{
    public class StocksServiceTest
    {
        private readonly IStocksService _stocksService;
        private readonly Mock<IStocksRepository> _stocksRepositoryMock;
        private readonly IStocksRepository _stocksRepository;
        private readonly Fixture _fixture;

        public StocksServiceTest()
        {
            _fixture = new Fixture();
            _stocksRepositoryMock = new Mock<IStocksRepository>();
            _stocksRepository = _stocksRepositoryMock.Object;
            _stocksService = new StocksService(_stocksRepository);

        }

        #region CreateBuyOrder

        [Fact]

        public async void CreateBuyOrder_NullBuyRequest_TodBeNull()
        {
            BuyOrderRequest? request = null;
            Func<Task> action = async () => {
                await _stocksService.CreateBuyOrder(request);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public void CreateBuyOrder_QuantityUnderMinimum()
        {
            BuyOrderRequest? request = _fixture
                .Build<BuyOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, new DateTime(2020, 1, 1))
                .With(temp => temp.Quantity, (uint)0)
                .Create();

            BuyOrder buyOrder = request.ToBuyOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            Func<Task> action = async () => {
                await _stocksService.CreateBuyOrder(request);
            };

            action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public void CreateBuyOrder_QuantityAboveMaximum()
        {
            BuyOrderRequest? request = _fixture
                .Build<BuyOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, new DateTime(2020, 1, 1))
                .With(temp => temp.Quantity, (uint)10001)
                .Create();

            BuyOrder buyOrder = request.ToBuyOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            Func<Task> action = async () => {
                await _stocksService.CreateBuyOrder(request);
            };

            action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public void CreateBuyOrder_PriceUnderMinimum()
        {
            BuyOrderRequest? request = _fixture
                .Build<BuyOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, new DateTime(2020, 1, 1))
                .With(temp => temp.Price, (uint)0)
                .Create();

            BuyOrder buyOrder = request.ToBuyOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            Func<Task> action = async () => {
                await _stocksService.CreateBuyOrder(request);
            };

            action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public void CreateBuyOrder_PriceAboveMaximum()
        {
            BuyOrderRequest? request = _fixture
                .Build<BuyOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, new DateTime(2020, 1, 1))
                .With(temp => temp.Price, (uint)100001)
                .Create();

            BuyOrder buyOrder = request.ToBuyOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            Func<Task> action = async () => {
                await _stocksService.CreateBuyOrder(request);
            };

            action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public void CreateBuyOrder_NullStockSymbol()
        {
            BuyOrderRequest? request = _fixture
                            .Build<BuyOrderRequest>()
                            .With(temp => temp.DateAndTimeOfOrder, new DateTime(2020, 1, 1))
                            .With(temp => temp.StockSymbol, null as string)
                            .Create();

            BuyOrder buyOrder = request.ToBuyOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            Func<Task> action = async () => {
                await _stocksService.CreateBuyOrder(request);
            };

            action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public void CreateBuyOrder_DateAndTimeOfOrderBeforeMinValue()
        {
            BuyOrderRequest? request = _fixture
                            .Build<BuyOrderRequest>()
                            .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("1999-12-31"))
                            .Create();

            BuyOrder buyOrder = request.ToBuyOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            Func<Task> action = async () => {
                await _stocksService.CreateBuyOrder(request);
            };

            action.Should().ThrowAsync<ArgumentException>();

        }

        [Fact]
        public async void CreateBuyOrder_AllValidValues()
        {

            BuyOrderRequest? request = _fixture
                .Build<BuyOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, new DateTime(2020, 1, 1))
                .Create();

            BuyOrder buyOrder = request.ToBuyOrder();
            BuyOrderResponse response_expected = buyOrder.ToBuyOrderResponse();

            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

             BuyOrderResponse response_from_create = await _stocksService.CreateBuyOrder(request);

            response_from_create.Should().Be(response_expected);

        }
        #endregion

        #region GetAllBuyOrders
        [Fact]
        public async void GetAllBuyOrders_Empty()
        {
            List<BuyOrder> buyOrders = new List<BuyOrder>();
            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders()).ReturnsAsync(buyOrders);

            List<BuyOrderResponse> buyOrders_from_get = await _stocksService.GetBuyOrders();

            buyOrders_from_get.Should().BeEmpty();
        }

        [Fact]
        public async void GetAllBuyOrders_ValidData()
        {

            List<BuyOrder> buyOrders = _fixture
                .Build<BuyOrder>()
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("2020-1-1"))
                .CreateMany<BuyOrder>()
                .ToList();

            List<BuyOrderResponse> responses_expected = buyOrders.Select(temp => temp.ToBuyOrderResponse()).ToList();

            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders()).ReturnsAsync(buyOrders);

            List<BuyOrderResponse> responses_from_get = await _stocksService.GetBuyOrders();

            responses_from_get.Should().BeEquivalentTo(responses_expected);


        }
        #endregion
        ///////////////////////////////////////////////
        #region CreateSellOrder

        [Fact]

        public async void CreateSellOrder_NullSellRequest_TodBeNull()
        {
            SellOrderRequest? request = null;
            Func<Task> action = async () => {
                await _stocksService.CreateSellOrder(request);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public void CreateSellOrder_QuantityUnderMinimum()
        {
            SellOrderRequest? request = _fixture
                .Build<SellOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, new DateTime(2020, 1, 1))
                .With(temp => temp.Quantity, (uint)0)
                .Create();

            SellOrder sellOrder = request.ToSellOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            Func<Task> action = async () => {
                await _stocksService.CreateSellOrder(request);
            };

            action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public void CreateSellOrder_QuantityAboveMaximum()
        {
            SellOrderRequest? request = _fixture
                .Build<SellOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, new DateTime(2020, 1, 1))
                .With(temp => temp.Quantity, (uint)10001)
                .Create();

            SellOrder sellOrder = request.ToSellOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            Func<Task> action = async () => {
                await _stocksService.CreateSellOrder(request);
            };

            action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public void CreateSellOrder_PriceUnderMinimum()
        {
            SellOrderRequest? request = _fixture
                .Build<SellOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, new DateTime(2020, 1, 1))
                .With(temp => temp.Price, (uint)0)
                .Create();

            SellOrder sellOrder = request.ToSellOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            Func<Task> action = async () => {
                await _stocksService.CreateSellOrder(request);
            };

            action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public void CreateSellOrder_PriceAboveMaximum()
        {
            SellOrderRequest? request = _fixture
                .Build<SellOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, new DateTime(2020, 1, 1))
                .With(temp => temp.Price, (uint)100001)
                .Create();

            SellOrder sellOrder = request.ToSellOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            Func<Task> action = async () => {
                await _stocksService.CreateSellOrder(request);
            };

            action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public void CreateSellOrder_NullStockSymbol()
        {
            SellOrderRequest? request = _fixture
                            .Build<SellOrderRequest>()
                            .With(temp => temp.DateAndTimeOfOrder, new DateTime(2020, 1, 1))
                            .With(temp => temp.StockSymbol, null as string)
                            .Create();

            SellOrder sellOrder = request.ToSellOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            Func<Task> action = async () => {
                await _stocksService.CreateSellOrder(request);
            };

            action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public void CreateSellOrder_DateAndTimeOfOrderBeforeMinValue()
        {
            SellOrderRequest? request = _fixture
                            .Build<SellOrderRequest>()
                            .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("1999-12-31"))
                            .Create();

            SellOrder sellOrder = request.ToSellOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            Func<Task> action = async () => {
                await _stocksService.CreateSellOrder(request);
            };

            action.Should().ThrowAsync<ArgumentException>();

        }

        [Fact]
        public async void CreateSellOrder_AllValidValues()
        {

            SellOrderRequest? request = _fixture
                .Build<SellOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, new DateTime(2020, 1, 1))
                .Create();

            SellOrder sellOrder = request.ToSellOrder();
            SellOrderResponse response_expected = sellOrder.ToSellOrderResponse();

            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            SellOrderResponse response_from_create = await _stocksService.CreateSellOrder(request);

            response_from_create.Should().Be(response_expected);

        }
        #endregion

        #region GetAllSellOrders
        [Fact]
        public async void GetAllSellOrders_Empty()
        {
            List<SellOrder> sellOrders = new List<SellOrder>();
            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders()).ReturnsAsync(sellOrders);

            List<SellOrderResponse> sellOrders_from_get = await _stocksService.GetSellOrders();

            sellOrders_from_get.Should().BeEmpty();
        }

        [Fact]
        public async void GetAllSellOrders_ValidData()
        {

            List<SellOrder> sellOrders = _fixture
                .Build<SellOrder>()
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("2020-1-1"))
                .CreateMany<SellOrder>()
                .ToList();

            List<SellOrderResponse> responses_expected = sellOrders.Select(temp => temp.ToSellOrderResponse()).ToList();

            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders()).ReturnsAsync(sellOrders);

            List<SellOrderResponse> responses_from_get = await _stocksService.GetSellOrders();

            responses_from_get.Should().BeEquivalentTo(responses_expected);


        }
        #endregion
    }
}