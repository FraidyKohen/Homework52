$(() => {
    console.log("NewAd Jquery up & running");
    $("textarea").on("change", function () {
        let valid = FormValidity();
        if (valid) {
            $("#submit-button").prop("disabled", false);
        }
        else {
            $("#submit-button").prop("disabled", true);

        }
    });


    function FormValidity() {
        let phoneNumber = $("#phone-number").val();
        let description = $("#description").val();
        if (phoneNumber && description) {
            return true;
        }
        return false;
    }
});