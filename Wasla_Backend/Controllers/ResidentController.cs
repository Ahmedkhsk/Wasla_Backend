namespace Wasla_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResidentController : ControllerBase
    {
        public IResidentService _residentService;
        public IResidentIdentityRepository _residentIdentityRepository;
        public ResidentController(IResidentService residentService,IResidentIdentityRepository residentRepository)
        {
            _residentService = residentService;
            _residentIdentityRepository = residentRepository;
        }
        [HttpPost("CompleteRegister")]
        public async Task<IActionResult> CompleteRegister([FromForm] ResidentCompleteRegisterDto model, string lan = "en")
        {
         
                if (!ModelState.IsValid)
                    return BadRequest(ResponseHelper.Fail("InvalidData", lan, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                await _residentService.CompleteResidentRegister(model);
                return Ok(ResponseHelper.Success("CompleteResidentRegisterSuccess", lan));
        
        }
        [HttpPost("UploadIdentity")]
        public Task AddIdentity(string NationalI,string gmail)
        {
            var residentIdentity =new ResidentIdentity { NationalId = NationalI,Gmail=gmail  };
            _residentIdentityRepository.AddAsync(residentIdentity);
            return _residentIdentityRepository.SaveChangesAsync();
        }
        [HttpPut("edit-Profile")]
        public async Task<IActionResult> EditProfile(EditProfileDto editProfileDto, string lan = "en")
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail("InvalidData", lan, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            await _residentService.EditProfile(editProfileDto);
            return Ok(ResponseHelper.Success("ProfileEditSuccess", lan));
        }

        [HttpGet("get-Profile")]
        public async Task<IActionResult> GetProfile(string userId, string lan = "en")
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail("InvalidData", lan, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            var response = await _residentService.GetProfile(userId);
            return Ok(ResponseHelper.Success("GetProfileSuccess", lan, response));
        }
    }
}
