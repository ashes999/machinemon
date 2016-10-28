using MachineMon.Core.Domain;
using System.Web.Mvc;
using System;
using MachineMon.Repository.Dapper.Repositories;
using MachineMon.Core.Repositories;
using System.Linq;
using MachineMon.Core.Services;

namespace MachineMon.Web.Controllers
{
    public class HostController : Controller
    {
        private IGenericRepository genericRepository;
        private IEncryptionService encryptionService;

        public HostController(IGenericRepository repository, IEncryptionService encryptionService)
        {
            this.genericRepository = repository;
            this.encryptionService = encryptionService;
        }

        // GET: Host
        public ActionResult Index()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"];
            var hosts = genericRepository.GetAll<Host>();
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

            host.Password = this.encryptionService.Encrypt(host.Password);
            this.genericRepository.Insert<Host>(host);

            TempData.Add("Message", string.Format("Host added: {0}.", host.FriendlyName));
            return RedirectToAction("Index");
        }
    }
}