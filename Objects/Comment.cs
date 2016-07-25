using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MessageBoard
{
  public class Comment
  {
    private int _id;
    private string _mainText;
    private int _postId;
    private int _parentId;
    private int _rating;

    public Comment(string mainText, int rating, int id = 0)
    {
      _id = id;
      _mainText = mainText;
      _rating = rating;
      _parentId = 0;
      _postId = 0;
    }

    public int GetId()
    {
      return _id;
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
        string newCommentMainText = rdr.GetString(1);
        int newCommentRating = rdr.GetInt32(2);

        Comment newComment = new Comment(newCommentMainText, newCommentRating, newCommentId);
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
        bool mainTextEquality = (this._mainText == newComment.GetMainText());
        bool ratingEquality = (this._rating == newComment.GetRating());
        return (idEquality && mainTextEquality && ratingEquality);
      }
    }

    public void Save()
   {
     SqlConnection conn = DB.Connection();
     SqlDataReader rdr = null;
     conn.Open();

     SqlCommand cmd = new SqlCommand("INSERT INTO comments (main_text, rating) OUTPUT INSERTED.id VALUES(@MainText, @Rating);", conn);

     SqlParameter commentTextParameter = new SqlParameter("@MainText", this.GetMainText());
     cmd.Parameters.Add(commentTextParameter);

     SqlParameter ratingParameter = new SqlParameter("@Rating", this.GetRating());
     cmd.Parameters.Add(ratingParameter);

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
     string foundCommentMainText = null;
     int foundCommentRating = 0;

     while(rdr.Read())
     {
         foundCommentId = rdr.GetInt32(0);
         foundCommentMainText = rdr.GetString(1);
         foundCommentRating = rdr.GetInt32(2);
     }
     Comment newComment = new Comment(foundCommentMainText, foundCommentRating, foundCommentId);

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

  }
}
