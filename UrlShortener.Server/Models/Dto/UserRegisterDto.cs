﻿using System.ComponentModel.DataAnnotations;
using UrlShortener.Server.Models.Entities;

namespace UrlShortener.Server.Models.Dto;

public class UserRegisterDto
{
    [EnumDataType(typeof(Roles))]
    public required Roles Role { get; set; }

    [StringLength(32)]
    public required string Username { get; set; }

    [StringLength(64)]
    public required string Password { get; set; }

    [StringLength(320)]
    [EmailAddress]
    public required string Email { get; set; }

    public static explicit operator User(UserRegisterDto v)
    {
        return new()
        {
            Role = v.Role,
            Username = v.Username,
            Password = v.Password,
            Email = v.Email,
            CreatedAt = DateTime.Now
        };
    }
}
