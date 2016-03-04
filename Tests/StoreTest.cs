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
      var result = Store.GetAll().Count;
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_CheckIfNameIsSame()
    {
      var storeOne = "Forever 21";
      var storeTwo = "Forever 21";
      Assert.Equal(storeOne, storeTwo);
    }

    [Fact]
    public void Test_Save_CheckIfStoreSavesToDatabase()
    {
      var testStore = new Store("Wet Seal");
      testStore.Save();

      var result = Store.GetAll();
      var testList = new List<Store> {testStore};
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_AssignsIdToStore()
    {
      var testStore = new Store("Walmart");
      testStore.Save();

      var savedStore = Store.GetAll()[0];
      var result = savedStore.GetId();
      var testId = testStore.GetId();
      Assert.Equal(testId, result);
    }

    public void Dispose()
    {
      Store.DeleteAll();
      // Brand.DeleteAll();
    }
  }
}
