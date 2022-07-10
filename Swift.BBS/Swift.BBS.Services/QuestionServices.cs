﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Swift.BBS.IRepositories;
using Swift.BBS.IRepositories.BASE;
using Swift.BBS.IServices;
using Swift.BBS.Model.Models;
using Swift.BBS.Services.BASE;

namespace Swift.BBS.Services
{
    public  class QuestionServices: BaseServices<Question>, IQuestionServices
    {
        private readonly IQuestionRepository _questionRepository;
        public QuestionServices(IBaseRepository<Question> baseRepository, IQuestionRepository questionRepository) : base(baseRepository)
        {
            _questionRepository = questionRepository;
        }


        public Task<Question> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return _questionRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<Question> GetQuestionDetailsAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _questionRepository.GetByIdAsync(id, cancellationToken);
            entity.Traffic += 1;

            await _questionRepository.UpdateAsync(entity, true, cancellationToken: cancellationToken);

            return entity;
        }

        public async Task AddQuestionComments(int id, int userId, string content, CancellationToken cancellationToken = default)
        {
            var entity = await _questionRepository.GetByIdAsync(id, cancellationToken);
            entity.QuestionComments.Add(new QuestionComment()
            {
                Content = content,
                CreateTime = DateTime.Now,
                CreateUserId = userId
            });
            await _questionRepository.UpdateAsync(entity, true, cancellationToken);
        }
    }
}
