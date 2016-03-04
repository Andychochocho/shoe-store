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
    }
  }
}
