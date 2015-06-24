'use strict';
app.controller('scoreboardController', ['GameState', 'ServerConnection', function (GameState, ServerConnection) {
    var vm = this;

    vm.connected = function () {
        return ServerConnection.isAlive();
    };

    vm.state = GameState.getState;

    vm.startNew = function () {
        GameState.startNew();
    }

    vm.getBlackScore = function () {
        return GameState.getState().ScoreBlack;
    };
    vm.getYellowScore = function () {
        return GameState.getState().ScoreYellow;
    };

    vm.isVolley = function () {
        return GameState.getState().InVolley;
    }

    vm.increaseScore = function (side) {
        GameState.increase(side);
    };

    vm.decreaseScore = function (side) {
        GameState.decrease(side);
    };
}]);