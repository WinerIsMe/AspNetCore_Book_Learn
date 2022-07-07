using Swift.BBS.IRepositories;
using Swift.BBS.IServices;

namespace Swift.BBS.Services
{
    public class CalculateServices : ICalculateServices
    {
        ICalculateRepository calculateRepository;

        public CalculateServices(ICalculateRepository calculateRepository)
        {
            this.calculateRepository = calculateRepository;
        }

        public int Sum(int i, int j)
        {
            return calculateRepository.Sum(i, j);
        }
    }
}
