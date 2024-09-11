using System;

namespace RubiksCube
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new RubiksCubeProgram()) 
                game.Run();
        }
    }
}
