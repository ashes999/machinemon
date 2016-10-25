using MachineMon.Core.Domain;
using System.Web.Mvc;
using System;
using MachineMon.Repository.Dapper.Repositories;
using MachineMon.Core.Repositories;

namespace MachineMon.Web.Controllers
{
    public class HostController : Controller
    {
        private IGenericRepository genericRepository;

        public HostController(IGenericRepository repository)
        {
            this.genericRepository = repository;
        }

        // GET: Host
        public ActionResult Index()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"];
            var hosts = genericRepository.GetAll<Host>("SELECT * FROM Host");
            return View(hosts);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Host host)
        {
            // TODO: validation.
            host.Id = Guid.NewGuid();
            this.genericRepository.Insert<Host>(host);

            TempData.Add("Message", string.Format("Host added: {0}.", host.FriendlyName));
            return RedirectToAction("Index");
        }
    }
}