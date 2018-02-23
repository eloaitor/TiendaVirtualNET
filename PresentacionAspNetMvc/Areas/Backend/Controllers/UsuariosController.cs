using System.Web.Mvc;
using TiendaVirtual.Entidades;
using TiendaVirtual.LogicaNegocio;

namespace PresentacionAspNetMvc.Areas.Backend
{
    public class UsuariosController : Controller
    {
        // GET: Backend/Usuarios
        public ActionResult Index()
        {
            var ln = (ILogicaNegocio)HttpContext.Application["logicaNegocio"];
            return View(ln.BuscarTodosUsuarios());
        }

        // GET: Backend/Usuarios/UsuarioEditar/id
        public ActionResult UsuarioEditar(int id)
        {
            var ln = (ILogicaNegocio)HttpContext.Application["logicaNegocio"];
            return View(ln.BuscarUsuarioPorId(id));
        }

        // GET: Backend/Usuarios/Modificar
        public ActionResult Modificar(int id, string nick, string password)
        {
            var ln = (ILogicaNegocio)HttpContext.Application["logicaNegocio"];
            IUsuario usuario = ln.BuscarUsuarioPorId(id);

            usuario.Nick = nick;
            usuario.Password = password;

            ln.ModificarUsuario(usuario);
            return RedirectToAction("Index");
        }

        public ActionResult EliminarUsuario(int id)
        {
            var ln = (ILogicaNegocio)HttpContext.Application["logicaNegocio"];
            ln.BajaUsuario(id);

            return RedirectToAction("Index");

        }

        // POST: Backend/Usuarios/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Anadir(int id, string nick, string password)
        {
            var ln = (ILogicaNegocio)HttpContext.Application["logicaNegocio"];

            Usuario usuario = new Usuario(id, nick, password);
            ln.AltaUsuario(usuario);

            return RedirectToAction("Index");
        }

        public ActionResult AnadirUsuario()
        {
            return View();
        }
    }
}
