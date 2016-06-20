using System;

namespace Test
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                using (Game1 game = new Game1())
                {
                    game.Run();
                }
            } catch(Exception e)
            {
                WANOK.PrintError(e.Message);
            }
        }
    }
}
