
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Persistance.Context;
using FluentValidation;

namespace CleanArchitecture.WebApi.Middleware
{
	public sealed class ExceptionMiddleware : IMiddleware
	{
		private readonly AppDbContext _context; // DB Kaydetme için

		public ExceptionMiddleware(AppDbContext context)
		{
			_context = context;
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next) // Bu metot, middleware'in kalbidir. Gelen her HTTP isteği bu metottan geçer.
		{
			try
			{
				await next(context);
				// Bu komut, "İsteği benden sonraki middleware'e veya controller'a pasla ve onun işini bitirmesini bekle" demektir.
			}
			catch (Exception ex)
			{
				await LogExceptionToDatabaseAsync(ex, context.Request); // DB Kaydetme için
				await HandleExceptionAsync(context, ex);
			}
		}
		// HttpContext context: Gelen istekle (request) ve gönderilecek cevapla (response) ilgili tüm bilgileri içerir.
		// RequestDelegate next: Bir sonraki middleware'i çağıran bir temsilcidir (delegate). Uygulama bir middleware zincirinden oluşur ve next bu zincirin bir sonraki halkasını temsil eder.
		private Task HandleExceptionAsync(HttpContext context, Exception ex)
		{
			// Hata tipi normal exception ise normal hata demektir.
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = 500;
			// Hata tipiş validation ise hatalaruı array' e çeviricez sonra atıcaz
			if (ex.GetType() == typeof(ValidationException))
			{
				return context.Response.WriteAsync(new ValidationErrorDetails
				{
					Errors = ((ValidationException)ex).Errors.Select(s =>
					s.PropertyName),
					StatusCode = 403
				}.ToString());
			}

			return context.Response.WriteAsync(new ErrorResult
			{
				Message = ex.Message,
				StatusCode = context.Response.StatusCode
			}.ToString());
		}

		private async Task LogExceptionToDatabaseAsync(Exception ex, HttpRequest request) // DB Kaydetme için
		{
			ErrorLog errorLog = new()
			{
				ErrorMessage = ex.Message,
				StackTrace = ex.StackTrace,
				RequestPath = request.Path,
				RequestMethod = request.Method,
				Timestamp = DateTime.Now
			};

			await _context.Set<ErrorLog>().AddAsync(errorLog, default);
			await _context.SaveChangesAsync(default);
		}
	}
}
