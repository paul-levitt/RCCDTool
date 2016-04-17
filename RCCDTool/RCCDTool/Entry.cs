using System;

namespace RCCDTool
{
    class Entry
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [STAThread]
        //[System.Diagnostics.DebuggerNonUserCodeAttribute()]
        //[System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        static void Main(string[] args)
        {
            Model model = new Model();

            Controller controller = new Controller(model);
            controller.StartApp();

        }
    }
}
