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
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM stores;", conn);
      cmd.ExecuteNonQuery();
    }
  }
}
