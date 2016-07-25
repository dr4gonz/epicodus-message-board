using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MessageBoard
{
  public class CategoryTest : IDisposable
  {
    public CategoryTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=message_board_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      Category.DeleteAll();
      OriginalPost.DeleteAll();
      Comment.DeleteAll();
    }

    [Fact]
    public void Category_Equals_TrueIfCategorysSame()
    {
      //Arrange, act
      Category firstCategory = new Category("Fishing");
      Category secondCategory = new Category("Fishing");
      //Assert
      Assert.Equal(firstCategory, secondCategory);
    }

    [Fact]
    public void Category_DatabaseEmptyAtFirst()
    {
      //Arrange, act
      int result = Category.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Category_Save_SavesCategoryToDatabase()
    {
      //Arrange
      Category testCategory = new Category("Fishing");
      //Act
      testCategory.Save();
      Category foundCategory = Category.GetAll()[0];
      //Assert
      Assert.Equal(testCategory, foundCategory);
    }

    [Fact]
    public void Category_Find_FindsCategoryInDatabase()
    {
      //Arrange
      Category testCategory = new Category("Fishing");
      testCategory.Save();
      //Act
      Category foundCategory = Category.Find(testCategory.GetId());
      //Assert
      Assert.Equal(testCategory, foundCategory);
    }

    [Fact]
    public void Category_GetPosts_GetsPostsByCategory()
    {
      //Arrange
      Category testCategory = new Category("Fishing");
      testCategory.Save();
      OriginalPost firstPost = new OriginalPost("Bob", "Fishing", "I like to fish");
      firstPost.Save();
      OriginalPost secondPost = new OriginalPost("Bob", "Fishing", "I like to fish");
      secondPost.Save();
      firstPost.AddCategory(testCategory);
      List<OriginalPost> expectedResult = new List<OriginalPost>{firstPost};
      //Act
      List<OriginalPost> result = testCategory.GetPosts();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Category_SearchByKeyword_SearchesCategoriesByKeyword()
    {
      //Arrange
      Category firstCategory = new Category("Fishing");
      firstCategory.Save();
      Category secondCategory = new Category("Swimming");
      secondCategory.Save();
      //Act
      List<Category> result = Category.SearchByKeyword("Fishing");
      List<Category> expectedResult = new List<Category>{firstCategory};
      //Assert
      Assert.Equal(expectedResult, result);
    }

  }
}
