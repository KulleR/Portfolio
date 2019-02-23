function detectIE() {
    var ua = window.navigator.userAgent;
    // Test values; Uncomment to check result …

    // IE 10
    // ua = 'Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)';

    // IE 11
    // ua = 'Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko';

    // Edge 12 (Spartan)
    // ua = 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36 Edge/12.0';

    // Edge 13
    // ua = 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2486.0 Safari/537.36 Edge/13.10586';
    if (window.external.IsWinOperator) {
        var trident7 = ua.indexOf('Trident/7.0');
        if (trident7 !== -1) {
            return 11;
        }
        var trident6 = ua.indexOf('Trident/6.0');
        if (trident6 !== -1) {
            return 10;
        }
        var trident5 = ua.indexOf('Trident/5.0');
        if (trident5 !== -1) {
            return 9;
        }
        var trident4 = ua.indexOf('Trident/4.0');
        if (trident4 !== -1) {
            return 8;
        }
    }

    var msie = ua.indexOf('MSIE ');
    if (msie > 0) {
        var ver = parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10);
        //console.log("Browser is IE v." + ver);
        // IE 10 or older => return version number
        return ver;
    }

    var edge = ua.indexOf('Edge/');
    if (edge > 0) {
        // Edge (IE 12+) => return version number
        var ver = parseInt(ua.substring(edge + 5, ua.indexOf('.', edge)), 10);
        // console.log("Browser is " + ver);
        return ver;
    }

    return false;
}