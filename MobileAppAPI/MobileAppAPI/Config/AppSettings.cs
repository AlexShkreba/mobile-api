using System.ComponentModel.DataAnnotations;

namespace MobileAppAPI.Config
{
    public class LaunchSettings
    {
        public enum AppMode
        {
            Dev,
            Tests,
        }
        [Required]
        public AppMode Mode { get; set; }
    }
}
