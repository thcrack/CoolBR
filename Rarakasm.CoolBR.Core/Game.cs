using System;

namespace Rarakasm.CoolBR.Core
{
    public class Game
    {
        private int _counter = 0;
        public static Game MakeGame()
        {
            return new Game();
        }

        private Game()
        {
            
        }

        public void Add()
        {
            _counter++;
        }

        public int GetCount()
        {
            return _counter;
        }

        public int GetRandom()
        {
            return new Random().Next();
        }
    }
}