using MachineMon.DataAccess.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MachineMon.Controllers
{
    public class HostController : Controller
    {
        // GET: Host
        public ActionResult Index()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"];
            var repo = new DataAccess.Repositories.Repository(connectionString);
            var hosts = repo.GetAll<Host>("SELECT * FROM Host");
            return View(hosts);
        }
    }
}