using System;
using Swordfish.Core;
using KitsuneProject.GameStates;

namespace KitsuneProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var game = new Game(600, 800, "KProject", new StageOne());
            WindowManager.Instance.SetNonResizable();
        }
    }
}
