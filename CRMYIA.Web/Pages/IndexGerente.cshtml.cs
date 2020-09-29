using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CRMYIA.Web.Pages
{
    [Authorize]
    public class IndexGerenteModel : PageModel
    {
        private readonly ILogger<IndexGerenteModel> _logger;

        public IndexGerenteModel(ILogger<IndexGerenteModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Session.GetObjectFromJson<List<Modulo>>("MODULO") == null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToPage("Login");
            }
            return Page();
        }
    }
}
