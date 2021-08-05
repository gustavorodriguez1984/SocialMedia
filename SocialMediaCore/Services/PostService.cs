﻿using SocialMediaCore.CustomEntities;
using SocialMediaCore.Entities;
using SocialMediaCore.Exceptions;
using SocialMediaCore.Interfaces;
using SocialMediaCore.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaCore.Services
{
   public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
      
        public PostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       

        public async Task<Post> GetPost(int id)
        {
            return await _unitOfWork.PostRepository.GetById(id);
        }

        public PagedList<Post> GetPosts(PostQueryFilter filters)
        {

            var posts =  _unitOfWork.PostRepository.GetAll();
            if (filters.UserId != null)
            {
                posts = posts.Where(x => x.UserId == filters.UserId);
            }
            if (filters.Date != null)
            {
                posts = posts.Where(x => x.Date.ToShortDateString() == filters.Date?.ToShortTimeString());
            }
            if (filters.Description != null)
            {
                posts = posts.Where(x => x.Description.ToLower().Contains(filters.Description.ToLower()));
            }

            var pagePost = PagedList<Post>.Create(posts, filters.PageNumber, filters.PageSize);
            return pagePost;
        }

        public async Task InsertPost(Post post)
        {
            var user = await _unitOfWork.UserRepository.GetById(post.UserId);
            if (user == null)
            {
                throw new BusinessException("El usuario no existe");            
            }

            var userPost = await _unitOfWork.PostRepository.GetPostsByUser(post.UserId);
            if (userPost.Count() < 10)
            {
                var lastPost = userPost.OrderByDescending(x => x.Date).FirstOrDefault();
                if ((DateTime.Now-lastPost.Date).TotalDays<7)
                {
                    throw new BusinessException("No puedes publicar ");
                }
            }

            if (post.Description.Contains("Sexo"))
            {
                throw new BusinessException("Contenido no permitido");
            }
            await _unitOfWork.PostRepository.Add(post);
            await _unitOfWork.SaveCHangesAsync();

        }

        public async Task<bool> UpdatePost(Post post)
        {
             _unitOfWork.PostRepository.Update(post);
           await _unitOfWork.SaveCHangesAsync();
            return true;

        }
        public async Task<bool> DeletePost(int id)
        {
            await _unitOfWork.PostRepository.Delete(id);
            return true;
        }
    }
}
