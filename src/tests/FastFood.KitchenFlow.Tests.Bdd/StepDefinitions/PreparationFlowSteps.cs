using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using TechTalk.SpecFlow;
using FastFood.KitchenFlow.Application.InputModels.PreparationManagement;
using FastFood.KitchenFlow.Application.UseCases.PreparationManagement;
using FastFood.KitchenFlow.Application.Ports;
using FastFood.KitchenFlow.Application.Responses.PreparationManagement;
using FastFood.KitchenFlow.Domain.Entities.PreparationManagement;
using FastFood.KitchenFlow.Domain.Entities.DeliveryManagement;
using FastFood.KitchenFlow.Domain.Common.Enums;

namespace FastFood.KitchenFlow.Tests.Bdd.StepDefinitions;

[Binding]
public class PreparationFlowSteps
{
    // Campos para compartilhar estado entre steps
    private Guid _orderId;
    private string _orderSnapshot = string.Empty;
    private Preparation? _preparation;
    private Delivery? _delivery;
    private Guid? _deliveryId;

    // Mocks para dependências
    private readonly Mock<IPreparationRepository> _mockPreparationRepository;
    private readonly Mock<IDeliveryRepository> _mockDeliveryRepository;

    // Use cases
    private CreatePreparationUseCase? _createPreparationUseCase;
    private StartPreparationUseCase? _startPreparationUseCase;
    private FinishPreparationUseCase? _finishPreparationUseCase;

    public PreparationFlowSteps()
    {
        _mockPreparationRepository = new Mock<IPreparationRepository>();
        _mockDeliveryRepository = new Mock<IDeliveryRepository>();
    }

    [Given(@"I have a valid order with ID ""(.*)""")]
    public void GivenIHaveAValidOrderWithId(string orderId)
    {
        _orderId = Guid.Parse(orderId);
    }

    [Given(@"the order snapshot contains valid order data")]
    public void GivenTheOrderSnapshotContainsValidOrderData()
    {
        _orderSnapshot = @"{
            ""orderId"": ""123e4567-e89b-12d3-a456-426614174000"",
            ""orderCode"": ""ORD-123"",
            ""customerId"": ""550e8400-e29b-41d4-a716-446655440000"",
            ""totalPrice"": 51.00,
            ""createdAt"": ""2024-01-01T10:00:00Z"",
            ""items"": [
                {
                    ""productId"": ""550e8400-e29b-41d4-a716-446655440000"",
                    ""productName"": ""Hambúrguer"",
                    ""quantity"": 2,
                    ""unitPrice"": 25.50,
                    ""notes"": ""Sem cebola""
                }
            ],
            ""paymentId"": ""660e8400-e29b-41d4-a716-446655440000"",
            ""paidAt"": ""2024-01-01T10:05:00Z""
        }";
    }

    [When(@"I create a preparation for this order")]
    public async Task WhenICreateAPreparationForThisOrder()
    {
        _createPreparationUseCase = new CreatePreparationUseCase(_mockPreparationRepository.Object);

        // Configurar mock para GetByOrderIdAsync (verificação de idempotência)
        _mockPreparationRepository
            .Setup(r => r.GetByOrderIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid orderId) => null); // Não existe preparação ainda

        // Configurar mock para CreateAsync
        Preparation? savedPreparation = null;
        _mockPreparationRepository
            .Setup(r => r.CreateAsync(It.IsAny<Preparation>()))
            .ReturnsAsync((Preparation p) =>
            {
                savedPreparation = p;
                return p.Id;
            });

        // Configurar mock para GetByIdAsync (para recuperar após criação)
        _mockPreparationRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => savedPreparation);

        // Executar use case
        var input = new CreatePreparationInputModel
        {
            OrderId = _orderId,
            OrderSnapshot = _orderSnapshot
        };

        var response = await _createPreparationUseCase.ExecuteAsync(input);

        // Extrair o Response do Content (que é um Dictionary<string, object>)
        var responseData = ExtractResponse<CreatePreparationResponse>(response.Content);
        
        // Recuperar a preparação criada
        if (responseData != null)
        {
            _preparation = await _mockPreparationRepository.Object.GetByIdAsync(responseData.Id);
        }
    }

    [Then(@"the preparation should be created with status ""(.*)""")]
    public void ThenThePreparationShouldBeCreatedWithStatus(string status)
    {
        _preparation.Should().NotBeNull();
        _preparation!.Status.ToString().Should().Be(status);
    }

    [Then(@"the preparation should have the correct order ID")]
    public void ThenThePreparationShouldHaveTheCorrectOrderId()
    {
        _preparation.Should().NotBeNull();
        _preparation!.OrderId.Should().Be(_orderId);
    }

    [When(@"I start the preparation")]
    public async Task WhenIStartThePreparation()
    {
        _startPreparationUseCase = new StartPreparationUseCase(_mockPreparationRepository.Object);

        // Configurar mock para GetOldestReceivedAsync
        _mockPreparationRepository
            .Setup(r => r.GetOldestReceivedAsync())
            .ReturnsAsync(_preparation); // Retorna a preparação criada anteriormente

        // Configurar mock para UpdateAsync
        _mockPreparationRepository
            .Setup(r => r.UpdateAsync(It.IsAny<Preparation>()))
            .Returns(Task.CompletedTask)
            .Callback((Preparation p) =>
            {
                _preparation = p; // Atualizar referência
            });

        // Executar use case
        var input = new StartPreparationInputModel();
        await _startPreparationUseCase.ExecuteAsync(input);
    }

    [Then(@"the preparation should have status ""(.*)""")]
    public void ThenThePreparationShouldHaveStatus(string status)
    {
        _preparation.Should().NotBeNull();
        _preparation!.Status.ToString().Should().Be(status);
    }

    [When(@"I finish the preparation")]
    public async Task WhenIFinishThePreparation()
    {
        _finishPreparationUseCase = new FinishPreparationUseCase(
            _mockPreparationRepository.Object,
            _mockDeliveryRepository.Object);

        // Configurar mock para GetByIdAsync (buscar preparação)
        _mockPreparationRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => _preparation);

        // Configurar mock para UpdateAsync
        _mockPreparationRepository
            .Setup(r => r.UpdateAsync(It.IsAny<Preparation>()))
            .Returns(Task.CompletedTask)
            .Callback((Preparation p) =>
            {
                _preparation = p; // Atualizar referência
            });

        // Configurar mock para GetByPreparationIdAsync (verificar se delivery já existe)
        _mockDeliveryRepository
            .Setup(r => r.GetByPreparationIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid preparationId) => null); // Não existe delivery ainda

        // Configurar mock para CreateAsync (criar delivery)
        Delivery? savedDelivery = null;
        _mockDeliveryRepository
            .Setup(r => r.CreateAsync(It.IsAny<Delivery>()))
            .ReturnsAsync((Delivery d) =>
            {
                savedDelivery = d;
                return d.Id;
            });

        // Configurar mock para GetByIdAsync (recuperar delivery criada)
        _mockDeliveryRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => savedDelivery);

        // Executar use case
        var input = new FinishPreparationInputModel
        {
            Id = _preparation!.Id
        };

        var response = await _finishPreparationUseCase.ExecuteAsync(input);

        // Extrair o Response do Content (que é um Dictionary<string, object>)
        var responseData = ExtractResponse<FinishPreparationResponse>(response.Content);
        
        // Recuperar delivery criada
        if (responseData != null && responseData.DeliveryId.HasValue)
        {
            _deliveryId = responseData.DeliveryId;
            _delivery = await _mockDeliveryRepository.Object.GetByIdAsync(responseData.DeliveryId.Value);
        }
    }

    [Then(@"a delivery should be created for this preparation")]
    public void ThenADeliveryShouldBeCreatedForThisPreparation()
    {
        _delivery.Should().NotBeNull();
        _delivery!.PreparationId.Should().Be(_preparation!.Id);
        _delivery.Status.Should().Be(EnumDeliveryStatus.ReadyForPickup);
    }

    /// <summary>
    /// Helper method para extrair o Response do Content do ApiResponse.
    /// O Content pode ser um Dictionary<string, object> ou o próprio objeto.
    /// </summary>
    private static T? ExtractResponse<T>(object? content) where T : class
    {
        if (content == null)
            return null;

        // Se for um Dictionary, pegar o primeiro valor
        if (content is Dictionary<string, object> dict)
        {
            var firstValue = dict.Values.FirstOrDefault();
            if (firstValue is T typedValue)
                return typedValue;
            
            // Tentar converter via System.Text.Json se necessário
            if (firstValue != null)
            {
                var json = System.Text.Json.JsonSerializer.Serialize(firstValue);
                return System.Text.Json.JsonSerializer.Deserialize<T>(json);
            }
        }

        // Se for diretamente do tipo T
        if (content is T directValue)
            return directValue;

        return null;
    }
}
