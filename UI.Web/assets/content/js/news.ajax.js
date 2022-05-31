var News = (function () {
    'use strict';

    /*  CommentSaveAjax  */
    function CommentSaveAjaxBegin() {
        if ($('#fullname').val() == '') {
            toastr.warning("Ad Soyad Giriniz");
            $('#fullname').focus();
            return false;
        }

        if ($('#email').val() == '') {
            toastr.warning("Email adresinizi giriniz.");
            $('#email').focus();
            return false;
        }

        if ($('#commentcontent').val() == '') {
            toastr.warning("Yorumunuzu yazınız.");
            $('#commentcontent').focus();
            return false;
        }

        if ($('#captchacode').val() == '') {
            toastr.warning("Güvenlik kodunu giriniz.");
            $('#captchacode').focus();
            return false;
        }

        global.showLoader();
        return true;
    }

    function CommentSaveAjaxSuccess(result) {
        if (result.IsSuccess) {
            $('#fullname').val('');
            $('#email').val('');
            $('#commentcontent').val('');
            $('#captchacode').val('');
        }

        reloadCaptcha();
        updateSuccess(result);
    }

    function CommentSaveAjaxFailure(result) {
        updateSuccess(result);
    }
    /*  CommentSaveAjax  */

    function updateSuccess(result) {
        global.hideLoader();
        if (result.IsSuccess) {
            toastr.success(result.Message);
        } else {
            toastr.error(result.Message);
        }
    }

    return {
        CommentSaveAjaxBegin: CommentSaveAjaxBegin,
        CommentSaveAjaxSuccess: CommentSaveAjaxSuccess,
        CommentSaveAjaxFailure: CommentSaveAjaxFailure
    }
})();