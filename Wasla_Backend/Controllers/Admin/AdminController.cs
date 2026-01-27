namespace Wasla_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        
        [HttpGet("CollectedCountBookings/{status}")]
        public async Task<IActionResult> GetCollectedCountBookingsPerYear(BookingStatus status,string lan = "en")
        {
            var result = await _adminService.GetCollectedCountBookingsPerYear(status);
            return Ok(ResponseHelper.Success("CollectedCountBookingsSuccess", lan, result));
        }

        [HttpPost("ChangeUserStatus")]
        public async Task<IActionResult> ChangeUserStatus(ChangeUserStsatusDto changeUserStsatus, string lan = "en")
        {
            await _adminService.ChangeUserStatus(changeUserStsatus);
            return Ok(ResponseHelper.Success("SuccessToChangeUserStatus", lan));
        }

        [HttpPost("AddContact")]
        public async Task<IActionResult> AddContact(ContactUsDto contactUsDto,string lan = "en")
        {
            await _adminService.AddContact(contactUsDto);
            return Ok(ResponseHelper.Success("SuccessToAddContact", lan));
        }

        [HttpGet("GetContacts")]
        public async Task<IActionResult> GetContacts(string lan = "en")
        {
            var result = await _adminService.GetContacts();
            return Ok(ResponseHelper.Success("SuccessToGetContacts", lan,result));
        }

        [HttpGet("UserApprove")]
        public async Task<IActionResult> UserApproveResponses(string roleId, int pageNumber = 1,int pageSize = 10,string lan = "en")
        {
            var result = await _adminService.UserApproveResponses(roleId,pageNumber,pageSize);
            return Ok(ResponseHelper.Success("SuccessToGetUserApproveResponses", lan, result));
        }
    }
}
