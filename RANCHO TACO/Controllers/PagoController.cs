using Microsoft.AspNetCore.Mvc;
using RANCHO_TACO.Services;
using RANCHO_TACO.Models;
using System.Text;
using System.Text.Json;

namespace RANCHO_TACO.Controllers
{
    // Controlador encargado de gestionar el proceso de pago, la generación del ticket y el envío de notificaciones por correo.
    public class PagoController : Controller
    {
        // Servicio externo para el manejo de envíos de correo electrónico.
        private readonly EmailService _email;

        // Constructor que recibe el servicio de email mediante inyección de dependencias.
        public PagoController(EmailService email)
        {
            _email = email;
        }

        // Muestra la pantalla donde el usuario ingresa sus datos de envío y confirma su compra.
        public IActionResult Index()
        {
            return View();
        }

        // Acción que procesa el formulario de pago. Recibe los datos del cliente y el contenido del carrito en formato JSON.
        [HttpPost]
        public IActionResult ConfirmarPago(
            string Nombre,
            string Telefono,
            string TipoEntrega,
            string Calle,
            string Numero,
            string Fraccionamiento,
            string CP,
            string Referencia,
            string CarritoJson
        )
        {
            try
            {
                // Deserializa la cadena JSON del carrito para convertirla nuevamente en una lista de objetos 'CarritoItem'.
                var carrito = string.IsNullOrEmpty(CarritoJson)
                    ? new List<CarritoItem>()
                    : JsonSerializer.Deserialize<List<CarritoItem>>(CarritoJson) ?? new List<CarritoItem>();

                // Calcula el monto total de la orden multiplicando precio por cantidad de cada artículo.
                decimal total = carrito.Sum(p => p.Precio * (p.Cantidad > 0 ? p.Cantidad : 1));

                // Construye una sola cadena de texto con la dirección completa si es a domicilio, o indica que se recoge en local.
                string direccion = TipoEntrega == "Domicilio"
                    ? $"{Calle} #{Numero}, {Fraccionamiento}, CP: {CP} ({Referencia})"
                    : "Recoger en tienda";

                // Utiliza StringBuilder para construir el cuerpo del correo electrónico en formato HTML (diseño del ticket).
                var sb = new StringBuilder();

                sb.Append($@"
                <div style='font-family:Arial; max-width:600px; margin:auto; border:1px solid #ddd; border-radius:10px; overflow:hidden;'>
                    <div style='background:#b30000; color:white; padding:15px; text-align:center;'>
                        <h2>🌮 Rancho Taco</h2>
                        <p>Nuevo pedido recibido</p>
                    </div>
                    <div style='padding:15px;'>
                        <h3>👤 Cliente</h3>
                        <p><b>Nombre:</b> {Nombre}</p>
                        <p><b>Teléfono:</b> {Telefono}</p>
                        <h3>🚚 Entrega</h3>
                        <p><b>Tipo:</b> {TipoEntrega}</p>
                        <p><b>Dirección:</b> {direccion}</p>
                        <h3>🧾 Pedido</h3>
                ");

                // Itera sobre los productos del carrito para agregarlos al cuerpo del mensaje.
                foreach (var item in carrito)
                {
                    int cantidad = item.Cantidad > 0 ? item.Cantidad : 1;
                    sb.Append($@"
                        <div style='border-bottom:1px dashed #ccc; padding:5px 0;'>
                            <b>{item.Nombre}</b> x{cantidad} - ${item.Precio}
                        </div>
                    ");
                }

                // Cierre del diseño del ticket con el total y la fecha/hora actual.
                sb.Append($@"
                        <h2 style='margin-top:15px;'>💵 Total: ${total}</h2>
                        <p style='color:gray;'>🕒 {DateTime.Now:dd/MM/yyyy HH:mm}</p>
                    </div>
                    <div style='background:#f5f5f5; padding:10px; text-align:center; font-size:12px; color:#666;'>
                        Rancho Taco © {DateTime.Now.Year}
                    </div>
                </div>
                ");

                string mensaje = sb.ToString();

                // Llama al servicio de email para enviar el ticket generado al correo configurado del negocio.
                _email.EnviarPedido(mensaje);

                // Si el proceso es exitoso, redirige a la vista de confirmación de éxito.
                return RedirectToAction("Confirmacion");
            }
            catch (Exception ex)
            {
                // En caso de error (ej. JSON mal formado o fallo de red), muestra el mensaje de excepción para depuración.
                return Content("Error al procesar el pedido: " + ex.Message);
            }
        }

        // Muestra la vista final que informa al usuario que su pedido ha sido enviado correctamente.
        public IActionResult Confirmacion()
        {
            return View();
        }
    }
}