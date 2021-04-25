using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Mediaverse.Domain.Authentication.Repositories;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;

namespace Mediaverse.Infrastructure.JointContentConsumption.Repositories
{
    public class ViewerRepository : IViewerRepository
    {
        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;
        
        public ViewerRepository(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        
        public async Task<Viewer> GetAsync(Guid memberId, CancellationToken cancellationToken)
        {
            var user =  await _userRepository.GetUserAsync(memberId, cancellationToken);
            return _mapper.Map<Viewer>(user);
        }
    }
}