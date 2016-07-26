using Nancy;
using System;
using System.Collections.Generic;

namespace MessageBoard
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ =>
      {
        List<OriginalPost> allOriginalPosts = OriginalPost.GetAll();
        return View["index.cshtml", allOriginalPosts];
      };
      Post["/"] = _ =>
      {
        OriginalPost newOriginalPost = new OriginalPost(Request.Form["author"], Request.Form["title"], Request.Form["main-text"], 0, DateTime.Now, 1);
        newOriginalPost.Save();
        List<OriginalPost> allOriginalPosts = OriginalPost.GetAll();
        return View["index.cshtml", allOriginalPosts];
      };
      Delete["/"] = _ =>
      {
        OriginalPost.DeleteAll();
        List<OriginalPost> allOriginalPosts = OriginalPost.GetAll();
        return View["index.cshtml", allOriginalPosts];
      };
      Get["/posts/{id}"] = parameters =>
      {
        OriginalPost selectedOriginalPost = OriginalPost.Find(parameters.id);
        return View["post.cshtml", selectedOriginalPost];
      };
      Post["/posts/{id}"] = parameters =>
      {
        OriginalPost selectedOriginalPost = OriginalPost.Find(parameters.id);
        Comment newComment = new Comment(Request.Form["comment-author"], Request.Form["comment-main-text"], 0, parameters.id, DateTime.Now, 3);
        newComment.Save();
        return View["post.cshtml", selectedOriginalPost];
      };
      Post["/posts/{id}/reply"] = parameters =>
      {
        OriginalPost selectedOriginalPost = OriginalPost.Find(parameters.id);
        Comment newComment = new Comment(Request.Form["reply-author"], Request.Form["reply-main-text"], 0, parameters.id, DateTime.Now, 3);
        newComment.SetParentId(Request.Form["parent-id"]);
        newComment.Save();
        return View["post.cshtml", selectedOriginalPost];
      };
      Delete["/posts/{id}/comment"] = parameters =>
      {
        OriginalPost selectedOriginalPost = OriginalPost.Find(parameters.id);
        Comment.DeleteAll();
        return View["post.cshtml", selectedOriginalPost];
      };
      Get["/register"] = _ => View["register.cshtml"];

      Post["/register/success"] = _ =>
      {
        User newUser = new User(Request.Form["name"], Request.Form["password1"]);
        newUser.Save();
        return View["register_success.cshtml", newUser];
      };
      // Post["/login"] = _ =>
      // {
      //   namePassword[0] = Request.Form["user-name"];
      //   namePassword[1] = Request.Form["password"];
      //   User.ValidateUserLogin()
      //
      // }
    }
  }
}
