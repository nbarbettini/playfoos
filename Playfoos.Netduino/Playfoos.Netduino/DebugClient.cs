using System;
using Microsoft.SPOT;

namespace NetchemiaFooz
{
    class DebugClient : IFoozClient
    {
        private uint YellowScore;
        private uint BlackScore;

        public DebugClient()
        {
            Debug.Print("New game!");
        }

        public void YellowScorePlus()
        {
            if (!gameOver())
            {
                YellowScore++;
            }
        }

        public void YellowScoreMinus()
        {
            if (!gameOver())
            {
                if (YellowScore > 0)
                    YellowScore--;
            }
        }

        public void BlackScorePlus()
        {
            if (!gameOver())
            {
                BlackScore++;
            }
        }

        public void BlackScoreMinus()
        {
            if (!gameOver())
            {
                if (BlackScore > 0)
                    BlackScore--;
            }
        }

        public string GetDisplay()
        {
            return "Yellow: " + YellowScore.ToString() + " Black: " + BlackScore.ToString();

            //if (!gameOver())
            //{
            //    return new string[] { "Netchemia Foosball", "Yellow: " + YellowScore.ToString() + " Black: " + BlackScore.ToString() };
            //}
            //else
            //{
            //    return new string[] { "Game over!", "Yellow: " + YellowScore.ToString() + " Black: " + BlackScore.ToString() };
            //}
        }

        public bool gameOver()
        {
            return false;
        }

    }
}
