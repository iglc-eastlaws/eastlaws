(function () {
    var ahkamModule = angular.module('myApp', []);

    ahkamModule.directive('cDropSortOptions', function () {
        return {
            restrict: 'E',
            replace: true,
            template:'<li><a ng-click="sortby(0)" href="javascript:;"><i class="fa fa-random"></i> افتراضي</a></li>'+
                     '<li><a ng-click="sortby(1)" href="javascript:;"><i class="fa fa-qrcode"></i> رقم الحكم</a></li>'+
                     '<li><a ng-click="sortby(2)" href="javascript:;"><i class="fa fa-calendar" aria-hidden="true"></i>  السنة القضائية</a></li>'+
                     '<li><a ng-click="sortby(3)" href="javascript:;"><i class="glyphicon glyphicon-time"></i> تاريخ الحكم</a></li>',

            scope: {
                selectedValue: '=',
            },
            link: function (scope, elem) {
                scope.selectedValue = 1;
                scope.sortby = function (val) {
                    scope.selectedValue = val;
                }
            }
        };
    })
})();
