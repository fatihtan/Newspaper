$(document).ready(function () {
    var images = $('img');
    images.addClass('entry__img');

    Log.IncreaseNews($('#NewsID').val());
    getNewsListPrevNext($('#NewsID').val(), $('#CategoryID').val());
});

function getNewsListPrevNext(id, categoryID) {
    $.post('/News/GetNewsListPrevNextAjax', { 'ID': id, 'CategoryID': categoryID })
        .done(function (r) {
            if (r.IsSuccess) {

                var showContainer = false;

                if (r.Data.Prev != null) {
                    var aPrev = $('#container-prev-next #container-prev #a-prev');
                    aPrev.text(r.Data.Prev.Title);
                    aPrev.attr('href', '/Haber/' + ToURL(r.Data.Prev.Title) + '/' + r.Data.Prev.ID);

                    showContainer = true;
                }

                if (r.Data.Next != null) {
                    var aNext = $('#container-prev-next #container-next #a-next');
                    aNext.text(r.Data.Next.Title);
                    aNext.attr('href', '/Haber/' + ToURL(r.Data.Next.Title) + '/' + r.Data.Next.ID);

                    showContainer = true;
                }

                if (showContainer) {
                    $('#container-prev-next').removeClass('hidden');
                }
            }
        })
        .fail(function () {
        })
}

function ShareOnTwitter(e, t, o, n, c) {
    return window.open("https://twitter.com/intent/tweet?hashtags=" + c + "," + o + "&original_referer=" + encodeURIComponent(e) + "&related=" + o + "&text=" + encodeURIComponent(n) + "&url=" + encodeURIComponent(e) + "&via=" + o, "shareit", "toolbar=0,status=0,width=626,height=436"),
    !1
}

function ShareOnFacebook(e) {
    return window.open("https://www.facebook.com/sharer/sharer.php?u=" + encodeURIComponent(e), "facebooksharedialog", "toolbar=0,status=0,width=626,height=436"),
    !1
}

function reloadCaptcha() {
    $('#captchacode').val('');
    document.getElementById("captcha-image").src = '/Captcha/Get?refresh=true&dt=' + new Date().getTime();
}

$('.log-redirect').on('click', function () {
    var surl = $(this).data('surl');
    var turl = $(this).data('turl');
    var rid = $(this).data('rid');

    $.post('/News/RedirectAjax', { 'surl': surl, 'turl': turl, 'rid': rid })
    .done(function (r) {
        console.log(r);
    })
    .fail(function () {

    })
});