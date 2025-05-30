using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Management_System.Areas.Admin.Controllers.Common;

[Authorize(Roles = "SuperAdmin, Admin")]
public class BaseAdminController : Controller
{

}