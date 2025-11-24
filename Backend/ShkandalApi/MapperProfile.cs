using AutoMapper;
using shkandal_api.DTOs.ArticleDtos;
using shkandal_api.DTOs.ClusterDtos;
using shkandal_api.DTOs.ClusterDTOs;
using shkandal_api.DTOs.MediaDtos;
using shkandalData.Models;

namespace shkandal_api
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
            CreateMap<Cluster, ClusterAdminReadDto>();
            CreateMap<Cluster, ClusterAdminUpdateDto>();
            CreateMap<ClusterAdminUpdateRequest, Cluster>().
                ForAllMembers(opt => opt.Condition(
                    (src, dest, srcMember) => srcMember != null));
        }
    }
}
