using ImGuiNET;
using System.Numerics;
using System.Threading;
// See https://aka.ms/new-console-template for more information

Thread thread = new Thread(GameManager.StartCombat);
thread.IsBackground = true;
thread.Start();

ImGuiProgram imguiDemo = new ImGuiProgramDemo();
imguiDemo.Run();
// GameManager.StartCombat();