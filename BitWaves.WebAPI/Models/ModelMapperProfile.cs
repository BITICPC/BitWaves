using AutoMapper;
using BitWaves.Data.DML;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Services;

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
            CreateMap<CreateSubmissionModel, Submission>(MemberList.Source)
                .ForSourceMember(m => m.LanguageId, opt => opt.DoNotValidate());
        }

        private void MapUpdateModels()
        {
            CreateMap<UpdateAnnouncementModel, AnnouncementUpdateInfo>(MemberList.Source);
            CreateMap<UpdateProblemModel, ProblemUpdateInfo>(MemberList.Source)
                .ForPath(u => u.Description.Legend, cfg => cfg.MapFrom(m => m.Legend))
                .ForPath(u => u.Description.Input, cfg => cfg.MapFrom(m => m.Input))
                .ForPath(u => u.Description.Output, cfg => cfg.MapFrom(m => m.Output))
                .ForPath(u => u.Description.Notes, cfg => cfg.MapFrom(m => m.Notes))
                .ForPath(u => u.JudgeInfo.JudgeMode, cfg => cfg.MapFrom(m => m.JudgeMode))
                .ForPath(u => u.JudgeInfo.TimeLimit, cfg => cfg.MapFrom(m => m.TimeLimit))
                .ForPath(u => u.JudgeInfo.MemoryLimit, cfg => cfg.MapFrom(m => m.MemoryLimit))
                .ForPath(u => u.JudgeInfo.BuiltinCheckerOptions, cfg => cfg.MapFrom(m => m.BuiltinCheckerOptions));
            CreateMap<UpdateUserModel, UserUpdateInfo>(MemberList.Source);
        }

        /// <summary>
        /// 创建从实体对象到输出模型的 AutoMapper 映射配置。
        /// </summary>
        private void MapOutputModels()
        {
            CreateMap<Announcement, AnnouncementListInfo>();
            CreateMap<Announcement, AnnouncementInfo>()
                .IncludeBase<Announcement, AnnouncementListInfo>();
            CreateMap<User, UserListInfo>();
            CreateMap<User, UserInfo>()
                .IncludeBase<User, UserListInfo>()
                .ForMember(u => u.Rank, opt => opt.Ignore());
            CreateMap<Content, ContentInfo>();
            CreateMap<Problem, ProblemListInfo>();
            CreateMap<Problem, ProblemInfo>()
                .IncludeBase<Problem, ProblemListInfo>()
                .IncludeMembers(p => p.Description, p => p.JudgeInfo);
            CreateMap<ProblemDescription, ProblemInfo>(MemberList.Source);
            CreateMap<ProblemJudgeInfo, ProblemInfo>(MemberList.Source)
                .ForMember(p => p.IsTestReady, opt => opt.MapFrom(e => e.TestDataArchiveFileId != null));
            CreateMap<ProblemSampleTest, ProblemSampleTestInfo>();
            CreateMap<ProblemTag, ProblemTagInfo>();
            CreateMap<Language, LanguageInfo>();
            CreateMap<Submission, SubmissionListInfo>()
                .ForMember(i => i.ProblemTitle, opt => opt.MapFrom((e, _) => e.Problem?.Title))
                .ForMember(i => i.ProblemArchiveId, opt => opt.MapFrom((e, _) => e.Problem?.ArchiveId))
                .ForMember(i => i.Language, opt => opt.MapFrom((e, _) => e.LanguageDisplayName))
                .ForMember(i => i.Verdict, opt => opt.MapFrom((s, _) => s.Result?.Verdict))
                .ForMember(i => i.Time, opt => opt.MapFrom((s, _) => s.Result?.Time))
                .ForMember(i => i.Memory, opt => opt.MapFrom((s, _) => s.Result?.Memory));
            CreateMap<Submission, SubmissionInfo>()
                .IncludeBase<Submission, SubmissionListInfo>();
            CreateMap<JudgeResult, SubmissionJudgeResultInfo>();
            CreateMap<TestCaseResult, TestCaseJudgeResultInfo>();
            CreateMap<JudgeNodeInfo, JudgeNodeInfoModel>();
            CreateMap<JudgeNodePerformanceInfo, JudgeNodePerformanceInfoModel>();
        }
    }
}
