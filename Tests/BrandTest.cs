using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace Program.Objects.Shoes
{
  public class BrandTest : IDisposable
  {
    public BrandTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=shoe_stores_test;Integrated Security=SSPI";
    }
    [Fact]
    public void Test_CheckIfBrandIsEmpty()
    {
      var result = Brand.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_CheckIfBrandsAreSame()
    {
      var brandOne = "Nike";
      var brandTwo = "Nike";
      Assert.Equal(brandOne, brandTwo);
    }

    [Fact]
    public void Test_Save_CheckIfBrandsSavesToDatabase()
    {
      var testBrand = new Brand("Osiris");
      testBrand.Save();

      var result = Brand.GetAll();
      var testList = new List<Brand> {testBrand};
      Assert.Equal(testList, result);
    }
    
    
    [Fact]
    public void Test_AssignsIdToBrand()
    {
      var testBrand = new Brand("Victoria Secret");
      testBrand.Save();

      var savedBrand = Brand.GetAll()[0];
      var result = savedBrand.GetId();
      var testId = testBrand.GetId();
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_FindBrandInDatabase()
    {
      var testBrand = new Brand("DC");
      testBrand.Save();

      var foundBrand = Brand.Find(testBrand.GetId());
      Assert.Equal(testBrand, foundBrand);
    }
    
    [Fact]
    public void Test_GetStore_RetrieveAllBrandsWithStores()
    {
      var testBrand = new Brand("Baby Gap");
      testBrand.Save();

      var storeOne = new Store("Hot Topic");
      storeOne.Save();

      var storeTwo = new Store("American apparallel");
      storeTwo.Save();

      testBrand.AddStores(storeOne);
      testBrand.AddStores(storeTwo);

      var testStoreList = new List<Store> {storeOne,storeTwo};
      var resultStoreList = testBrand.GetStores();
      Assert.Equal(testStoreList, resultStoreList);
    }

    public void Dispose()
    {
      Brand.DeleteAll();
      Store.DeleteAll();
    }
  }
}
