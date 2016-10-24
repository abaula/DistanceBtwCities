(function (app)
{
    "use strict";
    app.controller("headController", ["$scope", function ($scope)
    {
        $scope.title = "Привет!";
    }]);
})(angular.module("distanceBtwCities"));