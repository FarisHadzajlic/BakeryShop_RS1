﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PekaraKonjic.Controllers
{
    public class OnamaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}