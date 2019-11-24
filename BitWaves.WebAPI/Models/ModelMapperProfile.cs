using AutoMapper;
using BitWaves.Data;
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
            MapInputModels();
            MapOutputModels();
        }

        /// <summary>
        /// 创建从表示创建实体对象的数据模型到相应的实体对象的 AutoMapper 配置。
        /// </summary>
        private void MapInputModels()
        {
            CreateMap<Announcement, AnnouncementListInfo>();
            CreateMap<Announcement, AnnouncementInfo>();
            CreateMap<User, UserListInfo>();
            CreateMap<User, UserInfo>();
            CreateMap<Content, ContentInfo>();
            CreateMap<Problem, ProblemListInfo>();
            CreateMap<Problem, ProblemInfo>()
                .IncludeMembers(p => p.Description, p => p.JudgeInfo);
            CreateMap<ProblemDescription, ProblemInfo>(MemberList.Source);
            CreateMap<ProblemJudgeInfo, ProblemInfo>(MemberList.Source)
                .ForMember(p => p.IsTestReady, opt => opt.MapFrom(e => e.TestDataArchiveFileId != null));
            CreateMap<ProblemSampleTest, ProblemSampleTestInfo>();
            CreateMap<Language, LanguageInfo>();
        }

        /// <summary>
        /// 创建从实体对象到输出模型的 AutoMapper 映射配置。
        /// </summary>
        private void MapOutputModels()
        {
            CreateMap<CreateAnnouncementModel, Announcement>(MemberList.Source)
                .AfterMap<SetAuthorAction<Announcement>>();
            CreateMap<CreateUserModel, User>(MemberList.Source)
                .ForMember(e => e.PasswordHash, opt => opt.MapFrom(m => PasswordUtils.GetPasswordHash(m.Password)));
            CreateMap<CreateProblemModel, Problem>(MemberList.Source)
                .AfterMap<SetAuthorAction<Problem>>();
            CreateMap<CreateLanguageModel, Language>(MemberList.Source);
        }
    }
}
