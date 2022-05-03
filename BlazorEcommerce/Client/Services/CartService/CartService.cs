﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorEcommerce.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public event Action OnChange;

        public CartService(ILocalStorageService localStorage, HttpClient http, AuthenticationStateProvider authenticationStateProvider)
        {
            _localStorage = localStorage;
            _http = http;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task AddToCart(CartItem cartItem)
        {
            if (await IsUserAuthenticated())
                Console.WriteLine("user is authenticated");
            else
                Console.WriteLine("user is not authenticated");

            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                cart = new List<CartItem>();
            }

            var sameItem = cart.Find(x => x.ProductId.Equals(cartItem.ProductId) && x.ProductTypeId.Equals(cartItem.ProductTypeId));
            if (sameItem == null)
                cart.Add(cartItem);
            else
                sameItem.Quantity += cartItem.Quantity;

            await _localStorage.SetItemAsync("cart", cart);

            await GetCartItems();
        }


        public async Task<List<CartItem>> GetCartItems()
        {
            await GetCartItemCount();
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                cart = new List<CartItem>();
            }

            return cart;
        }

        public async Task<List<CartProductResponse>> GetCartProducts()
        {
            var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            var response = await _http.PostAsJsonAsync("api/cart/products", cartItems);
            var cartProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponse>>>();

            return cartProducts.Data;
        }

        public async Task RemoveProductFromCart(int productId, int productTypeId)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
                return;

            var cartItem = cart.Find(x => x.ProductId == productId && x.ProductTypeId == productTypeId);
            if(cartItem != null) { 
                cart.Remove(cartItem);

                await _localStorage.SetItemAsync("cart", cart);

                await GetCartItemCount();
            }
        }

        public async Task UpdateQuantity(CartProductResponse product)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
                return;

            var cartItem = cart.Find(x => x.ProductId == product.ProductId && x.ProductTypeId == product.ProductTypeId);
            if (cartItem != null)
            {
                cartItem.Quantity = product.Quantity;

                await _localStorage.SetItemAsync("cart", cart);
            }
        }

        public async Task StoreCartItems(bool emptyLocalCart = false)
        {
            var localCart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (localCart == null)
                return;

            await _http.PostAsJsonAsync("api/cart", localCart);

            if (emptyLocalCart)
                await _localStorage.RemoveItemAsync("cart");
        }

        private async Task<bool> IsUserAuthenticated()
        {
            return (await _authenticationStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }

        public async Task GetCartItemCount()
        {
            if (await IsUserAuthenticated())
            {
                var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("/api/cart/count");
                var count = result.Data;

                await _localStorage.SetItemAsync<int>("cartItemsCount", count);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

                await _localStorage.SetItemAsync<int>("cartItemsCount", cart !=null ? cart.Count : 0);
            }

            OnChange.Invoke();
        }
    }
}
