using System.Web.Mvc;
using AspMvcLibrary.Attributes;
using CameraMap.Configs;
using CameraMap.Models.Base;
using CameraMap.Models.Samples;
using System;
using System.Collections.Generic;
namespace CameraMap.Controllers
{
    public class SamplesController : Controller
    {
        public ActionResult TableList()
        {
            return View(TableInfoListModel.GetInstance());
        }

        public ActionResult TableListDetail(string id)
        {
            Session[SessionKeyConfig.SortTargetModel] = new SortTargetModel
            {
                TableName = "m_fishspeciesshape",
                IdColumn = "Guid",
                DisplayColumn = "FishSharpeName",
                RedirectController = "Samples",
                RedirectAction = "TableListDetail"
            };

            return View(new TableInfoModel(id));
        }

        [HttpPost]
        [ActionName("TableListDetail")]
        [SubmitCommand("back")]
        public ActionResult Back()
        {
            return RedirectToAction("TableList", "Samples");
        }

        [HttpGet]
        public ActionResult Test()
        {
            var lst = new List<TestModel> { 
                new TestModel { Name = "李軍", Birthday = DateTime.Parse("1983/11/21"),Age=30,Address="" } ,
                new TestModel { Name = "鴎外", Birthday = DateTime.Parse("1982/11/21"),Age=30,Address="" } ,
                new TestModel { Name = "竜間", Birthday = DateTime.Parse("1981/11/21"),Age=30,Address="" } ,
                new TestModel { Name = "生簀", Birthday = DateTime.Parse("1980/11/21"),Age=30,Address="" } 
            };
            var mdllist = new TestModelList();
            mdllist.Items = lst;
            return View(mdllist);
        }
    }
}
