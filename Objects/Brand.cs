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
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = new SqlCommand("DELETE FROM brands;", conn);
      cmd.ExecuteNonQuery();
    }
  }
}
