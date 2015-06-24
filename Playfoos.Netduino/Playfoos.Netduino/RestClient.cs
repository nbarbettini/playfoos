using System;
using Microsoft.SPOT;

namespace NetchemiaFooz
{
    class RestClient : IFoozClient
    {
        private const string API_URL = "http://fooz.cloudapp.net/api";
        private RestWrapper server = new RestWrapper(API_URL, 80);

        bool gameActive = false;

        public RestClient()
        {
            Debug.Print("New game!");
        }

        public void YellowScorePlus()
        {
            var response = server.PostEvent("score", "yellow");
            gameActive = (response == "true");
        }

        public void YellowScoreMinus()
        {
            throw new NotImplementedException();
        }

        public void BlackScorePlus()
        {
            var response = server.PostEvent("score", "black");
            gameActive = (response == "true");
        }

        public void BlackScoreMinus()
        {
            throw new NotImplementedException();
        }

        public string GetDisplay()
        {
            return "Game is active: " + gameOver().ToString();
        }

        public bool gameOver()
        {
            return gameActive;
        }

    }
}
