using AutoMapper;
using SocialMediaCore.DTOs;
using SocialMediaCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaInfrastructure.Mappings
{
    public class AutomapperProfile :Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Post, PostDto>();
            CreateMap<PostDto,Post>();
        }
    }
}
