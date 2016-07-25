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

    public Comment(string author, string mainText, int rating, int postId, int id = 0)
    {
      _id = id;
      _author = author;
      _mainText = mainText;
      _rating = rating;
      _parentId = 0;
      _postId = postId;
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
      SqlCommand cmd = new SqlCommand("DELETE FROM comments;", conn);
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

        Comment newComment = new Comment(newCommentAuthor, newCommentMainText, newCommentRating, newCommentPostId, newCommentId);
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
        return (idEquality && authorEquality && mainTextEquality && ratingEquality && postIdEquality);
      }
    }

    public void Save()
   {
     SqlConnection conn = DB.Connection();
     SqlDataReader rdr = null;
     conn.Open();

     SqlCommand cmd = new SqlCommand("INSERT INTO comments (author, main_text, rating, post_id, parent_id) OUTPUT INSERTED.id VALUES(@Author, @MainText, @Rating, @PostId, @ParentId);", conn);

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

     while(rdr.Read())
     {
         foundCommentId = rdr.GetInt32(0);
         foundCommentAuthor = rdr.GetString(1);
         foundCommentMainText = rdr.GetString(2);
         foundCommentRating = rdr.GetInt32(3);
         foundPostId = rdr.GetInt32(4);
     }
     Comment newComment = new Comment(foundCommentAuthor, foundCommentMainText, foundCommentRating, foundPostId, foundCommentId);

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

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM comments WHERE id = @CommentId;", conn);

      SqlParameter commentIdParameter = new SqlParameter("@CommentId", this.GetId());
      cmd.Parameters.Add(commentIdParameter);

      cmd.ExecuteNonQuery();

      if(conn != null) conn.Close();
    }

    public void Remove()
    {
      this.Update("[Removed]");
    }

    public List<Comment> GetChildren()
    {
      List<Comment> allChildren = new List<Comment>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM comments WHERE parent_id = @ParentId;", conn);
      SqlParameter ParentIdParameter = new SqlParameter("@ParentId", this.GetId());
      cmd.Parameters.Add(ParentIdParameter);
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
  }
}
