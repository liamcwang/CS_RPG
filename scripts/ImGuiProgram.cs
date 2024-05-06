using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

using static ImGuiNET.ImGuiNative;

namespace ImGuiNET {
    public class ImGuiProgram {
        protected Sdl2Window _window;
        protected GraphicsDevice _gd;
        protected ImGuiController _controller;
        protected CommandList _cl;

        public delegate void GuiRefresh();
        public event GuiRefresh guiUpdate;

        public static Vector3 _clearColor = new Vector3(0.45f, 0.55f, 0.6f);

        public void Run() {
            VeldridStartup.CreateWindowAndGraphicsDevice(
                new WindowCreateInfo(50, 50, 1280, 720, WindowState.Normal, "Big Chungus"), 
                new GraphicsDeviceOptions(true, null, true, ResourceBindingModel.Improved, true, true), 
                out _window,
                out _gd);
            _window.Resized += () =>
            {
                _gd.MainSwapchain.Resize((uint)_window.Width, (uint)_window.Height);
                _controller.WindowResized(_window.Width, _window.Height);
            };
            _cl = _gd.ResourceFactory.CreateCommandList();
            _controller = new ImGuiController(_gd, _gd.MainSwapchain.Framebuffer.OutputDescription, _window.Width, _window.Height);

            var stopwatch = Stopwatch.StartNew();
            float deltaTime = 0f;
            while (_window.Exists)
            {
                deltaTime = stopwatch.ElapsedTicks / (float)Stopwatch.Frequency;
                stopwatch.Restart();
                InputSnapshot snapshot = _window.PumpEvents();
                if (!_window.Exists) { break; }
                RunProgram();
                _controller.Update(deltaTime, snapshot);

                // TODO: should probably invoke some kind of event here
                SubmitUI();

                _cl.Begin();
                _cl.SetFramebuffer(_gd.MainSwapchain.Framebuffer);
                _cl.ClearColorTarget(0, new RgbaFloat(_clearColor.X, _clearColor.Y, _clearColor.Z, 1f));
                _controller.Render(_gd, _cl);
                _cl.End();
                _gd.SubmitCommands(_cl);
                _gd.SwapBuffers(_gd.MainSwapchain);
                
            }
            _gd.WaitForIdle();
            _controller.Dispose();
            _cl.Dispose();
            _gd.Dispose();
        }

        protected virtual void RunProgram() {
            guiUpdate?.Invoke();
        }

        protected virtual unsafe void SubmitUI()
        {
            
        }

        
    }
}
