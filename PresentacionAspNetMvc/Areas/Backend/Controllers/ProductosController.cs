using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaVirtual.Entidades;
using TiendaVirtual.LogicaNegocio;

namespace PresentacionAspNetMvc.Areas.Backend
{
    public class ProductosController : Controller
    {
        // GET: Productos
        public ActionResult Index()
        {
            var ln = (ILogicaNegocio)HttpContext.Application["logicaNegocio"];
            return View(ln.ListadoProductos());
        }

        [HttpGet]
        public ActionResult ProductEditar(int id)
        {
            var ln = (ILogicaNegocio)HttpContext.Application["logicaNegocio"];
            return View(ln.BuscarProductoPorId(id));
        }

        [HttpPost]
        public string Ficha(int id, int cantidad)
        {
            return $"Los datos recibidos son {id} {cantidad}";
        }


        public ActionResult Modificar(int id, string nombre, decimal precio)
        {
            var ln = (ILogicaNegocio)HttpContext.Application["logicaNegocio"];
            IProducto producto = ln.BuscarProductoPorId(id);

            producto.Nombre = nombre;
            producto.Precio = precio;

            ln.ModificarProducto((Producto)producto);
            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(int id)
        {
            var ln = (ILogicaNegocio)HttpContext.Application["logicaNegocio"];
            ln.BajaProducto(id);
            return RedirectToAction("Index");

        }

        public ActionResult Anadir(int id, string nombre, decimal precio)
        {
            var ln = (ILogicaNegocio)HttpContext.Application["logicaNegocio"];

            Producto producto = new Producto(id, nombre, precio);

            ln.AltaProducto(producto);
            return RedirectToAction("Index");
        }

        public ActionResult AnadirProducto()
        {
            return View();
        }
    }
}