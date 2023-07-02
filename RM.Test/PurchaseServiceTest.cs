using Moq;
using RM.Domain.Entities;
using RM.Domain.Interfaces.Repositories;
using RM.Domain.Interfaces.Services;
using RM.Domain.Services;
using RM.Domain.Services.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RM.Test
{
    public class PurchaseServiceTest
    {
        private readonly Mock<IFileCsvService> _fileCsvServiceMock;
        private readonly Mock<IPurchaseRepository> _purchaseRepositoryMock;
        private readonly IPurchaseService _purchaseService;
        public PurchaseServiceTest()
        {
            _fileCsvServiceMock = new Mock<IFileCsvService>();
            _fileCsvServiceMock.Setup(x => x.ReadFile(It.IsAny<string>())).Returns(new List<ProductDto>
            {
                new ProductDto
                {
                    Cpf = "3712144284",
                    Mercado = "tonin",
                    Nome = "Lingui�a Sadia",
                    NomePesquisa = "linguica",
                    Preco = 12.56d,
                    TemOferta = 0,
                    DataCompra = "26/08/2020"
                }
            });

            _purchaseRepositoryMock = new Mock<IPurchaseRepository>();
            var items = new List<Item>();
            _purchaseRepositoryMock.Setup(x => x.AddAsync(items)).Returns(Task.FromResult(new { }));

            _purchaseService = new PurchaseService(_fileCsvServiceMock.Object, _purchaseRepositoryMock.Object);
        }

        [Fact(DisplayName = "Deve adicionar os produtos do csv no firebase")]
        [Trait("Recomenda��o de Mercado", "Adicionar itens no Firebase")]
        public async Task  ShouldAddProductInFirebase()
        {
            var result = await _purchaseService.AddPurchaseAsync(It.IsAny<string>());
            var purchase = (Purchase)result.Data;            
            Assert.Equal("tonin", purchase.Market);
            Assert.False(purchase.Items.FirstOrDefault().HaveOffer);
        }
    }
}
