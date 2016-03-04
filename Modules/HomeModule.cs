using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace Program.Objects.Shoes
{
  public class HomeModule: NancyModule
  {
    public HomeModule()
    {
      Get["/"] =_=> {
        return View["index.cshtml"];
      };

      Get["/stores"] =_=> {
        List<Store> allStores = Store.GetAll();
        return View["stores.cshtml", allStores];
      };

      Post["/stores/new"] =_=> {
        Store newStore = new Store(Request.Form["store-name"]);
        newStore.Save();
        List<Store> allStores = Store.GetAll();
        return View["stores.cshtml", allStores];
      };

      Get["/stores/delete_all"]=_=>{
        Store.DeleteAll();
        var allStores = Store.GetAll();
        return View["stores.cshtml", allStores];
      };

      Get["stores/{id}"] = parameters => {
        var model = new Dictionary<string, object> ();
        Store selectedStore = Store.Find(parameters.id);
        List<Brand> storeBrand = selectedStore.GetBrands();
        List<Store> allStores = Store.GetAll();
        model.Add("store", selectedStore);
        model.Add("brands", storeBrand);
        model.Add("stores", allStores);
        return View["brands.cshtml", model];
      };

      Post["stores/{id}/new"] = parameters => {
        Brand newBrand = new Brand(Request.Form["name"], Request.Form["store-id"]);
        newBrand.Save();

        Dictionary<string, object> model = new Dictionary<string, object>();
        Store selectedStore = Store.Find(Request.Form["store-id"]);
        selectedStore.AddBrands(newBrand);
        List<Brand> storeBrand = selectedStore.GetBrands();
        List<Store> allStores = Store.GetAll();
        model.Add("store", selectedStore);
        model.Add("brands", storeBrand);
        model.Add("stores", allStores);
        return View["brands.cshtml", model];
      };

      Get["/stores/{id}/edit"] = parameters => {
        Store selectedStore = Store.Find(parameters.id);
        return View["store_edit.cshtml", selectedStore];
      };

      Patch["/stores/{id}/edit"] = parameters => {
        Store selectedStore = Store.Find(parameters.id);
        selectedStore.Update(Request.Form["store-name"]);
        var allStores = Store.GetAll();
        return View["stores.cshtml", allStores];
      };

      Delete["/stores/{id}/delete"] = parameters => {
        Store selectedStore = Store.Find(parameters.id);
        selectedStore.Delete();
        List<Store> allStores = Store.GetAll();
        return View["stores.cshtml", allStores];
      };

      Get["/brands"] =_=> {
        var allBrands = Brand.GetAll();
        return View["brandsAll.cshtml", allBrands];
      };

      Post["/brands/new"] =_=> {
        Brand newBrand = new Brand(Request.Form["name"]);
        newBrand.Save();
        var allBrands = Brand.GetAll();
        return View["brandsAll.cshtml", allBrands];
      };
      Get["/brands/delete_all"] =_=> {
        Brand.DeleteAll();
        var allBrands = Brand.GetAll();
        return View["brandsAll.cshtml", allBrands];
      };
    }
  }
}
