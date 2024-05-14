using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;
using static EventUtil.GameLogs;

namespace ImGuiNET
{
    public class ImGuiProgramMain : ImGuiProgram
    {
        private static int _counter = 0;
        private static Queue<string> _messageLog = new Queue<string>();
        
        public ImGuiProgramMain() {
            windowName = "MyProgram";
            SendLog += ReceiveMessage;
        }

        protected override unsafe void SubmitUI()
        {
            if (_messageLog.Count() > 0) {
                string message = _messageLog.Dequeue();
                ImGui.Text(message);

            }
            if (ImGui.Button("Button")) GameManager.StartCombat();
        }

        private void ReceiveMessage(string message) {
            _messageLog.Enqueue(message);
        }
    }
}