 
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

})();