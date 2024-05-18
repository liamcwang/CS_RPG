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
        private const int PAGE_BUFFER_SIZE = 64;

        private static int _counter = 0;
        private static int _pageCounter = 0;
        private static int _pagePointer = 0;
        private static string[] _messageLog = new string[PAGE_BUFFER_SIZE];
        private static Queue<string> _messageBuffer = new Queue<string>();
        private static Vector2 _messageLogSize = new Vector2(300,200);
        // Set flags for the table
        private static ImGuiTableFlags tableFlags = 
        ImGuiTableFlags.RowBg | 
        ImGuiTableFlags.Borders | ImGuiTableFlags.BordersOuter | ImGuiTableFlags.BordersInner |
        ImGuiTableFlags.BordersH | ImGuiTableFlags.BordersOuterH | ImGuiTableFlags.BordersInnerH | 
        ImGuiTableFlags.BordersV | ImGuiTableFlags.BordersOuterV | ImGuiTableFlags.BordersInnerV;
        
        private static bool _showSkillEditor = false;
        private static Combatant _skillEditorCombatant;

        private static Vector2 _mousePos = ImGui.GetMousePos();

        public ImGuiProgramMain() {
            windowName = "MyProgram";
            SendLog += ReceiveMessage;
        }

        protected override unsafe void SubmitUI()
        {
            #region Combat Simulation
            ImGui.Begin("Combat Simulation");
            if (ImGui.BeginChild("Message Log", _messageLogSize, ImGuiChildFlags.Border)) {
                // ImGuiChildFlags 1 indicates that the child should have a border

                if (_pageCounter == 0) {
                    _messageLog[_pagePointer] = "";
                }
                if (_messageBuffer.Count() > 0) {
                    // TODO: Improve space efficiency of pages (Currently adds strings on every newline, rather than total string length)
                    _messageLog[_pagePointer] += _messageBuffer.Dequeue() + '\n';
                    _pageCounter++;
                }
                for (int i = 0; i < _pagePointer + 1; i++) {
                    ImGui.TextWrapped(_messageLog[i]);
                }
                if (_pageCounter >= 32) {
                    _pagePointer++;
                    _pageCounter = 0;
                    if(_pagePointer == PAGE_BUFFER_SIZE) {
                        _messageLog.RemoveFirstElement();
                        _pagePointer--;
                    }
                }
                ImGui.EndChild();
            }

            if (ImGui.Button("Simulate Combat")) GameManager.StartCombat();
            ImGui.End();
            #endregion

            #region Combatant Editor
            // TODO: this doesn't work, I have a few ideas
            /* 1: Edit button that creates a window to edit one row at a time
               2: Research a potential way to edit it like in a spreadsheet
            */
            ImGui.Begin("Combatant Editor");
            if (ImGui.BeginTable("Table", 3, tableFlags)) {
                for (int row = 0; row < GameManager.combatantRef.Count(); row++)
                {
                    ImGui.TableNextRow();

                    Combatant currCombatant = GameManager.combatantRef[row];
                    ImGui.TableSetColumnIndex(0);
                    ImGui.PushID(row);
                    string label = "Name";
                    string currentValue = currCombatant.name;
                    if (ImGui.InputText(label, ref currentValue, 32))
                        currCombatant.name = currentValue;
                    ImGui.PopID();

                    ImGui.TableSetColumnIndex(1);
                    ImGui.PushID(row + 1);
                    label = "Team";
                    currentValue = currCombatant.team.ToString();
                    if (ImGui.InputText(label, ref currentValue, 32)) {
                        int result;
                        if (int.TryParse(currentValue, out result)) {
                            currCombatant.team = result;
                        }
                    }
                    ImGui.PopID();

                    ImGui.TableSetColumnIndex(2);
                    ImGui.PushID(row + 2);
                    label = "Skills";
                    if (ImGui.Button(label)) {
                        if (!_showSkillEditor) {
                            _showSkillEditor = true;
                        }
                        _skillEditorCombatant = currCombatant;
                    }
                    ImGui.TextUnformatted(currCombatant.skills.Count().ToString());
                    ImGui.PopID();


                }
                ImGui.EndTable();

                if (_showSkillEditor) {
                                
                        RightClickCloseWindow(ref _showSkillEditor);
                        
                        ImGui.Begin("Skill Editor");
                        ImGui.Text($"{_skillEditorCombatant.name}");
                        ImGui.Text("Let's fucking go");
                        ImGui.End();
                    }
            }
            ImGui.End();
            #endregion
        }

        private void ReceiveMessage(string message) {
            _messageBuffer.Enqueue(message);
        }

        private void RightClickCloseWindow(ref bool showWindow) {
            Vector2 windowPos = ImGui.GetWindowPos();
            Vector2 windowSize = ImGui.GetWindowSize();
            bool isMouseInWindow = _mousePos.X >= windowPos.X && _mousePos.X <= (windowPos.X + windowSize.X) &&
                    _mousePos.Y >= windowPos.Y && _mousePos.Y <= (windowPos.Y + windowSize.Y);

            if (isMouseInWindow && ImGui.IsMouseDown(ImGuiMouseButton.Right))
                            showWindow = false;
        }
    }
}