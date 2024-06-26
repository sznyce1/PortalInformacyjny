﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjektZaliczeniowy.entities;
using ProjektZaliczeniowy.Exceptions;
using ProjektZaliczeniowy.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ProjektZaliczeniowy.Services
{
    public interface IAccountService
    {
        string GenerateJwt(LoginDto dto);
        void ReisterUser(RegisterUserDto dto);
    }
    public class AccountService : IAccountService
    {
        private readonly ArticleDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(ArticleDbContext context,IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _context = context;
            _passwordHasher =passwordHasher;
            _authenticationSettings = authenticationSettings;
        }
        public void ReisterUser(RegisterUserDto dto)
        {
            
            var newUser = new User()
            {
                Email = dto.Email,
                Name = dto.Name,
                Nationality = dto.Nationality,
                RoleId = dto.RoleId
            };
            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;
            _context.Users.Add(newUser);
            _context.SaveChanges(); 

        }
        public string GenerateJwt(LoginDto dto)
        {
            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == dto.Email);
            if (user is null)
            {
                throw new BadRequestException("invalid username or password");
            }
            var result = _passwordHasher.VerifyHashedPassword(user,user.PasswordHash,dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("invalid username or password");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name.ToString()),
                
            };
            if (!string.IsNullOrEmpty(user.Nationality))
            {
                claims.Add( new Claim("Nationality", user.Nationality )); 
            }
         
                
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }
    }
}
