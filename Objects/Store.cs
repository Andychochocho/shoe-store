using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Program.Objects.Shoes
{
  public class Store
  {
    private int _id;
    private string _name;

  public Store (string name, int id= 0)
  {
    _id = id;
    _name = name;
  }
  public override bool Equals(System.Object otherStore)
  {
    if(!(otherStore is Store))
    {
      return false;
    }
    else
    {
      var newStore = (Store) otherStore;
      bool idEquality = this.GetId() == newStore.GetId();
      bool nameEquality = this.GetName() == newStore.GetName();

      return (idEquality && nameEquality);
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

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      var cmd = new SqlCommand("INSERT INTO stores (name) OUTPUT INSERTED.id VALUES (@Name);", conn);
      var nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@Name";
      nameParameter.Value = this.GetName();

      cmd.Parameters.Add(nameParameter);
      rdr= cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static List<Store> GetAll()
    {
      var allStore = new List<Store>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      var cmd = new SqlCommand("SELECT * FROM stores;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        var StoreId = rdr.GetInt32(0);
        var StoreName = rdr.GetString(1);

        var newStore = new Store(StoreName, StoreId);
        allStore.Add(newStore);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return allStore;
    }

    public static Store Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      var cmd = new SqlCommand("SELECT * FROM stores WHERE id=@StoreId", conn);
      var storeIdParameter = new SqlParameter();
      storeIdParameter.ParameterName = "@StoreId";
      storeIdParameter.Value = id;
      cmd.Parameters.Add(storeIdParameter);
      rdr = cmd.ExecuteReader();

      int foundStoreId = 0;
      string foundStoreName = null;

      while(rdr.Read())
      {
        foundStoreId = rdr.GetInt32(0);
        foundStoreName = rdr.GetString(1);
      }
      var foundStore = new Store(foundStoreName, foundStoreId);
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return foundStore;
    }

    public List<Brand> GetBrands()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT brands.* FROM stores JOIN stores_brands ON (stores.id = stores_brands.store_id) JOIN brands ON (brands.id = stores_brands.brand_id) WHERE stores.id =@StoreId;",conn);
      
      SqlParameter StoreIdParameter = new SqlParameter();
      StoreIdParameter.ParameterName = "@StoreId";
      StoreIdParameter.Value = this.GetId();
      cmd.Parameters.Add(StoreIdParameter);

      rdr = cmd.ExecuteReader();
      
      List<Brand> brands = new List<Brand> {};

        while (rdr.Read())
        {
            int thisBrandId = rdr.GetInt32(0);
            string brandName = rdr.GetString(1);

            Brand foundBrand = new Brand(brandName, thisBrandId);
            brands.Add(foundBrand);
        }
        if(rdr != null)
        {
            rdr.Close();
        }
        
      if (conn != null)
      {
        conn.Close();
      }
      return brands;
    }

    public void AddBrands(Brand newBrand)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO stores_brands (store_id, brand_id) VALUES (@StoreId, @BrandId)", conn);
      SqlParameter storeIdParameter = new SqlParameter();
      storeIdParameter.ParameterName = "@StoreId";
      storeIdParameter.Value = this.GetId();
      cmd.Parameters.Add(storeIdParameter);

      SqlParameter brandParameter = new SqlParameter();
      brandParameter.ParameterName = "@BrandId";
      brandParameter.Value = newBrand.GetId();
      cmd.Parameters.Add(brandParameter);

      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }

    public void Update(string newStoreName)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      var cmd = new SqlCommand("UPDATE stores SET name=@newStoreName OUTPUT INSERTED.name WHERE id=@StoreId;", conn);

      var newStoreParameter = new SqlParameter();
      newStoreParameter.ParameterName = "@newStoreName";
      newStoreParameter.Value = newStoreName;
      cmd.Parameters.Add(newStoreParameter);

      var storeIdParameter = new SqlParameter();
      storeIdParameter.ParameterName = "@StoreId";
      storeIdParameter.Value = this.GetId();
      cmd.Parameters.Add(storeIdParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._name = rdr.GetString(0);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = new SqlCommand("DELETE FROM stores WHERE id=@StoreId; DELETE FROM stores_brands WHERE store_id=@StoreId;", conn);

      var storeIdParameter= new SqlParameter();
      storeIdParameter.ParameterName = "@StoreId";
      storeIdParameter.Value = this.GetId();

      cmd.Parameters.Add(storeIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM stores;", conn);
      cmd.ExecuteNonQuery();
    }
  }
}
