$(function () {
    var strToken = generate_token(10);
    var js = document.querySelectorAll('script');

    for (var i = 0; i < js.length; i++) {
        js[i].src = js[i].src + '?' + Date.now();
    }

    $(".navigation li a").each(function () {
        var str_href = $(this).attr("href") + "?" + strToken;
        $(this).attr("href", str_href);

    });

    /*var css = document.querySelectorAll('link');

    for (var i = 0; i < css.length; i++) {
        css[i].href = css[i].href + '?' + Date.now();
    }*/

    function generate_token(length) {
        //edit the token allowed characters
        var a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".split("");
        var b = [];
        for (var i = 0; i < length; i++) {
            var j = (Math.random() * (a.length - 1)).toFixed(0);
            b[i] = a[j];
        }
        return b.join("");
    }

});