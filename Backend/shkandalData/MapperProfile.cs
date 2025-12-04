using AutoMapper;
using shkandalData.DTOs.ArticleDtos;
using shkandalData.DTOs.ClusterDtos;
using shkandalData.DTOs.MediaDtos;
using shkandalData.Models;

namespace shkandalData
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Article, ArticleReadDto>();
            CreateMap<Article, ArticleAdminReadDto>();
            CreateMap<Article, ArticleAdminUpdateDto>();
            CreateMap<ArticleAdminUpdateRequest, Article>() 
               .ForAllMembers(opt => opt.Condition(
                   (src, dest, srcMember) => srcMember != null));

            CreateMap<Media, MediaReadDto>();

            CreateMap<Cluster, ClusterReadDto>();
            CreateMap<Cluster, ClusterDetailedReadDto>();
            CreateMap<Cluster, ClusterUpdateDto>();
            CreateMap<Cluster, ClusterAdminDetailedReadDto>();
            CreateMap<Cluster, ClusterAdminUpdateDto>();
            CreateMap<ClusterAdminUpdateRequest, Cluster>().
                ForAllMembers(opt => opt.Condition(
                    (src, dest, srcMember) => srcMember != null));
        }
    }
}
