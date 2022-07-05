using Swift.BBS.IRepositories;
using System;

namespace Swift.BBS.Repositories
{
    public class CalculateRepository : ICalculateRepository
    {
        public int Sum(int i, int j)
        {
            return i + j;
        }
    }
}
