var genomes = angular.module('genomes', ['ui.bootstrap']);

genomes.controller('GenomeController',  ['$scope', '$http', function GenomeController($scope, $http){
	$scope.filteredGenomes = []
   , $scope.currentPage = 1
   , $scope.numPerPage = 100
   , $scope.maxSize = 5;

	//$scope.$watch('currentPage + numPerPage', function () {
	//	var begin = (($scope.currentPage - 1) * $scope.numPerPage)
	//	, end = begin + $scope.numPerPage;

	//	//$scope.filteredGenomes = $scope.genomes.slice(begin, end);
	//});

	$scope.$watch('modelId', function () {
		$http({method: 'GET', url: '/Genome/PartialIndex/' +  $scope.modelId + '?page=0&pageSize=100'}).
		success(function(data, status, headers, config) {
			$scope.filteredGenomes = data;
		}).
		error(function(data, status, headers, config) {
			debugger;
		})
	});
}]);