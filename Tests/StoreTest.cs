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

    [Fact]
    public void Test_FindStoreInDatabase()
    {
      var testStore = new Store("Target");
      testStore.Save();

      var foundStore = Store.Find(testStore.GetId());
      Assert.Equal(testStore, foundStore);
    }

    [Fact]
    public void Test_GetBrand_RetrieveAllBrandsWithStores()
    {
      var testStore = new Store("KNYEW");
      testStore.Save();

      var brandOne = new Brand("Vans");
      brandOne.Save();

      var brandTwo = new Brand("Slippers");
      brandTwo.Save();

      testStore.AddBrands(brandOne);
      testStore.AddBrands(brandTwo);

      var testBrandList = new List<Brand> {brandOne,brandTwo};
      var resultBrandList = testStore.GetBrands();
      Assert.Equal(testBrandList, resultBrandList);
    }

    public void Dispose()
    {
      Store.DeleteAll();
      Brand.DeleteAll();
    }
  }
}
