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
  }
}
