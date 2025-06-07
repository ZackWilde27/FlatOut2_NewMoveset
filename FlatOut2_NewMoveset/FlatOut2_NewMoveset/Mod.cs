using FlatOut2_NewMoveset.Configuration;
using FlatOut2_NewMoveset.Template;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using FlatOut2.SDK;
using FlatOut2.SDK.API;
using FlatOut2.SDK.Structs;
using FlatOut2.SDK.Enums;
using System.Numerics;

namespace FlatOut2_NewMoveset
{
    /// <summary>
    /// Your mod logic goes here.
    /// </summary>
    public class Mod : ModBase // <= Do not Remove.
    {
        /// <summary>
        /// Provides access to the mod loader API.
        /// </summary>
        private readonly IModLoader _modLoader;

        /// <summary>
        /// Provides access to the Reloaded.Hooks API.
        /// </summary>
        /// <remarks>This is null if you remove dependency on Reloaded.SharedLib.Hooks in your mod.</remarks>
        private readonly IReloadedHooks? _hooks;

        /// <summary>
        /// Provides access to the Reloaded logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Entry point into the mod, instance that created this class.
        /// </summary>
        private readonly IMod _owner;

        /// <summary>
        /// Provides access to this mod's configuration.
        /// </summary>
        private Config _configuration;

        /// <summary>
        /// The configuration of the currently executing mod.
        /// </summary>
        private readonly IModConfig _modConfig;

        private static float StoredJump = 0.0f;
        private static bool Jumped = false;
        private static bool MoonGravity = false;

        private static int FramesSinceLastPress = 0;

        private static float maxMagnitude = 150.0f;

        private static unsafe void CheckBoost(Car* car)
        {
            if (Info.Controller.IsKeyPressed(KeyboardKeys.Control))
            {
                if (FramesSinceLastPress > 0)
                {
                    if (car->Nitro > 1.5f)
                    {
                        car->Velocity.X = float.Clamp(car->Velocity.X * 3.0f, -maxMagnitude, maxMagnitude);
                        car->Velocity.Z = float.Clamp(car->Velocity.Z * 3.0f, -maxMagnitude, maxMagnitude);
                        car->Nitro -= 1.5f;
                    }
                }

                // Gives you a 50-frame time limit to press control again and activate the boost move
                FramesSinceLastPress = 50;
            }

            if (FramesSinceLastPress > 0)
                FramesSinceLastPress--;
        }

        private static KeyboardKeys GlideKey;
        private static KeyboardKeys JumpKey;
        private static KeyboardKeys StrafeLeftKey;
        private static KeyboardKeys StrafeRightKey;

        private static unsafe void CheckGlide(Car* car)
        {
            if (Info.Controller.IsKeyHeld(GlideKey))
                car->Velocity.Y = Math.Max(car->Velocity.Y, 0.0f);
        }

        private static unsafe void CheckSidestep(Car* car)
        {
            if (Info.Controller.IsKeyPressed(StrafeLeftKey))
            {
                Vector3 forward = new(car->Velocity.X, 0.0f, car->Velocity.Z);
                forward /= forward.Length();
                Vector3 right = Vector3.Cross(forward, new(0.0f, 1.0f, 0.0f));

                car->Velocity += new Vector4(right * 16.0f, 0.0f);
            }

            if (Info.Controller.IsKeyPressed(StrafeRightKey))
            {
                Vector3 forward = new(car->Velocity.X, 0.0f, car->Velocity.Z);
                forward /= forward.Length();
                Vector3 right = Vector3.Cross(forward, new(0.0f, 1.0f, 0.0f));

                car->Velocity += new Vector4(-right * 16.0f, 0.0f);
            }
        }

        private static unsafe void CheckJump(Car* car)
        {
            if (Info.Controller.IsKeyPressed(JumpKey))
            {
                StoredJump = 0.0f;
                Jumped = false;
            }

            if (Info.Controller.IsKeyHeld(JumpKey))
                StoredJump = MathF.Min(StoredJump + 0.25f, 8.0f);
            else if (!Jumped)
            {
                car->Velocity.Y = StoredJump;
                Jumped = true;
            }
        }

        private static bool EnableBoost;
        private static bool EnableGlide;
        private static bool EnableJump;
        private static bool EnableStrafe;

        private static unsafe void PerFrame()
        {
            var hostObject = *PlayerHost.Instance;
            if (hostObject != null)
            {
                Car* car = hostObject->Players[0]->Car;
                if (EnableBoost)
                    CheckBoost(car);

                if (EnableGlide)
                    CheckGlide(car);

                if (EnableJump)
                    CheckJump(car);

                if (EnableStrafe)
                    CheckSidestep(car);
            }
        }

        public unsafe Mod(ModContext context)
        {
            _modLoader = context.ModLoader;
            _hooks = context.Hooks;
            _logger = context.Logger;
            _owner = context.Owner;
            _configuration = context.Configuration;
            _modConfig = context.ModConfig;

            SDK.Init(_hooks!);
            Helpers.HookPerFrame(PerFrame);

            // Makes it so everything can remain static
            GlideKey = _configuration.GlideKey;
            JumpKey = _configuration.JumpKey;
            StrafeLeftKey = _configuration.StrafeLeftKey;
            StrafeRightKey = _configuration.StrafeRightKey;
            EnableBoost = _configuration.EnableBoost;
            EnableGlide = _configuration.EnableGlide;
            EnableJump = _configuration.EnableJump;
            EnableStrafe = _configuration.EnableStrafe;
            maxMagnitude = _configuration.MaxVelocity;
        }

        #region Standard Overrides
        public override void ConfigurationUpdated(Config configuration)
        {
            // Apply settings from configuration.
            // ... your code here.
            _configuration = configuration;
            _logger.WriteLine($"[{_modConfig.ModId}] Config Updated: Applying");
        }
        #endregion

        #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Mod() { }
#pragma warning restore CS8618
        #endregion
    }
}