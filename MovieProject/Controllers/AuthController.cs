using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieProject.Models;

namespace MovieProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<User> _simgr;
        private readonly UserManager<User> _umgr;

        public LoginController(SignInManager<User> sim, UserManager<User> um)
        {
            _simgr = sim;
            _umgr = um;
        }

        [AllowAnonymous]
        [Route("login/{userName}/{pwd}")]
        [HttpPost]
        public async Task<IActionResult> LoginUser(string userName, string pwd)
        {
            var result = await _simgr.PasswordSignInAsync(userName, pwd, true, false);

            if (result.Succeeded) return Ok();

            return Unauthorized();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("logout/")]
        public async Task<IActionResult> Logout()
        {
            await _simgr.SignOutAsync();
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("create/{userName}/{pwd}")]
        public async Task<IActionResult> CreateUser(string userName, string pwd)
        {
            var user = new User { UserName = userName };


            var result = await _umgr.CreateAsync(user, pwd);
            if (result.Succeeded)
            {
                await _simgr.SignInAsync(user, isPersistent: false);
                return Ok("User created a new account with password.");
            }
    
            return Unauthorized();
        }

    }
}