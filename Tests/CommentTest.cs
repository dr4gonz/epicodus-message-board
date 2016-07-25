using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MessageBoard
{
  public class CommentTest : IDisposable
  {
    public CommentTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=message_board_test;Integrated Security=SSPI;";
      // DBConfiguration.ConnectionString = "Data Source=DESKTOP-7OLC9FT\\SQLEXPRESS;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Comment.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_ChecksIfCommentsAreEqual()
    {
      //Arrange, Act
      Comment firstComment = new Comment("Cool Stuff", "This stuff is really cool!", 0);
      Comment secondComment = new Comment("Cool Stuff", "This stuff is really cool!", 0);
      //Assert
      Assert.Equal(firstComment, secondComment);
    }

    public void Dispose()
    {
      Comment.DeleteAll();
    }

  }
}
