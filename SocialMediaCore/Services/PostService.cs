﻿using SocialMediaCore.Entities;
using SocialMediaCore.Interfaces;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await _unitOfWork.PostRepository.GetAll();
        }

        public async Task InsertPost(Post post)
        {
            var user = await _unitOfWork.UserRepository.GetById(post.UserId);
            if (user == null)
            {
                throw new Exception("El usuario no existe");            
            }
            if (post.Description.Contains("Sexo"))
            {
                throw new Exception("Contenido no permitido");
            }
            await _unitOfWork.PostRepository.Add(post);

        }

        public async Task<bool> UpdatePost(Post post)
        {
            await _unitOfWork.PostRepository.Update(post);
            return true;

        }
        public async Task<bool> DeletePost(int id)
        {
            await _unitOfWork.PostRepository.Delete(id);
            return true;
        }
    }
}
