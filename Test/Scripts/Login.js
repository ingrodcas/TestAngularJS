var app = angular.module("TestApp", []);
app.controller("LoginCntrl", function ($scope, $http) {
    $scope.LogIn = function () {
        if ($scope.UserName == null || $scope.Password == null) {
            alert("Error, Debe especificar el Usuario y la Contraseña");
        }
        else {
            $http({
                method: "POST",
                url: "https://localhost:44363/Users/Authenticate",
                datatype: "json",
                params: { UserName: $scope.UserName, Password: $scope.Password }
            }).then(function (response) {
                if (response.status == 200 && response.data) {
                    $scope.user = response.data;
                    if ($scope.user.Person.PersonTypeId == 2) {
                        window.location.href = 'https://localhost:44363/Person/Index';
                    }
                    else {
                        window.location.href = 'https://localhost:44363/ResultsDocuments/Index';
                    }
                } else if (response.data == null) {
                    alert("Error, Usuario o Contraseña Incorrectos.");
                } else {
                    alert("Error, Usuario o Contraseña Incorrectos.");
                }
            }).catch(function (errResponse) {
                alert("Error");
                console.log(errResponse);
            });
        }

    };
});