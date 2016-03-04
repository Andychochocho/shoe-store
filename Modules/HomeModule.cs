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
    }
  }
}
