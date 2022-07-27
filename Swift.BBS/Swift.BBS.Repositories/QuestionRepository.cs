using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Swift.BBS.EntityFramework;
using Swift.BBS.EntityFramework.EfContext;
using Swift.BBS.IRepositories;
using Swift.BBS.Model.Models;
using Swift.BBS.Repositories.BASE;

namespace Swift.BBS.Repositories
{
    public class QuestionRepository: BaseRepository<Question> , IQuestionRepository
    {
        public QuestionRepository(SwiftBbsContext context) : base(context)
        {
        }

        public Task<Question> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return DbContext().Questions.Where(x => x.Id == id)
                .Include(x => x.QuestionComments).ThenInclude(x => x.CreateUser).SingleOrDefaultAsync(cancellationToken);
        }
    }
}
