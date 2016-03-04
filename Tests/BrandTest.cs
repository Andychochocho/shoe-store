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

    public void Dispose()
    {
      Brand.DeleteAll();
      // Author.DeleteAll();
    }
  }
}
