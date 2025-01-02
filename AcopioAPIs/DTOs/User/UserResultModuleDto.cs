namespace AcopioAPIs.DTOs.User
{
    public class UserResultModuleDto
    {
        public int ModuleId { get; set; }
        public required string ModuleName { get; set; }
        public bool ModuleAgregado { get; set; }
    }
}
