(function (app)
{
    "use strict";
    app.controller("headController", ["$scope", function ($scope)
    {
        $scope.title = "Расстояние между городами";
    }]);
})(angular.module("distanceBtwCities"));