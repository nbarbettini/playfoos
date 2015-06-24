app.factory('GameState', ['$rootScope', 'Hub', '$http', function ($rootScope, Hub, $http) {

    var _blankGameState = {
        GameOver: true,
        IsVolley: false,
    };

    var currentState = _blankGameState;

    var increase = function (side) {
        $http({
            method: 'PUT',
            url: 'api/score/' + side,
        });
    };
    var decrease = function (side) {
        $http({
            method: 'PUT',
            url: 'api/score/' + side,
            params: {
                increase: false
            }
        });
    }

    var startNew = function () {
        $http({
            method: 'POST',
            url: 'api/game'
        });
    }

    //declaring the hub connection
    var hub = new Hub('notifyHub', {

        rootPath: '/playfoos/api/signalr',

        //client side methods
        listeners: {
            'updateGameState': function (gameState) {
                currentState = gameState || _blankGameState;
                $rootScope.$apply();
            }
        },

        //handle connection error
        errorHandler: function (error) {
            console.error(error);
        },

        stateChanged: function (state) {
            switch (state.newState) {
                case $.signalR.connectionState.connecting:
                    //your code here
                    break;
                case $.signalR.connectionState.connected:
                    //your code here
                    break;
                case $.signalR.connectionState.reconnecting:
                    //your code here
                    break;
                case $.signalR.connectionState.disconnected:
                    //your code here
                    break;
            }
        }
    });

    return {
        getState: function () { return currentState; },
        increase: increase,
        decrease: decrease,
        startNew: startNew
    };
}]);