

$(document).ready(function(){

  $("#register").validate({
    rules: {
      password1: {
        required: true,
        minlength: 6,
        maxlength: 10,
      },
      password2: {
        equalTo: "#password1",
        minlength: 6,
        maxlength: 10,
      }
    },

    messages: {
      password1: {
        required: "the password is required"
      }
    }

  });
});
