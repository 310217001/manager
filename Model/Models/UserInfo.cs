using SqlSugar;
using System;

namespace Pi.PiManager.Model.Models
{
    /// <summary>
    /// 用户表
    /// </summary>
    [SugarTable("t_user")]
    public class UserInfo
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        /// <summary>
        /// 用户平台唯一ID
        /// </summary>
        public string PiUserID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadPortrait { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 银行卡
        /// </summary>
        public string BankCard { get; set; }
        /// <summary>
        /// 是否实名认证
        /// </summary>
        public bool IsCertification { get; set; }
        /// <summary>
        /// 性别,0:未知,1:男,2:女
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime AddDate { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string MobilePhone { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime BirthDate { get; set; }
        /// <summary>
        /// 支部ID
        /// </summary>
        public int CompanyID { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Mail { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginData { get; set; }
        /// <summary>
        /// 状态 1未审核 2已审核 3禁用 4被标记删除
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 支部申请状态0.未绑定1.申请2.绑定
        /// </summary>
        public int CompanyState { get; set; }
        /// <summary>
        /// 用户类型 1导入 2申请
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 角色ID 1.超级管理员 2.区域管理员 3.支部管理员 4.用户
        /// </summary>
        public int RoleID { get; set; }
        /// <summary>
        /// 注册的IP
        /// </summary>
        public string RegistIP { get; set; }
        /// <summary>
        /// 推荐人
        /// </summary>
        public int Referrer { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public int AreasID { get; set; }
        /// <summary>
        /// 地区 文字
        /// </summary>
        public string Areas { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNumber { get; set; }
        /// <summary>
        /// 余额（用户充值）
        /// </summary>
        public decimal Fund { get; set; }
        /// <summary>
        /// 累计充值金额
        /// </summary>
        public decimal SumFund { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public decimal Gold { get; set; }
        /// <summary>
        /// 累计赠送积分
        /// </summary>
        public decimal SumGold { get; set; }
        /// <summary>
        /// 荣誉
        /// </summary>
        public decimal Honor { get; set; }
        /// <summary>
        /// 用户设备ID(app推送使用)
        /// </summary>
        public string ClientID { get; set; }
        /// <summary>
        /// 主ID
        /// </summary>
        public int MasterID { get; set; }
        /// <summary>
        /// 手机绑定时间
        /// </summary>
        public DateTime BindDate { get; set; }
    }
}
