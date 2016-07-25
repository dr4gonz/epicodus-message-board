using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MessageBoard
{
  public class PostTest : IDisposable
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

    [Fact]
    public void Post_Save_SavesPostToDatabase()
    {
      //Arrange
      Post testPost = new Post("Bob", "Fishing", "I like to fish");
      //Act
      testPost.Save();
      Post foundPost = Post.GetAll()[0];
      //Assert
      Assert.Equal(testPost, foundPost);
    }

    [Fact]
    public void Post_Find_FindsPostInDatabase()
    {
      //Arrange
      Post testPost = new Post("Bob", "Fishing", "I like to fish");
      testPost.Save();
      //Act
      Post foundPost = Post.Find(testPost.GetId());
      //Assert
      Assert.Equal(testPost, foundPost);
    }
  }
}
