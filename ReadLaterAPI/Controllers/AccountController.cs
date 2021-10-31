﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReadLaterAPI.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReadLaterAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        UserManager<IdentityUser> _userManager;
        SignInManager<IdentityUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpPost("register")]
        public async Task<ActionResult<IdentityUser>> Register(RegisterUserDto model)
        {
            IdentityUser user = new IdentityUser()
            {
                UserName = model.Username,
                Email = model.Email
            };

            IdentityResult userResult = await _userManager.CreateAsync(user, model.Password);

            if (userResult.Succeeded)
            {
                bool roleExcist = await _roleManager.RoleExistsAsync("User");
                if (!roleExcist)
                {
                    await AddRole("User");
                }

                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (roleResult.Succeeded)
                {
                    return user;
                }

                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }

            foreach (var error in userResult.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState.Values);
        }

        [HttpPost("signIn")]
        public async Task<IActionResult> SignIn(SignInUserDto model)
        {
            var singInResult = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            if (singInResult.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                var roles = await _userManager.GetRolesAsync(user);

                IdentityOptions identityOptions = new IdentityOptions();
                var claims = new Claim[]
                {
                    new Claim(identityOptions.ClaimsIdentity.UserIdClaimType, user.Id),
                    new Claim(identityOptions.ClaimsIdentity.UserNameClaimType, user.UserName),
                    new Claim(identityOptions.ClaimsIdentity.RoleClaimType, roles[0])
                };

                var singingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this-is-my-secret-key"));
                var signingCredentials = new SigningCredentials(singingKey, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(signingCredentials: signingCredentials,
                    expires: DateTime.Now.AddHours(1), claims: claims);

                var obj = new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    UserId = user.Id,
                    UserName = user.UserName,
                    Role = roles[0]
                };

                return Ok(obj);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("logOut")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return NoContent();
        }

        private async Task<IdentityResult> AddRole(string roleName)
        {
            var role = new IdentityRole();
            role.Name = roleName;
            return await _roleManager.CreateAsync(role);
        }
    }
}
