var Generic = (function () {
    'use strict';

    /*  ContactMessageSaveAjax  */
    function ContactMessageSaveAjaxBegin() {
        if ($('#firstname').val() == '') {
            toastr.warning("Adınızı giriniz.");
            $('#firstname').focus();
            return false;
        }

        if ($('#lastname').val() == '') {
            toastr.warning("Soyadınızı giriniz.");
            $('#lastname').focus();
            return false;
        }

        if ($('#email').val() == '') {
            toastr.warning("Email adresinizi giriniz.");
            $('#email').focus();
            return false;
        }

        if ($('#message').val() == '') {
            toastr.warning("Mesajınızı yazınız.");
            $('#message').focus();
            return false;
        }

        if (!$('#terms').prop('checked')) {
            toastr.warning("Aydınlatma metnini onaylamanız gerekiyor.");
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

    function ContactMessageSaveAjaxSuccess(result) {
        if (result.IsSuccess) {
            $('#firstname').val('');
            $('#lastname').val('');
            $('#email').val('');
            $('#phonenumber').val('');
            $('#message').val('');
            $('#terms').prop('checked', false);
            $('#captchacode').val('');
        }

        reloadCaptcha();
        updateSuccess(result);
    }

    function ContactMessageSaveAjaxFailure(result) {
        reloadCaptcha();
        updateSuccess(result);
    }
    /*  ContactMessageSaveAjax  */


    /*  SubscribeSaveAjax  */
    function SubscribeSaveAjaxBegin(r) {
        console.log(r);

        if ($('#subscribe_email').val() == '') {
            toastr.warning("Email adresinizi giriniz.");
            $('#subscribe_email').focus();
            return false;
        }

        global.showLoader();
        return true;
    }

    function SubscribeSaveAjaxSuccess(result) {
        if (result.IsSuccess) {
            $('#subscribe_email').val('');
        }

        updateSuccess(result);
    }

    function SubscribeSaveAjaxFailure(result) {
        updateSuccess(result);
    }
    /*  SubscribeSaveAjax  */


    function updateSuccess(result) {
        global.hideLoader();
        if (result.IsSuccess) {
            toastr.success(result.Message);
        } else {
            toastr.error(result.Message);
        }
    }

    return {
        SubscribeSaveAjaxBegin: SubscribeSaveAjaxBegin,
        SubscribeSaveAjaxSuccess: SubscribeSaveAjaxSuccess,
        SubscribeSaveAjaxFailure: SubscribeSaveAjaxFailure,

        ContactMessageSaveAjaxBegin: ContactMessageSaveAjaxBegin,
        ContactMessageSaveAjaxSuccess: ContactMessageSaveAjaxSuccess,
        ContactMessageSaveAjaxFailure: ContactMessageSaveAjaxFailure
    }
})();