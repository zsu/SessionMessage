using SessionMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcExample.Controllers
{
	public class HomeController : Controller
	{
		[HttpGet]
		public ActionResult Index()
		{
			var vm = new SessionMessageViewModel { Type = MessageType.Success, Behaviors = MessageBehaviors.StatusBar };
			return View(vm);
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
		[HttpPost]
		public ActionResult Index(SessionMessageViewModel vm)
		{
			ViewBag.Message = "Message page.";
			SessionMessageManager.SetMessage(vm.Type, vm.Behaviors, vm.Message);
			return View(vm);
		}
	}
	public class SessionMessageViewModel
	{
		public string Caption { get; set; }
		public string Message { get; set; }
		public MessageType Type { get; set; }
		public MessageBehaviors Behaviors { get; set; }
	}
}