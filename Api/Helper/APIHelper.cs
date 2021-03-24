using Pi.PiManager.Model.Models;
using Pi.PiManager.Model.ViewModels;
using Pi.PiManager.Model.ViewModels.API;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using Pi.Common;

namespace Pi.PiManager.Api.Helper
{
    public static class APIHelper
    {
        // 循环绑定子级
        public static List<ELTreeViewModel> GetChildren(IEnumerable<ArticleMenu> data, int ID)
        {
            var data2 = data.Where(a => a.ParentID == ID);
            if (data2 != null && data2.Count() > 0)
            {
                List<ELTreeViewModel> list = new List<ELTreeViewModel>();
                foreach (var item in data2)
                {
                    list.Add(new ELTreeViewModel
                    {
                        id = item.ID,
                        label = item.Names,
                        children = GetChildren(data, item.ID)
                    });
                }
                return list;
            }
            return null;
        }

        /// <summary>
        /// 获取菜单列表的子项
        /// </summary>
        /// <param name="FID"></param>
        /// <param name="list2"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static IEnumerable<MenuViewModel> GetMenu(int FID, IEnumerable<Menu> list2, Role role)
        {
            List<MenuViewModel> list = null;
            IEnumerable<Menu> mList1 = list2.Where(a => a.FID == FID);
            if (mList1 != null && mList1.Count() > 0)
            {
                list = new List<MenuViewModel>();
                foreach (var item in mList1)
                {
                    if (role.ID == 1 || role.Menus.Contains("," + item.ID + ","))
                    {
                        list.Add(new MenuViewModel
                        {
                            name = item.Names,
                            url = item.Url,
                            icon = "",
                            childMenus = GetMenu(item.ID, list2, role),
                        });
                    }
                }
            }
            return list;
        }

        public static Lastweek GetLastweek()
        {
            Lastweek lastweek = new Lastweek();
            var date = DateTime.Now;
            var m = (date.DayOfWeek == DayOfWeek.Sunday ? (DayOfWeek)7 : date.DayOfWeek) - DayOfWeek.Monday;
            var s = (date.DayOfWeek == DayOfWeek.Sunday ? (DayOfWeek)7 : date.DayOfWeek) - (DayOfWeek)7;
            lastweek.Mon = C.DateTimes(date.AddDays((-7 - m)).ToString("yyyy-MM-dd"));
            lastweek.Tue = C.DateTimes(date.AddDays((-7 - m + 1)).ToString("yyyy-MM-dd"));
            lastweek.Wed = C.DateTimes(date.AddDays((-7 - m + 2)).ToString("yyyy-MM-dd"));
            lastweek.Thu = C.DateTimes(date.AddDays((-7 - m + 3)).ToString("yyyy-MM-dd"));
            lastweek.Fir = C.DateTimes(date.AddDays((-7 - m + 4)).ToString("yyyy-MM-dd"));
            lastweek.Sat = C.DateTimes(date.AddDays((-7 - m + 5)).ToString("yyyy-MM-dd"));
            lastweek.Sun = C.DateTimes(date.AddDays((-7 - s)).ToString("yyyy-MM-dd"));
            lastweek.NextMon = C.DateTimes(date.AddDays((-7 - s + 1)).ToString("yyyy-MM-dd"));
            return lastweek;
        }
    }
}
