using Nancy;
using System;
using System.Collections.Generic;

namespace MessageBoard
{
  public class HomeModule : NancyModule
  {
    private string userName = "";
    private string password = "";
    private bool validate = true;
    private bool registrationBool = true;
    public HomeModule()
    {
      Get["/"] = _ =>
      {
        Dictionary<string, object> model = new Dictionary<string, object> {};
        User currentUser = User.ValidateUserLogin(userName, password);
        model.Add("user", currentUser);
        List<OriginalPost> allOriginalPosts = OriginalPost.GetAll();
        model.Add("posts", allOriginalPosts);
        model.Add("validate", validate);
        return View["index.cshtml", model];
      };

      Get["/new"] = _ =>
      {
        List<Category> categories = Category.GetAll();
        return View["new_post.cshtml", categories];
      };

      Post["/new"] = _ =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        User currentUser = User.ValidateUserLogin(userName, password);
        OriginalPost newOriginalPost = new OriginalPost(currentUser.GetName(), Request.Form["title"], Request.Form["main-text"], 0, DateTime.Now, currentUser.GetId());
        newOriginalPost.Save();
        Category foundCategory = Category.Find(Request.Form["category"]);
        newOriginalPost.AddCategory(foundCategory);
        List<OriginalPost> allOriginalPosts = OriginalPost.GetAll();
        model.Add("user", currentUser);
        model.Add("posts", allOriginalPosts);
        return View["index.cshtml", model];
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

      Get["/comments/{id}"] = parameters =>
      {
        Comment comment = Comment.Find(parameters.id);
        return View["comment.cshtml", comment];
      };
      Post["/comments/{id}"] = parameters =>
      {
        Comment comment = Comment.Find(parameters.id);
        int postId = comment.GetPostId();
        Comment newComment = new Comment(Request.Form["comment-author"], Request.Form["comment-main-text"], 0, postId, DateTime.Now, 3);
        newComment.SetParentId(Request.Form["comment-id"]);
        newComment.Save();
        return View["comment.cshtml", comment];
      };

      Get["/register"] = _ => {

        return View["register.cshtml", registrationBool];
      };
      Post["/register"] = _ =>
      {
        List<User> allUsers = User.GetAll();
        string newUserName = Request.Form["name"];
        foreach(var user in allUsers)
        {
          if(user.GetName() == newUserName)
          {
            registrationBool = false;
            return View["register.cshtml", registrationBool];
          }
        }
        User newUser = new User(newUserName, Request.Form["password1"]);
        newUser.Save();
        return View["register_success.cshtml", newUser];
      };
      Post["/login"] = _ =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        userName = Request.Form["user-name"];
        password = Request.Form["password"];
        User currentUser = User.ValidateUserLogin(userName, password);
        if (currentUser == null)
        {
          model.Remove("validate");
          validate = false;
          model.Add("validate", validate);
        }
        else
        {
          model.Add("validate", validate);
        }
        List<OriginalPost> allPosts = OriginalPost.GetAll();
        model.Add("user", currentUser);
        model.Add("posts", allPosts);
        return View["index.cshtml", model];
      };
      Post["/logout"] = _ =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        userName = "";
        password = "";
        User currentUser = null;
        List<OriginalPost> allPosts = OriginalPost.GetAll();
        model.Add("user", currentUser);
        model.Add("posts", allPosts);
        model.Add("validate", validate);
        return View["index.cshtml", model];
      };
    }
  }
}
