﻿namespace Booking.Auth.WebAPI.Models;

public class TokenDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}