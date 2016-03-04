using System;
using System.Collections.Generic;
using Xunit;
using System.Data;
using System.Data.SqlClient;

namespace Program.Objects.Shoes
{
  public class StoreTest : IDisposable
  {
    public StoreTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=shoe_stores_test;Integrated Security=SSPI";
    }
    [Fact]
    public void Test_CheckIfShoesIsEmpty()
    {
      int result = Store.GetAll().Count;
      Assert.Equal(0, result);
    }

    public void Dispose()
    {
      // Store.DeleteAll();
      // Brand.DeleteAll();
    }
  }
}
