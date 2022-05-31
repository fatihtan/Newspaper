var global = (function () {
    'use strict';

    function loaderStart(selector) {
        $(selector).addClass('is-active')
    }

    function loaderStop(selector) {
        $(selector).removeClass('is-active');
    }

    function getParameterByName(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }

    function checkSearch(form) {
        if ($(form).find('input[name="q"]').val() == '') {
            alert("Arama terimini giriniz.");
            return false;
        }

        if ($(form).find('input[name="q"]').val().length < 5) {
            alert("Arama terimi minimum 5 harften oluşmalıdır.");
            return false;
        }

        global.showLoader();
        return true;
    }

    function showLoader() {
        global.loaderStart('#loaderProcess');
        return true;
    }

    function hideLoader() {
        global.loaderStop('#loaderProcess');
        return true;
    }

    function getFlashNews() {
        $.post('/News/GetFlashNewsListAjax')
        .done(function (result) {
            console.log(result);
            if (result.IsSuccess) {
                for (var i = 0; i < result.Data.length; i++) {
                    var item = result.Data[i];
                    var liItem = $('<li class="newsticker__item"><a href="/Haber/' + ToURL(item.Title) + '/' + item.ID + '" class="newsticker__item-url">' + item.Title + '</a></li>');

                    liItem.appendTo($('.newsticker .newsticker__list'));

                    /* News Ticker*/
                    var $newsTicker = $('.newsticker__list');

                    if ($newsTicker.length) {
                        $newsTicker.newsTicker({
                            row_height: 34,
                            max_rows: 1,
                            prevButton: $('#newsticker-button--prev'),
                            nextButton: $('#newsticker-button--next')
                        });
                    }

                }
            }
            
        })
        .fail(function () {

        })
    }

    return {
        loaderStart: loaderStart,
        loaderStop: loaderStop,
        getParameterByName: getParameterByName,
        checkSearch:checkSearch,
        showLoader: showLoader,
        hideLoader: hideLoader,
        getFlashNews: getFlashNews
    }
})();