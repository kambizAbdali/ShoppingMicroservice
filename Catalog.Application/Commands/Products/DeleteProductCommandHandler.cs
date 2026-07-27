using AutoMapper;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Commands.Products
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public DeleteProductCommand(string id)
        {
            Id = id;
        }
    }
    public class DeleteProductCommandHandler(IProductRepository productRepository, IMapper mapper) :
        IRequestHandler<DeleteProductCommand, bool>
    {
        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var result = await productRepository.DeleteProductAsync(request.Id);
            return result;
        }
    }
}
