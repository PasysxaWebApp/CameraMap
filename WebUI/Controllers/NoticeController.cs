using AspMvcLibrary.Attributes;
using CameraMap.Configs;
using CameraMap.Models.Base;
using CameraMap.Models.Files;
using CameraMap.Models.Notice;
using CameraMap.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CameraMap.Controllers
{
    [SessionExpireFilterAttribute(SessionKeyConfig.OrganizationID)]
    public class NoticeController : Controller
    {
        //
        // GET: /Notice/

        public ActionResult Index(int PageIndex=0, int PageSize=15)
        {
            var loginInfo = SessionLoginInfo.GetInstance(Session);
            if (PageSize <= 0)
            {
                PageSize = 15;
            }
            PageSize = Math.Max(1, PageSize);
            var viewModel = new NoticeModelListViewModel()
            {
                PageIndex = PageIndex,
                PageSize = PageSize
            };

            var items = NoticeModelReg.GetNoticeListForEdit(loginInfo.SystemRoll, loginInfo.OrganizationID, PageIndex, PageSize);
            viewModel.RowTotal = NoticeModelReg.GetNoticeListForEditCount(loginInfo.SystemRoll, loginInfo.OrganizationID);
            viewModel.Items = items;

            viewModel.PageIndex = Math.Min(viewModel.PageIndex, viewModel.PageCount - 1);
            viewModel.PageIndex = Math.Max(viewModel.PageIndex, 0);

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Index")]
        [SubmitCommand("AddNew")]
        public ActionResult AddNew(NoticeModel requestModel)
        {
            return RedirectToAction("New", "Notice", new { PageIndex = requestModel.PageIndex, PageSize = requestModel.PageSize });
        }

        [HttpPost]
        [ActionName("Index")]
        [SubmitCommand("Order")]
        public ActionResult Order()
        {
            var loginInfo = SessionLoginInfo.GetInstance(Session);
            var items = NoticeModelReg.GetNoticeListForOrder(loginInfo.SystemRoll, loginInfo.OrganizationID);
            
            Session[SessionKeyConfig.SortTargetModel] = new SortTargetModel
            {
                TableName = "t_notice",
                IdColumn = "NoticeID",
                DisplayColumn = "Title",
                DisplayNoColumn = "DisplayNo",
                RedirectController = "Notice",
                RedirectAction = "Index",
                SortItems = items
            };
            return RedirectToAction("SortList", "Sorts");
        }


        [HttpPost]
        [ActionName("Edit")]
        [SubmitCommand("Back")]
        public ActionResult BackToList(NoticeModel requestModel)
        {
            return RedirectToAction("Index", "Notice", new { PageIndex = requestModel.PageIndex, PageSize = requestModel.PageSize });
        }

        [HttpPost]
        [ActionName("View")]
        [SubmitCommand("Back")]
        public ActionResult BackToHomeLindex()
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult New(int PageIndex = 0, int PageSize = 15)
        {
            var model = new NoticeModel()
            {
                NoticeDateTime= DateTime.Now.Date,
                ContentType = 1,
            };
            model.PageIndex = PageIndex;
            model.PageSize = PageSize;
            return View("Edit", model);
        }

        public ActionResult Edit(long id, int PageIndex = 0, int PageSize = 15)
        {
            var loginInfo = SessionLoginInfo.GetInstance(Session);
            var model = NoticeModelReg.GetNoticeModel(id);
            if (model.CreateUserID == loginInfo.LoginID)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult View(long id)
        {
            var loginInfo = SessionLoginInfo.GetInstance(Session);
            var model = NoticeModelReg.GetNoticeModel(id);

            return View(model);

        }

        [HttpPost]
        public ActionResult SaveCheck(NoticeModel requestModel)
        {
            var checkReault = ModelState.IsValid;
            var sb = new StringBuilder();
            if (!checkReault)
            {
                foreach (var ms in ModelState)
                {
                    foreach (var er in ms.Value.Errors)
                    {
                        if (!string.IsNullOrWhiteSpace(er.ErrorMessage))
                        {
                            sb.AppendLine(er.ErrorMessage);
                        }
                    }
                }
            }
            var r = new { CheckReault = checkReault, ErrorMessage = sb.ToString() };
            return Json(r);
        }

        [HttpPost]
        [ActionName("Edit")]
        [SubmitCommand("OK")]
        public ActionResult Save(NoticeModel requestModel)
        {
            var viewName = "Edit";
            if (!ModelState.IsValid)
            {
                return View(viewName, requestModel);
            }

            var loginInfo = SessionLoginInfo.GetInstance(Session);
            long fileID = 0;
            if (this.Request.Files.Count > 0)
            {
                if (Request.Files["AttachmentFile"].ContentLength > 0)
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(Request.Files["AttachmentFile"].FileName);
                    string FileName = fi.Name;

                    var bytes = new byte[Request.Files["AttachmentFile"].InputStream.Length];
                    Request.Files["AttachmentFile"].InputStream.Read(bytes, 0, bytes.Length);
                    fileID = FileModelReg.Insert(FileName, bytes);
                }
            }

            var model = NoticeModelReg.GetNoticeModel(requestModel.NoticeID);
            model.Title = requestModel.Title;
            model.ContentType = requestModel.ContentType;
            if (requestModel.ContentType == 1)
            {
                try
                {
                    var desc= System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(requestModel.ContentDesc));
                    if (desc.Length > 50)
                    {
                        desc = desc.Substring(0, 50);
                    }
                    model.ContentDesc = desc;
                }
                catch (Exception)
                { }
                try
                {
                    model.ContentTxt = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(requestModel.ContentTxt));
                }
                catch (Exception) {
                    model.ContentTxt = requestModel.ContentDesc;
                }
            }
            else
            {
                model.ContentTxt = requestModel.ContentTxt;
            }
            model.Sticky = requestModel.Sticky;
            if (fileID > 0)
            {
                model.AttachmentFileID1 = fileID;
            }
            else
            {
                if (requestModel.AttachmentFileID1 != -1)
                {
                    model.AttachmentFileID1 = 0;
                }
                else {
                    model.AttachmentFileID1 = -1;
                }
            }

            model.OrganizationID = loginInfo.OrganizationID;
            model.NoticeDateTime = requestModel.NoticeDateTime;
            model.CreateUserID = loginInfo.LoginID;
            model.CreateDateTime = DateTime.Now;
            model.LastUserID = loginInfo.LoginID;
            model.LastUpdatetime = DateTime.Now;
            model.StartDate = DateTime.Now;

            var bl = NoticeModelReg.Save(model, loginInfo.SystemRoll);
            if (bl)
            {
                return RedirectToAction("Index", new { PageIndex = requestModel.PageIndex, PageSize = requestModel.PageSize });
            }
            else
            {
                ModelState.AddModelError("NoticeID", "操作が失敗しました");
                return View(viewName, requestModel);
            }
        }

        [HttpPost]
        [ActionName("Edit")]
        [SubmitCommand("Delete")]
        public ActionResult Delete(NoticeModel requestModel)
        {
            var viewName = "Edit";
            var loginInfo = SessionLoginInfo.GetInstance(Session);
            var bl = NoticeModelReg.Delete(requestModel.NoticeID, loginInfo.LoginID);
            if (bl)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(viewName, requestModel);
            }
        }


    }
}
