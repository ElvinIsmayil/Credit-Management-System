using Credit_Management_System.Areas.Admin.ViewModels;
using Credit_Management_System.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Credit_Management_System.Areas.Admin.Controllers.Common;

[Authorize(Roles = $"{Roles.Admin},{Roles.SuperAdmin}")]
public abstract class BaseAdminController : Controller 
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        var controllerName = context.RouteData.Values["controller"]?.ToString();
        var actionName = context.RouteData.Values["action"]?.ToString();
        var areaName = context.RouteData.Values["area"]?.ToString();

        // Helper to convert PascalCase/camelCase to readable text (e.g., "roles" -> "Roles")
        // Using TextInfo.ToTitleCase for more robust capitalization
        // You might want to adjust the culture if your app is not English-centric
        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo; // Use current culture for title casing

        string ToReadableText(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            // Add space before capital letters, then trim and apply title case
            return textInfo.ToTitleCase(Regex.Replace(input, "([A-Z])", " $1").Trim());
        }

        var breadcrumbs = new List<BreadCrumbItem>();
        string pageTitle = "";

        // 1. Always add "Home" breadcrumb
        breadcrumbs.Add(new BreadCrumbItem { Text = "Home", Url = Url.Action("Index", "Dashboard", new { area = "Admin" }) });

        // 2. Add Controller-level breadcrumb
        // Avoid adding "Dashboard" if "Home" already links to it, or if it's the root controller
        if (!string.IsNullOrEmpty(controllerName) && controllerName.ToLower() != "dashboard")
        {
            var controllerText = ToReadableText(controllerName);
            // Link to the Index action of the current controller
            breadcrumbs.Add(new BreadCrumbItem { Text = controllerText, Url = Url.Action("Index", controllerName, new { area = areaName }) });
            pageTitle = controllerText;
        }
        else if (controllerName.ToLower() == "dashboard")
        {
            // Explicitly set page title for Dashboard Index (e.g., "Dashboard")
            pageTitle = "Dashboard";
        }

        // 3. Add Action-level breadcrumb (if not the default 'Index' action)
        if (!string.IsNullOrEmpty(actionName) && actionName.ToLower() != "index")
        {
            string actionText;
            switch (actionName.ToLower()) // Use ToLower() for robust comparison
            {
                case "detail":
                    actionText = "Details";
                    break;
                case "create":
                    actionText = "Create New";
                    break;
                case "edit":
                    actionText = "Edit";
                    break;
                case "assignrole": // Specific to your roles example
                    actionText = "Assign Role";
                    break;
                case "removeuserfromrole": // Specific to your roles example
                    actionText = "Remove User";
                    break;
                // Add more specific mappings for common action names if desired
                default:
                    actionText = ToReadableText(actionName);
                    break;
            }

            breadcrumbs.Add(new BreadCrumbItem { Text = actionText }); // This will be the active item

            // Combine controller and action for the page title
            if (!string.IsNullOrEmpty(pageTitle))
            {
                pageTitle += " - " + actionText;
            }
            else
            {
                pageTitle = actionText; // Fallback if controller name was not included (e.g., Dashboard specific actions)
            }
        }
        else // If it is the Index action, set page title based on controller only if not already set
        {
            if (string.IsNullOrEmpty(pageTitle) && !string.IsNullOrEmpty(controllerName))
            {
                pageTitle = ToReadableText(controllerName);
            }
        }


        // 4. Set the last breadcrumb as active and clear its URL
        if (breadcrumbs.Any()) // Ensure there's at least one item
        {
            breadcrumbs.Last().IsActive = true;
            breadcrumbs.Last().Url = null; // The current page doesn't need a link
        }

        // Set ViewData for the view to consume
        ViewData["PageTitle"] = pageTitle;
        ViewData["Breadcrumbs"] = breadcrumbs;
    }
}