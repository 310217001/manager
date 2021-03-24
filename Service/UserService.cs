using Pi.PiManager.IService;
using Pi.PiManager.Model.Models;
using Pi.PiManager.Service.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Pi.PiManager.Service
{
    public class UserService : BaseService<UserInfo>, IUserService
    {
        /// <summary>
        /// 获取用户显示名称
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public string GetNames(UserInfo u)
        {
            if (!String.IsNullOrWhiteSpace(u.NickName))
                return u.NickName;
            if (!String.IsNullOrWhiteSpace(u.FullName))
                return u.FullName;
            if (!String.IsNullOrWhiteSpace(u.UserName))
                return u.UserName;
            return u.ID.ToString();
        }
        //        #region 更新
        //        /// <summary>
        //        /// 修改密码
        //        /// </summary>
        //        /// <param name="ID">ID</param>
        //        /// <param name="Password"></param>
        //        /// <returns></returns>
        //        public bool ModifySolve(int ID, string Password)
        //        {
        //            return Db.Updateable<UserInfo>()
        //               .SetColumns(it => it.Password == Password)
        //               .Where(it => it.ID == ID).ExecuteCommand() > 0;
        //        }
        //        /// <summary>
        //        /// 实名认证
        //        /// </summary>
        //        /// <param name="ID">用户</param>
        //        /// <param name="RealName">姓名</param>
        //        /// <param name="IdCard">身份证</param>
        //        /// <param name="IsCertification">是否认证</param>
        //        /// <returns></returns>
        //        public bool ModifyCertification(int ID, string RealName, string IdCard, bool IsCertification = true)
        //        {
        //            return Db.Updateable<UserInfo>()
        //                .SetColumns(it => new UserInfo() { IdCard = IdCard, FullName = RealName, IsCertification = IsCertification })
        //                .Where(it => it.ID == ID).ExecuteCommand() > 0;
        //        }
        //        /// <summary>
        //        /// 更新最后登录时间
        //        /// </summary>
        //        /// <param name="ID"></param>
        //        /// <param name="lastLoginData"></param>
        //        /// <returns></returns>
        //        public bool ModifyLoginDate(int ID, DateTime lastLoginData)
        //        {
        //            return Db.Updateable<UserInfo>()
        //             .SetColumns(it => new UserInfo() { LastLoginData = lastLoginData })
        //             .Where(it => it.ID == ID).ExecuteCommand() > 0;
        //        }
        //        /// <summary>
        //        /// 更新用户状态
        //        /// </summary>
        //        /// <param name="ID"></param>
        //        /// <param name="state">1未审核 2已审核 3禁用 4被标记删除</param>
        //        /// <returns></returns>
        //        public bool ModifyState(int ID, int state)
        //        {
        //            return Db.Updateable<UserInfo>()
        //              .SetColumns(it => new UserInfo() { State = state })
        //              .Where(it => it.ID == ID).ExecuteCommand() > 0;
        //        }
        //        /// <summary>
        //        /// 更新用户头像
        //        /// </summary>
        //        /// <param name="ID"></param>
        //        /// <param name="Head">头像地址 url</param>
        //        /// <returns></returns>
        //        public bool ModifyHead(int ID, string Head)
        //        {
        //            return Db.Updateable<UserInfo>()
        //               .SetColumns(it => new UserInfo() { HeadPortrait = Head })
        //               .Where(it => it.ID == ID).ExecuteCommand() > 0;
        //        }
        //        /// <summary>
        //        /// 更新用户地区
        //        /// </summary>
        //        /// <param name="ID"></param>
        //        /// <param name="AreasID">地区ID</param>
        //        /// <returns></returns>
        //        public bool ModifyAreas(int ID, int AreasID)
        //        {
        //            return Db.Updateable<UserInfo>()
        //               .SetColumns(it => new UserInfo() { AreasID = AreasID })
        //               .Where(it => it.ID == ID).ExecuteCommand() > 0;
        //        }
        //        /// <summary>
        //        /// 更新用户推荐人
        //        /// </summary>
        //        /// <param name="ID"></param>
        //        /// <param name="Referrer">推荐人</param>
        //        /// <returns></returns>
        //        public bool ModifyReferrer(int ID, int Referrer)
        //        {
        //            return Db.Updateable<UserInfo>()
        //               .SetColumns(it => new UserInfo() { Referrer = Referrer })
        //               .Where(it => it.ID == ID).ExecuteCommand() > 0;
        //        }

        //        /// <summary>
        //        /// 更新用户信息
        //        /// </summary>
        //        /// <param name="ID"></param>
        //        /// <param name="NickName">昵称</param>
        //        /// <param name="Gender">性别</param>
        //        /// <returns></returns>
        //        public bool ModifyInfo(int ID, string NickName, int? Gender = null)
        //        {
        //            return Db.Updateable<UserInfo>()
        //                .SetColumnsIF(Gender == null, a => new UserInfo() { NickName = NickName })
        //                .SetColumnsIF(Gender != null, a => new UserInfo() { NickName = NickName, Gender = Gender.Value })
        //                .Where(a => a.ID == ID).ExecuteCommand() > 0;
        //        }
        //        /// <summary>
        //        /// 更新用户设备ID信息
        //        /// </summary>
        //        /// <param name="ID"></param>
        //        /// <param name="ClientID">设备ID</param>
        //        /// <returns></returns>
        //        public bool ModifyClientID(int ID, string ClientID)
        //        {
        //            return Db.Updateable<UserInfo>()
        //               .SetColumns(it => new UserInfo() { ClientID = ClientID })
        //               .Where(it => it.ID == ID).ExecuteCommand() > 0;
        //        }
        //        /// <summary>
        //        /// 更新用户主ID
        //        /// </summary>
        //        /// <param name="ID"></param>
        //        /// <param name="MasterID">主ID</param>
        //        /// <returns></returns>
        //        public bool ModifyMasterID(int ID, int MasterID)
        //        {
        //            return Db.Updateable<UserInfo>()
        //               .SetColumns(it => new UserInfo() { MasterID = MasterID })
        //               .Where(it => it.ID == ID).ExecuteCommand() > 0;
        //        }
        //        public bool AddHonor(int ID, decimal Honor)
        //        {
        //            return Db.Updateable<UserInfo>().SetColumns(it => new UserInfo() { Honor = it.Honor + Honor }).Where(it => it.ID == ID).ExecuteCommand() > 0;
        //        }
        //        public bool AddGold(int ID, decimal Gold)
        //        {
        //            return Db.Updateable<UserInfo>().SetColumns(it => new UserInfo() { Gold = it.Gold + Gold, SumGold = it.SumGold + Gold }).Where(it => it.ID == ID).ExecuteCommand() > 0;
        //        }
        //        public bool ModifyHonorGold(int ID, decimal Honor, decimal Gold)
        //        {
        //            return Db.Updateable<UserInfo>().SetColumns(it => new UserInfo() { Honor = it.Honor + Honor, Gold = it.Gold + Gold }).Where(it => it.ID == ID).ExecuteCommand() > 0;
        //        }
        //        public bool AddFund(int ID, decimal Fund)
        //        {
        //            return Db.Updateable<UserInfo>().SetColumns(it => new UserInfo() { Fund = it.Fund + Fund, SumFund = it.SumFund + Fund }).Where(it => it.ID == ID).ExecuteCommand() > 0;
        //        }

        //        public bool ModifyGold(int ID, decimal Gold)
        //        {
        //            return Db.Updateable<UserInfo>().SetColumns(it => new UserInfo() { Gold = it.Gold + Gold }).Where(it => it.ID == ID).ExecuteCommand() > 0;
        //        }
        //        public bool ModifyFund(int ID, decimal Fund)
        //        {
        //            return Db.Updateable<UserInfo>().SetColumns(it => new UserInfo() { Fund = it.Fund + Fund }).Where(it => it.ID == ID).ExecuteCommand() > 0;
        //        }
        //        /// <summary>
        //        /// 更新手机
        //        /// </summary>
        //        /// <param name="ID"></param>
        //        /// <param name="MobilePhone"></param>
        //        /// <returns></returns>
        //        public bool ModifyMobilePhone(int ID, string MobilePhone)
        //        {
        //            return Db.Updateable<UserInfo>()
        //               .SetColumns(it => new UserInfo() { MobilePhone = MobilePhone })
        //               .Where(it => it.ID == ID).ExecuteCommand() > 0;
        //        }


        //        public bool ModifyCompanyID(int ID, int CompanyID, int CompanyState, int? RoleID = null)
        //        {
        //            //return Db.Updateable<UserInfo>()
        //            //   .UpdateColumnsIF(RoleID == null, it => new UserInfo() { CompanyID = CompanyID, CompanyState = CompanyState })
        //            //   .UpdateColumnsIF(RoleID != null, it => new UserInfo() { CompanyID = CompanyID, CompanyState = CompanyState, RoleID = RoleID.Value })
        //            //   .Where(it => it.ID == ID).ExecuteCommand() > 0;

        //            if (RoleID == null)
        //                return Db.Updateable<UserInfo>()
        //                    .SetColumns(it => new UserInfo() { CompanyID = CompanyID, CompanyState = CompanyState })
        //                   .Where(it => it.ID == ID).ExecuteCommand() > 0;
        //            return Db.Updateable<UserInfo>()
        //                .SetColumns(it => new UserInfo() { CompanyID = CompanyID, CompanyState = CompanyState, RoleID = RoleID.Value })
        //                .Where(it => it.ID == ID).ExecuteCommand() > 0;
        //        }
        //        public bool ModifyCompanyState(int ID, int CompanyState)
        //        {
        //            return Db.Updateable<UserInfo>()
        //               .SetColumns(it => new UserInfo() { CompanyState = CompanyState })
        //               .Where(it => it.ID == ID).ExecuteCommand() > 0;
        //        }
        //        public bool ModifyRoleID(int ID, int RoleID)
        //        {
        //            return Db.Updateable<UserInfo>()
        //               .SetColumns(it => new UserInfo() { RoleID = RoleID })
        //               .Where(it => it.ID == ID).ExecuteCommand() > 0;
        //        }
        //        #endregion
        //        #region 查询
        //        /// <summary>
        //        /// 根据用户名和密码查询
        //        /// </summary>
        //        /// <param name="ID"></param>
        //        /// <returns></returns>
        //        public UserInfo QueryOne(string userName, string password)
        //        {
        //            return Db.Queryable<UserInfo>().First(it => it.UserName == userName && it.Password == password);
        //        }
        //        /// <summary>
        //        /// 根据手机号查用户
        //        /// </summary>
        //        /// <param name="Idcard"></param>
        //        /// <returns></returns>
        //        public UserInfo QueryOneByMobilePhone(string MobilePhone)
        //        {
        //            return Db.Queryable<UserInfo>().First(it => it.MobilePhone == MobilePhone);
        //        }
        //        /// <summary>
        //        /// 根据身份证查用户
        //        /// </summary>
        //        /// <param name="Idcard"></param>
        //        /// <returns></returns>
        //        public UserInfo QueryOneByIdCard(string IdCard)
        //        {
        //            //DataTable dt = SqlHelper.ExecuteTable("select * from [T_User] where IdCard=@IdCard AND IsCertification=1", new SqlParameter("@IdCard", IdCard));
        //            //if (dt != null && dt.Rows.Count > 0)
        //            //    return DataToIList.CreateItem<UserInfo>(dt.Rows[0]);
        //            //return null;
        //            return Db.Queryable<UserInfo>().First(it => it.IdCard == IdCard && it.IsCertification);
        //        }
        //        /// <summary>
        //        /// 根据ID查询用户平台唯一ID
        //        /// </summary>
        //        /// <param name="ID"></param>
        //        /// <returns></returns>
        //        public string QueryPiUserIDByID(int ID)
        //        {
        //            //return SqlHelper.ExecuteScalar("select PiUserID from [T_User] where ID=" + ID) + "";

        //            //return Db.Queryable<UserInfo>().Select(a => a.PiUserID).First(it=>it.it);
        //            UserInfo u = Db.Queryable<UserInfo>().First(it => it.ID == ID);
        //            if (u != null)
        //                return u.PiUserID;
        //            return null;
        //        }
        //        /// <summary>
        //        /// 根据用户平台唯一ID
        //        /// </summary>
        //        /// <param name="PiUserID">用户平台唯一ID</param>
        //        /// <returns></returns>
        //        public UserInfo QueryOneByPiUserID(string PiUserID)
        //        {
        //            return Db.Queryable<UserInfo>().First(it => it.PiUserID == PiUserID);
        //        }
        //        /// <summary>
        //        /// 通过用户名查询ID
        //        /// </summary>
        //        /// <param name="userName">用户名</param>
        //        /// <returns></returns>
        //        public int QueryIdByUserName(string userName)
        //        {
        //            /*
        //             * 生成语句
        //             * 查询了整条应该是程序处理只返回了对应的字段给我。
        //exec sp_executesql 
        //N'SELECT * FROM (SELECT [ID],[PiUserID],[UserName],[Password],[HeadPortrait],[NickName],[FullName],[Position],[IdCard],[BankCard],[IsCertification],[Gender],[AddDate],[MobilePhone],[QQ],[BirthDate],[CompanyID],[Mail],[LastLoginData],[State],[CompanyState],[Type],[RoleID],[RegistIP],[Referrer],[AreasID],[Areas],[PlateNumber],[Fund],[SumFund],[Gold],[SumGold],[Honor],[ClientID],[MasterID],[BindDate],ROW_NUMBER() OVER(ORDER BY GETDATE() ) AS RowIndex  
        //FROM [T_User]  
        //WHERE ( [UserName] = @UserName0 )) T WHERE RowIndex BETWEEN 1 AND 1',N'@UserName0 nvarchar(4000)',@UserName0=N'jhyzl'             
        //             */
        //            return Db.Queryable<UserInfo>().Where(a => a.UserName == userName).Select(a => a.ID).ObjToInt();
        //        }

        //        /// <summary>
        //        /// 查询用户的手机号码列表
        //        /// </summary>
        //        /// <param name="UserID">用户ID组</param>
        //        /// <param name="State">状态 1未审核 2已审核 3禁用 4被标记删除</param>
        //        /// <returns></returns>
        //        public List<long> QueryPhone(int[] UserID, int? State = null)
        //        {
        //            Expressionable<UserInfo> exp = Expressionable.Create<UserInfo>();
        //            if (UserID != null)
        //            {
        //                if (UserID.Length == 1)
        //                {
        //                    int id = UserID[0];
        //                    exp.And(a => a.ID == id);
        //                }
        //                else
        //                    exp.And(a => UserID.Contains(a.ID));
        //            }
        //            if (State != null)
        //                exp.And(a => a.State == State);
        //            return Db.Queryable<UserInfo>().Where(exp.ToExpression()).Select(a => long.Parse(a.MobilePhone)).ToList();
        //        }
        //        /// <summary>
        //        /// 查询用户徒弟
        //        /// </summary>
        //        /// <param name="UserID"></param>
        //        /// <returns></returns>
        //        public List<int> QueryDisciple(int UserID)
        //        {
        //            return Db.Queryable<UserInfo>().Where(a => a.Referrer == UserID).Select(a => a.ID).ToList();
        //        }

        //        /// <summary>
        //        /// 根据ID查询
        //        /// </summary>
        //        /// <param name="UserID">ID数组</param>
        //        /// <param name="State">状态 1未审核 2已审核 3禁用 4被标记删除</param>
        //        /// <returns></returns>
        //        public Task<IEnumerable<UserInfo>> QueryList(int[] UserID, int? State = null)
        //        {
        //            Expressionable<UserInfo> exp = Expressionable.Create<UserInfo>();
        //            exp.And(it => UserID.Contains(it.ID));
        //            if (State != null)
        //                exp.And(a => a.State == State);
        //            var data = Db.Queryable<UserInfo>().Where(exp.ToExpression()).OrderBy(a => a.UserName).OrderBy(a => a.ID, OrderByType.Desc);

        //            return Task.Run(function: () => data.ToList().AsEnumerable());
        //        }
        //        /// <summary>
        //        /// 排名
        //        /// </summary>
        //        /// <param name="Intergal">用户成长值</param>
        //        /// <returns></returns>
        //        public int QueryCount(decimal Intergal, int Type)
        //        {
        //            Expressionable<UserInfo> exp = Expressionable.Create<UserInfo>();
        //            if (Intergal != 0)
        //            {
        //                if (Type == 1)
        //                    exp.And(a => a.Gold > Intergal);    //成长值
        //                else if (Type == 2)
        //                    exp.And(a => a.SumGold > Intergal); //累计成长值
        //            }
        //            return Db.Queryable<UserInfo>().Count(exp.ToExpression());
        //        }
        //        /// <summary>
        //        /// 查询数量
        //        /// </summary>
        //        public int QueryCount(string Name = null, string MobilePhone = null, int? State = null, int? Type = null, int? RoleID = null, decimal? Fund1 = null, decimal? Fund2 = null, decimal? SumFund1 = null, decimal? SumFund2 = null, int? AreasID = null, int? Referrer = null, bool? IsCertification = null, string IdCard = null, string PlateNumber = null, int? CompanyID = null, bool? IsComplete = null, int? CompanyState = null, string FullNameAndMobilePhone = null, int[] RoleIDs = null)
        //        {
        //            return Db.Queryable<UserInfo>().Count(GetPara(Name, MobilePhone, State, Type, RoleID, Fund1, Fund2, SumFund1, SumFund2, AreasID, Referrer, IsCertification, IdCard, PlateNumber, CompanyID, IsComplete, CompanyState, FullNameAndMobilePhone, RoleIDs).ToExpression());
        //        }
        //        /// <summary>
        //        /// 列表
        //        /// </summary>
        //        public Task<IEnumerable<UserInfo>> QueryList(DBPage p, string Name = null, string MobilePhone = null, int? State = null, int? Type = null, int? RoleID = null, decimal? Fund1 = null, decimal? Fund2 = null, decimal? SumFund1 = null, decimal? SumFund2 = null, int? AreasID = null, int? Referrer = null, bool? IsCertification = null, string IdCard = null, string PlateNumber = null, int? CompanyID = null, bool? IsComplete = null, int? CompanyState = null, string FullNameAndMobilePhone = null, int[] RoleIDs = null)
        //        {
        //            var data = Db.Queryable<UserInfo>().Where(GetPara(Name, MobilePhone, State, Type, RoleID, Fund1, Fund2, SumFund1, SumFund2, AreasID, Referrer, IsCertification, IdCard, PlateNumber, CompanyID, IsComplete, CompanyState, FullNameAndMobilePhone, RoleIDs).ToExpression())
        //                .OrderBy(a => a.AddDate, OrderByType.Desc).OrderBy(a => a.ID, OrderByType.Desc);

        //            if (p != null)
        //            {
        //                int count = 0;
        //                List<UserInfo> list = data.ToPageList(p.PageIndex, p.PageSize, ref count);
        //                p.Count = count;
        //                return Task.Run(function: () => list.AsEnumerable());
        //            }
        //            return Task.Run(function: () => data.ToList().AsEnumerable());
        //        }

        //        /// <summary>
        //        /// 查询人数
        //        /// </summary>
        //        /// <param name="date1">开始时间</param>
        //        /// <param name="date2">结束时间</param>
        //        /// <param name="State">状态 1未审核 2已审核 3禁用 4被标记删除</param>
        //        /// <returns></returns>
        //        public List<DateTime> QueryNumber(DateTime date1, DateTime date2, int? State = null)
        //        {
        //            Expressionable<UserInfo> exp = Expressionable.Create<UserInfo>();
        //            exp.And(a => a.AddDate >= date1 && a.AddDate < date2);
        //            //string sql = "SELECT AddDate FROM [T_User] WHERE 1=1 AND AddDate>='"
        //            //+ date1.ToString("yyyy-MM-dd HH:mm:ss") + "' AND AddDate<'" + date2.ToString("yyyy-MM-dd HH:mm:ss") + "'";
        //            if (State != null)
        //                exp.And(a => a.State == State);
        //            var dt = Db.Queryable<UserInfo>().Where(exp.ToExpression()).Select(a => a.AddDate).ToList();
        //            if (dt != null && dt.Count > 0)
        //            {
        //                List<DateTime> list = new List<DateTime>(dt.Count);
        //                foreach (DateTime item in dt)
        //                {
        //                    list.Add(item);
        //                }
        //                return list;
        //            }
        //            return null;
        //        }
        //        /// <summary>
        //        /// 查找该用户发展的下线
        //        /// </summary>
        //        /// <param name="PageIndex">页数</param>
        //        /// <param name="PageSize">显示数</param>
        //        /// <param name="count">总数</param>
        //        /// <param name="userID">用户</param>
        //        /// <param name="userName">用户名</param>
        //        /// <param name="State">1未审核 2已审核 3禁用 4被标记删除</param>
        //        /// <returns></returns>
        //        public List<UserInfo> QueryJuniorList(int PageIndex, int PageSize, out int count, int userID, string userName = null, int? State = null)
        //        {
        //            Expressionable<UserInfo> exp = Expressionable.Create<UserInfo>();
        //            exp.And(a => a.Referrer == userID);
        //            if (userName != null)
        //                exp.And(a => a.UserName.Contains(userName));
        //            if (State != null)
        //                exp.And(a => a.State == State);
        //            var data = Db.Queryable<UserInfo>().Where(exp.ToExpression()).OrderBy(a => a.AddDate, OrderByType.Desc).OrderBy(a => a.ID, OrderByType.Desc);
        //            count = 0;
        //            List<UserInfo> list = data.ToPageList(PageIndex, PageSize, ref count);
        //            return list;
        //        }
        //        /// <summary>
        //        /// 查找该用户发展的下线用户数量
        //        /// </summary>
        //        /// <param name="UserID">用户</param>
        //        /// <param name="userName">用户名</param>
        //        /// <param name="State">1未审核 2已审核 3禁用 4被标记删除</param>
        //        /// <returns></returns>
        //        public long QueryJuniorCount(int userID, string userName = null, int? State = null)
        //        {
        //            Expressionable<UserInfo> exp = Expressionable.Create<UserInfo>();
        //            exp.And(a => a.Referrer == userID);
        //            if (userName != null)
        //                exp.And(a => a.UserName.Contains(userName));
        //            if (State != null)
        //                exp.And(a => a.State == State);
        //            return Db.Queryable<UserInfo>().Count(exp.ToExpression());
        //        }
        //        /// <summary>
        //        /// 查询用户推荐人
        //        /// </summary>
        //        /// <param name="UserID"></param>
        //        /// <returns></returns>
        //        public int QueryReferrer(int UserID)
        //        {
        //            //return C.Int(SqlHelper.ExecuteScalar("SELECT Referrer FROM [T_User] WHERE ID=" + UserID));
        //            UserInfo u = Db.Queryable<UserInfo>().First(it => it.ID == UserID);
        //            if (u != null)
        //                return u.Referrer;
        //            return 0;
        //        }
        //        /// <summary>
        //        /// 查询地区
        //        /// </summary>
        //        /// <param name="UserID"></param>
        //        /// <returns></returns>
        //        public int QueryAreasID(int UserID)
        //        {
        //            //return C.Int(SqlHelper.ExecuteScalar("SELECT AreasID FROM [T_User] WHERE ID=" + UserID));
        //            UserInfo u = Db.Queryable<UserInfo>().First(it => it.ID == UserID);
        //            if (u != null)
        //                return u.AreasID;
        //            return 0;
        //        }

        //        /// <summary>
        //        /// 获取条件
        //        /// </summary>
        //        /// <param name="lp1">条件</param>
        //        /// <param name="sql">SQL语句</param>
        //        /// <param name="Name">用户名 or 姓名</param>
        //        /// <param name="MobilePhone">手机号码</param>
        //        /// <param name="State">状态 1未审核 2已审核 3禁用 4被标记删除</param>
        //        /// <param name="Type">用户类型 1微信 2PC</param>
        //        /// <param name="RoleID">角色ID</param>
        //        private Expressionable<UserInfo> GetPara(string Name = null, string MobilePhone = null, int? State = null, int? Type = null, int? RoleID = null, decimal? Fund1 = null, decimal? Fund2 = null, decimal? SumFund1 = null, decimal? SumFund2 = null, int? AreasID = null, int? Referrer = null, bool? IsCertification = null, string IdCard = null, string PlateNumber = null, int? CompanyID = null, bool? IsComplete = null, int? CompanyState = null, string FullNameAndMobilePhone = null, int[] RoleIDs = null)
        //        {
        //            Expressionable<UserInfo> exp = Expressionable.Create<UserInfo>();
        //            if (RoleIDs != null && RoleIDs.Length > 0)
        //            {
        //                if (RoleIDs.Length == 1)
        //                {
        //                    int roleid = RoleIDs[0];
        //                    exp.And(a => a.RoleID == roleid);
        //                }
        //                else
        //                    exp.And(a => RoleIDs.Contains(a.RoleID));
        //            }
        //            if (State != null)
        //                exp.And(a => a.State == State);
        //            if (Type != null)
        //                exp.And(a => a.Type == Type);
        //            if (RoleID != null)
        //                exp.And(a => a.RoleID == RoleID);
        //            if (Name != null)
        //            {
        //                exp.And(a => a.UserName.Contains(Name) || a.FullName.Contains(Name) || a.NickName.Contains(Name) || a.MobilePhone.Contains(Name));
        //            }
        //            if (PlateNumber != null)
        //            {
        //                exp.And(a => a.PlateNumber == PlateNumber);
        //            }
        //            if (MobilePhone != null)
        //            {
        //                exp.And(a => a.MobilePhone.Contains(MobilePhone));
        //            }
        //            if (AreasID != null)
        //            {
        //                string areas = AreasID.ToString();
        //                if (areas.Length == 6)
        //                {
        //                    if (areas.Substring(4, 2) != "00")
        //                        exp.And(a => a.AreasID == AreasID);   // 区级
        //                    else if (areas.Substring(2, 2) != "00")
        //                    {
        //                        exp.And(a => a.AreasID >= C.Int(areas.Substring(0, 4) + "00") && a.AreasID <= C.Int(areas.Substring(0, 4) + "99"));
        //                    }
        //                    else
        //                    {
        //                        exp.And(a => a.AreasID >= C.Int(areas.Substring(0, 2) + "0000") && a.AreasID <= C.Int(areas.Substring(0, 2) + "9999"));
        //                    }
        //                }
        //                //sql += " AND AreasID=" + AreasID;
        //            }
        //            if (Referrer != null)
        //                exp.And(a => a.Referrer == Referrer);
        //            if (Fund1 != null)
        //            {
        //                exp.And(a => a.Fund >= Fund1);
        //            }
        //            if (Fund2 != null)
        //            {
        //                exp.And(a => a.Fund < Fund2);
        //            }
        //            if (SumFund1 != null)
        //            {
        //                exp.And(a => a.SumFund >= SumFund1);
        //            }
        //            if (SumFund2 != null)
        //            {
        //                exp.And(a => a.SumFund < SumFund2);
        //            }
        //            if (IsCertification != null)
        //            {
        //                exp.And(a => a.IsCertification == IsCertification);
        //            }
        //            if (IdCard != null)
        //            {
        //                exp.And(a => a.IdCard == IdCard);
        //            }
        //            if (CompanyID != null)
        //                exp.And(a => a.CompanyID == CompanyID);
        //            if (IsComplete != null)
        //            {
        //                //sql += " AND ID IN (SELECT UserID FROM T_CourseUser WHERE IsComplete=@IsComplete)";
        //                //lp1.Add(new SqlParameter("@IsComplete", IsComplete));
        //            }
        //            if (CompanyState != null)
        //                exp.And(a => a.CompanyState == CompanyState);
        //            if (FullNameAndMobilePhone != null)
        //            {
        //                exp.And(a => a.FullName.Contains(FullNameAndMobilePhone) || a.MobilePhone.Contains(FullNameAndMobilePhone));
        //            }
        //            return exp;
        //        }

        //        /// <summary>
        //        /// 查询总用户余额
        //        /// </summary>
        //        /// <param name="m"></param>
        //        /// <returns></returns>
        //        public Decimal UserSumFund()
        //        {
        //            //string sql = "SELECT SUM(Fund) FROM T_User";
        //            //return C.Decimals(SqlHelper.ExecuteScalar(sql));
        //            return Db.Queryable<UserInfo>().Sum(a => a.Fund);
        //        }

        //        public List<UserInfo> QueryList(int? State = null, int[] IDs = null, int[] CompanyIDs = null)
        //        {
        //            Expressionable<UserInfo> exp = Expressionable.Create<UserInfo>();
        //            if (State != null)
        //                exp.And(a => a.State == State);
        //            if (IDs != null)
        //            {
        //                if (IDs.Length == 1)
        //                {
        //                    int id = IDs[0];
        //                    exp.And(a => a.ID == id);
        //                }
        //                else
        //                    exp.And(a => IDs.Contains(a.ID));
        //            }
        //            if (CompanyIDs != null)
        //            {
        //                if (CompanyIDs.Length == 1)
        //                {
        //                    int id = IDs[0];
        //                    exp.And(a => a.CompanyID == id);
        //                }
        //                else
        //                    exp.And(a => CompanyIDs.Contains(a.CompanyID));
        //            }
        //            return Db.Queryable<UserInfo>().Where(exp.ToExpression()).OrderBy(a => a.FullName).OrderBy(a => a.ID).ToList();
        //        }
        //        #endregion
    }
}
