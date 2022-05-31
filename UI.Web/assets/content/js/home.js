$(document).ready(function () {
    initHeadLines();
    configureHover();
    putNumberHeadlines();
});

function initHeadLines() {
    $('#head-lines').owlCarousel({
        center: false,
        items: 1,
        loop: true,
        nav: false,
        dots: true,
        autoplay: false,
        margin: 30,
        lazyLoad: true,
        navSpeed: 500,
        navText: ['<i class="ui-arrow-left">', '<i class="ui-arrow-right">'],
        responsive: {
            768: {
                items: 1
            },
            540: {
                items: 1
            }
        }
    });
}

function configureHover() {
    setTimeout(function () {
        $('.owl-dot').hover(function () {
            $(this).click();
        });
    }, 1000)
}
function putNumberHeadlines() {
    var dots = $('.owl-dots .owl-dot');

    for (var i = 0; i < dots.length; i++) {
        $(dots[i]).find('span').text(i + 1)
        console.log();
    }
}