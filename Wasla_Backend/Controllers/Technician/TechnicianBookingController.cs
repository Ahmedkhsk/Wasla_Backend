using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wasla_Backend.Controllers.Technician
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicianBookingController : ControllerBase
    {
        private readonly ITechnicianBookingService _technicianBookingService;
        public TechnicianBookingController(ITechnicianBookingService technicianBookingService)
        {
            _technicianBookingService = technicianBookingService;
        }
    }
}
