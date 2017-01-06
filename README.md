#What is SessionMessage

SessionMessage is a asp.net MVC/Web Form library for Modal dialog/StatusBar notifications. jQuery is required. The goal is to create a simple core library that can be customized and extended.

Some of the features of SessionMessage are:

  * Support modal dialog blocking notification and StatusBar non-blocking notification
  * Support cross page notification
  * Support Ajax request notification

#Getting started with SessionMessage

  * Reference SessionMessage.dll
  * Call SessionMessageManager.SetMessage(MessageType.Success, MessageBehaviors.StatusBar, "your notification message") when you want to display message;
  * Put @Html.RenderMessages() on your page;
  * Link to Jquery and JQuery.UI on your page: 
    <link href="http://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" rel="stylesheet"/>
    <script src='https://code.jquery.com/jquery-1.12.4.min.js' type='text/javascript'></script>
    <script src='https://code.jquery.com/ui/1.12.1/jquery-ui.min.js' type='text/javascript'></script>
  * Place fail.gif,info.gif,ok.gif,warn.gif in /Content/Images folder
  * If you need Ajax request notification support, put the following in filterconfig.cs
```js
    		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new MvcAjaxMessagesFilterAttribute());
		}
		public static void RegisterWebApiGlobalFilters()
		{
			System.Web.Http.GlobalConfiguration.Configuration.Filters.Add(new AjaxMessagesFilterAttribute());
		}

```
#Screenshots
![SessionMessage](screenshots/modal.jpg?raw=true "modaldialog")
![SessionMessage](screenshots/statusbar_success.jpg?raw=true "statusbar success")
![SessionMessage](screenshots/statusbar_error.jpg?raw=true "statusbar error")
![SessionMessage](screenshots/statusbar_warning.jpg?raw=true "statusbar warning")
![SessionMessage](screenshots/statusbar_info.jpg?raw=true "statusbar info")


