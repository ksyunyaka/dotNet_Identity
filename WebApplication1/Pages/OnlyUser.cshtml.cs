using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Pages
{
    public class OnlyUserModel : PageModel
    {
        private readonly ILogger<OnlyUserModel> _logger;

        public OnlyUserModel(ILogger<OnlyUserModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
