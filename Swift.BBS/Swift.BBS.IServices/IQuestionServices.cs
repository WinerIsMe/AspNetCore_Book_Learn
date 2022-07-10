using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Swift.BBS.IServices.BASE;
using Swift.BBS.Model.Models;

namespace Swift.BBS.IServices
{
    public interface IQuestionServices : IBaseServices<Question>
    {
        Task<Question> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Question> GetQuestionDetailsAsync(int id, CancellationToken cancellationToken = default);

        Task AddQuestionComments(int id, int userId, string content, CancellationToken cancellationToken = default);

    }
}
