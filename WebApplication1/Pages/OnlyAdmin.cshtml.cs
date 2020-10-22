﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Pages
{
    public class OnlyAdminModel : PageModel
    {
        private readonly ILogger<OnlyAdminModel> _logger;

        public OnlyAdminModel(ILogger<OnlyAdminModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}