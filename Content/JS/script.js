$(document).ready(function(){
  $(".show-comment-form-top").click(function(){
    $(".show-form").show();
    $(".show-comment-form-top").hide();
    $("comment-form").hide();
    $(".comment-form-top").show();
  });

  $(".hide-comment-form-top").click(function(){
    $(".show-form").show();
    $(".comment-form-top").hide();
  });

  $(".show-comment-form-child").click(function(){
    $(".show-form").show();
    $(".comment-form").hide();
    var id = this.id;
    $("#"+id+"-form").show();
    $("#"+id).hide();
  });

  $(".hide-comment-form-child").click(function(){
    $(".comment-form-child").hide();
    $(".show-form").show();
  });

  $(".show-comment-form-grandchild").click(function(){
    $(".show-form").show();
    $(".comment-form").hide();
    var id = this.id;
    $("#"+id+"-form").show();
    $("#"+id).hide();
  });

  $(".hide-comment-form-grandchild").click(function(){
    $(".comment-form-grandchild").hide();
    $(".show-form").show();
  });

});
