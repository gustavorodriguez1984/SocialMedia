using SocialMediaCore.QueryFilters;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaCore.Interfaces
{
    public interface IUriService
    {
        Uri GetPostPaginationUri(PostQueryFilter filter, string actionUrl);
    }
}
