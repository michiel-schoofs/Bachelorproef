﻿using Nethereum.Web3;
using System.Threading.Tasks;

namespace Console_Application.Services.Interfaces {
    public interface IUserService {
        public Web3 GetUser();
        public Web3 RegisterAccount();
        public Task RegisterUsername();
        public bool HasAccount();
        public Task<bool> HasUsername();
        public Task<string> GetUsername();
        public Task RequireTest();
    }
}
