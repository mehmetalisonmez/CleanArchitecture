using CleanArchitecture.Application.Features.CarFeatures.Commands.CreateCar;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Dtos;
using CleanArchitecture.Presentation.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CleanArchitecture.UnitTest
{
	public class CarsControllerUnitTest
	{
		[Fact]
		public async void Create_ReturnsOkResult_WhenRequestIsValid()
		{
			// Arrange : Tanımlamaları yaptığımız parçadır.
			var mediatorMock = new Mock<IMediator>(); // Mock : fake bir yapı
			CreateCarCommand createCarCommand = new(
				"Toyota", "Corolla", 5000);
			MessageResponse response = new("Araç başarıyla kaydedildi!");
			CancellationToken cancellationToken = new();

			mediatorMock.Setup(m => m.Send(createCarCommand, cancellationToken)).ReturnsAsync(response); //mock kullandığımızı için mediator' ın kendi metoduna gitmiyor, metoda ulaşmöış gibi set ederiz fake bir cevap dönmesi gerektiğini söyleriz 

			CarsController carsController = new(mediatorMock.Object);

			// Act : İşlemi yaptırırız sonucu bir değişkene atarız.
			var result = await carsController.Create(createCarCommand, cancellationToken);

			// Assert : Kontrolü kodladığımız yerdir.
			var okResult = Assert.IsType<OkObjectResult>(result);
			var returnValue = Assert.IsType<MessageResponse>(okResult.Value);

			Assert.Equal(response, returnValue);
			mediatorMock.Verify(m => m.Send(createCarCommand, cancellationToken), Times.Once);
		}
	}
}
