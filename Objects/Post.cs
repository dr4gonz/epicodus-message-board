using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MessageBoard
{
  public class Post
  {
    private int _id;
    private string _author;
    private string _title;
    private string _mainText;

    public Post(string author, string title, string mainText, int id=0)
    {
      _id = id;
      _author = author;
      _title = title;
      _mainText = mainText;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetAuthor()
    {
      return _author;
    }

    public string GetTitle()
    {
      return _title;
    }

    public string GetMainText()
    {
      return _mainText;
    }

    public override bool Equals(System.Object obj)
    {
      if(!(obj is Post)) return false;
      else
      {
        Post otherPost = (Post) obj;
        bool idEquality = _id == otherPost._id;
        bool authorEquality = _author == otherPost._author;
        bool titleEquality = _title == otherPost._title;
        bool mainTextEquality = _mainText == otherPost._mainText;
        return(idEquality && authorEquality && titleEquality && mainTextEquality);
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM posts;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static List<Post> GetAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand("SELECT * FROM posts;", conn);
      rdr = cmd.ExecuteReader();
      List<Post> allPosts = new List<Post>{};
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string author = rdr.GetString(1);
        string title = rdr.GetString(2);
        string mainText = rdr.GetString(3);
        Post post = new Post(author, title, mainText, id);
        allPosts.Add(post);
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return allPosts;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand("INSERT INTO posts (author, title, main_text) OUTPUT INSERTED.id VALUES (@Author, @Title, @MainText);", conn);

      SqlParameter authorParameter = new SqlParameter("@Author", _author);
      SqlParameter titleParameter = new SqlParameter("@Title", _title);
      SqlParameter mainTextParameter = new SqlParameter("@MainText", _mainText);
      cmd.Parameters.Add(authorParameter);
      cmd.Parameters.Add(titleParameter);
      cmd.Parameters.Add(mainTextParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        _id = rdr.GetInt32(0);
      }

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
    }

    public static Post Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand("SELECT * FROM posts WHERE id = @PostId;", conn);

      SqlParameter postIdParameter = new SqlParameter("@PostId", id);
      cmd.Parameters.Add(postIdParameter);

      int foundId = 0;
      string foundAuthor = null;
      string foundTitle = null;
      string foundMainText = null;

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        foundId = rdr.GetInt32(0);
        foundAuthor = rdr.GetString(1);
        foundTitle = rdr.GetString(2);
        foundMainText = rdr.GetString(3);
      }

      Post foundPost = new Post(foundAuthor, foundTitle, foundMainText, foundId);

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();

      return foundPost;
    }

    public static void DeleteById(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM posts WHERE id = @PostId;", conn);

      SqlParameter postIdParameter = new SqlParameter("@PostId", id);
      cmd.Parameters.Add(postIdParameter);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void Delete()
    {
      DeleteById(_id);
    }

    public static Post UpdateById(string newAuthor, string newTitle, string newMainText, int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("UPDATE posts SET author = @Author, title = @Title, main_text = @MainText OUTPUT INSERTED.author WHERE id = @PostId;", conn);

      SqlParameter postIdParameter = new SqlParameter("@PostId", id);
      SqlParameter titleParameter = new SqlParameter("@Title", newTitle);
      SqlParameter authorParameter = new SqlParameter("@Author", newAuthor);
      SqlParameter mainTextParameter = new SqlParameter("@MainText", newMainText);

      cmd.Parameters.Add(authorParameter);
      cmd.Parameters.Add(postIdParameter);
      cmd.Parameters.Add(titleParameter);
      cmd.Parameters.Add(mainTextParameter);

      cmd.ExecuteNonQuery();

      Post post = new Post(newAuthor, newTitle, newMainText, id);

      if (conn != null) conn.Close();

      return post;
    }

    public void Update(string newAuthor, string newTitle, string newMainText)
    {
      UpdateById(newAuthor, newTitle, newMainText, _id);
      _author = newAuthor;
      _title = newTitle;
      _mainText = newMainText;
    }

    public void Update(string newTitle, string newMainText)
    {
      this.Update(_author, newTitle, newMainText);
    }

    public static void RemoveById(int id)
    {
      UpdateById("[removed]", "[removed]", "[removed]", id);
    }

    public void Remove()
    {
      RemoveById(_id);
    }

    public List<Comment> GetAllChildComments(string orderBy = "default")
    {
      List<Comment> allChildren = new List<Comment>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();
      SqlCommand cmd = null;
      switch(orderBy)
      {
        case "rating":
          cmd = new SqlCommand("SELECT * FROM comments WHERE post_id = @PostId ORDER BY rating DESC;", conn);
          break;
        case "newest":
          cmd = new SqlCommand("SELECT * FROM comments WHERE post_id = @PostId ORDER BY date ASC;", conn);
          break;
        case "oldest":
          cmd = new SqlCommand("SELECT * FROM comments WHERE post_id = @PostId ORDER BY date DESC;", conn);
          break;
        default:
          cmd = new SqlCommand("SELECT * FROM comments WHERE post_id = @PostId ORDER BY id;", conn);
          break;
      }
      SqlParameter postIdParameter = new SqlParameter("@PostId", _id);
      cmd.Parameters.Add(postIdParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int newCommentId = rdr.GetInt32(0);
        string newCommentAuthor = rdr.GetString(1);
        string newCommentMainText = rdr.GetString(2);
        int newCommentRating = rdr.GetInt32(3);
        int newCommentPostId = rdr.GetInt32(4);

        Comment newComment = new Comment(newCommentAuthor, newCommentMainText, newCommentRating, newCommentPostId, newCommentId);
        allChildren.Add(newComment);
      }

      if(rdr != null) rdr.Close();
      if(conn != null) conn.Close();

      return allChildren;
    }

    public List<Comment> GetAllDirectChildren(string orderBy = "default")
    {
      List<Comment> children = new List<Comment>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();
      SqlCommand cmd = null;
      switch(orderBy)
      {
        case "rating":
          cmd = new SqlCommand("SELECT * FROM comments WHERE post_id = @PostId, parent_id = 0 ORDER BY rating DESC;", conn);
          break;
        case "newest":
          cmd = new SqlCommand("SELECT * FROM comments WHERE post_id = @PostId, parent_id = 0 ORDER BY date ASC;", conn);
          break;
        case "oldest":
          cmd = new SqlCommand("SELECT * FROM comments WHERE post_id = @PostId, parent_id = 0 ORDER BY date DESC;", conn);
          break;
        default:
          cmd = new SqlCommand("SELECT * FROM comments WHERE post_id = @PostId, parent_id = 0 ORDER BY id;", conn);
          break;
      }
      SqlParameter postIdParameter = new SqlParameter("@PostId", _id);
      cmd.Parameters.Add(postIdParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int newCommentId = rdr.GetInt32(0);
        string newCommentAuthor = rdr.GetString(1);
        string newCommentMainText = rdr.GetString(2);
        int newCommentRating = rdr.GetInt32(3);
        int newCommentPostId = rdr.GetInt32(4);

        Comment newComment = new Comment(newCommentAuthor, newCommentMainText, newCommentRating, newCommentPostId, newCommentId);
        children.Add(newComment);
      }

      if(rdr != null) rdr.Close();
      if(conn != null) conn.Close();

      return children;
    }

    public void AddCategory(Category category)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("INSERT INTO posts_categories (post_id, category_id) VALUES (@PostId, @CategoryId);", conn);
      SqlParameter postParameter = new SqlParameter("@PostId", _id);
      SqlParameter categoryParameter = new SqlParameter("@CategoryId", category.GetId());
      cmd.Parameters.Add(postParameter);
      cmd.Parameters.Add(categoryParameter);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
