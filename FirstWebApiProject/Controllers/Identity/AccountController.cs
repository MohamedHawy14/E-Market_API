using FirstWebApiProject.DTO.Identity;
using FirstWebApiProject.Interfaces;
using FirstWebApiProject.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FirstWebApiProject.Controllers.Identity
{
    [Route("Account/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
          
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser();
                applicationUser.UserName = registerDTO.UserName;
                applicationUser.Email = registerDTO.Email;
                IdentityResult result = await userManager.CreateAsync(applicationUser, registerDTO.Password);
                if (result.Succeeded)
                {
                    return Created();
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("Password", item.Description);
                    }
                }

            }

            return BadRequest(ModelState);

        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = await userManager.FindByNameAsync(loginDTO.UserName);
                if (applicationUser != null)
                {
                    bool Found = await userManager.CheckPasswordAsync(applicationUser, loginDTO.Password);
                    if (Found == true)
                    {
                        //genrate token 



                        List<Claim> claims = new List<Claim>();
                        //Token Id that Changed (jti)
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        claims.Add(new Claim(ClaimTypes.NameIdentifier, applicationUser.Id));
                        claims.Add(new Claim(ClaimTypes.Name, applicationUser.UserName));
                        var UserRoles = await userManager.GetRolesAsync(applicationUser);
                        foreach (var RoleName in UserRoles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, RoleName));
                        }

                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecurityKey"]));

                        var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                        //design token
                        var MyToken = new JwtSecurityToken
                            (
                            issuer: configuration["JWT:issuerIP"],
                            audience: configuration["JWT:audienceIP"],
                            expires: DateTime.Now.AddHours(1),
                            claims: claims,
                            signingCredentials: signingCred
                            );

                        //response token
                        return Ok
                        (new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(MyToken),
                            expirition = DateTime.Now.AddHours(1)
                        }
                        );
                    }
                }
                ModelState.AddModelError("UserName", "UserName Or Password Is InValid");
            }
            return BadRequest(ModelState);
        }

    }
}
