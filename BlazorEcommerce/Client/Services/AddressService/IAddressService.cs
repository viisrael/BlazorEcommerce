namespace BlazorEcommerce.Client.Services.AddressService
{
    public interface IAddressService
    {
        Task<Address> CreateOrUpdateAddress(Address address);
        Task<Address> GetAddress();
    }
}