namespace AcopioAPIs.DTOs.User
{
    public class UserModulesResultDto
    {
        public int ModuleId { get; set; }

        public string ModuleName { get; set; } = null!;

        public string? ModuleIcon { get; set; }

        public string? ModuleColor { get; set; }

        public string? ModuleRoute { get; set; }

        public List<UserSubModulesResultDto>? SubModules { get; set; }
    }
    public class UserSubModulesResultDto
    {
        public int ModuleId { get; set; }

        public string ModuleName { get; set; } = null!;

        public string? ModuleIcon { get; set; }

        public string? ModuleColor { get; set; }

        public string? ModuleRoute { get; set; }
    }
}
