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

            CreateMap<Media, MediaReadDto>();

            CreateMap<Cluster, ClusterReadDto>();

            CreateMap<Cluster, ClusterDetailedReadDto>();

            CreateMap<Cluster, ClusterUpdateDto>();
        }
    }
}
