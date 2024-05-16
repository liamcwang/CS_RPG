using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

using CollectionsUtil;
using static EventUtil.EventLogs;

namespace ImGuiNET
{
    public class ImGuiProgramMain : ImGuiProgram
    {
        private const int PAGE_BUFFER = 64;

        private static int _counter = 0;
        private static int _pageCounter = 0;
        private static int _pagePointer = 0;
        private static string[] _messageLog = new string[PAGE_BUFFER];
        // private static string _messageLog = "";
        private static Queue<string> _messageBuffer = new Queue<string>();
        private static Vector2 _scrollSize = new Vector2(300,200);

        
        public ImGuiProgramMain() {
            windowName = "MyProgram";
            SendLog += ReceiveMessage;
        }

        protected override unsafe void SubmitUI()
        {
            if (ImGui.BeginChild("Scrollable", _scrollSize, (ImGuiChildFlags) 1)) {
                if (_pageCounter == 0) {
                    _messageLog[_pagePointer] = "";
                }
                if (_messageBuffer.Count() > 0) {
                    
                    _messageLog[_pagePointer] += _messageBuffer.Dequeue() + '\n';
                    _pageCounter++;
                    

                    // _messageLog += _messageBuffer.Dequeue() + '\n';

                }
                // ImGui.InputTextMultiline("##ScrollableTextbox", ref _messageLog, BufferSize, _scrollSize);
                for (int i = 0; i < _pagePointer + 1; i++) {
                    ImGui.TextWrapped(_messageLog[i]);
                }
                if (_pageCounter >= 32) {
                    _pagePointer++;
                    _pageCounter = 0;
                    if(_pagePointer == PAGE_BUFFER) {
                        _messageLog.RemoveFirstElement();
                        _pagePointer--;
                    }
                }
                // ImGui.TextWrapped(_messageLog);
                ImGui.EndChild();
            }
            if (ImGui.Button("Simulate Combat")) GameManager.StartCombat();
            if (ImGui.Button("Console MessageLog")) Console.WriteLine(_messageLog);
        }

        private void ReceiveMessage(string message) {
            _messageBuffer.Enqueue(message);
        }
    }
}