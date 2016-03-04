using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Program.Objects.Shoes
{
  public class Brand
  {
    private int _id;
    private string _name;

    public Brand(string name, int id = 0)
    {
      _id = id;
      _name = name;
    }

    public override bool Equals(System.Object otherBrand)
    {
      if(!(otherBrand is Brand))
      {
        return false;
      }
      else
      {
        var newBrand = (Brand) otherBrand;
        bool idEquality = this.GetId() == newBrand.GetId();
        bool nameEqualuity = this.GetName() == newBrand.GetName();

        return(idEquality && nameEqualuity);
      }
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public static List<Brand> GetAll()
    {
      var allBrands = new List<Brand>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      var cmd = new SqlCommand("SELECT * FROM brands;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        var BrandId = rdr.GetInt32(0);
        var BrandName = rdr.GetString(1);

        var newBrand = new Brand(BrandName, BrandId);
        allBrands.Add(newBrand);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return allBrands;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO brands(name) OUTPUT INSERTED.id VALUES (@Name);", conn);
      var nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@Name";
      nameParameter.Value = this.GetName();

      cmd.Parameters.Add(nameParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if(conn != null)
      {
        conn.Close();
      }
      if(rdr != null)
      {
        rdr.Close();
      }
    }

    public static Brand Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      var cmd = new SqlCommand("SELECT * FROM brands WHERE id=@BrandId", conn);
      var brandIdParameter = new SqlParameter();
      brandIdParameter.ParameterName = "@BrandId";
      brandIdParameter.Value = id;
      cmd.Parameters.Add(brandIdParameter);

      rdr= cmd.ExecuteReader();

      int foundBrandId = 0;
      string foundBrandName = null;

      while (rdr.Read())
      {
        foundBrandId = rdr.GetInt32(0);
        foundBrandName = rdr.GetString(1);
      }
      var foundBrand = new Brand(foundBrandName, foundBrandId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundBrand;
    }

    public List<Store> GetStores()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT stores.* FROM brands JOIN stores_brands ON (brands.id = stores_brands.brand_id) JOIN stores ON (stores.id = stores_brands.store_id) WHERE brands.id=@BrandsId", conn);
      var brandIdParameter = new SqlParameter();
      brandIdParameter.ParameterName = "@BrandsId";
      brandIdParameter.Value = this.GetId();
      cmd.Parameters.Add(brandIdParameter);

      rdr = cmd.ExecuteReader();

      var storesIds = new List<int> {};
      while(rdr.Read())
      {
        int StoreId = rdr.GetInt32(0);
        storesIds.Add(StoreId);
      }
      if(rdr != null)
      {
        rdr.Close();
      }

      var stores = new List<Store>{};
      foreach(int StoreId in storesIds)
      {
        SqlDataReader queryReader = null;
        SqlCommand storeQuery = new SqlCommand("SELECT * FROM stores WHERE id=@StoreId", conn);

        SqlParameter storeIdParameter = new SqlParameter();
        storeIdParameter.ParameterName = "@StoreId";
        storeIdParameter.Value = StoreId;
        storeQuery.Parameters.Add(storeIdParameter);

        queryReader = storeQuery.ExecuteReader();
        while(queryReader.Read())
        {
          var thisStoreId = queryReader.GetInt32(0);
          var storeName = queryReader.GetString(1);

          Store foundStore= new Store(storeName, thisStoreId);
          stores.Add(foundStore);
        }
        if (queryReader != null)
        {
          queryReader.Close();
        }
      }
        if (conn != null)
        {
          conn.Close();
        }
      return stores;
      }

    public void AddStores(Store newStore)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO stores_brands(brand_id, store_id) VALUES (@BrandId, StoreId)", conn);

      var brandIdParameter = new SqlParameter();
      brandIdParameter.ParameterName = "@BrandId";
      brandIdParameter.Value = this.GetId();
      cmd.Parameters.Add(brandIdParameter);

      var storeIdParameter = new SqlParameter();
      storeIdParameter.ParameterName = "@StoreId";
      storeIdParameter.Value = newStore.GetId();
      cmd.Parameters.Add(storeIdParameter);

      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = new SqlCommand("DELETE FROM brands;", conn);
      cmd.ExecuteNonQuery();
    }
  }
}
