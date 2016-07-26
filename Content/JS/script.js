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

});
