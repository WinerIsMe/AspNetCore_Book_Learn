using Swift.BBS.IRepositories;
using Swift.BBS.IServices;
using Swift.BBS.Repositories;
using System;

namespace Swift.BBS.Services
{
    public class CalculateServices : ICalculateServices
    {
        ICalculateRepository calculateRepository = new CalculateRepository();
        public int Sum(int i, int j)
        {
            return calculateRepository.Sum(i, j);
        }
    }
}
