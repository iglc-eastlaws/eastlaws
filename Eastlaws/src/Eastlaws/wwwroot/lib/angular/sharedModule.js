
(function () {
    var sharedModule = angular.module('sharedModule', []);
    sharedModule.directive('cPageing', function ($rootScope) {
        return {
            restrict: 'E',
            template: '<ul class="pagination">' +
                        '<li ng-class="{disabled : currentPage == 1}"><a href="javascript:;" ng-click="getPrevPage()">«</a></li>' +
                        '<li ng-repeat="p in totalPagesCount()  | limitTo:limitNo" ng-class="{active : currentPage == p}">' +
                            '<a href="javascript:;" ng-click="getCurrentPage(p)">{{p}}</a>' +
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

                    if ((endpage - startpage < mylimt) && (mylimt < mycount)) {
                        startpage = (mycount - mylimt) + 1;
                    }

                    for (var i = startpage; i <= endpage; i++) {
                        pagesArray.push(i);
                    }
                    return pagesArray;
                }

                elem.on('click', function () {
                    scope.onGetpage();
                });

                scope.getCurrentPage = function (p) {
                    scope.currentPage = p;
                }


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

                ////reset directive
                //$rootScope.$on(attr.onInit, function () {
                //    scope.currentPage = 1;
                //    transclude(function (clone) {
                //        elem.parent().append(clone);
                //    });
                //});

            }
        };
    })

    sharedModule.component('cPageingCtrl', {
        template: '<ul class="pagination">' +
                          '<li ng-class="{disabled : model.currentPage == 1}"><a href="javascript:;" ng-click="model.getPrevPage()">«</a></li>' +
                          '<li ng-repeat="p in model.totalPagesCount()  | limitTo:model.limitNo" ng-class="{active : model.currentPage == p}">' +
                              '<a href="javascript:;" ng-click="model.getCurrentPage(p)">{{p}}</a>' +
                         '</li>' +
                          '<li ng-class="{disabled : model.currentPage == model.pagesCount}"><a href="javascript:;" ng-click="model.getNextPage()">»</a></li>' +
                      '</ul>',
        bindings: {
            onGetpage: '&',
            limitNo: '@',
            pagesCount: '@',
            currentPage: '=?'

        },
        controllerAs:'model',
        controller: function () {
            var model = this;

            model.$onInit = function () {
                //console.log('$On  Init')
            }

            model.$onChanges = function () {
                 console.log('$On  $OnChanges');
                //model.currentPage = 1;
                model.totalPagesCount();
            }

            model.getCurrentPage = function (p) {
                model.currentPage = p;
                console.log('Page >>> '+p)
                model.onGetpage();
            }


            model.getPrevPage = function () {
                if (model.currentPage > 1) {
                    model.currentPage -= 1;
                    model.onGetpage();
                }
            }

            model.getNextPage = function (p) {
                if (model.currentPage < model.pagesCount) {
                    model.currentPage += 1;
                    model.onGetpage();
                }
            }

            model.totalPagesCount = function() {
                var pagesArray = [];
                if (model.limitNo === undefined) { model.limitNo = 10; }
                 if (model.currentPage === undefined) { model.currentPage = 1; }
    
                    var startpage = 1;
                    var mylimt = Number(model.limitNo);
                    var mycount = Number(model.pagesCount);
                    var mycurrentpage = Number(model.currentPage);

                    var halfLimit = Math.ceil((mylimt / 2));
                    if (mycurrentpage >= halfLimit) {
                        var sPage = (mycurrentpage - halfLimit);
                        startpage = (sPage < 1) ? 1 : sPage;
                    }


                    var endpage = (startpage + mylimt);
                    if (endpage > mycount) { endpage = mycount; }

                    if ((endpage - startpage < mylimt) && (mylimt < mycount)) {
                        startpage = (mycount - mylimt) + 1;
                    }

                    for (var i = startpage; i <= endpage; i++) {
                        pagesArray.push(i);
                    }

                    return pagesArray;
            }

        }
    })

    sharedModule.directive('cDropMatchSearch', function () {

        return {
            restrict: 'E',
            replace: true,
            template: '<div class="multiple-form-group input-group">' +
                        '<div class="input-group-btn input-group-select">' +
                         '<button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">' +
                         '<span class="concept">مطابق للجملة</span> <span class="caret"></span>' +
                     '</button>' +
                     '<ul class="dropdown-menu" role="menu">' +
                         '<li><a ng-click="changval(0)" href="javascript:;">مطابق للجملة</a></li>' +
                         '<li><a ng-click="changval(1)" href="javascript:;">مطابق جميع الكلمات</a></li>' +
                     '</ul>' +
                 '</div>' +
                 '<input ng-model="inputText" type="text" class="form-control">' +
                 '</div>'
            ,
            scope: {
                selectedValue: '=',
                inputText: '='
            },
            link: function (scope, elem) {
                scope.inputText = '';
                scope.selectedValue =0;
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
            template: '<div class="col-sm-9">' +
                '<span class="">' +
                  '<label for="{{forId}}1">' +
                        '<input id="{{forId}}1" ng-model="modelVal" type="radio" class="radio_buttons " name="searchtype" value="{{firstval}}" ng-change="changval({{firstval}})" />{{firstName}}</label>' +
              '</span>&nbsp;' +
              '<label for="{{forId}}2">' +
                  '<input id="{{forId}}2" type="radio" ng-model="modelVal" class="radio_buttons" name="searchtype" value="{{secondval}}" ng-change="changval({{secondval}})"/>{{secName}}</label>' +
          '</div>'
            ,
            scope: {
                modelVal: '=',
                firstName: '@',
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

      
        //best  //  template = '<span>السنة</span><select ng-options="t.val for t in years track by t.key" ng-model="modelyear" ng-change="updateDate(3)" class="ng-pristine ng-untouched ng-valid ng-not-empty"><option label="2016" value="2016" selected="selected">2016</option><option label="2015" value="2015">2015</option><option label="2014" value="2014">2014</option><option label="2013" value="2013">2013</option><option label="2012" value="2012">2012</option><option label="2011" value="2011">2011</option><option label="2010" value="2010">2010</option><option label="2009" value="2009">2009</option><option label="2008" value="2008">2008</option><option label="2007" value="2007">2007</option><option label="2006" value="2006">2006</option><option label="2005" value="2005">2005</option><option label="2004" value="2004">2004</option><option label="2003" value="2003">2003</option><option label="2002" value="2002">2002</option><option label="2001" value="2001">2001</option><option label="2000" value="2000">2000</option><option label="1999" value="1999">1999</option><option label="1998" value="1998">1998</option><option label="1997" value="1997">1997</option><option label="1996" value="1996">1996</option><option label="1995" value="1995">1995</option><option label="1994" value="1994">1994</option><option label="1993" value="1993">1993</option><option label="1992" value="1992">1992</option><option label="1991" value="1991">1991</option><option label="1990" value="1990">1990</option><option label="1989" value="1989">1989</option><option label="1988" value="1988">1988</option><option label="1987" value="1987">1987</option><option label="1986" value="1986">1986</option><option label="1985" value="1985">1985</option><option label="1984" value="1984">1984</option><option label="1983" value="1983">1983</option><option label="1982" value="1982">1982</option><option label="1981" value="1981">1981</option><option label="1980" value="1980">1980</option><option label="1979" value="1979">1979</option><option label="1978" value="1978">1978</option><option label="1977" value="1977">1977</option><option label="1976" value="1976">1976</option><option label="1975" value="1975">1975</option><option label="1974" value="1974">1974</option><option label="1973" value="1973">1973</option><option label="1972" value="1972">1972</option><option label="1971" value="1971">1971</option><option label="1970" value="1970">1970</option><option label="1969" value="1969">1969</option><option label="1968" value="1968">1968</option><option label="1967" value="1967">1967</option><option label="1966" value="1966">1966</option><option label="1965" value="1965">1965</option><option label="1964" value="1964">1964</option><option label="1963" value="1963">1963</option><option label="1962" value="1962">1962</option><option label="1961" value="1961">1961</option><option label="1960" value="1960">1960</option><option label="1959" value="1959">1959</option><option label="1958" value="1958">1958</option><option label="1957" value="1957">1957</option><option label="1956" value="1956">1956</option><option label="1955" value="1955">1955</option><option label="1954" value="1954">1954</option><option label="1953" value="1953">1953</option><option label="1952" value="1952">1952</option><option label="1951" value="1951">1951</option><option label="1950" value="1950">1950</option><option label="1949" value="1949">1949</option><option label="1948" value="1948">1948</option><option label="1947" value="1947">1947</option><option label="1946" value="1946">1946</option><option label="1945" value="1945">1945</option><option label="1944" value="1944">1944</option><option label="1943" value="1943">1943</option><option label="1942" value="1942">1942</option><option label="1941" value="1941">1941</option><option label="1940" value="1940">1940</option><option label="1939" value="1939">1939</option><option label="1938" value="1938">1938</option><option label="1937" value="1937">1937</option><option label="1936" value="1936">1936</option><option label="1935" value="1935">1935</option><option label="1934" value="1934">1934</option><option label="1933" value="1933">1933</option><option label="1932" value="1932">1932</option><option label="1931" value="1931">1931</option><option label="1930" value="1930">1930</option><option label="1929" value="1929">1929</option><option label="1928" value="1928">1928</option><option label="1927" value="1927">1927</option><option label="1926" value="1926">1926</option><option label="1925" value="1925">1925</option><option label="1924" value="1924">1924</option><option label="1923" value="1923">1923</option><option label="1922" value="1922">1922</option><option label="1921" value="1921">1921</option><option label="1920" value="1920">1920</option><option label="1919" value="1919">1919</option><option label="1918" value="1918">1918</option><option label="1917" value="1917">1917</option><option label="1916" value="1916">1916</option><option label="1915" value="1915">1915</option><option label="1914" value="1914">1914</option><option label="1913" value="1913">1913</option><option label="1912" value="1912">1912</option><option label="1911" value="1911">1911</option><option label="1910" value="1910">1910</option><option label="1909" value="1909">1909</option><option label="1908" value="1908">1908</option><option label="1907" value="1907">1907</option><option label="1906" value="1906">1906</option><option label="1905" value="1905">1905</option><option label="1904" value="1904">1904</option><option label="1903" value="1903">1903</option><option label="1902" value="1902">1902</option><option label="1901" value="1901">1901</option><option label="1900" value="1900">1900</option><option label="1899" value="1899">1899</option><option label="1898" value="1898">1898</option><option label="1897" value="1897">1897</option><option label="1896" value="1896">1896</option><option label="1895" value="1895">1895</option><option label="1894" value="1894">1894</option><option label="1893" value="1893">1893</option><option label="1892" value="1892">1892</option><option label="1891" value="1891">1891</option><option label="1890" value="1890">1890</option><option label="1889" value="1889">1889</option><option label="1888" value="1888">1888</option><option label="1887" value="1887">1887</option><option label="1886" value="1886">1886</option><option label="1885" value="1885">1885</option><option label="1884" value="1884">1884</option><option label="1883" value="1883">1883</option><option label="1882" value="1882">1882</option><option label="1881" value="1881">1881</option></select>';
        //worst    //template = '<span>السنة</span><select ng-options="t.val for t in ::years track by t.key" ng-model="modelyear" ng-change="updateDate(3)" class="ng-pristine ng-untouched ng-valid ng-not-empty"></select>';
        //best //WoooooooooW   //loop be carful loop must be equal to binbding
        //make your databinding and get  woooow performance

        //global scope for directive
           
        var getTemplate = function () {
            var d = new Date();
            var template = '';
            template += '<div class="input-group date date-piker-input" >';
            template += '<input type="text" ng-model="date" ng-click="clicktext($event)" class="form-control " /> ';
           // template +='<span class="input-group-addon">';
           // template += '<span class="glyphicon glyphicon-calendar"></span>';
           // template +=  '</span>';
           
            template += '<div class="c-date-picker bootstrap-datetimepicker-widget dropdown-menu form-inline" style="display:none"> ';
            template += '<div ><label class="col-lg-3 col-md-3">اليوم </label><div class="col-lg-9 col-md-9 "> <select class="search   btn btn-primary ng-isolate-scope" ng-options="t.val for t in days track by t.key" ng-model="modelday" ng-change="updateDate(1)">';
            for (var i = 1; i <= 30; i++) {
                template += '<option label="' + i + '" value="' + i + '">' + i + '</option>';
            }
            template += '</select></div></div>';
            template += '<hr class="spacer"/>';
            template += '<div ><label class="col-lg-3 col-md-3">الشهر </label><div class="col-lg-9 col-md-9 "><select class="search  btn btn-primary ng-isolate-scope" ng-options="t.val for t in ::months track by t.key" ng-model="modelmonth"  ng-change="updateDate(2)">';
            for (var i = 1; i <= 12; i++) {
                template += '<option label="' + i + '" value="' + i + '">' + i + '</option></div>';
            }
            template += '</select></div></div>';
            template += '<hr class="spacer"/>';
            template += '<div ><label class="col-lg-3 col-md-3">السنة </label><div class="col-lg-9 col-md-9"><select class="search btn btn-primary ng-isolate-scope" ng-options="t.val for t in ::years track by t.key" ng-model="modelyear" ng-change="updateDate(3)">';
            for (var i = d.getFullYear() ; i >= 1881; i--) {
                template += '<option label="' + i + '" value="' + i + '">' + i + '</option>'
            }
            template += '</select></div></div>';
            template += '<hr class="spacer"/>';
            template += '<div class="col-sm-12 text-left"><label> </label><button type="button" value="ok" ng-click="clickDate($event)"><i class="fa fa-check" aria-hidden="true"></i></ button>';
            template += '<button type="button" value="remove" ng-click="removeDate($event)" class=""><i class="fa fa-trash-o" aria-hidden="true"></i></button>';
            template += '<button type="button" value="close" ng-click="closeDate($event)"> <i class="fa fa-times" aria-hidden="true"></i></button></div>';
            template += '</div></div>';
            return template;
            //var template = '';
            //template += '<div>';
            //template += '<input type="text" ng-model="date" ng-click="clicktext($event)" />';
            //template += '</br>';
            //template += '<div class="c-date-picker" style="display:none">';
            //template += '<span>اليوم</span><select ng-options="t.val for t in days track by t.key" ng-model="modelday" ng-change="updateDate(1)">';
            //for (var i = 1; i<= 30; i++) {
            //    template += '<option label="' + i + '" value="' + i + '">' + i + '</option>';
            //}
            //template += '</select>';
            //template += '</br>';
            //template += '<span>الشهر</span><select ng-options="t.val for t in ::months track by t.key" ng-model="modelmonth"  ng-change="updateDate(2)">';
            //for (var i = 1; i <= 12; i++) {
            //    template += '<option label="' + i + '" value="' + i + '">' + i + '</option>';
            //}
            //template += '</select>';
            //template += '</br>';
            //template += '<span>السنة</span><select ng-options="t.val for t in ::years track by t.key" ng-model="modelyear" ng-change="updateDate(3)">';
            //for (var i = d.getFullYear() ; i >= 1881; i--) {
            //    template +='<option label="' + i + '" value="' + i + '">' + i + '</option>'
            //}
            //template += '</select>';
            //template += '</br>';
            //template += '<input type="button" value="ok" ng-click="clickDate($event)" />';
            //template += '<input type="button" value="remove" ng-click="removeDate($event)" />';
            //template += '<input type="button" value="close" ng-click="closeDate($event)" />';
            //template += '</div></div>';
            //return template;
        };

   
        return {
            restrict: "E",
            scope: {
                date: '=',
                closePicker: '&',
                formID: '@'
            },

            ////bad performance   // do it for evey componant
            //template: '<div>' +
            //    '<input type="text" ng-model="date" ng-click="clicktext($event)" />' +
            //      '</br>' +
            //    '<div class="c-date-picker" style="display:none">' +
            //    '<span>اليوم</span><select ng-options="t.val for t in days track by t.key" ng-model="modelday" ng-change="updateDate(1)"></select>' +
            //    '</br>' +
            //    '<span>الشهر</span><select ng-options="t.val for t in ::months track by t.key" ng-model="modelmonth"  ng-change="updateDate(2)"></select>' +
            //    '</br>' +
            //    '<span>السنة</span><select  ng-options="t.val for t in ::years track by t.key" ng-model="modelyear" ng-change="updateDate(3)"></select>' +
            //    '</br>' +
            //    '<input type="button" value="ok" ng-click="clickDate($event)" />' +
            //     '<input type="button" value="remove" ng-click="removeDate($event)" />' +
            //     '<input type="button" value="close" ng-click="closeDate($event)" />' +
            //    '</div></div>'
            //    ,
            replace: true,
            controller: function ($scope) {
                    $scope.enddays = 30;
                    $scope.objectArray = function (start, end, order) {
                        var arr = [];
                        if (order === 1) {
                            for (var i = end; i >= start; i--) {
                                arr.push({ key: i, val: i });
                            }
                        } else {
                            for (var i = start; i <= end; i++) {
                                arr.push({ key: i, val: i });
                            }
                        }
                             
                            return arr;
                    }

                    $scope.setDate = function () {
                             if (($scope.modelyear !== (undefined || null)) && ($scope.modelmonth !== (undefined || null)) &&($scope.modelday !== (undefined || null)))
                             {
                                 $scope.datetemp =
                                     (($scope.modelday.key < 10) ? '0' + $scope.modelday.key : $scope.modelday.key) + '-' +
                                     (($scope.modelmonth.key < 10) ? '0' + $scope.modelmonth.key : $scope.modelmonth.key) + '-' +
                                      $scope.modelyear.key;
                             }
                         }
         
                    $scope.removeDate = function () { $scope.date = '';}
         
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
                                     
                                     $scope.days = $scope.objectArray(1, $scope.enddays);
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
                   $scope.years = $scope.objectArray(1881, d.getFullYear(), 1);
                    $scope.months = $scope.objectArray(1, 12);
                    $scope.modelyear = { key: d.getFullYear(), val: d.getFullYear() };
                    $scope.modelmonth = { key: d.getMonth() + 1, val: d.getMonth() + 1 };
                    $scope.days = $scope.objectArray(1, $scope.enddays);
                    $scope.modelday = { key: d.getDate(), val: d.getDate() };
                    $scope.selectedDay = d.getDate();
         
                        
                         
            },
            link: function (scope, elm, attrs) {
                ////get template dynamic for wooow performance
                elm.html(getTemplate());
                $compile(elm.contents())(scope);
 
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

                var obj = $(elm).closest('form');
                obj.on('click', function (e) {
                    if (!(($(e.target).hasClass('date-piker-input')) || ($(e.target).parents('.date-piker-input').length))) {
                        scope.datetemp = scope.date;
                        $('.c-date-picker').hide();
                    }
                });


  
            }

        };
    });




    sharedModule.directive('cTreeV', function () {

        return {
            restrict: 'E',
            replace: true,
            template: '<div class="mytree"></div>' ,
            scope: {
                nodeItems: '='
            },
            link: function (scope, elem, attr, ctrl, transclude) {

                scope.tree1 = [];
                scope.mytree = '';
                scope.$watch('nodeItems', function (newVal, oldVal) {
                    if (newVal !== oldVal) {
                        var nodes = [];

                        nodes = scope.nodeItems;

                        var draw = '';
                        var h = buildTreeRec(nodes, 0, draw);
                      //  $('#treeview').html(h)
                        ///scope.mytree = buildTreeRec(nodes, 0, draw);
                        elem.html(h);

                        // var elementResult = $(elem).find('ul:first');
                        var elementResult = $(elem).first('ul');
                        $(elementResult).treed({ openedClass: 'glyphicon-folder-open', closedClass: 'glyphicon-folder-close' });
                  
                       // console.log(h)
                    }
                });


                function buildTreeRec(arr, parentID, html) {
                    var html = '';
                    var rows = arr.where(function (t) { return t.ParentID == parentID });
                    if (rows.length > 0) {
                        html += "<ul class='tree-fahrs tree'>";
                    }

                    for (var i = 0; i < rows.length; i++) {
                        html += "<li id=" + rows[i].ID + ">";
                        html += "<a href='javascript:;'> " + rows[i].Name + "</a>";
                        html += buildTreeRec(arr, rows[i].ID, html);
                        html += "</li>";

                    }

                    if (rows.length > 0) {
                        html += "</ul>";
                    }
                    return html;
                }


            }

        };


    })



    //--- by angularjs   and too slow
    //---- verions 1
    sharedModule.directive('cTreeView', function () {

        return {
            restrict: 'E',
            replace: true,
            template: '<div class="mytree">' +

                '<script type="text/ng-template"  id="tree_item_renderer.html">' +
                    '<ul class="tree-fahrs">' +
                        '<li ng-repeat="n in tree1"> ' +
                            '<a href="#">{{n.Name}}</a>' +
                              '<div ng-include="\'tree_item_renderer.html\'" onload="tree1 = n.children" include-replace></div>' +
                        '</li>' +
                    '</ul>' +
                     '</script>' +
                    '<div ng-include="\'tree_item_renderer.html\'"></div>' +
                  '</div>'
            ,
            scope: {
                nodeItems: '='
            },
            link: function (scope, elem, attr, ctrl, transclude) {

                scope.tree1 = [];
                // var nodes = scope.nodeItems;

                /*var nodes = [{ "id": "1","parentId": "0", "text": "text1" },
                               { "id": "2", "parentId": "0", "text": "text2" },
                               { "id": "3", "parentId": "1", "text": "text3" },
                               { "id": "4", "parentId": "3", "text": "text4" },
                               { "id": "5", "parentId": "0", "text": "text5" },                          
                               { "id": "6", "parentId": "1", "text": "text6" },
                               { "id": "7", "parentId": "4", "text": "text7" },
                               { "id": "8", "parentId": "1", "text": "text8" },
                               { "id": "9", "parentId": "5", "text": "text9" },
                               { "id": "10","parentId": "5", "text": "text10" }
                   ];
                   var map = {}, node, roots = [];
                   for (var i = 0; i < nodes.length; i += 1) {
                       node = nodes[i];
                       node.children = [];
                       map[node.id] = i; // use map to look-up the parents
                       if (node.parentId !== "0") {
                           nodes[map[node.parentId]].children.push(node);
                       } else {
                           roots.push(node);
                       }
                   }
                   console.log(roots); // <-- there's your tree
   
                   scope.tree1 = roots;
                          //  var elementResult = $(elem).first('ul');
                          // $(elementResult).treed({ openedClass: 'glyphicon-folder-open', closedClass: 'glyphicon-folder-close' });
                   
                   */

                scope.$watch('nodeItems', function (newVal, oldVal) {
                    if (newVal !== oldVal) {
                        var nodes = [];
                        //nodes = [

                        //               { "ID": "5", "ParentID": "0", "Name": "text5" },
                        //               { "ID": "6", "ParentID": "1", "Name": "text6" },
                        //               { "ID": "7", "ParentID": "4", "Name": "text7" },
                        //               { "ID": "8", "ParentID": "1", "Name": "text8" },
                        //               { "ID": "9", "ParentID": "5", "Name": "text9" },
                        //               { "ID": "10", "ParentID": "5", "Name": "text10" },
                        //                { "ID": "1", "ParentID": "0", "Name": "text1" },
                        //               { "ID": "2", "ParentID": "0", "Name": "text2" },
                        //               { "ID": "3", "ParentID": "1", "Name": "text3" },
                        //               { "ID": "4", "ParentID": "3", "Name": "text4" }
                        //   ];
                        nodes = scope.nodeItems;

                        // console.log(nodes);

                        unflatten = function (array, parent, tree) {
                            tree = typeof tree !== 'undefined' ? tree : [];
                            parent = typeof parent !== 'undefined' ? parent : { ID: 0 };
                            var children = _.filter(array, function (child) { return child.ParentID == parent.ID; });

                            if (!_.isEmpty(children)) {
                                if (parent.ID == 0) {
                                    tree = children;
                                } else {
                                    parent['children'] = children;
                                }
                                _.each(children, function (child) { unflatten(array, child) });
                            }

                            return tree;
                        }
                        roots = unflatten(nodes);
                        scope.tree1 = roots;
                       // console.log(roots);
                        var elementResult = $(elem).first('ul');
                        $(elementResult).treed({ openedClass: 'glyphicon-folder-open', closedClass: 'glyphicon-folder-close' });
                    }
                });


                //transclude(function (clone) {
                //    elem.parent().append(clone);
                //});
                //$(elem).first('ul').css('color', 'blue');
                //  var elementResult = $(elem).first('ul');
                //  //console.log(elementResult.html)
                //$(elementResult).treed({ openedClass: 'glyphicon-folder-open', closedClass: 'glyphicon-folder-close' });
            }

        };


    })

    //--
    //---- verions 2
    //sharedModule.directive('cTreeView', function () {

    //    return {
    //        restrict: 'E',
    //        replace: true,
    //        template:  '<div>' +
    //                '<ul id="tree2" class="tree-fahrs1">'+
    //                    '<li ng-repeat="n in tree1"> '+
    //                        '<a href="#">{{n.text}}</a>' +
    //                          '<ul ng-repeat="d in tree1 | filter :{parentId:n.id}"><li>{{d.text}}</li></ul>' +
    //                    '</li>'+
    //                '</ul>' +
    //                '</div>'
    //        ,
    //        scope: {
    //        },
    //        link: function (scope, elem, attr, ctrl, transclude) {
    //            var nodes = [{ "id": "1", "parentId": "0", "text": "text1" },
    //                        { "id": "2", "parentId": "0", "text": "text2" },
    //                        { "id": "3", "parentId": "1", "text": "text3" },
    //                        { "id": "4", "parentId": "3", "text": "text4" },
    //                        { "id": "5", "parentId": "0", "text": "text5" }];
    //            var map = {}, node, roots = [];
    //            for (var i = 0; i < nodes.length; i += 1) {
    //                node = nodes[i];
    //                node.children = [];
    //                map[node.id] = i; // use map to look-up the parents
    //                if (node.parentId !== "0") {
    //                    nodes[map[node.parentId]].children.push(node);
    //                } else {
    //                    roots.push(node);
    //                }
    //            }
    //            //  console.log(roots); // <-- there's your tree

    //            scope.tree1 = roots;
    //            //transclude(function (clone) {
    //            //    elem.parent().append(clone);
    //            //});
    //            //$(elem).first('ul').css('color', 'blue');
    //            //r elementResult = $(elem).first('ul');
    //            //console.log(elementResult.html)
    //            $('#tree2').treed({ openedClass: 'glyphicon-folder-open', closedClass: 'glyphicon-folder-close' });
    //        }

    //    };


    //})



    //-- 
    /*
    //sharedModule.directive('cTreeView', function () {

    //    return {
    //        restrict: 'E',
    //        replace: true,
    //        template: ''
    //        ,
    //        scope: {
    //        },
    //        link: function (scope, elem) {
    //            var nodes = [{ "id": "1", "parentId": "0", "text": "text1" },
    //                        { "id": "2", "parentId": "0", "text": "text2" },
    //                        { "id": "3", "parentId": "1", "text": "text3" },
    //                        { "id": "4", "parentId": "3", "text": "text4" },
    //                        { "id": "5", "parentId": "0", "text": "text5" }];
    //            var map = {}, node, roots = [];
    //            for (var i = 0; i < nodes.length; i += 1) {
    //                node = nodes[i];
    //                node.children = [];
    //                map[node.id] = i; // use map to look-up the parents
    //                console.log(node);
    //                if (node.parentId !== "0") {
    //                    console.log("1");
    //                    nodes[map[node.parentId]].children.push(node);
    //                } else {
    //                    console.log("0");
    //                    roots.push(node);
    //                }
    //            }
    //            console.log(roots); // <-- there's your tree

    //            var html = '<div>' +
    //                    '<ul id="tree2" class="tree-fahrs">' +
    //                    '<li><a href="javascript:;">itemA</a><ul><li><a href="javascript:;">item A 1</a></li></ul></li>' +
    //                    '<li><a href="javascript:;">itemB</a></li>' +
    //                    '<li><a href="javascript:;">itemC</a></li>' +
    //                   '</ul>' +
    //                '</div>';
    //            elem.html(html)
    //            elem.treed({ openedClass: 'glyphicon-folder-open', closedClass: 'glyphicon-folder-close' });
    //        }

    //    };


    //})


    //sharedModule.directive('cTreeView', function () {

    //    return {
    //        restrict: 'E',
    //        replace: true,
    //        template: ''
    //        ,
    //        scope: {
    //        },
    //        link: function (scope, elem) {
    //            var html = '<div>' +
    //                    '<ul id="tree2" class="tree-fahrs">' +
    //                    '<li><a href="javascript:;">itemA</a><ul><li><a href="javascript:;">item A 1</a></li></ul></li>' +
    //                    '<li><a href="javascript:;">itemB</a></li>' +
    //                    '<li><a href="javascript:;">itemC</a></li>' +
    //                   '</ul>' +
    //                '</div>';
    //            elem.html(html)
    //            elem.treed({ openedClass: 'glyphicon-folder-open', closedClass: 'glyphicon-folder-close' });
    //        }

    //    };


    //})


    //sharedModule.directive('cTreeView', function () {

    //    return {
    //        restrict: 'E',
    //        replace: true,
    //        template: '<div class="mytree">' +

    //            '<script type="text/ng-template"  id="tree_item_renderer.html">'+
    //                '<ul class="tree-fahrs1">'+
    //                    '<li ng-repeat="n in tree1"> '+
    //                        '<a href="javascript:;">{{n.text}}</a>' +
    //                          '<div ng-include="\'tree_item_renderer.html\'" onload="tree1 = n.node" include-replace>></div>' +
    //                    '</li>'+
    //                '</ul>' +
    //                 '</script>'+
    //                '<div ng-include="\'tree_item_renderer.html\'"></div>'+
    //              '</div>'
    //        ,
    //        scope: {
    //        },
    //        link: function (scope, elem, attr, ctrl, transclude) {

    //            scope.tree1 = [
    //               {
    //                   text: "item A",
    //                   node: [
    //                       {
    //                           text: "item a 1",
    //                           node: [
    //                           {
    //                               text: "item a 1-1",
    //                               node: [{ text: "item a 1-1-1" }
    //                               ]
    //                           },
    //                           {}
    //                           ]
    //                       },
    //                         {
    //                             text: "item b 1",
    //                             node: [
    //                             {
    //                                 text: "item b 1-1",
    //                                 node: [{ text: "item b 1-1-1" }
    //                                 ]
    //                             },
    //                             {}
    //                             ]
    //                         }
    //                   ]
    //               },
    //               { text: "item B" },
    //               { text: "item C" },
    //               { text: "item D" }
    //            ]
    //            //transclude(function (clone) {
    //            //    elem.parent().append(clone);
    //            //});
    //            $(elem).first('ul').css('color', 'blue');
    //            var elementResult = $(elem).first('ul');
    //            console.log(elementResult.html)
    //           $(elementResult).treed({ openedClass: 'glyphicon-folder-open', closedClass: 'glyphicon-folder-close' });
    //        }

    //    };


    //})

    //sharedModule.directive('cTreeView', function () {

    //    return {
    //        restrict: 'E',
    //        replace: true,
    //        template: '<div>' +
    //                   'TreeView' +
    //                   '<ul id="tree2" class="tree-fahrs">'+
    //                    '<li><a href="javascript:;">itemA</a><ul><li><a href="javascript:;">item A 1</a></li></ul></li>'+
    //                    '<li><a href="javascript:;">itemB</a></li>'+
    //                    '<li><a href="javascript:;">itemC</a></li>'+
    //                   '</ul>'+
    //                '</div>'
    //        ,
    //        scope: {
    //        },
    //        link: function (scope, elem) {


    //            elem.treed({ openedClass: 'glyphicon-folder-open', closedClass: 'glyphicon-folder-close' });
    //        }

    //    };


    //})
    */

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


   
    sharedModule.directive('infiniteScroll', function ($parse) {
        return function ($scope, element, attrs) {
                var handler = $parse(attrs.infiniteScroll);
                element.scroll(function (evt) {
                    var scrollTop = element[0].scrollTop,
                        scrollHeight = element[0].scrollHeight,
                        offsetHeight = element[0].offsetHeight;
                    if (scrollTop === (scrollHeight - offsetHeight)) {
                        $scope.$apply(function () {
                            handler($scope);
                        });
                    }
                });
            };
        });
    sharedModule.directive('includeReplace', function () {
        return {
            require: 'ngInclude',
            restrict: 'A', /* optional */
            link: function (scope, el, attrs) {
                el.replaceWith(el.children());
            }
        };
    });

    //----- Global pure js 
    Array.prototype.findElemnet = function (val, typeElemnet) {
        var Result = 0;
        var arr = this;
        var arrlenth = arr.length;
        var typeval = 'next';
        if (typeElemnet == 'prev')
        { typeval = 'prev'; }

        if ((val <= arr[0]) && (typeval == 'prev')) {
            Result = arr[0]; return Result;
        }

        if ((val >= arr[arrlenth - 1]) && (typeval == 'next')) {
            Result = arr[arrlenth - 1]; return Result;
        }

        for (var i = 0; i < arrlenth ; i++) {
            switch (typeval) {
                case 'next': {
                    if (val < arr[i]) {
                        Result = arr[i]; return Result;
                    }
                    break;
                }
                case 'prev': {
                    if ((val >= arr[i]) && (val <= arr[i + 1])) {
                        Result = arr[i]; return Result;
                    }
                    break;
                }
            }

        }
        return Result;
    }


    Array.prototype.select = Array.prototype.map || function (selector, context) {
        context = context || window;
        var arr = [];
        var l = this.length;
        for (var i = 0; i < l; i++)
            arr.push(selector.call(context, this[i], i, this));
        return arr;
    };
    

    Array.prototype.where = Array.prototype.filter || function (predicate, context) {
        context = context || window;
        var arr = [];
        var l = this.length;
        for (var i = 0; i < l; i++)
            if (predicate.call(context, this[i], i, this) === true) arr.push(this[i]);
        return arr;
    };
})();