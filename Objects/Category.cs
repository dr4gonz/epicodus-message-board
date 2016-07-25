using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MessageBoard
{
  public class Category
  {
    private int _id;
    private string _name;

    public Category(string name, int id = 0)
    {
      _id = id;
      _name = name;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public override bool Equals(System.Object obj)
    {
      if(!(obj is Category)) return false;
      else
      {
        Category otherCategory = (Category) obj;
        bool idEquality = _id == otherCategory.GetId();
        bool nameEquality = _name == otherCategory.GetName();
        return(idEquality && nameEquality);
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM categories;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static List<Category> GetAll()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();
      SqlCommand cmd = new SqlCommand("SELECT * FROM categories;", conn);
      rdr = cmd.ExecuteReader();

      List<Category> allCategories = new List<Category>{};

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Category category = new Category(name, id);
        allCategories.Add(category);
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
      SqlCommand cmd = new SqlCommand("INSERT INTO categories (name) OUTPUT INSERTED.id VALUES (@Name);", conn);

      SqlParameter nameParameter = new SqlParameter("@Name", _name);
      cmd.Parameters.Add(nameParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        _id = rdr.GetInt32(0);
      }

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
    }

    public static Category Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand("SELECT * FROM categories WHERE id = @CategoryId;", conn);

      SqlParameter categoryIdParameter = new SqlParameter("@CategoryId", id);
      cmd.Parameters.Add(categoryIdParameter);

      int foundId = 0;
      string foundName = null;

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        foundId = rdr.GetInt32(0);
        foundName = rdr.GetString(1);
      }
      Category foundCategory = new Category(foundName, foundId);

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();

      return foundCategory;
    }

    public List<OriginalPost> GetPosts(string orderBy = "default")
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = null;
      switch(orderBy)
      {
        case "rating":
          cmd = new SqlCommand("SELECT * FROM posts JOIN posts_categories ON posts.id=posts_categories.post_id JOIN categories ON categories.id=posts_categories.category_id WHERE category_id = @CategoryId ORDER BY rating DESC;", conn);
          break;
        case "newest":
          cmd = new SqlCommand("SELECT * FROM posts JOIN posts_categories ON posts.id=posts_categories.post_id JOIN categories ON categories.id=posts_categories.category_id WHERE category_id = @CategoryId ORDER BY date ASC;", conn);
          break;
        case "oldest":
          cmd = new SqlCommand("SELECT * FROM posts JOIN posts_categories ON posts.id=posts_categories.post_id JOIN categories ON categories.id=posts_categories.category_id WHERE category_id = @CategoryId ORDER BY date DESC;", conn);
          break;
        default:
          cmd = new SqlCommand("SELECT * FROM posts JOIN posts_categories ON posts.id=posts_categories.post_id JOIN categories ON categories.id=posts_categories.category_id WHERE category_id = @CategoryId ORDER BY posts.id;", conn);
          break;
      }

      SqlParameter categoryParameter = new SqlParameter("@CategoryId", _id);
      cmd.Parameters.Add(categoryParameter);
      rdr = cmd.ExecuteReader();
      List<OriginalPost> allPosts = new List<OriginalPost>{};
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string author = rdr.GetString(1);
        string title = rdr.GetString(2);
        string mainText = rdr.GetString(3);
        int rating = rdr.GetInt32(4);
        OriginalPost post = new OriginalPost(author, title, mainText, rating, id);
        allPosts.Add(post);
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return allPosts;
    }

    public static List<Category> SearchByKeyword(string keyword)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      keyword = "%" + keyword + "%";
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand("SELECT * FROM categories WHERE name LIKE @NameKeyword;", conn);
      SqlParameter keywordNameParameter = new SqlParameter("@NameKeyword", keyword);
      cmd.Parameters.Add(keywordNameParameter);
      rdr = cmd.ExecuteReader();
      List<Category> allCategories = new List<Category>{};
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Category category = new Category(name, id);
        allCategories.Add(category);
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return allCategories;
    }

  }
}
