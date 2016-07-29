

$(document).ready(function(){


  var counter = 8;
  var interval = setInterval(function() {
      counter--;
      if (counter == 0) {
          redirect();
          clearInterval(interval);
      }
  }, 60000);

  function redirect() {
  document.logout.submit();
  alert("You've been logged out due to inactivity.")
  }


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

  $(".navform-show").click(function(){
    $(".navform").show();
    $(".navform-show").hide();
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
