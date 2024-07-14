using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNoiBai.Dto.User
{
    public class UserPermissionDto
    {
        public int PermissionId {  get; set; }
        public string Controller {  get; set; }
        public string ControllerName { get; set; }
        public string Action { get; set; }
        public string ActionName { get; set; }
        public int? IsGranted {  get; set; }
    }

    public class UserPermissionViewDto
    {
        public string Controller { get; set; }
        public string ControllerName { get; set; }
        public List<UserPermissionDto> LstPermission {  get; set; }
    }
}