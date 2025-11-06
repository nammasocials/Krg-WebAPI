using DBLayer.Models;
using DBLayer.ViewModels;
using Riok.Mapperly.Abstractions;

namespace DBLayer.Profiler
{
    [Mapper]
    public static partial class CustomerMapper
    {
        public static partial VCustomerInput ToDto(InvCustomer customer);
        public static partial InvCustomer ToEntity(VCustomerInput customerDto);
    }
    [Mapper]
    public static partial class ProductMapper
    {
        [MapperIgnoreTarget(nameof(VProductInput.ProductLogo))]
        public static partial VProductInput ToDto(InvProduct product);
        [MapperIgnoreTarget(nameof(InvProduct.ProductLogo))]
        public static partial InvProduct ToEntity(VProductInput productDto);
    }
}
