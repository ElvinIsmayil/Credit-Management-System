using Credit_Management_System.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Credit_Management_System.Areas.Admin.ViewComponents
{
    public class BreadCrumbViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<BreadCrumbItem> items)
        {
            // You can add default items here if you want them on every page
            // For example, always start with "Home"
            if (!items.Any() || items.All(i => i.Text != "Home"))
            {
                items.Insert(0, new BreadCrumbItem { Text = "Home", Url = Url.Action("Index", "Dashboard", new { area = "Admin" }) });
            }

            // Ensure the last item is marked as active if it's not already
            if (items.LastOrDefault() != null && !items.Last().IsActive)
            {
                items.Last().IsActive = true;
                items.Last().Url = null; // Active item usually doesn't have a URL
            }

            return View(items);
        }
    }
}
