var app = angular.module("TestApp", []);
app.controller("PersonCntrl", function ($scope, $http) {
    $scope.pageIndex = 1;
    $scope.pageSize = 5;
    $scope.recordCount = 0;
    $scope.totalPage = 0;
    $scope.maxSize = 5;

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

    $scope.UpdatePerson = function (person) {
        sessionStorage.setItem("personID", person.Id)
        $scope.pId = person.Id;
        $scope.pFirstName = person.FirstName;
        $scope.pLastName = person.LastName;
        $scope.pAge = person.Age;
        $scope.pPhone = person.Phone;
        $scope.pEmail = person.Email;
        document.getElementById("btnInsertPerson").setAttribute("value", "Actualizar");
    }

    $scope.CancelInsert = function () {
        sessionStorage.setItem("personID", "")
        $scope.pId = "";
        $scope.pFirstName = "";
        $scope.pLastName = "";
        $scope.pAge = "";
        $scope.pPhone = "";
        $scope.pEmail = "";
        document.getElementById("btnInsertPerson").setAttribute("value", "Registrar");
    }

    $scope.InsertPerson = function () {
        var type = document.getElementById("btnInsertPerson").getAttribute("value");
        if (type == "Registrar") {
            $scope.person = {};
            $scope.person.FirstName = $scope.pFirstName;
            $scope.person.LastName = $scope.pLastName;
            $scope.person.Age = $scope.pAge;
            $scope.person.Phone = $scope.pPhone;
            $scope.person.Email = $scope.pEmail;
            $scope.person.Identificacion = $scope.pIdentificacion;
            $http({
                method: "post",
                url: "https://localhost:44363/Person/InsertPerson",
                datatype: "json",
                data: JSON.stringify($scope.person)
            }).then(function (respose) {
                alert(respose.data);
                $scope.GetPersons();
                $scope.CancelInsert();
            })
        }
        else {
            $scope.person = {};
            $scope.person.Id = $scope.pId;
            $scope.person.FirstName = $scope.pFirstName;
            $scope.person.LastName = $scope.pLastName;
            $scope.person.Age = $scope.pAge;
            $scope.person.Phone = $scope.pPhone;
            $scope.person.Email = $scope.pEmail;
            $scope.person.Identificacion = $scope.pIdentificacion;
            $http({
                method: "post",
                url: "https://localhost:44363/Person/UpdatePerson",
                datatype: "json",
                data: JSON.stringify($scope.person)
            }).then(function (respose) {
                alert(respose.data);
                $scope.GetPersons();
                $scope.CancelInsert();
            })
        }
    }

    $scope.CargarDoc = function (person) {
        sessionStorage.setItem("personID", person.Id)
        $scope.mId = person.Id;
        $scope.mNombre = person.FirstName + " " + person.LastName;
    }

    $scope.fileList = [];

    $scope.curFile;

    $scope.ImageProperty = { file: '' }

    $scope.setFile = function (element) {
        $scope.fileList = [];
        var files = element.files;
        for (var i = 0; i < files.length; i++) {
            $scope.ImageProperty.file = files[i];
            $scope.fileList.push($scope.ImageProperty);
            $scope.ImageProperty = {};
            $scope.$apply();
        }
    }

    $scope.UploadFile = function () {
        for (var i = 0; i < $scope.fileList.length; i++) {
            $scope.UploadFileIndividual($scope.fileList[i].file,
                $scope.fileList[i].file.name,
                $scope.fileList[i].file.type,
                $scope.fileList[i].file.size,
                i);
        }
    }

    $scope.UploadFileIndividual = function (fileToUpload, name, type, size, index) {
        var reqObj = new XMLHttpRequest();
        reqObj.open("POST", "https://localhost:44363/Person/UploadFiles", true);
        reqObj.setRequestHeader("Content-Type", "multipart/form-data");
        reqObj.setRequestHeader('X-File-Name', name);
        reqObj.setRequestHeader('X-File-Type', type);
        reqObj.setRequestHeader('X-File-Size', size);
        reqObj.setRequestHeader('X-Person-Id', $scope.mId);
        reqObj.setRequestHeader('X-Procedure-Id', $scope.mProcedimiento);
        reqObj.send(fileToUpload);
        $scope.GetPersons();
    }

});
