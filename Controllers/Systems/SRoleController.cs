using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using WebNoiBai.Authorize;
using WebNoiBai.Dto;
using WebNoiBai.Models;
using WebNoiBai.WHttpMessage;

namespace WebNoiBai.Controllers.Systems
{
    public class SRoleController : BaseController
    {
        // GET: SRole
        [AuthorizeAccessRole(TypeHandle = "view")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetTable(SearchDto itemSearch)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var query = db.SRoles.AsNoTracking().Where(x => true);
                if (!string.IsNullOrEmpty(itemSearch.Ma))
                {
                    query = query.Where(x => x.Ma == itemSearch.Ma);
                }
                if (!string.IsNullOrEmpty(itemSearch.Ten))
                {
                    query = query.Where(x => x.Ten.Contains(itemSearch.Ten));
                }
                if (itemSearch.Enable != -1)
                {
                    query = query.Where(x => x.Enable == itemSearch.Enable);
                }
                var result = query.OrderBy(x => x.Id).Skip(itemSearch.Skip).Take(itemSearch.PageSize).ToList();
                httpMessage.Body.Data = result;
                httpMessage.Body.Pagination = new HttpMessagePagination
                {
                    NumberRowsOnPage = itemSearch.PageSize,
                    PageNumber = itemSearch.PageNum,
                    TotalRowsOnPage = result.Count(),
                    TotalRows = query.Count()
                };
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetItem(int id)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var item = db.SRoles.Find(id);
                if (item == null)
                {
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Không tìm thấy thông tin");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                var lstPermissionId = db.SRolePermissions.AsNoTracking().Where(x => x.RoleId == item.Id && x.IsGranted == 1).Select(x => x.PermissionId.Value).ToList();
                httpMessage.Body.Data = new { Item = item, LstPermissionId = lstPermissionId };
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeAccessRole(TypeHandle = "create")]
        public JsonResult Create(SRole item, string strPermissionId)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    httpMessage = CheckValid(item);
                    if (!httpMessage.IsOk)
                    {
                        return Json(httpMessage, JsonRequestBehavior.AllowGet);
                    }
                    item = db.SRoles.Add(item);
                    db.SaveChanges();

                    List<int> lstPermissionId = JsonConvert.DeserializeObject<List<int>>(strPermissionId);
                    if (lstPermissionId.Any())
                    {
                        var lstPermissionRole = lstPermissionId.Select(x => new SRolePermission
                        {
                            RoleId = item.Id,
                            PermissionId = x,
                            IsGranted = 1
                        }).ToList();
                        db.SRolePermissions.AddRange(lstPermissionRole);
                        db.SaveChanges();
                    }

                    trans.Commit();
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("200", null, "Thêm mới thành công");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        [AuthorizeAccessRole(TypeHandle = "update")]
        public JsonResult Update(SRole item, string strPermissionId)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var exist = db.SRoles.Find(item.Id);
                    if (exist == null)
                    {
                        httpMessage.IsOk = false;
                        httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Không tìm thấy thông tin");
                        return Json(httpMessage, JsonRequestBehavior.AllowGet);
                    }
                    db.Entry(exist).State = EntityState.Detached;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();

                    List<int> lstPermissionId = JsonConvert.DeserializeObject<List<int>>(strPermissionId);

                    var lstPermissionExist = db.SRolePermissions.AsNoTracking().Where(x=>x.RoleId == item.Id && x.IsGranted == 1).Select(x=>x.PermissionId.Value).ToList();
                    var lstPermissionRemove = lstPermissionExist.Except(lstPermissionId);
                    var lstItemRemove = db.SRolePermissions.Where(x=>lstPermissionRemove.Contains(x.PermissionId.Value) && x.RoleId == item.Id).ToList();
                    foreach(var p in lstItemRemove)
                    {
                        db.SRolePermissions.Remove(p);
                    }

                    var lstPermissionRoleAdd = lstPermissionId.Except(lstPermissionExist).Select(x => new SRolePermission
                    {
                        RoleId = item.Id,
                        PermissionId = x,
                        IsGranted = 1
                    }).ToList();
                    db.SRolePermissions.AddRange(lstPermissionRoleAdd);
                    db.SaveChanges();

                    trans.Commit();
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("200", null, "Cập nhật thông tin thành công");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        [AuthorizeAccessRole(TypeHandle = "delete")]
        public JsonResult Delete(int id)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var item = db.SRoles.Find(id);
                if (item == null)
                {
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Không tìm thấy thông tin");
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                db.SRoles.Remove(item);
                db.SaveChanges();
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CheckExistUser(int id)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var item = db.SUserRoles.AsNoTracking().FirstOrDefault(x => x.RoleId == id);
                httpMessage.Body.Data = item != null;
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetListUser(int id)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            try
            {
                var lstUserId = db.SUserRoles.AsNoTracking().Where(x => x.RoleId == id).Select(x => x.UserId).ToList();
                httpMessage.Body.Data = lstUserId;
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return Json(httpMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeAccessRole(TypeHandle = "update")]
        public JsonResult UpdateListUser(string strUserId, int id)
        {
            HttpMessage httpMessage = new HttpMessage(true);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var lstUserId = JsonConvert.DeserializeObject<List<int>>(strUserId);
                    var lstUserIdExist = db.SUserRoles.AsNoTracking().Where(x => x.RoleId == id).Select(x => x.UserId).ToList();
                    var lstUserAdd = lstUserId.Except(lstUserIdExist).ToList();
                    var lstUserRemove = lstUserIdExist.Except(lstUserId).ToList();

                    var lstItemRemove = db.SUserRoles.Where(x => x.RoleId == id && lstUserRemove.Contains(x.UserId)).ToList();
                    foreach (var item in lstItemRemove)
                    {
                        db.SUserRoles.Remove(item);
                    }
                    db.SaveChanges();

                    var lstItemAdd = lstUserAdd.Select(x => new SUserRole
                    {
                        RoleId = id,
                        UserId = x
                    }).ToList();
                    db.SUserRoles.AddRange(lstItemAdd);
                    db.SaveChanges();

                    trans.Commit();
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    httpMessage.IsOk = false;
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                    return Json(httpMessage, JsonRequestBehavior.AllowGet);
                }
            }
        }

        private HttpMessage CheckValid(SRole item)
        {
            HttpMessage httpMessage = new HttpMessage(false);
            try
            {
                if (string.IsNullOrEmpty(item.Ma))
                {
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Vui lòng nhập mã phòng ban");
                    return httpMessage;
                }
                if (string.IsNullOrEmpty(item.Ten))
                {
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Vui lòng nhập tên phòng ban");
                    return httpMessage;
                }
                var exist = db.SRoles.AsNoTracking().FirstOrDefault(x => x.Id != item.Id && x.Ma == item.Ma);
                if (exist != null)
                {
                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Mã phòng ban đã tồn tại");
                    return httpMessage;
                }
                httpMessage.IsOk = true;
                return httpMessage;
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return httpMessage;
            }
        }
    }
}