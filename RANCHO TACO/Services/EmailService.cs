using System.Net;
using System.Net.Mail;

namespace RANCHO_TACO.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void EnviarPedido(string contenido)
        {
            var host = _config["EmailSettings:Host"] ?? "smtp.gmail.com";
            var port = int.Parse(_config["EmailSettings:Port"] ?? "587");
            var email = _config["EmailSettings:Email"] ?? "regrivglz@gmail.com";
            var password = _config["EmailSettings:Password"] ?? "vckw ubak hxrb qpqn";

            var cliente = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(email, password),
                EnableSsl = true
            };

            var mensaje = new MailMessage
            {
                From = new MailAddress(email),
                Subject = "Nuevo pedido - Rancho Taco",
                Body = contenido,
                IsBodyHtml = true
            };

            // 🔥 A QUIÉN SE ENVÍA
            mensaje.To.Add(email);

            cliente.Send(mensaje);
        }
    }
}