var Log = (function () {
    'use strict';

    /*  URLError  */
    function URLError() {

        $.post('/Log/URLError',
            { 'URL': window.location.href })
            .done(function (r) {
                console.log(r);
            })
            .fail(function () {
            })

        return true;
    }
    /*  URLError  */

    /*  Navigation  */
    function Navigation() {

        $.post('/Log/Navigation',
            { 'URL': window.location.href })
            .done(function (r) {
                console.log(r);
            })
            .fail(function () {
            })

        return true;
    }
    /*  Navigation  */

    /*  IncreaseNews  */
    function IncreaseNews(id) {

        $.post('/Log/IncreaseNews',
            { 'ID': id })
            .done(function (r) {
                console.log(r);
            })
            .fail(function () {
            })

        return true;
    }
    /*  IncreaseNews  */


    function updateSuccess(result) {
        if (result.IsSuccess) {
            toastr.success(result.Message);
        } else {
            toastr.error(result.Message);
        }
        global.loaderStop('#loaderProcess');
    }

    return {
        URLError: URLError,
        Navigation: Navigation,
        IncreaseNews: IncreaseNews
    }
})();