using AutoMapper;
using BitWaves.Data.Entities;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为应用程序数据模型提供 AutoMapper 配置。
    /// </summary>
    public sealed class ModelMapperProfile : Profile
    {
        /// <summary>
        /// 初始化 <see cref="ModelMapperProfile"/> 类的新实例。该构造器将会创建数据模型的所有 AutoMapper 配置。
        /// </summary>
        public ModelMapperProfile()
        {
            CreateMap<Announcement, AnnouncementListInfo>();
            CreateMap<Announcement, AnnouncementInfo>();
            CreateMap<User, UserListInfo>();
            CreateMap<User, UserInfo>();
            CreateMap<Content, ContentInfo>();
            CreateMap<Problem, ProblemListInfo>();
            CreateMap<Problem, ProblemInfo>()
                .IncludeMembers(p => p.Description, p => p.JudgeInfo);
            CreateMap<ProblemDescription, ProblemInfo>();
            CreateMap<ProblemJudgeInfo, ProblemInfo>();
            CreateMap<ProblemSampleTest, ProblemSampleTestInfo>();
            CreateMap<Language, LanguageInfo>();
        }
    }
}
