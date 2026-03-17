
namespace Wasla_Backend.Enums
{
    public enum TechnicianSpecialty
    {
        [Display(Name = "Plumber", Description = "سباك")]
        Plumber = 1,

        [Display(Name = "Electrician", Description = "كهربائي")]
        Electrician = 2,

        [Display(Name = "Carpenter", Description = "نجار")]
        Carpenter = 3,

        [Display(Name = "AC Technician", Description = "فني تكييف")]
        AC_Technician = 4,

        [Display(Name = "Appliance Repair", Description = "فني صيانة أجهزة منزلية")]
        ApplianceRepair = 5,

        [Display(Name = "Painter", Description = "نقاش")]
        Painter = 6,

        [Display(Name = "Satellite Technician", Description = "فني دش")]
        SatelliteTechnician = 7,

        [Display(Name = "Locksmith", Description = "صانع أقفال")]
        Locksmith = 8,

        [Display(Name = "Glass Technician", Description = "فني زجاج")]
        GlassTechnician = 9,

        [Display(Name = "Flooring Technician", Description = "فني أرضيات")]
        FlooringTechnician = 10
    }
}