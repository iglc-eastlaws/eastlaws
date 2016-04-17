 
(function () { 
    var  sharedModule = angular.module('sharedModule', []);
    sharedModule.directive('cPageing', function ($rootScope) {
        /* <c-pageing pages-count="250" limit-no="10" current-page="mypage" on-getpage="getmypage()">
          </c-pageing> */
 
    return{
        restrict: 'E',
        template: '<ul class="pagination">' +
                    '<li ng-class="{disabled : currentPage == 1}"><a href="javascript:;" ng-click="getPrevPage()">«</a></li>' +
                    '<li ng-repeat="p in totalPagesCount()  | limitTo:limitNo" ng-class="{active : currentPage == p}">' +
                        '<a href="javascript:;" ng-click="getCurrentPage(p)">{{ p }}</a>' +
                   '</li>' +
                    '<li ng-class="{disabled : currentPage == pagesCount}"><a href="javascript:;" ng-click="getNextPage()">»</a></li>' +
                '</ul>',
        replace: true,
        transclude: 'element',
        scope: {
            onGetpage: '&',
            limitNo: '@',
            pagesCount: '@',
            currentPage: '='
        },
        link: function (scope, elem, attr, ctrl, transclude) {

            scope.totalPagesCount = function () {
                pagesArray = [];
                if (scope.limitNo === undefined)
                { scope.limitNo = 10; }

                var startpage = 1;
                var mylimt = Number(scope.limitNo);
                var mycount = Number(scope.pagesCount);
                var mycurrentpage = Number(scope.currentPage);

                var halfLimit = Math.ceil((mylimt / 2));
                if (mycurrentpage >= halfLimit) {
                    var sPage = (mycurrentpage - halfLimit);
                    startpage = (sPage < 1) ? 1 : sPage;
                }


                var endpage = (startpage + mylimt);
                if (endpage > mycount) { endpage = mycount; }

                if ((endpage - startpage < mylimt) && (mylimt < mycount))
                {
                    startpage = (mycount - mylimt) + 1;
                }

                for (var i = startpage; i <= endpage; i++) {
                    pagesArray.push(i);
                }
                return pagesArray;
            }

            elem.on('click', function () {scope.onGetpage(); });

            scope.getCurrentPage = function (p) {scope.currentPage = p;}

            
            //scope.getFirstPage = function () {
            //    if (scope.currentPage > 1) {
            //        scope.currentPage = 1;
            //    }
            //}

            scope.getPrevPage = function () {
                if (scope.currentPage > 1) {
                    scope.currentPage -= 1;
                }
            }

            scope.getNextPage = function (p) {
                if (scope.currentPage < scope.pagesCount) {
                    scope.currentPage += 1;
                }
            }

            //reset directive
            $rootScope.$on(attr.onInit, function () {
                scope.currentPage = 1;
                transclude(function (clone) {
                    elem.parent().append(clone);
                });
            });

        }
    };
})
    sharedModule.directive('cDropMatchSearch', function () {

           return {
               restrict: 'E',
               replace: true,
               template: '<div class="multiple-form-group input-group">'+
                           '<div class="input-group-btn input-group-select">'+
                            '<button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">'+
                            '<span class="concept">مطابق للجملة</span> <span class="caret"></span>'+
                        '</button>'+
                        '<ul class="dropdown-menu" role="menu">'+
                            '<li><a ng-click="changval(1)" href="javascript:;">مطابق للجملة</a></li>' +
                            '<li><a ng-click="changval(2)" href="javascript:;">مطابق جميع الكلمات</a></li>' +
                        '</ul>'+
                    '</div>'+
                    '<input ng-model="inputText" type="text" class="form-control">'+
                    '</div>'
               ,
               scope: {
                   selectedValue: '=',
                   inputText:'='
               },
               link: function (scope, elem) {
                   scope.inputText = '';
                  scope.selectedValue = 1;
                   scope.changval = function (val) {
                       scope.selectedValue = val;
                   }
               }
              
           };


       })
    sharedModule.directive('cRadioMatchSearch', function () {

        return {
            restrict: 'E',
            replace: true,
            template: '<div class="col-sm-9">'+
                '<span class="">'+
                  '<label for="{{forId}}1">' +
                        '<input id="{{forId}}1" ng-model="modelVal" type="radio" class="radio_buttons " name="searchtype" value="1" ng-change="changval(1)" />{{firstName}}</label>' +
              '</span>&nbsp;'+
              '<label for="{{forId}}2">' +
                  '<input id="{{forId}}2" type="radio" ng-model="modelVal" class="radio_buttons" name="searchtype" value="2" ng-change="changval(2)"/>{{secName}}</label>' +
          '</div>'
            ,
            scope: {
                modelVal: '=',
                firstName:'@',
                secName: '@',
                forId:'@'
            },
            link: function (scope, elem) {
                scope.modelVal = 1;
                scope.changval = function (val) {
                    scope.modelVal = val;
                }
            }

        };


    })
    sharedModule.directive('cDateDiv', function ($compile) {
        return {
            restrict: 'A',
            replace: true,
            scope: {
                elmshow: '@'
             },
            link: function (scope, elem, attr) {
                scope.elmshow = false;
                elem.css("border", "1px solid #cccccc");
                //$compile(angular.element('<div ng-show="elmshow"><h3>Hello</h3></div>').insertAfter(elem))(scope);

                ////scope.$apply($compile(angular.element('<div ng-show="elmshow"><h3>Hello</h3></div>').insertAfter(elem))(scope));
                //elem.bind('focus', function () {
                //    scope.elmshow = true; 
                //});
                //elem.bind('blur', function () {
                //    scope.elmshow = false; 
                //});
               
            }
        }

    })


    sharedModule.directive('test', function ($compile) {
        return {
            restrict: 'A',
            replace: true,
            scope: {
                elmshow: '@'
            },
            link: function (scope, elem, attr) {

                var years = [];
                for (var i = 1980; i <= 2016; i++) {
                    years.push(i);
                    console.log(i);
                }

                var htmlyears = '<select><option ng-repeat="n in years track by index">{{$index}}</option></select>';
                scope.elmshow = false;
                scope.name = "ahmed"
                elem.css("border", "1px solid #cccccc");
                //angular.element('<h4>after {{name}} </h4>').insertAfter(elem);
                $compile(angular.element('<h4>after{{name}}</h4>'+htmlyears+'').insertAfter(elem))(scope);

                //$compile(angular.element('<div ng-show="elmshow"><h3>Hello</h3></div>').insertAfter(elem))(scope);

                ////scope.$apply($compile(angular.element('<div ng-show="elmshow"><h3>Hello</h3></div>').insertAfter(elem))(scope));
                //elem.bind('focus', function () {
                //    scope.elmshow = true; 
                //});
                //elem.bind('blur', function () {
                //    scope.elmshow = false; 
                //});

            }
        }

    })

})();