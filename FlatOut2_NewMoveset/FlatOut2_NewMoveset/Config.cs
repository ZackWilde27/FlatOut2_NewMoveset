using System.ComponentModel;
using FlatOut2.SDK.Enums;
using FlatOut2_NewMoveset.Template.Configuration;
using Reloaded.Mod.Interfaces.Structs;

namespace FlatOut2_NewMoveset.Configuration
{
    public class Config : Configurable<Config>
    {
        /*
            User Properties:
                - Please put all of your configurable properties here.
    
            By default, configuration saves as "Config.json" in mod user config folder.    
            Need more config files/classes? See Configuration.cs
    
            Available Attributes:
            - Category
            - DisplayName
            - Description
            - DefaultValue

            // Technically Supported but not Useful
            - Browsable
            - Localizable

            The `DefaultValue` attribute is used as part of the `Reset` button in Reloaded-Launcher.
        */

        [DisplayName("Enable Glide")]
        [Description("Holding shift will cancel out gravity")]
        [DefaultValue(true)]
        public bool EnableGlide { get; set; } = true;

        [DisplayName("Enable Boost")]
        [Description("Pressing the boost button twice will trade a 3rd of your nitro to tripple your speed instantly, kinda like the boost mechanic in sonic games")]
        [DefaultValue(true)]
        public bool EnableBoost { get; set; } = true;

        [DisplayName("Enable Jump")]
        [Description("Press W to jump, the longer you hold it, the higher you'll go")]
        [DefaultValue(true)]
        public bool EnableJump { get; set; } = true;

        [DisplayName("Enable Strafe")]
        [Description("Pressing A or D will cause your car to strafe, for quickly dodging obstacles.")]
        [DefaultValue(true)]
        public bool EnableStrafe { get; set; } = true;

        [DisplayName("Max Velocity")]
        [Description("The max speed you can go when boosting. I had to cap it so you don't start phasing through the level")]
        [DefaultValue(150.0f)]
        public float MaxVelocity { get; set; } = 150.0f;

        [DisplayName("Jump Key")]
        [Description("You can remap the jump button if you want")]
        [DefaultValue(KeyboardKeys.W)]
        public KeyboardKeys JumpKey { get; set; } = KeyboardKeys.W;

        [DisplayName("Glide Key")]
        [Description("You can remap the glide button if you want")]
        [DefaultValue(KeyboardKeys.S)]
        public KeyboardKeys GlideKey { get; set; } = KeyboardKeys.S;

        [DisplayName("Strafe-Left Key")]
        [Description("You can remap the Strafe buttons if you want")]
        [DefaultValue(KeyboardKeys.A)]
        public KeyboardKeys StrafeLeftKey { get; set; } = KeyboardKeys.A;

        [DisplayName("Strafe-Right Key")]
        [Description("You can remap the Strafe buttons if you want")]
        [DefaultValue(KeyboardKeys.D)]
        public KeyboardKeys StrafeRightKey { get; set; } = KeyboardKeys.D;
    }

    /// <summary>
    /// Allows you to override certain aspects of the configuration creation process (e.g. create multiple configurations).
    /// Override elements in <see cref="ConfiguratorMixinBase"/> for finer control.
    /// </summary>
    public class ConfiguratorMixin : ConfiguratorMixinBase
    {
        // 
    }
}
