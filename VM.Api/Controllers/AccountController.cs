using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VM.Api.Models;
using VM.Core.Entities;

namespace VM.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<VmUser> _signInManager;
        private readonly UserManager<VmUser> _userManager;


        public AccountController(SignInManager<VmUser> signInManager, UserManager<VmUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new VmUser { UserName = model.Email, FirstName = model.Name, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "VM_ADMIN");

            return Ok(new { userCreated = true, userName = model.Email });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] RegisterRequestModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var userFound = await _userManager.FindByNameAsync(loginViewModel.Email);

                if (userFound == null) return Unauthorized();

                var result = await _signInManager.PasswordSignInAsync(userFound.UserName, loginViewModel.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return Ok();
                }
                
            }


            
            

            return BadRequest(ModelState);
        }
    }
}
