using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class SaveData
    {
        public int Score;
        public List<LevelData> Levels;

        public SaveData()
        {
            Levels = new List<LevelData>();
        }
    }

    class LevelData
    {
        public int Seed;
        public bool Win;
        public bool Lose;

        public LevelData(int seed, bool win, bool lose)
        {
            Seed = seed;
            Win = win;
            Lose = lose;
        }
    }
}
