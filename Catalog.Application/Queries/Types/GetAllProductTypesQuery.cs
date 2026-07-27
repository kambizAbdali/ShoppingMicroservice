using AutoMapper;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Queries.Types
{
    public class GetAllProductTypesQuery : IRequest<IEnumerable<TypeResponse>>
    {
    }

    public class GetAllProductTypesQueryHandler(ITypeRepository repository, IMapper mapper)
        : IRequestHandler<GetAllProductTypesQuery, IEnumerable<TypeResponse>>
    {
        public async Task<IEnumerable<TypeResponse>> Handle(GetAllProductTypesQuery request, CancellationToken CT)
        {
            var result = await repository.GetProductTypeAsync();
            return mapper.Map<IEnumerable<TypeResponse>>(result);
        }
    }
}
