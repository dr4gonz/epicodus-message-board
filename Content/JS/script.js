$(document).ready(function(){

  $(".hide-form").click(function(){
    $(".comment-form").hide();
    $(".show-form").show();
  });

  $(".show-form").click(function(){
    $(".show-form").show();
    $(".comment-form").hide();
    var id = this.id;
    $("#"+id+"-form").show();
    $("#"+id).hide();
  });


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
