using Nancy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace MessageBoard
{
  public class HomeModule : NancyModule
  {
    private static string userName = "";
    private static string password = "";
    private static bool validate = true;
    private static bool registrationBool = true;
    public HomeModule()
    {
      Get["/"] = _ =>
      {
        Dictionary<string, object> model = new Dictionary<string, object> {};
        User currentUser = User.ValidateUserLogin(userName, password);
        model.Add("user", currentUser);
        List<OriginalPost> allOriginalPosts = OriginalPost.GetAll("rating");
        List<Category> allCategories = Category.GetAll();
        model.Add("posts", allOriginalPosts);
        model.Add("validate", validate);
        model.Add("categories", allCategories);
        return View["index.cshtml", model];
      };

      Get["/new"] = _ =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        User currentUser = User.ValidateUserLogin(userName, password);
        List<Category> categories = Category.GetAll();
        model.Add("user", currentUser);
        model.Add("categories", categories);
        return View["new_post.cshtml", model];
      };

      Post["/new"] = _ =>
      {
        Dictionary<string, object> model = new Dictionary<string, object> {};
        User currentUser = User.ValidateUserLogin(userName, password);
        OriginalPost newOriginalPost = new OriginalPost(Request.Form["author"], Request.Form["title"], Request.Form["main-text"], 0, DateTime.Now, currentUser.GetId());
        newOriginalPost.Save();
        List<Category> allCategories = Category.GetAll();
        // string categoryString = Request.Form["category"];
        // if (categoryString != "None")
        // {
        //   Category foundCategory = Category.Find(Request.Form["category"]);
        //   newOriginalPost.AddCategory(foundCategory);
        // }
        foreach(var category in allCategories)
        {
          string inputString = "category-" + category.GetId();
          if (Request.Form[inputString] != null)
          {
            int newInt = Request.Form[inputString];
            Category foundCategory = Category.Find(newInt);
            newOriginalPost.AddCategory(foundCategory);
          }
        }
        if ((Request.Form["new-category"] != null) && (Request.Form["new-category"] != ""))
        {
          Category newCategory = new Category(Request.Form["new-category"]);
          newCategory.Save();
          newOriginalPost.AddCategory(newCategory);
        }
        allCategories = Category.GetAll();
        List<OriginalPost> allOriginalPosts = OriginalPost.GetAll();
        model.Add("user", currentUser);
        model.Add("posts", allOriginalPosts);
        model.Add("categories", allCategories);
        model.Add("validate", validate);
        return View["index.cshtml", model];
      };

      Delete["/"] = _ =>
      {
        OriginalPost.DeleteAll();
        Dictionary<string, object> model = new Dictionary<string, object> {};
        User currentUser = User.ValidateUserLogin(userName, password);
        List<OriginalPost> allOriginalPosts = OriginalPost.GetAll();
        List<Category> allCategories = Category.GetAll();
        model.Add("user", currentUser);
        model.Add("posts", allOriginalPosts);
        model.Add("validate", validate);
        model.Add("categories", allCategories);
        return View["index.cshtml", model];
      };

      Get["/newest"] = _ =>
      {
        Dictionary<string, object> model = new Dictionary<string, object> {};
        User currentUser = User.ValidateUserLogin(userName, password);
        List<OriginalPost> allOriginalPosts = OriginalPost.GetAll("newest");
        List<Category> allCategories = Category.GetAll();
        model.Add("user", currentUser);
        model.Add("posts", allOriginalPosts);
        model.Add("validate", validate);
        model.Add("categories", allCategories);
        return View["index.cshtml", model];
      };

      Get["/oldest"] = _ =>
      {
        Dictionary<string, object> model = new Dictionary<string, object> {};
        User currentUser = User.ValidateUserLogin(userName, password);
        List<OriginalPost> allOriginalPosts = OriginalPost.GetAll("oldest");
        List<Category> allCategories = Category.GetAll();
        model.Add("user", currentUser);
        model.Add("posts", allOriginalPosts);
        model.Add("validate", validate);
        model.Add("categories", allCategories);
        return View["index.cshtml", model];
      };

      Get["/popular"] = _ =>
      {
        Dictionary<string, object> model = new Dictionary<string, object> {};
        User currentUser = User.ValidateUserLogin(userName, password);
        List<OriginalPost> allOriginalPosts = OriginalPost.GetAll("popular");
        List<Category> allCategories = Category.GetAll();
        model.Add("user", currentUser);
        model.Add("posts", allOriginalPosts);
        model.Add("validate", validate);
        model.Add("categories", allCategories);
        return View["index.cshtml", model];
      };

      Delete["/categories"] = _ =>
      {
        Category.DeleteAll();
        Dictionary<string, object> model = new Dictionary<string, object> {};
        User currentUser = User.ValidateUserLogin(userName, password);
        model.Add("user", currentUser);
        List<OriginalPost> allOriginalPosts = OriginalPost.GetAll();
        List<Category> allCategories = Category.GetAll();
        model.Add("posts", allOriginalPosts);
        model.Add("validate", validate);
        model.Add("categories", allCategories);
        return View["index.cshtml", model];
      };

      Delete["/categories/{id}"] = parameters =>
      {
        Category.DeleteById(parameters.id);
        Dictionary<string, object> model = new Dictionary<string, object> {};
        User currentUser = User.ValidateUserLogin(userName, password);
        model.Add("user", currentUser);
        List<OriginalPost> allOriginalPosts = OriginalPost.GetAll();
        List<Category> allCategories = Category.GetAll();
        model.Add("posts", allOriginalPosts);
        model.Add("validate", validate);
        model.Add("categories", allCategories);
        return View["index.cshtml", model];
      };

      Get["/posts/{id}"] = parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        OriginalPost selectedOriginalPost = OriginalPost.Find(parameters.id);
        User currentUser = User.ValidateUserLogin(userName, password);
        List<Category> allCategories = Category.GetAll();
        model.Add("post", selectedOriginalPost);
        model.Add("categories", allCategories);
        model.Add("user", currentUser);
        return View["post.cshtml", model];
      };

      Post["/posts/{id}"] = parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        User currentUser = User.ValidateUserLogin(userName, password);
        OriginalPost selectedOriginalPost = OriginalPost.Find(parameters.id);
        Comment newComment = new Comment(Request.Form["comment-author"], Request.Form["comment-main-text"], 0, parameters.id, DateTime.Now, currentUser.GetId());
        newComment.Save();
        model.Add("user", currentUser);
        model.Add("post", selectedOriginalPost);
        return View["post.cshtml", model];
      };

      Patch["/posts/{id}"] = parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        User currentUser = User.ValidateUserLogin(userName, password);
        OriginalPost selectedOriginalPost = OriginalPost.Find(parameters.id);
        selectedOriginalPost.Update(Request.Form["author"], Request.Form["main-text"]);
        model.Add("user", currentUser);
        model.Add("post", selectedOriginalPost);
        return View["post.cshtml", model];
      };
      Get["/posts/{id}/remove"] = parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        User currentUser = User.ValidateUserLogin(userName, password);
        OriginalPost selectedOriginalPost = OriginalPost.Find(parameters.id);
        selectedOriginalPost.Remove();
        model.Add("user", currentUser);
        model.Add("post", selectedOriginalPost);
        return View["post.cshtml", model];
      };

      Post["/posts/{id}/reply"] = parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        User currentUser = User.ValidateUserLogin(userName, password);
        OriginalPost selectedOriginalPost = OriginalPost.Find(parameters.id);
        Comment newComment = new Comment(Request.Form["reply-author"], Request.Form["reply-main-text"], 0, parameters.id, DateTime.Now, currentUser.GetId());
        newComment.SetParentId(Request.Form["parent-id"]);
        newComment.Save();
        model.Add("user", currentUser);
        model.Add("post", selectedOriginalPost);
        return View["post.cshtml", model];
      };
      Patch["/posts/{postId}/{commentId}"] = parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        User currentUser = User.ValidateUserLogin(userName, password);
        OriginalPost selectedOriginalPost = OriginalPost.Find(parameters.postId);
        Comment selectedComment = Comment.Find(parameters.commentId);
        selectedComment.Update(Request.Form["comment-main-text"]);
        model.Add("user", currentUser);
        model.Add("post", selectedOriginalPost);
        return View["post.cshtml", model];
      };
      Get["/posts/{postId}/{commentId}/remove-comment"] = parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        User currentUser = User.ValidateUserLogin(userName, password);
        OriginalPost selectedOriginalPost = OriginalPost.Find(parameters.postId);
        Comment selectedComment = Comment.Find(parameters.commentId);
        selectedComment.Remove();
        model.Add("user", currentUser);
        model.Add("post", selectedOriginalPost);
        return View["post.cshtml", model];
      };

      Delete["/posts/{id}/comment"] = parameters =>
      {
        OriginalPost selectedOriginalPost = OriginalPost.Find(parameters.id);
        Comment.DeleteAll();
        return View["post.cshtml", selectedOriginalPost];
      };

      Get["/comments/{id}"] = parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        User currentUser = User.ValidateUserLogin(userName, password);
        Comment comment = Comment.Find(parameters.id);
        model.Add("user", currentUser);
        model.Add("comment", comment);
        return View["comment.cshtml", model];
      };

      Post["/comments/{id}"] = parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        User currentUser = User.ValidateUserLogin(userName, password);
        Comment comment = Comment.Find(parameters.id);
        int postId = comment.GetPostId();
        Comment newComment = new Comment(Request.Form["comment-author"], Request.Form["comment-main-text"], 0, postId, DateTime.Now, currentUser.GetId());
        newComment.SetParentId(Request.Form["comment-id"]);
        newComment.Save();
        model.Add("user", currentUser);
        model.Add("comment", comment);
        return View["comment.cshtml", model];
      };
      Patch["/comments/{id}"] = parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        User currentUser = User.ValidateUserLogin(userName, password);
        Comment selectedComment = Comment.Find(parameters.id);
        selectedComment.Update(Request.Form["update-main-text"]);
        model.Add("user", currentUser);
        model.Add("comment", selectedComment);
        return View["comment.cshtml", model];
      };
      Get["/comments/{id}/remove-comment"] = parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        User currentUser = User.ValidateUserLogin(userName, password);
        Comment selectedComment = Comment.Find(parameters.id);
        selectedComment.Remove();
        model.Add("user", currentUser);
        model.Add("comment", selectedComment);
        return View["comment.cshtml", model];
      };
      Patch["/comments/{origCommentId}/{commentId}"] = parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        User currentUser = User.ValidateUserLogin(userName, password);
        Comment originalComment = Comment.Find(parameters.origCommentId);
        Comment selectedComment = Comment.Find(parameters.commentId);
        selectedComment.Update(Request.Form["update-main-text"]);
        model.Add("user", currentUser);
        model.Add("comment", originalComment);
        return View["comment.cshtml", model];
      };
      Get["/comments/{origCommentId}/{commentId}/remove-comment"] = parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        User currentUser = User.ValidateUserLogin(userName, password);
        Comment originalComment = Comment.Find(parameters.origCommentId);
        Comment selectedComment = Comment.Find(parameters.commentId);
        selectedComment.Remove();
        model.Add("user", currentUser);
        model.Add("comment", originalComment);
        return View["comment.cshtml", model];
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
        string newUserPassword = User.HashPassword(Request.Form["password1"]);
        User newUser = new User(newUserName, newUserPassword);
        newUser.Save();
        return View["register_success.cshtml", newUser];
      };
      
      Get["/login"] = _ =>
      {
        return View["login.cshtml"];
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
        List<Category> allCategories = Category.GetAll();
        model.Add("user", currentUser);
        model.Add("posts", allPosts);
        model.Add("categories", allCategories);
        return View["index.cshtml", model];
      };

      Post["/logout"] = _ =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        userName = "";
        password = "";
        User currentUser = null;
        List<OriginalPost> allPosts = OriginalPost.GetAll();
        List<Category> allCategories = Category.GetAll();
        model.Add("user", currentUser);
        model.Add("posts", allPosts);
        model.Add("categories", allCategories);
        model.Add("validate", validate);
        return View["index.cshtml", model];
      };

      Post["/posts/search"] = _ =>
      {
        Dictionary<string, object> model = new Dictionary<string, object> {};
        User currentUser = User.ValidateUserLogin(userName, password);
        model.Add("user", currentUser);
        model.Add("validate", validate);
        return View["template.cshtml", model];
      };

      Patch["/vote"] = _ =>
      {
        User currentUser = User.ValidateUserLogin(userName, password);
        OriginalPost post = OriginalPost.Find(Request.Form["post-id"]);
        post.Vote(currentUser.GetId(), Request.Form["vote"]);
        Dictionary<string, object> model = new Dictionary<string, object> {};
        List<OriginalPost> allOriginalPosts = OriginalPost.GetAll();
        List<Category> allCategories = Category.GetAll();
        model.Add("posts", allOriginalPosts);
        model.Add("validate", validate);
        model.Add("categories", allCategories);
        model.Add("user", currentUser);
        return View["index.cshtml", model];
      };

      Get["/categories/{id}"] = parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object> {};
        User currentUser = User.ValidateUserLogin(userName, password);
        model.Add("user", currentUser);
        Category foundCategory = Category.Find(parameters.id);
        List<OriginalPost> categoryOriginalPosts = foundCategory.GetPosts();
        List<Category> allCategories = Category.GetAll();
        model.Add("category", foundCategory);
        model.Add("categories", allCategories);
        model.Add("posts", categoryOriginalPosts);
        model.Add("validate", validate);
        return View["category.cshtml", model];
      };

      Post["/categories/search"] = _ =>
      {
        Dictionary<string, object> model = new Dictionary<string, object> {};
        User currentUser = User.ValidateUserLogin(userName, password);
        model.Add("user", currentUser);
        List<Category> searchResults = Category.SearchByKeyword(Request.Form["keyword"]);
        List<Category> allCategories = Category.GetAll();
        model.Add("results", searchResults);
        model.Add("categories", allCategories);
        model.Add("validate", validate);
        model.Add("keyword", Request.Form["keyword"]);
        return View["category_results.cshtml", model];
      };

      Get["/new_category"] = _ =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        User currentUser = User.ValidateUserLogin(userName, password);
        model.Add("user", currentUser);
        return View["new_category.cshtml", model];
      };

      Post["/new_category"] = _ =>
      {
        Dictionary<string, object> model = new Dictionary<string, object> {};
        User currentUser = User.ValidateUserLogin(userName, password);
        Category newCategory = new Category(Request.Form["name"]);
        newCategory.Save();
        List<OriginalPost> allOriginalPosts = OriginalPost.GetAll();
        List<Category> allCategories = Category.GetAll();
        model.Add("user", currentUser);
        model.Add("posts", allOriginalPosts);
        model.Add("categories", allCategories);
        model.Add("validate", validate);
        return View["index.cshtml", model];
      };

      Patch["/posts/{id}/vote"] = parameters =>
      {
        User currentUser = User.ValidateUserLogin(userName, password);
        Comment comment = Comment.Find(Request.Form["comment-id"]);
        comment.Vote(currentUser.GetId(), Request.Form["vote"]);
        Dictionary<string, object> model = new Dictionary<string, object>{};
        OriginalPost selectedOriginalPost = OriginalPost.Find(parameters.id);
        List<Category> allCategories = Category.GetAll();
        model.Add("post", selectedOriginalPost);
        model.Add("categories", allCategories);
        model.Add("user", currentUser);
        return View["post.cshtml", model];
      };
    }
  }
}
