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
}
