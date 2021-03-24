using AutoMapper;
using Pi.Common;
using Pi.PiManager.Model.Models;
using Pi.PiManager.Model.Models.Main;
using Pi.PiManager.Model.Models.Project;
using Pi.PiManager.Model.ViewModels;
using Pi.PiManager.Model.ViewModels.API;

namespace Pi.PiManager.IServices.AutoMapper
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// 对象转换配置
        /// </summary>
        public MappingProfile()
        {
            // 广告
            CreateMap<Advert, AdverViewModel>()
              .ForMember(a => a.Pic, a => a.MapFrom(s => Utility.GetImgUrl(s.Pic)))
              .ForMember(d => d.StartDate, o => o.MapFrom(s => Utility.GetDateFormat(s.StartDate)))
              .ForMember(d => d.EndDate, o => o.MapFrom(s => Utility.GetDateFormat(s.EndDate)));
            // 文章
            CreateMap<Article, ArticleViewModel>()
              .ForMember(d => d.ImgUrl, o => o.MapFrom(s => Utility.GetImgUrl(s.ImgUrl)))
              .ForMember(d => d.FileUrl, o => o.MapFrom(s => Utility.GetImgUrl(s.FileUrl)))
              .ForMember(d => d.AddDate, o => o.MapFrom(s => Utility.GetDateFormat(s.AddDate)));
            // 证书
            CreateMap<Certificate, CertificateViewModel>()
                .ForMember(d => d.ExpireDate, o => o.MapFrom(s => Utility.GetDateFormat(s.ExpireDate)))
                .ForMember(d => d.RegisterDate, o => o.MapFrom(s => Utility.GetDateFormat(s.RegisterDate)));
            //域名
            CreateMap<DomainName, DomainNameViewModel>()
                .ForMember(d => d.ExpireDate, o => o.MapFrom(s => Utility.GetDateFormat(s.ExpireDate)))
                .ForMember(d => d.RegisterDate, o => o.MapFrom(s => Utility.GetDateFormat(s.RegisterDate)));
            //服务商
            CreateMap<Provider, ProviderViewModel>()
           .ForMember(d => d.AddDate, o => o.MapFrom(s => Utility.GetDateFormat(s.AddDate)));
            //服务器
            CreateMap<ServerInfo, ServerInfoViewModel>()
           .ForMember(d => d.ExpireDate, o => o.MapFrom(s => Utility.GetDateFormat(s.ExpireDate)))
            .ForMember(d => d.RegisterDate, o => o.MapFrom(s => Utility.GetDateFormat(s.RegisterDate)));
            //节点
            CreateMap<ProjectNodeInfo, ProjectNodeViewModel>()
            .ForMember(d => d.StartDate, o => o.MapFrom(s => Utility.GetDateFormat(s.StartDate,1)))
            .ForMember(d => d.EndDate, o => o.MapFrom(s => Utility.GetDateFormat(s.EndDate,1)))
            .ForMember(d => d.CompleteDate, o => o.MapFrom(s => Utility.GetDateFormat(s.CompleteDate)))
            .ForMember(d => d.LastUpdateDate, o => o.MapFrom(s => Utility.GetDateFormat(s.LastUpdateDate)));

        }
    }
}
