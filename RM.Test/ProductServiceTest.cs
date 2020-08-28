using Moq;
using RM.Domain.Entities;
using RM.Domain.Interfaces.Repositories;
using RM.Domain.Interfaces.Services;
using RM.Domain.Services;
using RM.Domain.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RM.Test
{
    public class ProductServiceTest
    {
        private readonly Mock<IFileCsvService> _fileCsvServiceMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly IProductService _productService;
        public ProductServiceTest()
        {
            _fileCsvServiceMock = new Mock<IFileCsvService>();
            _fileCsvServiceMock.Setup(x => x.ReadFile(It.IsAny<string>())).Returns(new List<ProductDto>
            {
                new ProductDto
                {
                    Cpf = "3712144284",
                    Mercado = "tonin",
                    Nome = "Linguiça Sadia",
                    NomePesquisa = "linguica",
                    Preco = 12.56d,
                    TemOferta = 0,
                    DataCompra = "26/08/2020"
                }
            });

            _productRepositoryMock = new Mock<IProductRepository>();
            _productRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).Returns(Task.FromResult<ICollection<Product>>(new List<Product>
            {
                new Product
                {
                    Name = "Agua",
                    Market = "tonin",
                    Price = 2.05d,
                    PenultimatePrice = 2.05d,
                    SearchableName = "agua",
                    TemOferta = false,
                    DateOfLastPurchase = "26/08/2020",
                    PenultimatePurchaseDate = "26/08/2020"                    
                }
            }));
            _productRepositoryMock.Setup(x => x.GetSearchableNamesAsync()).Returns(Task.FromResult<ICollection<string>>(new List<string>
            {
                "linguica","agua"
            }));

            _productRepositoryMock.Setup(x => x.AddAsync(new List<Product>(), "")).Returns(Task.FromResult(new { }));
            _productRepositoryMock.Setup(x => x.AddSearchableNamesAsync(new List<string>())).Returns(Task.FromResult(new { }));

            _productService = new ProductService(_fileCsvServiceMock.Object, _productRepositoryMock.Object);
        }

        // verificar se não esta duplicando nomes pesquisaveis
        // verificar se não esta duplicando produtos
        // verificar se esta atualizando preço e data dos produtos
        // verificar se não esta sobrepondo produtos do arquivo com os que estão no firebase
    }
}
