using MachineMon.Core.Domain;
using System.Web.Mvc;
using System;
using MachineMon.Repository.Dapper.Repositories;

namespace MachineMon.Web.Controllers
{
    public class HostController : Controller
    {
        // GET: Host
        public ActionResult Index()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"];
            var repo = new GenericRepository(connectionString);
            var hosts = repo.GetAll<Host>("SELECT * FROM Host");
            return View(hosts);
        }
    }
}