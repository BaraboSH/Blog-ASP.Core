using System;
using Blog.API.Services.Abstractions;
using Blog.API.ViewModels.Auth;
using Blog.API.ViewModels.Stories;
using Blog.Data.Abstractions;
using Blog.Model.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private IUserRepository _userRepository;
        public AuthController(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        } 

        [HttpPost("login")]
        public ActionResult<AuthData> Post([FromBody] LoginViewModel model)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var user = _userRepository.GetSingle(u => u.Email == model.Email);
            if(user == null)
            {
                return BadRequest(new {email = "no user with this email"});
            }
            var passwordValid = _authService.VerifyPassword(model.Password, user.Password);
            if(!passwordValid)
            {
                return BadRequest(new {password = "invalid password"});
            }
            return _authService.GetAuthData(user.Id);
        }

        [HttpPost("register")]
        public ActionResult<AuthData> Post([FromBody] RegisterViewModel model)
        {
            if(!ModelState.IsValid) return  BadRequest(ModelState);

            var emailUniq = _userRepository.IsEmailUniq(model.Email);
            if(!emailUniq) return BadRequest(new {email = "user with this email already exists"});
            var usernameUniq = _userRepository.IsUsernameUniq(model.Username);
            if(!usernameUniq) return BadRequest(new {username = "user with this username already exists"});

            var id = Guid.NewGuid().ToString();
            var user = new User
            {
                Id = id,
                Email = model.Email,
                UserName = model.Username,
                Password = _authService.HashPassword(model.Password) 
            };
            _userRepository.Add(user);
            _userRepository.Commit();

            return _authService.GetAuthData(id);
        }

        
    }
}