///-------Date filter-----------------
//
//--------------------------------------------------------
angular.module('ng').filter('jsonDate', ['$filter', function ($filter) {
    return function (input, format) {
        return (input) ? $filter('date')(parseInt(input.substr(6)), format) : '';
    };
} ]);

///-------Unique filter-----------------
//
//--------------------------------------------------------

angular.module('ng').filter('unique', function () {

    return function (items, filterOn) {

        if (filterOn === false) {
            return items;
        }

        if ((filterOn || angular.isUndefined(filterOn)) && angular.isArray(items)) {
            var hashCheck = {}, newItems = [];

            var extractValueToCompare = function (item) {
                if (angular.isObject(item) && angular.isString(filterOn)) {
                    return item[filterOn];
                } else {
                    return item;
                }
            };

            angular.forEach(items, function (item) {
                var valueToCheck, isDuplicate = false;

                for (var i = 0; i < newItems.length; i++) {
                    if (angular.equals(extractValueToCompare(newItems[i]), extractValueToCompare(item))) {
                        isDuplicate = true;
                        break;
                    }
                }
                if (!isDuplicate) {
                    newItems.push(item);
                }

            });
            items = newItems;
        }
        return items;
    };
});


///-------Array filter-----------------
//
//--------------------------------------------------------

angular.module('ng').filter('mulf', function () {
    return function (list, genres) {


        if (genres.length > 0) {
            var items = [];

            angular.forEach(genres, function (gen, key) {

                angular.forEach(list, function (value, key) {
                    if (gen.Payee == value.Payee) {
                        this.push(value);
                    }
                }, items);

            });



            return items;
        }
        else {
            return list;
        }

    };
}); 

///-------Date filter-----------------
//
//--------------------------------------------------------