using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MessageBoard
{
  public class PostTest
  {
    public PostTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=message_board_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      Post.DeleteAll();
    }

    [Fact]
    public void Post_Equals_TrueIfPostsSame()
    {
      //Arrange, act
      Post firstPost = new Post("Bob", "Fishing", "I like to fish");
      Post secondPost = new Post("Bob", "Fishing", "I like to fish");
      //Assert
      Assert.Equal(firstPost, secondPost);
    }

    [Fact]
    public void Post_DatabaseEmptyAtFirst()
    {
      //Arrange, act
      int result = Post.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }
  }
}
