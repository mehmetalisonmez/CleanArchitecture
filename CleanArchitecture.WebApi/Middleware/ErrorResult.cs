using Newtonsoft.Json;

namespace CleanArchitecture.WebApi.Middleware
{
	public class ErrorResult : ErrorStatusCode //İçinde StatusCode ve mesajımız yer alacak
	{
		public string Message { get; set; }
	}

	public class ErrorStatusCode
	{
		public int StatusCode { get; set; }
		public override string ToString() //  Varsayılan ToString metodunu ezip yerine kendi versiyonumuzu yazdığımızı belirtir.
		{ 
			return JsonConvert.SerializeObject(this);
			//Popüler Newtonsoft.Json kütüphanesinden gelen bir metottur. this kelimesi o anki ErrorStatusCode nesnesini ifade eder.
			//Bu metot, nesneyi ve içindeki StatusCode gibi özellikleri alıp standart bir JSON metnine dönüştürür.
		}
		// Kısaca, ToString() metodunu override etmek, bir nesnenin metin olarak temsil edildiğinde ne göstereceğini değiştirmektir.
		// Normalde, bir nesne için .ToString() metodunu çağırdığında, C# sana sadece o nesnenin sınıf adını verir. Bu pek bir işe yaramaz. 
		// Kendi kodunla bu metodu "ezerek" (override ederek), nesnenin içindeki verileri gösteren daha anlamlı bir sonuç döndürebilirsin.
	}
	public sealed class ValidationErrorDetails : ErrorStatusCode
	{
		public IEnumerable<string> Errors { get; set; } //Validation kurallarımda birden fazla hata dönebileceği için Error listesi oluşturduk
	}


}
