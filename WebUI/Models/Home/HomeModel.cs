using CameraMap.Models.Notice;
using System.Collections.Generic;
namespace CameraMap.Models.Home
{
    public class HomeModel
    {
        public List<NoticeModel> Items { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        //public static HomeModel GetInstance()
        //{
        //    return new HomeModel();
        //}
    }
}