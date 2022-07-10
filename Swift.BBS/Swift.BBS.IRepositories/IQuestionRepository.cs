using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Swift.BBS.IRepositories.BASE;
using Swift.BBS.Model.Models;

namespace Swift.BBS.IRepositories
{
    public interface IQuestionRepository : IBaseRepository<Question>
    {
        Task<Question> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    }
}
