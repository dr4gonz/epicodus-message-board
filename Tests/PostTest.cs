using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MessageBoard
{
  public class PostTest
  {
    //overrride sqlconnection string

    public void Dispose()
    {
      // Post.DeleteAll()
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
  }
}
