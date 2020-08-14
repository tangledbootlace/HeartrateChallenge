using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorHRC.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlazorHRC.Data.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        [Route("Login")]
        public bool Authorized()
        {
            var cookie = Request.Cookies["AuthToken"];

            return cookie != null;
        }
    }
}
