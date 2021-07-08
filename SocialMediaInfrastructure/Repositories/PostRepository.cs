﻿using Microsoft.EntityFrameworkCore;
using SocialMediaCore.Entities;
using SocialMediaCore.Interfaces;
using SocialMediaInfrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaInfrastructure.Repositories
{
    public class PostRepository  : IPostRepository
    {
        private readonly SocialMediaContext _context;


        public PostRepository(SocialMediaContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Post>> GetPosts()
        {
            var posts = await _context.Posts.ToListAsync();
            return posts;
            
        }
        public async Task<Post> GetPost(int id)
        {
            var posts = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == id);
            return posts;

        }


    }
}
