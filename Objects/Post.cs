using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MessageBoard
{
  public class OriginalPost
  {
    private int _id;
    private string _author;
    private string _title;
    private string _mainText;
    private DateTime? _time;
    private int _rating;
    private int _userId;

    public OriginalPost(string author, string title, string mainText, int rating, DateTime? time, int userId, int id=0)
    {
      _id = id;
      _author = author;
      _title = title;
      _mainText = mainText;
      _time = time;
      _rating = rating;
      _userId = userId;
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

    public int GetRating()
    {
      return _rating;
    }

    public DateTime? GetTime()
    {
      return _time;
    }

    public int GetUserId()
    {
      return _userId;
    }

    public override bool Equals(System.Object obj)
    {
      if(!(obj is OriginalPost)) return false;
      else
      {
        OriginalPost otherOriginalPost = (OriginalPost) obj;
        bool idEquality = _id == otherOriginalPost._id;
        bool authorEquality = _author == otherOriginalPost._author;
        bool titleEquality = _title == otherOriginalPost._title;
        bool mainTextEquality = _mainText == otherOriginalPost._mainText;
        bool timeEquality = _time == otherOriginalPost._time;
        bool ratingEquality = _rating == otherOriginalPost._rating;
        bool userIdEquality = _userId == otherOriginalPost._userId;
        return(idEquality && authorEquality && titleEquality && mainTextEquality && timeEquality && ratingEquality && userIdEquality);

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

    public static List<OriginalPost> GetAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand("SELECT * FROM posts ORDER BY rating;", conn); //remove ORDER BY
      rdr = cmd.ExecuteReader();
      List<OriginalPost> allOriginalPosts = new List<OriginalPost>{};
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string author = rdr.GetString(1);
        string title = rdr.GetString(2);
        string mainText = rdr.GetString(3);
        int rating = rdr.GetInt32(4);
        DateTime time = rdr.GetDateTime(5);
        int userId = rdr.GetInt32(6);
        OriginalPost post = new OriginalPost(author, title, mainText, rating, time, userId,  id);
        allOriginalPosts.Add(post);
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return allOriginalPosts;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand("INSERT INTO posts (author, title, main_text, rating, time, user_id) OUTPUT INSERTED.id VALUES (@Author, @Title, @MainText, @Rating, @Date, @UserId);", conn);

      SqlParameter authorParameter = new SqlParameter("@Author", _author);
      SqlParameter titleParameter = new SqlParameter("@Title", _title);
      SqlParameter mainTextParameter = new SqlParameter("@MainText", _mainText);
      SqlParameter ratingParameter = new SqlParameter("@Rating", _rating);
      SqlParameter dateParameter = new SqlParameter("@Date", _time);
      SqlParameter userIdParameter = new SqlParameter("@UserId", _userId);
      cmd.Parameters.Add(authorParameter);
      cmd.Parameters.Add(titleParameter);
      cmd.Parameters.Add(mainTextParameter);
      cmd.Parameters.Add(ratingParameter);
      cmd.Parameters.Add(dateParameter);
      cmd.Parameters.Add(userIdParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        _id = rdr.GetInt32(0);
      }

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
    }

    public static OriginalPost Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand("SELECT * FROM posts WHERE id = @OriginalPostId;", conn);

      SqlParameter postIdParameter = new SqlParameter("@OriginalPostId", id);
      cmd.Parameters.Add(postIdParameter);

      int foundId = 0;
      string foundAuthor = null;
      string foundTitle = null;
      string foundMainText = null;
      int foundRating = 0;
      DateTime? foundTime = null;
      int foundUserId = 0;

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        foundId = rdr.GetInt32(0);
        foundAuthor = rdr.GetString(1);
        foundTitle = rdr.GetString(2);
        foundMainText = rdr.GetString(3);
        foundTime = rdr.GetDateTime(5);
        foundRating = rdr.GetInt32(4);
        foundUserId = rdr.GetInt32(6);
      }
      OriginalPost foundOriginalPost = new OriginalPost(foundAuthor, foundTitle, foundMainText, foundRating, foundTime, foundUserId, foundId);

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();

      return foundOriginalPost;
    }

    public static void DeleteById(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM posts WHERE id = @OriginalPostId; DELETE FROM comments WHERE post_id = @OriginalPostId;", conn);

      SqlParameter postIdParameter = new SqlParameter("@OriginalPostId", id);
      cmd.Parameters.Add(postIdParameter);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void Delete()
    {
      DeleteById(_id);
    }

    public static OriginalPost UpdateById(string newAuthor, string newTitle, string newMainText, int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("UPDATE posts SET author = @Author, title = @Title, main_text = @MainText OUTPUT INSERTED.rating, INSERTED.time WHERE id = @OriginalPostId;", conn);
      SqlDataReader rdr = null;

      SqlParameter postIdParameter = new SqlParameter("@OriginalPostId", id);
      SqlParameter titleParameter = new SqlParameter("@Title", newTitle);
      SqlParameter authorParameter = new SqlParameter("@Author", newAuthor);
      SqlParameter mainTextParameter = new SqlParameter("@MainText", newMainText);

      cmd.Parameters.Add(authorParameter);
      cmd.Parameters.Add(postIdParameter);
      cmd.Parameters.Add(titleParameter);
      cmd.Parameters.Add(mainTextParameter);

      int rating = 0;
      DateTime? time = null;
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        rating = rdr.GetInt32(0);
        time = rdr.GetDateTime(1);
      }

      OriginalPost post = new OriginalPost(newAuthor, newTitle, newMainText, rating, time, id);

      if (rdr != null) rdr.Close();
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

    public void Update(int newRating)
     {
       SqlConnection conn = DB.Connection();
       SqlDataReader rdr = null;
       conn.Open();

       SqlCommand cmd = new SqlCommand("UPDATE posts SET rating = @NewRating OUTPUT INSERTED.rating WHERE id = @PostId;", conn);

       SqlParameter newRatingParameter = new SqlParameter("@NewRating", newRating);
       cmd.Parameters.Add(newRatingParameter);

       SqlParameter postIdParameter = new SqlParameter("@PostId", this.GetId());
       cmd.Parameters.Add(postIdParameter);

       rdr = cmd.ExecuteReader();

       while(rdr.Read())
       {
         this._rating = rdr.GetInt32(0);
       }

       if(rdr != null) rdr.Close();
       if(conn != null) conn.Close();
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
          cmd = new SqlCommand("SELECT * FROM comments WHERE post_id = @PostId ORDER BY time ASC;", conn);
          break;
        case "oldest":
          cmd = new SqlCommand("SELECT * FROM comments WHERE post_id = @PostId ORDER BY time DESC;", conn);
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
        int newCommentParentId = rdr.GetInt32(5);
        DateTime newCommentDateTime = rdr.GetDateTime(6);
        int newUserId = rdr.GetInt32(7);

        Comment newComment = new Comment(newCommentAuthor, newCommentMainText, newCommentRating, newCommentPostId, newCommentDateTime, newUserId, newCommentId);
        newComment.SetParentId(newCommentParentId);
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
          cmd = new SqlCommand("SELECT * FROM comments WHERE (post_id = @PostId AND parent_id = 0) ORDER BY rating DESC;", conn);
          break;
        case "newest":
          cmd = new SqlCommand("SELECT * FROM comments WHERE (post_id = @PostId AND parent_id = 0) ORDER BY time ASC;", conn);
          break;
        case "oldest":
          cmd = new SqlCommand("SELECT * FROM comments WHERE (post_id = @PostId AND parent_id = 0) ORDER BY time DESC;", conn);
          break;
        default:
          cmd = new SqlCommand("SELECT * FROM comments WHERE (post_id = @PostId AND parent_id = 0) ORDER BY id;", conn);
          break;
      }
      SqlParameter postIdParameter = new SqlParameter("@PostId", this.GetId());
      cmd.Parameters.Add(postIdParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int newCommentId = rdr.GetInt32(0);
        string newCommentAuthor = rdr.GetString(1);
        string newCommentMainText = rdr.GetString(2);
        int newCommentRating = rdr.GetInt32(3);
        int newCommentPostId = rdr.GetInt32(4);
        int newCommentParentId = rdr.GetInt32(5);
        DateTime newCommentDateTime = rdr.GetDateTime(6);
        int newUserId = rdr.GetInt32(7);

        Comment newComment = new Comment(newCommentAuthor, newCommentMainText, newCommentRating, newCommentPostId, newCommentDateTime, newUserId, newCommentId);
        newComment.SetParentId(newCommentParentId);
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

    public static List<OriginalPost> SearchByKeyword(string keyword)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      keyword = "%" + keyword + "%";
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand("SELECT * FROM posts WHERE title LIKE @TitleKeyword OR main_text LIKE @TextKeyword;", conn);
      SqlParameter keywordTitleParameter = new SqlParameter("@TitleKeyword", keyword);
      SqlParameter keywordTextParameter = new SqlParameter("@TextKeyword", keyword);
      cmd.Parameters.Add(keywordTitleParameter);
      cmd.Parameters.Add(keywordTextParameter);
      rdr = cmd.ExecuteReader();
      List<OriginalPost> allPosts = new List<OriginalPost>{};
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string author = rdr.GetString(1);
        string title = rdr.GetString(2);
        string mainText = rdr.GetString(3);
        int rating = rdr.GetInt32(4);
        DateTime time = rdr.GetDateTime(5);
        int userId = rdr.GetInt32(6);
        OriginalPost post = new OriginalPost(author, title, mainText, rating, time, userId, id);
        allPosts.Add(post);
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return allPosts;
    }

    public void DeleteAllChildren()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM comments WHERE post_id = @PostId;", conn);

      SqlParameter postIdParameter = new SqlParameter("@PostId", this.GetId());
      cmd.Parameters.Add(postIdParameter);

      cmd.ExecuteNonQuery();

      if(conn!=null) conn.Close();
    }

    public void Upvote()
    {
      this.Update(this.GetRating() + 1);
    }
    public void Downvote()
    {
      this.Update(this.GetRating() - 1);
    }


  }
}
