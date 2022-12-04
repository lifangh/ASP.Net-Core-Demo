using SwiftCode.BBS.EntityFramework;
using SwiftCode.BBS.IService.BASE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SwiftCode.BBS.IService
{
    public interface IQuestionService : IBaseServices<Question>
    {
        Task<Question> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Question> GetQuestionDetailsAsync(int id, CancellationToken cancellationToken = default);

        Task AddQuestionComments(int id, int userId, string content, CancellationToken cancellationToken = default);
    }
}
