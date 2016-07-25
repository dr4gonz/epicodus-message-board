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
        OriginalPost newOriginalPost = new OriginalPost(Request.Form["author"], Request.Form["title"], Request.Form["main-text"]);
        newOriginalPost.Save();
        List<OriginalPost> allOriginalPosts = OriginalPost.GetAll();
        return View["index.cshtml", allOriginalPosts];
      };
      Delete["/"] = _ =>
      {
        Post.DeleteAll();
        List<OriginalPost> allOriginalPosts = OriginalPost.GetAll();
        return View["index.cshtml", allOriginalPosts];
      };
      Get["/posts/{id}"] = parameters =>
      {
        OriginalPost selectedOriginalPost = OriginalPost.Find(parameters.id);
        return View["post.cshtml", selectedOriginalPost];
      };
    }
  }
}
