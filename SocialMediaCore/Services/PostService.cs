﻿using Microsoft.Extensions.Options;
using SocialMediaCore.CustomEntities;
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
        private readonly PaginationOptions _paginationOptions;
      
        public PostService(IUnitOfWork unitOfWork,IOptions<PaginationOptions>options)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = options.Value;
        }

       

        public async Task<Post> GetPost(int id)
        {
            return await _unitOfWork.PostRepository.GetById(id);
        }

        public PagedList<Post> GetPosts(PostQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? _paginationOptions.DefaultPageSize : filters.PageSize;

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
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task<bool> UpdatePost(Post post)
        {
            var existingPost = await _unitOfWork.PostRepository.GetById(post.Id);
            existingPost.Image = post.Image;
            existingPost.Description = post.Description;

            _unitOfWork.PostRepository.Update(existingPost);
            await _unitOfWork.SaveChangesAsync();
            return true;

        }
        public async Task<bool> DeletePost(int id)
        {
            await _unitOfWork.PostRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
