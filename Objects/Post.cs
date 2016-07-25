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
  }
}
