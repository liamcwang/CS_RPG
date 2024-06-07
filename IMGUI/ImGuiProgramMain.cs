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
        private enum EditorState {COMBATANT, SKILL};
        private const int PAGE_BUFFER_SIZE = 64;

        private static int _counter = 0;
        private static int _pageCounter = 0;
        private static int _pagePointer = 0;
        private static string[] _messageLog = new string[PAGE_BUFFER_SIZE];
        private static Queue<string> _messageBuffer = new Queue<string>();
        private static Vector2 _messageLogSize = new Vector2(300,200);
        // Set flags for the table
        private static ImGuiTableFlags _tableFlags = 
        ImGuiTableFlags.RowBg | 
        ImGuiTableFlags.Borders | ImGuiTableFlags.BordersOuter | ImGuiTableFlags.BordersInner |
        ImGuiTableFlags.BordersH | ImGuiTableFlags.BordersOuterH | ImGuiTableFlags.BordersInnerH | 
        ImGuiTableFlags.BordersV | ImGuiTableFlags.BordersOuterV | ImGuiTableFlags.BordersInnerV;
        
        private static EditorState _currentEditorState = EditorState.COMBATANT;
        private static string[] _editorNames = {"Combatants", "Skills"};
        private static int _combatantEditorPointer = -1;
        
        private static int _skillEditorPointer = -1;
        private static string[] _skillTargetTypeNames = Enum.GetNames(typeof(TargetType));
        private static string[] _skillRangeTypeNames = Enum.GetNames(typeof(RangeType));

        private static Vector2 _mousePos = ImGui.GetMousePos();



        public ImGuiProgramMain() {
            windowName = "MyProgram";
            SendLog += ReceiveMessage;
            
        }

        protected override unsafe void SubmitUI()
        {
            _mousePos = ImGui.GetMousePos();

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


            #region Game Editor
            if (ImGui.Begin("Game Editor")) 
            {
                for (int i = 0; i < _editorNames.Length; i++) 
                {
                    ImGui.PushID(i);
                    string currName = _editorNames[i];
                    bool editorSelected = _currentEditorState == (EditorState) i;
                    ImGui.Selectable(currName, ref editorSelected);
                    if (editorSelected) 
                    {
                        _currentEditorState = (EditorState) i;
                    }
                    ImGui.PopID();
                }

                #region Combatant Editor
                if (_currentEditorState == EditorState.COMBATANT) 
                {
                    ImGui.Begin("Combatant Editor");
                    // TODO: Implement adding and removing new combatants
                    // if (ImGui.Button("Add New")) {
                    //     GameManager.combatantRef.Add(new Combatant("", 0));
                    // }
                    Vector2 combatantEditorBounds = ImGui.GetContentRegionAvail();
                    if (ImGui.BeginChild("Combatant Selector", new Vector2(combatantEditorBounds.X * 0.2f, combatantEditorBounds.Y), ImGuiChildFlags.Border)) 
                    {
                        if (ImGui.BeginTable("Combatant Editor Table", 1, _tableFlags)) 
                        {
                            // TODO: Remove every reference to GameManager
                            for (int i = 0; i < GameManager.combatantRef.Count(); i++) {
                                ImGui.TableNextRow();
                                ImGui.TableNextColumn();
                                ImGui.PushID(i);
                                bool combatantSelected = _combatantEditorPointer == i;
                                ImGui.Selectable(GameManager.combatantRef[i].name, ref combatantSelected);
                                if (combatantSelected) {
                                    _combatantEditorPointer = i;
                                }
                                ImGui.PopID();

                            }
                            ImGui.EndTable();
                        }
                        ImGui.EndChild();
                    }
                    ImGui.SameLine();
                    if (_combatantEditorPointer > -1) 
                    {
                        ImGui.BeginGroup();
                        Combatant currCombatant = GameManager.combatantRef[_combatantEditorPointer];

                        if (ImGui.BeginChild("Combatant Info", new Vector2(combatantEditorBounds.X * 0.3f, combatantEditorBounds.Y * 0.3f), ImGuiChildFlags.Border)) 
                        {
                            string name = currCombatant.name;
                            if (ImGui.InputText("Name", ref name, 32))
                                currCombatant.name = name;
                            int teamID = currCombatant.team;
                            if(ImGui.InputInt("TeamID", ref teamID, 1))
                                currCombatant.team = teamID;
                            ImGui.EndChild();
                        }

                        ImGui.SameLine();
                        if (ImGui.BeginChild("Something", new Vector2(ImGui.GetContentRegionAvail().X, combatantEditorBounds.Y * 0.3f), ImGuiChildFlags.Border)) 
                        {
                            ImGui.Text("Something Extra");

                            ImGui.EndChild();
                        }
                        
                        if (ImGui.BeginChild("Combatant Skills", ImGui.GetContentRegionAvail(), ImGuiChildFlags.Border)) 
                        {
                            if (ImGui.BeginTable("Combatant Skills Table", 1, _tableFlags)) 
                            {
                                for (int i = 0; i < currCombatant.skills.Length; i++) 
                                {
                                    ImGui.TableNextRow();
                                    ImGui.TableNextColumn();
                                    ImGui.PushID(i);
                                    string s = "";
                                    if (currCombatant.skills[i] != null) {
                                        s = currCombatant.skills[i].name;
                                    }
                                    if (ImGui.Selectable(s)) 
                                    {
                                        // TODO: Implement changing skills via reference
                                        int currentItem = 0;
                                        // ImGui.Combo)

                                    }
                                    ImGui.PopID();
                                }
                                ImGui.EndTable();

                            }
                            ImGui.EndChild();
                        }
                        ImGui.EndGroup();
                    }  
                    ImGui.End();
                }
                #endregion

                #region Skill Editor
                if (_currentEditorState == EditorState.SKILL) 
                {
                    ImGui.Begin("Skill Editor");
                    Vector2 skillEditorBounds = ImGui.GetContentRegionAvail();
                    if (ImGui.BeginChild("Skill Selector", new Vector2(skillEditorBounds.X * 0.2f, skillEditorBounds.Y), ImGuiChildFlags.Border)) 
                    {
                        if (ImGui.BeginTable("Skill Editor Table", 1, _tableFlags)) 
                        {
                            for (int i = 0; i < GameManager.combatSkillRef.Count(); i++) {
                                ImGui.TableNextRow();
                                ImGui.TableNextColumn();
                                ImGui.PushID(i);
                                bool skillSelected = _skillEditorPointer == i;
                                ImGui.Selectable(GameManager.combatSkillRef[i].name, ref skillSelected);
                                if (skillSelected) {
                                    _skillEditorPointer = i;
                                }
                                ImGui.PopID();

                            }
                            ImGui.EndTable();
                        }
                        ImGui.EndChild();
                    }

                    ImGui.SameLine();

                    if (_skillEditorPointer > -1) 
                    {
                        ImGui.BeginGroup();
                        CombatSkill currCombatSkill = GameManager.combatSkillRef[_skillEditorPointer];

                        if (ImGui.BeginChild("Skill Info", new Vector2(skillEditorBounds.X * 0.4f, ImGui.GetContentRegionAvail().Y), ImGuiChildFlags.Border)) {
                            string name = currCombatSkill.name;
                            if (ImGui.InputText("Name", ref name, 32))
                                currCombatSkill.name = name;
                            
                            int priority = currCombatSkill.priority;
                            if (ImGui.InputInt("Priority", ref priority, 32))
                                currCombatSkill.priority = priority;
                            
                            int targetType = (int) currCombatSkill.targetType;
                            if (ImGui.Combo("Target Type", ref targetType, _skillTargetTypeNames, _skillTargetTypeNames.Length))
                                currCombatSkill.targetType = (TargetType) targetType;
                            
                            int rangeType = (int) currCombatSkill.rangeType;
                            if (ImGui.Combo("Range Type", ref rangeType, _skillTargetTypeNames, _skillRangeTypeNames.Length))
                                currCombatSkill.rangeType = (RangeType) rangeType;
                            ImGui.EndChild();
                        }

                        ImGui.SameLine();

                        if (ImGui.BeginChild("Combatant Skills", ImGui.GetContentRegionAvail(), ImGuiChildFlags.Border)) 
                        {
                            if (ImGui.BeginTable("Combatant Skills Table", 1, _tableFlags)) 
                            {
                                for (int i = 0; i < currCombatSkill.effects.Count(); i++) 
                                {
                                    ImGui.TableNextRow();
                                    ImGui.TableNextColumn();
                                    ImGui.PushID(i);
                                    string s = "";
                                    if (currCombatSkill.effects[i].effectType != EffectType.NONE)
                                        s = Enum.GetName(currCombatSkill.effects[i].effectType);
                                    if (ImGui.Selectable(s)) 
                                    {
                                        // TODO: Implement changing skills via reference
                                        int currentItem = 0;
                                        // ImGui.Combo)

                                    }
                                    ImGui.PopID();
                                }
                                ImGui.EndTable();

                            }
                            ImGui.EndChild();
                        }

                        ImGui.EndGroup();
                    }
                    
                    ImGui.End();
                }
                #endregion

                ImGui.End();
            }
            #endregion

        }

        private void ReceiveMessage(string message) {
            _messageBuffer.Enqueue(message);
        }

        private void RightClickCloseWindow(ref bool showWindow) {
            // in order for pos and size to be obtained properly, place within
            // bounds of begin() end() of a window
            Vector2 windowPos = ImGui.GetWindowPos();
            Vector2 windowSize = ImGui.GetWindowSize();
            bool isMouseInWindow = _mousePos.X >= windowPos.X && 
            _mousePos.X <= (windowPos.X + windowSize.X) &&
            _mousePos.Y >= windowPos.Y && 
            _mousePos.Y <= (windowPos.Y + windowSize.Y);

            if (isMouseInWindow && ImGui.IsMouseDown(ImGuiMouseButton.Right))
                            showWindow = false;
        }
    }
}