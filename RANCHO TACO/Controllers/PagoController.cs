using Microsoft.AspNetCore.Mvc;
using RANCHO_TACO.Services;
using RANCHO_TACO.Models;
using System.Text;
using System.Text.Json;

namespace RANCHO_TACO.Controllers
{
    public class PagoController : Controller
    {
        private readonly EmailService _email;

        public PagoController(EmailService email)
        {
            _email = email;
        }

        // Vista de pago
        public IActionResult Index()
        {
            return View();
        }

        // 🔥 CONFIRMAR PAGO
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
            string CarritoJson // 🔥 IMPORTANTE
        )
        {
            try
            {
                // 🔹 Convertir carrito
                var carrito = string.IsNullOrEmpty(CarritoJson)
                    ? new List<CarritoItem>()
                    : JsonSerializer.Deserialize<List<CarritoItem>>(CarritoJson) ?? new List<CarritoItem>();

                // 🔹 Total
                decimal total = carrito.Sum(p => p.Precio * (p.Cantidad > 0 ? p.Cantidad : 1));

                // 🔹 Dirección
                string direccion = TipoEntrega == "Domicilio"
                    ? $"{Calle} #{Numero}, {Fraccionamiento}, CP: {CP} ({Referencia})"
                    : "Recoger en tienda";

                // 🔹 Construir correo tipo ticket
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

                foreach (var item in carrito)
                {
                    int cantidad = item.Cantidad > 0 ? item.Cantidad : 1;

                    sb.Append($@"
                        <div style='border-bottom:1px dashed #ccc; padding:5px 0;'>
                            <b>{item.Nombre}</b> x{cantidad} - ${item.Precio}
                        </div>
                    ");
                }

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

                // 🔹 Enviar correo
                _email.EnviarPedido(mensaje);

                return RedirectToAction("Confirmacion");
            }
            catch (Exception ex)
            {
                // 🔥 Para debug
                return Content("Error al procesar el pedido: " + ex.Message);
            }
        }

        public IActionResult Confirmacion()
        {
            return View();
        }
    }
}