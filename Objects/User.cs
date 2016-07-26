using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MessageBoard
{
  public class User
  {
    private int _id;
    private string _name;
    private string _password;

    public User(string name, string password, int id = 0)
    {
      _id = id;
      _name = name;
      _password = password;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }
    public string GetPassword()
    {
      return _password;
    }

    public override bool Equals(System.Object obj)
    {
      if(!(obj is User)) return false;
      else
      {
        User otherUser = (User) obj;
        bool idEquality = _id == otherUser.GetId();
        bool nameEquality = _name == otherUser.GetName();
        bool passwordEquality = _password == otherUser.GetPassword();
        return(idEquality && nameEquality && passwordEquality);
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM users;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM users WHERE id = @UserId;", conn);

      SqlParameter userIdParameter = new SqlParameter("@UserId", this.GetId());
      cmd.Parameters.Add(userIdParameter);

      cmd.ExecuteNonQuery();

      if(conn != null) conn.Close();
    }

    public static List<User> GetAll()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();
      SqlCommand cmd = new SqlCommand("SELECT * FROM users;", conn);
      rdr = cmd.ExecuteReader();

      List<User> allCategories = new List<User>{};

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string password = rdr.GetString(2);
        User user = new User(name, password, id);
        allCategories.Add(user);
      }

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return allCategories;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();
      SqlCommand cmd = new SqlCommand("INSERT INTO users (user_name, password) OUTPUT INSERTED.id VALUES (@Name, @Password);", conn);

      SqlParameter nameParameter = new SqlParameter("@Name", _name);
      cmd.Parameters.Add(nameParameter);

      SqlParameter passwordParameter = new SqlParameter("@Password", _password);
      cmd.Parameters.Add(passwordParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        _id = rdr.GetInt32(0);
      }

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
    }

    public static User Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand("SELECT * FROM users WHERE id = @UserId;", conn);

      SqlParameter userIdParameter = new SqlParameter("@UserId", id);
      cmd.Parameters.Add(userIdParameter);

      int foundId = 0;
      string foundName = null;
      string foundPassword = null;

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        foundId = rdr.GetInt32(0);
        foundName = rdr.GetString(1);
        foundPassword = rdr.GetString(2);
      }
      User foundUser = new User(foundName, foundPassword, foundId);

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();

      return foundUser;
    }
  }
}
