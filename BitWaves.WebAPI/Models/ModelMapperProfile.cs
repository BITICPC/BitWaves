using AutoMapper;
using BitWaves.Data.DML;
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
            // Do not use private or protected constructors because this may lead to broken data.
            ShouldUseConstructor = ci => ci.IsPublic;

            MapCreateModels();
            MapUpdateModels();
            MapOutputModels();
        }

        /// <summary>
        /// 创建从表示创建实体对象的数据模型到相应的实体对象的 AutoMapper 配置。
        /// </summary>
        private void MapCreateModels()
        {
            CreateMap<CreateAnnouncementModel, Announcement>(MemberList.Source);
            CreateMap<CreateUserModel, User>(MemberList.Source);
            CreateMap<CreateProblemModel, Problem>(MemberList.Source);
            CreateMap<CreateLanguageModel, Language>(MemberList.Source);
        }

        private void MapUpdateModels()
        {
            CreateMap<UpdateAnnouncementModel, AnnouncementUpdateInfo>(MemberList.Source);
            CreateMap<UpdateProblemModel, ProblemUpdateInfo>(MemberList.Source)
                .ForMember(u => u.Description.Legend, cfg => cfg.MapFrom(m => m.Legend))
                .ForMember(u => u.Description.Input, cfg => cfg.MapFrom(m => m.Input))
                .ForMember(u => u.Description.Output, cfg => cfg.MapFrom(m => m.Output))
                .ForMember(u => u.Description.Notes, cfg => cfg.MapFrom(m => m.Notes))
                .ForMember(u => u.JudgeInfo.JudgeMode, cfg => cfg.MapFrom(m => m.JudgeMode))
                .ForMember(u => u.JudgeInfo.TimeLimit, cfg => cfg.MapFrom(m => m.TimeLimit))
                .ForMember(u => u.JudgeInfo.MemoryLimit, cfg => cfg.MapFrom(m => m.MemoryLimit))
                .ForMember(u => u.JudgeInfo.BuiltinCheckerOptions, cfg => cfg.MapFrom(m => m.BuiltinCheckerOptions));
            CreateMap<UpdateUserModel, UserUpdateInfo>(MemberList.Source);
        }

        /// <summary>
        /// 创建从实体对象到输出模型的 AutoMapper 映射配置。
        /// </summary>
        private void MapOutputModels()
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
            CreateMap<ProblemTag, ProblemTagInfo>();
            CreateMap<Language, LanguageInfo>();
        }
    }
}
