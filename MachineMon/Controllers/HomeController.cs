﻿using MachineMon.DataAccess.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MachineMon.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"];
            var repo = new DataAccess.Repositories.Repository(connectionString);
            var messages = repo.GetAll<Message>("SELECT * FROM Message");
            messages = messages.OrderBy(m => m.Sender).ThenByDescending(m => m.MessageDateTimeUtc);
            ViewBag.Messages = messages;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}