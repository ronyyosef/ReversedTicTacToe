using System;

namespace ReversedTicTacToe
{
    internal class Program
    {
        public static void Main()
        {
            try
            {
                GameUi game = new GameUi();
                game.Run();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
        }
    }
}