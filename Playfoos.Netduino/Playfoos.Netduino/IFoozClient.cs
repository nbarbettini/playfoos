using System;
using Microsoft.SPOT;

namespace NetchemiaFooz
{
    public interface IFoozClient
    {
        void YellowScorePlus();
        void YellowScoreMinus();
        void BlackScorePlus();
        void BlackScoreMinus();
        string GetDisplay();
        bool gameOver();
    }
}
