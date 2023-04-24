$(() => {
    console.log("NewUser Jquery up & running");
    $("input").on("change", function () {
        let valid = FormValidity();
        if (valid) {
            console.log("All inputs have value")
            $("#submit-button").prop("disabled", false);
        }
        else {
            $("#submit-button").prop("disabled", true);
        }
    });


    function FormValidity() {
        let name = $("#name").val();
        let email = $("#email").val();
        let phoneNumber = $("#phone-number").val();
        let password = $("#password").val();
        if (password.length < 8) {
            password = null;
        }
        if (name && email && phoneNumber && password) {
            return true;
        }
        return false;
    }
});