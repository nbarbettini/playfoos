app.factory('ServerConnection', ['$rootScope', '$http', '$timeout', function ($rootScope, $http, $timeout) {
    var serverAlive = false;

    pingServer();

    function pingServer() {
        var request = $http({
            method: 'GET',
            url: 'api/alive'
        });

        request.then(function (response) {
            serverAlive = response.data;
            $timeout(pingServer, 5 * 1000);
        }, function (error) {
            serverAlive = false;
            $timeout(pingServer, 5 * 1000);
        });
    }

    return {
        isAlive: function () { return serverAlive; }
    }
}]);