using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MessageBoard
{
  public class Comment
  {
    private int _id;
    private string _author;
    private string _mainText;
    private int _postId;
    private int _parentId;
    private int _rating;
    private DateTime? _time;
    private int _userId;

    public Comment(string author, string mainText, int rating, int postId, DateTime? time, int userId, int id = 0)
    {
      _id = id;
      _author = author;
      _mainText = mainText;
      _rating = rating;
      _parentId = 0;
      _postId = postId;
      _time = time;
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
    public string GetMainText()
    {
      return _mainText;
    }
    public int GetPostId()
    {
      return _postId;
    }
    public int GetParentId()
    {
      return _parentId;
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
    public void SetParentId(int newParentId)
    {
      _parentId = newParentId;
    }
    public void SetPostId(int newPostId)
    {
      _postId = newPostId;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM comments; DELETE FROM voting WHERE comment_id IS NOT NULL;", conn);
      cmd.ExecuteNonQuery();
      if(conn!=null) conn.Close();
    }

    public static List<Comment> GetAll()
    {
      List<Comment> allComments = new List<Comment>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM comments;", conn);
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
        allComments.Add(newComment);
      }

      if(rdr != null) rdr.Close();
      if(conn != null) conn.Close();

      return allComments;
    }

    public override bool Equals(System.Object otherComment)
    {
      if (!(otherComment is Comment))
      {
          return false;
      }
      else
      {
        Comment newComment = (Comment) otherComment;
        bool idEquality = (this._id == newComment.GetId());
        bool authorEquality = (this._author == newComment.GetAuthor());
        bool mainTextEquality = (this._mainText == newComment.GetMainText());
        bool ratingEquality = (this._rating == newComment.GetRating());
        bool postIdEquality = (this._postId == newComment.GetPostId());
        bool dateTimeEquality = (this._time == newComment.GetTime());
        bool userIdEquality = (this._userId == newComment.GetUserId());
        return (idEquality && authorEquality && mainTextEquality && ratingEquality && postIdEquality && dateTimeEquality && userIdEquality);
      }
    }

    public List<Comment> GetChildren(string orderBy = "default")
    {
      List<Comment> allChildren = new List<Comment>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();
      SqlCommand cmd = null;
      switch(orderBy)
      {
        case "rating":
          cmd = new SqlCommand("SELECT * FROM comments WHERE parent_id = @ParentId ORDER BY rating DESC;", conn);
          break;
        case "newest":
          cmd = new SqlCommand("SELECT * FROM comments WHERE parent_id = @ParentId ORDER BY time ASC;", conn);
          break;
        case "oldest":
          cmd = new SqlCommand("SELECT * FROM comments WHERE parent_id = @ParentId ORDER BY time DESC;", conn);
          break;
        default:
          cmd = new SqlCommand("SELECT * FROM comments WHERE parent_id = @ParentId ORDER BY id;", conn);
          break;
      }
      SqlParameter parentIdParameter = new SqlParameter("@ParentId", _id);
      cmd.Parameters.Add(parentIdParameter);

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

    public void Save()
   {
     SqlConnection conn = DB.Connection();
     SqlDataReader rdr = null;
     conn.Open();

     SqlCommand cmd = new SqlCommand("INSERT INTO comments (author, main_text, rating, post_id, parent_id, time, user_id) OUTPUT INSERTED.id VALUES(@Author, @MainText, @Rating, @PostId, @ParentId, @Time, @UserId);", conn);

     SqlParameter commentAuthorParameter = new SqlParameter("@Author", this.GetAuthor());
     cmd.Parameters.Add(commentAuthorParameter);

     SqlParameter commentTextParameter = new SqlParameter("@MainText", this.GetMainText());
     cmd.Parameters.Add(commentTextParameter);

     SqlParameter ratingParameter = new SqlParameter("@Rating", this.GetRating());
     cmd.Parameters.Add(ratingParameter);

     SqlParameter postIdParameter = new SqlParameter("@PostId", this.GetPostId());
     cmd.Parameters.Add(postIdParameter);

     SqlParameter parentIdParameter = new SqlParameter("@ParentId", this.GetParentId());
     cmd.Parameters.Add(parentIdParameter);

     SqlParameter timeParameter = new SqlParameter("@Time", this.GetTime());
     cmd.Parameters.Add(timeParameter);

     SqlParameter userIdParameter = new SqlParameter("UserId", this.GetUserId());
     cmd.Parameters.Add(userIdParameter);

     rdr = cmd.ExecuteReader();

     while(rdr.Read())
     {
       this._id = rdr.GetInt32(0);
     }

     if(rdr != null) rdr.Close();
     if(conn != null) conn.Close();
   }

   public static Comment Find(int id)
   {
     SqlConnection conn = DB.Connection();
     SqlDataReader rdr = null;
     conn.Open();

     SqlCommand cmd = new SqlCommand("SELECT * FROM comments WHERE id = @CommentId;", conn);
     SqlParameter commentIdParameter = new SqlParameter("@CommentId", id.ToString());
     cmd.Parameters.Add(commentIdParameter);

     rdr = cmd.ExecuteReader();

     int foundCommentId = 0;
     string foundCommentAuthor = null;
     string foundCommentMainText = null;
     int foundCommentRating = 0;
     int foundPostId = 0;
     int foundParentId = 0;
     DateTime? foundTime = null;
     int foundUserId = 0;

     while(rdr.Read())
     {
         foundCommentId = rdr.GetInt32(0);
         foundCommentAuthor = rdr.GetString(1);
         foundCommentMainText = rdr.GetString(2);
         foundCommentRating = rdr.GetInt32(3);
         foundPostId = rdr.GetInt32(4);
         foundParentId = rdr.GetInt32(5);
         foundTime = rdr.GetDateTime(6);
         foundUserId = rdr.GetInt32(7);
     }

     Comment newComment = new Comment(foundCommentAuthor, foundCommentMainText, foundCommentRating, foundPostId, foundTime, foundUserId, foundCommentId);
     newComment.SetParentId(foundParentId);

     if(rdr != null) rdr.Close();
     if(conn != null) conn.Close();

     return newComment;
   }

   public void Update(string newMainText)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE comments SET main_text = @NewMainText OUTPUT INSERTED.main_text WHERE id = @CommentId;", conn);

      SqlParameter newMainTextParameter = new SqlParameter("@NewMainText", newMainText);
      cmd.Parameters.Add(newMainTextParameter);

      SqlParameter commentIdParameter = new SqlParameter("@CommentId", this.GetId());
      cmd.Parameters.Add(commentIdParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._mainText = rdr.GetString(0);
      }

      if(rdr != null) rdr.Close();
      if(conn != null) conn.Close();
    }

   public void Update(string newAuthor, string newMainText)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE comments SET author = @NewAuthor, main_text = @NewMainText OUTPUT INSERTED.author, INSERTED.main_text WHERE id = @CommentId;", conn);

      SqlParameter newMainTextParameter = new SqlParameter("@NewMainText", newMainText);
      cmd.Parameters.Add(newMainTextParameter);

      SqlParameter newAuthorParameter = new SqlParameter("@NewAuthor", newAuthor);
      cmd.Parameters.Add(newAuthorParameter);

      SqlParameter commentIdParameter = new SqlParameter("@CommentId", this.GetId());
      cmd.Parameters.Add(commentIdParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._author = rdr.GetString(0);
        this._mainText = rdr.GetString(1);

      }

      if(rdr != null) rdr.Close();
      if(conn != null) conn.Close();
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM comments WHERE id = @CommentId; DELETE FROM voting WHERE comment_id = @CommentId;", conn);

      SqlParameter commentIdParameter = new SqlParameter("@CommentId", this.GetId());
      cmd.Parameters.Add(commentIdParameter);

      cmd.ExecuteNonQuery();

      if(conn != null) conn.Close();
    }

    public void Remove()
    {
      this.Update("[Removed]", "[Removed]");
    }
    public void Upvote(int userId)
    {
      Vote(userId, 1);
    }

    public void Downvote(int userId)
    {
      Vote(userId, -1);
    }

    public void Unvote(int userId)
    {
      Vote(userId, 0);
    }

    public void Vote(int userId, int voteValue)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand("DELETE FROM voting WHERE voter_id = @Voter AND comment_id = @CommentId; INSERT INTO voting (voter_id, comment_id, vote) VALUES (@Voter, @CommentId, @Vote); UPDATE comments SET rating=(SELECT SUM(vote) FROM voting WHERE comment_id = @CommentId) OUTPUT INSERTED.rating WHERE comments.id=@CommentId;", conn);
      SqlParameter userIdParameter = new SqlParameter("@Voter", userId);
      SqlParameter commentIdParameter = new SqlParameter("@CommentId", _id);
      SqlParameter voteParameter = new SqlParameter("@Vote", voteValue);
      cmd.Parameters.Add(userIdParameter);
      cmd.Parameters.Add(commentIdParameter);
      cmd.Parameters.Add(voteParameter);

      rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        _rating = rdr.GetInt32(0);
      }

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
    }
  }
}
