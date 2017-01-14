using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SessionMessages
{
	public static class SessionMessageHelper
	{
		public static FluentSessionMessage FluentSessionMessage(this HtmlHelper helper)
		{
			return new FluentSessionMessage();
		}
	}
}
