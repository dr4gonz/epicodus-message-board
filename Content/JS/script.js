$(document).ready(function(){
  $(".show-comment-form-top").click(function(){
    $(".show-comment-form-top").hide();
    $(".child-reply").hide();
    $(".reply-comment").show();
    $(".comment-form-top").show();
  });

  $(".hide-comment-form-top").click(function(){
    $(".show-comment-form-top").show();
    $(".comment-form-top").hide();
  });

  $(".show-comment-form-child").click(function(){
    $(".show-comment-form-child").show();
    $(".comment-form-top").hide();
    $(".comment-form-child").hide();
    var id = this.id;
    $("#"+id+"-form").show();
    $("#"+id).hide();
  });

  $(".hide-comment-form-child").click(function(){
    $(".comment-form-child").hide();
    $(".show-comment-form-child").show();
  });

  $(".reply-comment-child").click(function(){
    $(".reply-comment-form").hide();
    $(".show-comment-form-top").show();
    $(".comment-form-top").hide();
    $(".comment-child-reply").hide();
    var id = this.id;
    $("#"+id+"-child-form").show();
    $("#"+id+"-form").hide();
    $("#"+id).hide();
  });

});
