 
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

            scope.getCurrentPage = function (p) {
               // console.log(scope.currentPage + ' ' + scope.pagesCount);
                scope.currentPage = p;
            }

            
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
                        '<input id="{{forId}}1" ng-model="modelVal" type="radio" class="radio_buttons " name="searchtype" value="{{firstval}}" ng-change="changval({{firstval}})" />{{firstName}}</label>' +
              '</span>&nbsp;'+
              '<label for="{{forId}}2">' +
                  '<input id="{{forId}}2" type="radio" ng-model="modelVal" class="radio_buttons" name="searchtype" value="{{secondval}}" ng-change="changval({{secondval}})"/>{{secName}}</label>' +
          '</div>'
            ,
            scope: {
                modelVal: '=',
                firstName:'@',
                secName: '@',
                forId: '@',
                firstval: '@val1',
                secondval: '@val2'
            },
            link: function (scope, elem) {
                scope.modelVal = scope.firstval;
                scope.changval = function (val) {
                    scope.modelVal = val;
                }
            }

        };


    })
    sharedModule.directive('cDatePicker', function ($compile) {
        return {
            restrict: "E",
            scope: {
                date:'='
            },
            template:'<div>'+
                '<input type="text" ng-model="date" ng-click="clicktext($event)" />' +
                  '</br>' +
                '<div class="c-date-picker" style="display:none">' +
                '<span>اليوم</span><select ng-options="t.val for t in days track by t.key" ng-model="modelday" ng-change="updateDate(1)"></select>' +
                '</br>' +
                '<span>الشهر</span><select ng-options="t.val for t in months track by t.key" ng-model="modelmonth"  ng-change="updateDate(2)"></select>' +
                '</br>' +
                '<span>السنة</span><select  ng-options="t.val for t in ::years track by t.key" ng-model="modelyear" ng-change="updateDate(3)"></select>' +
                '</br>' +
                '<input type="button" value="ok" ng-click="clickDate($event)" />' +
                 '<input type="button" value="remove" ng-click="removeDate($event)" />' +
                 '<input type="button" value="close" ng-click="closeDate($event)" />' +
                '</div></div>'
                ,
            replace: true,
            controller: function ($scope) {
       /*
              $scope.enddays = 30;
              $scope.fillArray = function (start, end,order) {
                  var arr = [];
                  if (order === 1) {
                      for (var i = end; i >= start; i--) {
                          arr.push({ 'key': i, 'val': i });
                      }
                  } else {
                      for (var i = start; i <= end; i++) {
                          arr.push({ 'key': i, 'val': i });
                      }
                  }
                    
                    return arr;
              }

              $scope.years = totalyear;//$scope.fillArray(1881, 2016,1);
              $scope.months = $scope.fillArray(1, 12);
            

                $scope.setDate = function () {
                    if (($scope.modelyear !== (undefined || null)) && ($scope.modelmonth !== (undefined || null)) &&($scope.modelday !== (undefined || null)))
                    {
                        $scope.datetemp =
                            (($scope.modelday.key < 10) ? '0' + $scope.modelday.key : $scope.modelday.key) + '-' +
                            (($scope.modelmonth.key < 10) ? '0' + $scope.modelmonth.key : $scope.modelmonth.key) + '-' +
                             $scope.modelyear.key;
                    }
                }

                $scope.removeDate = function () {
                    $scope.date = '';
                }

                $scope.updateDate = function (t) {
                    if (t !== 1) {
                        if (($scope.modelyear !== undefined) && ($scope.modelmonth !== undefined)) {
                       
                            switch ($scope.modelmonth.key) {
                                case 2: {
                                    $scope.enddays = ($scope.modelyear.key % 4 == 0) ? 29 : 28;
                                    break;
                                }
                                case 1: case 3: case 5: case 7: case 8: case 10: case 12: {
                                    $scope.enddays = 31;
                                    break;
                                }
                                default: {
                                    $scope.enddays = 30;
                                    break;
                                }
                            }
                            
                            $scope.days = $scope.fillArray(1, $scope.enddays);
                            if ($scope.selectedDay > 0) {
                                var mykey = $.grep($scope.days, function (e) { return e.key == $scope.selectedDay; });
                                if (mykey.length > 0) {
                                    $scope.modelday = { key: $scope.selectedDay, val: $scope.selectedDay };;
                                } else {
                                    $scope.modelday = { key: 1, val: 1 };
                                }
                            } else {
                                $scope.modelday = { key: 1, val: 1 };
                            }
                        }
                    } else {
                        if ($scope.modelday !== (undefined || null)) {
                            $scope.selectedDay = $scope.modelday.key;
                        }
                    }

                }

                var d = new Date();
                $scope.modelyear = { key: d.getFullYear(), val: d.getFullYear() };
                $scope.modelmonth = { key: d.getMonth() + 1, val: d.getMonth() + 1 };
                $scope.days = $scope.fillArray(1, $scope.enddays);
                $scope.modelday = { key: d.getDate(), val: d.getDate() };
                $scope.selectedDay = d.getDate();

                //scope.fillArray = function (start, end, order) {
                //    var arr = [];
                //    if (order === 1) {
                //        for (var i = end; i >= start; i--) {
                //            arr.push({ 'key': i, 'val': i });
                //        }
                //    } else {
                //        for (var i = start; i <= end; i++) {
                //            arr.push({ 'key': i, 'val': i });
                //        }
                //    }

                //    return arr;
                //}


                //$scope.totalyear = fillArray(1881, 2016, 1);
                //console.log($scope.totalyear)
                */
            },
            link: function (scope,elm) {

               
                
                scope.closeDate = function (ev) {
                    scope.datetemp = scope.date;
                    $(ev.target).closest('.c-date-picker').hide();
                }
             
                scope.clickDate = function (ev) {
                    scope.setDate();
                    scope.date = scope.datetemp;
                    $(ev.target).closest('.c-date-picker').hide();
                }


                scope.clicktext = function (ev) {
                    $('.c-date-picker').hide();
                    $(ev.target).parent().find('.c-date-picker').show();
                }
            
            //---------------------
              
                scope.enddays = 30;
                scope.fillArray = function (start, end, order) {
                    var arr = [];
                    if (order === 1) {
                        for (var i = end; i >= start; i--) {
                            arr.push({ 'key': i, 'val': i });
                        }
                    } else {
                        for (var i = start; i <= end; i++) {
                            arr.push({ 'key': i, 'val': i });
                        }
                    }

                    return arr;
                }
         //  scope.fillArray(1881, 2016, 1);



                scope.setDate = function () {
                    if ((scope.modelyear !== (undefined || null)) && (scope.modelmonth !== (undefined || null)) && (scope.modelday !== (undefined || null))) {
                        scope.datetemp =
                            ((scope.modelday.key < 10) ? '0' + scope.modelday.key : scope.modelday.key) + '-' +
                            ((scope.modelmonth.key < 10) ? '0' + scope.modelmonth.key : scope.modelmonth.key) + '-' +
                             scope.modelyear.key;
                    }
                }

                scope.removeDate = function () {
                    scope.date = '';
                }

                scope.updateDate = function (t) {
                    if (t !== 1) {
                        if ((scope.modelyear !== undefined) && (scope.modelmonth !== undefined)) {

                            switch (scope.modelmonth.key) {
                                case 2: {
                                    scope.enddays = (scope.modelyear.key % 4 == 0) ? 29 : 28;
                                    break;
                                }
                                case 1: case 3: case 5: case 7: case 8: case 10: case 12: {
                                    scope.enddays = 31;
                                    break;
                                }
                                default: {
                                    scope.enddays = 30;
                                    break;
                                }
                            }

                            scope.days = fillArray(1, scope.enddays);
                            if (scope.selectedDay > 0) {
                                var mykey = $.grep(scope.days, function (e) { return e.key == scope.selectedDay; });
                                if (mykey.length > 0) {
                                    scope.modelday = { key: scope.selectedDay, val: scope.selectedDay };;
                                } else {
                                    scope.modelday = { key: 1, val: 1 };
                                }
                            } else {
                                scope.modelday = { key: 1, val: 1 };
                            }
                        }
                    } else {
                        if (scope.modelday !== (undefined || null)) {
                            scope.selectedDay = scope.modelday.key;
                        }
                    }

                }

                scope.clicktext = function (ev) {
                    $('.c-date-picker').hide();
                    $(ev.target).parent().find('.c-date-picker').show(function () {
                        //var d = new Date();
                        //scope.years = totalyear;//fillArray(1881, 2016, 1);
                        //scope.months = fillArray(1, 12);
                        //scope.modelyear = { key: d.getFullYear(), val: d.getFullYear() };
                        //scope.modelmonth = { key: d.getMonth() + 1, val: d.getMonth() + 1 };
                        //scope.days = fillArray(1, scope.enddays);
                        //scope.modelday = { key: d.getDate(), val: d.getDate() };
                        //scope.selectedDay = d.getDate();
                        //$compile(elm)(scope)
                    });

                    //console.log(scope.allYears)

                    var d = new Date();
                    scope.years = scope.fillArray(1881, 2016, 1);
                  //  scope.years = scope.allYears;
                    scope.months = scope.fillArray(1, 12);
                   
                    scope.modelyear = { key: d.getFullYear(), val: d.getFullYear() };
                    scope.modelmonth = { key: d.getMonth() + 1, val: d.getMonth() + 1 };
                    scope.days = scope.fillArray(1, scope.enddays);
                    scope.modelday = { key: d.getDate(), val: d.getDate() };
                    scope.selectedDay = d.getDate();

                }
                
               
                /******************/
            }

        };
    });

    //sharedModule.directive('cDateText', function () {
    //    return {
    //        link: function (scope, elm, attr, ctrl, transclude) {
    //            elm.on('click', function () {
    //                $('.c-date-picker').hide();
    //                elm.next('.c-date-picker').show();
    //            });

    //        }
    //    }
    //});

    sharedModule.directive('cFontScale', function () {
        return {
            link: function (scope, elm, attr, ctrl, transclude) {

                //var size = attr['cFontScale'];
                //scope.$watch(attr['cFontScale'], function (val) {
                //    elm.css('font-size', val + 'px');
                //})
             
            }
        }

    });


    //var objectArray = function (start,end) {
    //    var arr = [];
    //    for (var i = start; i <= end; i++) {
    //        arr.push({ 'key': i, 'val': i });
    //    }
    //    return arr;
    //}
 
    var fillArray = function (start, end, order) {
        var arr = [];
        if (order === 1) {
            for (var i = end; i >= start; i--) {
                arr.push({ 'key': i, 'val': i });
            }
        } else {
            for (var i = start; i <= end; i++) {
                arr.push({ 'key': i, 'val': i });
            }
        }

        return arr;
    }

    var totalyear = fillArray(1881, 2016, 1);
    //console.log(totalyear)


})();