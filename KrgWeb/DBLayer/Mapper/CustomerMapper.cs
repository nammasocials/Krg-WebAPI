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
    [Mapper]
    public static partial class InvoiceMapper
    {
        [MapperIgnoreTarget(nameof(VInvoiceInput.EwayBillLogo))]
        public static partial VInvoiceInput ToDto(InvInvoice invoice);
        [MapperIgnoreTarget(nameof(VInvoiceInput.EwayBillLogo))]
        public static partial InvInvoice ToEntity(VInvoiceInput invoiceDto);
    }
    [Mapper]
    public static partial class InvoiceItemsMapper
    {
        public static partial VInvoiceProductsInput ToDto(InvInvoiceItem invoiceItem);
        public static partial InvInvoiceItem ToEntity(VInvoiceProductsInput invoiceItemsDto);
        public static partial List<VInvoiceProductsInput> ToDto(List<InvInvoiceItem> invoiceItems);
        public static partial List<InvInvoiceItem> ToEntity(List<VInvoiceProductsInput> invoiceItemsDtos);
    }
}
