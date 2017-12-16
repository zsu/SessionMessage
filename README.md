# What is SessionMessage

SessionMessage is an asp.net MVC/Web Form library for Modal dialog/StatusBar notifications.

Some of the features of SessionMessage are:

  * Support modal dialog blocking notification and StatusBar non-blocking notification
  * Support cross page notification
  * Support Ajax request notification
  * Customize options of display position, display timeout, animation effect,etc.

# Demo
  * Demo can be found at: http://devs.azurewebsites.net/

# NuGet
```xml
Install-Package SessionMessages.Mvc
```
# Getting started with SessionMessage

  * Reference SessionMessages.Core.dll
  * Call SessionMessageManager.SetMessage(MessageType.Success, MessageBehaviors.StatusBar, "your notification message") when you want to display message;
  * Use it on your page;
```xml
Razor:
@using SessionMessages.Mvc

WebForms:
<%@ Import Namespace="SessionMessages.Mvc" %>
```
```xml
Razor:
@Html.FluentSessionMessage().DisplayPosition(Position.TopRight).Timeout(5000)

WebForms:
<%@Html.FluentSessionMessage().DisplayPosition(Position.TopRight).Timeout(5000).ImagePath("/Content/Images/") %> 
```
  * Link to Jquery/JQuery.UI/toastr on your page(Before the call to @Html.FluentSessionMessage()): 
```xml
    <link href="http://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" rel="stylesheet"/>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/css/toastr.min.css" rel="stylesheet"/>
    <script src='https://code.jquery.com/jquery-1.12.4.min.js' type='text/javascript'></script>
    <script src='https://code.jquery.com/ui/1.12.1/jquery-ui.min.js' type='text/javascript'></script>
    <script src='https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js' type='text/javascript'></script>
```
  * Place fail.gif,info.gif,ok.gif,warn.gif in /Content/Images folder or use extension function ImagePath to set the actual path
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
# Screenshots
![SessionMessage](screenshots/modaldialog.jpg?raw=true "modaldialog")
![SessionMessage](screenshots/statusbar_success.jpg?raw=true "statusbar success")
![SessionMessage](screenshots/statusbar_error.jpg?raw=true "statusbar error")
![SessionMessage](screenshots/statusbar_warning.jpg?raw=true "statusbar warning")
![SessionMessage](screenshots/statusbar_info.jpg?raw=true "statusbar info")

# License
All source code is licensed under MIT license - http://www.opensource.org/licenses/mit-license.php
