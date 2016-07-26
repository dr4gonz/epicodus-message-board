$(document).ready(function(){
  $(".reply-toplevel").click(function(){
    $(".reply-toplevel").hide();
    $(".child-reply").hide();
    $(".reply-comment").show();
    $(".comment-form-top").show();
  });

  $(".reply-toplevel-hide").click(function(){
    $(".reply-toplevel").show();
    $(".comment-form-top").hide();
  });

  $(".reply-comment").click(function(){
    $(".reply-comment-form").hide();
    $(".reply-toplevel").show();
    $(".comment-form-top").hide();
    var id = this.id;
    $("#"+id+"-form").show();
    $("#"+id).hide();
  });

  $(".reply-comment-child").click(function(){
    $(".reply-comment-form").hide();
    $(".reply-toplevel").show();
    $(".comment-form-top").hide();
    $(".comment-child-reply").hide();
    var id = this.id;
    $("#"+id+"-child-form").show();
    $("#"+id+"-form").hide();
    $("#"+id).hide();
  });

});
