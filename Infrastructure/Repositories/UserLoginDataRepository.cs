﻿using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserLoginDataRepository :IUserLoginDataRepository
    {
        private readonly UserDbContext context;

        public UserLoginDataRepository(UserDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(UserLoginData userLoginData)
        {
            await context.UserLoginDatas.AddAsync(userLoginData);
            await context.SaveChangesAsync();
        }
        public async Task<UserLoginData?> GetByEmailAsync(string email)
        {
            return await context.UserLoginDatas.Where(uld => uld.Email == email).FirstOrDefaultAsync();
        }
        public async Task UpdateRefreshToken(int ID, string? refreshToken, DateTime refreshTokenExpirationTime)
        {
            var userLoginData = await context.UserLoginDatas.FindAsync(ID);
            userLoginData.RefreshToken = refreshToken;
            userLoginData.RefreshTokenExpirationTime = refreshTokenExpirationTime;
            await context.SaveChangesAsync();
        }
    }
}