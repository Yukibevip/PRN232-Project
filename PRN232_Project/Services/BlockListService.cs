using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BlockListService : IBlockListService
    {
        private readonly IBlockListRepository _blockListRepository;
        public BlockListService(IBlockListRepository blockListRepository)
        {
            _blockListRepository = blockListRepository;
        }
    }
}
