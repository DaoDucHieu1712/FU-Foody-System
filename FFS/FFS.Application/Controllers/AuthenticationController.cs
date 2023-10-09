using System.Net.Mail;
using System.Text.RegularExpressions;

using AutoMapper;

using FFS.Application.Data;
using FFS.Application.DTOs;
using FFS.Application.Entities;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FFS.Application.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticateController : ControllerBase {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AuthenticateController(ApplicationDbContext db, UserManager<ApplicationUser> userManager) {
            _db = db;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterShipper(ShipperRegisterDTO shipperRegisterDTO)
        {
            try
            {
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

                Regex regex = new Regex(pattern);

                if (!regex.IsMatch(shipperRegisterDTO.email))
                {
                    throw new Exception("Email không hợp lệ!");
                }
                //_userManager.CreateAsync(shipperRegisterDTO, shipperRegisterDTO.password);
                return Ok();
           
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
