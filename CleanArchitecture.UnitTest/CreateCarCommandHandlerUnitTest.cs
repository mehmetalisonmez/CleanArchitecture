using CleanArchitecture.Application.Features.CarFeatures.Commands.CreateCar;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Dtos;
using Moq;

namespace CleanArchitecture.UnitTest.Features.CarFeatures
{
	public class CreateCarCommandHandlerUnitTest
	{
		[Fact]
		public async Task Handle_ShouldCallCreateAsyncOnCarServiceAndReturnSuccessMessage()
		{
			// Arrange (Hazırlık)
			var carServiceMock = new Mock<ICarService>();
			CreateCarCommand createCarCommand = new(
				"Toyota", "Corolla", 5000);			
			CancellationToken cancellationToken = new();	
			CreateCarCommandHandler createCarCommandHandler = new(carServiceMock.Object);
			carServiceMock.Setup(s =>
				s.CreateAsync(createCarCommand, cancellationToken))
				.Returns(Task.CompletedTask);

			// Act (Eylem)
			var result = await createCarCommandHandler.Handle(createCarCommand, cancellationToken);

			// Assert (Doğrulama)
			carServiceMock.Verify(
				s => s.CreateAsync(createCarCommand, cancellationToken),
				Times.Once);			
			Assert.NotNull(result);
			Assert.Equal("Araç başarıyla kaydedildi!", result.Message);
		}
	}
}