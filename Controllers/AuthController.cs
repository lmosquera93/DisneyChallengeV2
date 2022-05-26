using DisneyChallengeV2.Data;
using DisneyChallengeV2.Models.Users;
using DisneyChallengeV2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisneyChallengeV2.Controllers
{
    [Route("auth/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        ITokenHandlerService _service;
        private IMailServices _mailServices;

        public AuthController(UserManager<IdentityUser> userManager, ITokenHandlerService service, IMailServices mailServices)
        {
            _userManager = userManager;
            _service = service;
            _mailServices = mailServices;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest user)
        {
            if (ModelState.IsValid)
            {
                var existeUser = await _userManager.FindByEmailAsync(user.Email);
                if (existeUser != null)
                {
                    return BadRequest("Existe el correo ingresado.");
                }
                else
                {
                    var nuevoUser = await _userManager.CreateAsync(new IdentityUser() { Email = user.Email, UserName = user.Email }, user.Password);
                    if (nuevoUser.Succeeded)
                    {
                        await _mailServices.SendEmailAsync(user.Email, "Usuario registrado correctamente.", "El usuario fue creado correctamente.");
                        return Ok("Se creo el Usuario.");
                    }
                    else
                    {
                        return BadRequest("No se creo correctamente.");

                    }
                }
            }
            else
            {
                return BadRequest("No se creo correctamente.");
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {
                var existeUser = await _userManager.FindByEmailAsync(user.Email);

                if (existeUser == null)
                {
                    return BadRequest(new UserLoginResponse()
                    {
                        Login = false,
                        Errors = new List<String>()
                        {
                            "Usuario o contraseña incorrecto."
                        }
                    });
                }

                var estaCorrecto = await _userManager.CheckPasswordAsync(existeUser, user.Password);

                if (estaCorrecto)
                {
                    var pars = new TokenParameters()
                    {
                        Id = existeUser.Id,
                        PasswordHash = existeUser.PasswordHash,
                        UserName = existeUser.UserName
                    };

                    var jwtToken = _service.GenerateJwtToken(pars);

                    return Ok(new UserLoginResponse()
                    {
                        Login = true,
                        Token = jwtToken
                    });

                }
                else
                {
                    return BadRequest(new UserLoginResponse()
                    {
                        Login = false,
                        Errors = new List<String>()
                        {
                            "Usuario o contraseña incorrecto."
                        }
                    });
                }
            }
            else
            {
                return BadRequest(new UserLoginResponse()
                {
                    Login = false,
                    Errors = new List<String>()
                        {
                            "Usuario o contraseña incorrecto."
                        }
                });
            }
        }

    }
}