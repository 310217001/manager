using Pi.PiManager.Model.Models;
using Pi.PiManager.Model.ViewModels;
using Pi.PiManager.Model.ViewModels.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pi.PiManager.Api.Helper
{
    public class GetMenusHelps
    {
        public static List<ELTreeViewModel> GetChildren(IEnumerable<Menu> data, int ID)
        {
            var data2 = data.Where(a => a.FID == ID&&a.IsEnable==true);
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
    }
}
