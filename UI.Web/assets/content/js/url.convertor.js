function ToURL(val) {
    var retVal =
    val.toLocaleLowerCase('tr-TR')
    .replaceAll('ç', 'c')
    .replaceAll('ı', 'i')
    .replaceAll('ğ', 'g')
    .replaceAll('ö', 'o')
    .replaceAll('ş', 's')
    .replaceAll('ü', 'u')
    .replaceAll('(', '-')
    .replaceAll(')', '-')
    .replaceAll('&', '-ve-')
    .replaceAll('#', '')
    .replaceAll('?', '')
    .replaceAll(':', '-')
    .replaceAll('"', '')
    .replaceAll("'", "")
    .replaceAll('%', 'yuzde')
    .replaceAll('.', '')
    .replaceAll(',', '')
    .replaceAll('-', '')
    .replaceAll(' ', '-')
    .replaceAll('/', '-')
    .replaceAll('\\', '-')
    .replaceAll('--', '-')
    .replaceAll('  ', '')

    if (retVal.charAt(retVal.length - 1) == '-') {
        retVal = retVal.substr(0, retVal.length - 1);
    }
    return retVal;
}

String.prototype.replaceAll = function (str1, str2, ignore) {
    return this.replace(new RegExp(str1.replace(/([\/\,\!\\\^\$\{\}\[\]\(\)\.\*\+\?\|\<\>\-\&])/g, "\\$&"), (ignore ? "gi" : "g")), (typeof (str2) == "string") ? str2.replace(/\$/g, "$$$$") : str2);
}