using NourhanRageb.G06.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace NourhanRageb.G06.PL.Helpers
{
	public static class EmailSettings
	{
		public static void SendEmail(Emails email)
		{
			// Mail Server : gmail.com

			// Smtp
			var client = new SmtpClient("smtp.gmail.com", 587);
		
			client.EnableSsl = true; 
			
			client.Credentials = new NetworkCredential("NourhanRageb345@gmail.com", "ecnqjdhfnraxoorz");


			client.Send("NourhanRageb345@gmail.com", email.To, email.Subject , email.Body);
		
		}   
	}
}
