var app = angular.module("TestApp", []);
app.controller("ResultadosCntrl", function ($scope, $http) {
    $scope.pageIndex = 1;
    $scope.pageSize = 5;
    $scope.recordCount = 0;
    $scope.totalPage = 0;
    $scope.maxSize = 5;

    $scope.GetResult = function () {
        $http({
            method: "get",
            url: "https://localhost:44363/ResultsDocuments/GetResult",
            params: { pageIndex: $scope.pageIndex, pageSize: $scope.pageSize }
        }).then(function (response) {
            $scope.results = response.data;
        }, function () {
            alert("Error");
        })
    }

    $scope.GetResult();

    $scope.GetPersons = function () {
        $http({
            method: "get",
            url: "https://localhost:44363/Person/GetPersons",
            params: { pageIndex: $scope.pageIndex, pageSize: $scope.pageSize }
        }).then(function (response) {
            $scope.persons = response.data;
            $scope.recordCount = response.data[0].totalcount;
            $scope.totalPage = Math.ceil((response.data[0].totalcount) / $scope.pageSize);
        }, function () {
            alert("Error");
        })
    }

    $scope.GetPersons();

    $scope.sorBy = function (column) {
        $scope.sortColumn = column;
        $scope.reverse = !$scope.reverse;
    }


    $scope.pagechanged = function () {
        $scope.GetPersons();
    }

    $scope.paging = function (pageIndex) {
        $scope.pageIndex = pageIndex;
        $scope.GetPersons();
    }

    $scope.changePageSize = function () {
        $scope.pageIndex = 1;
        $scope.GetPersons();
    }


});
