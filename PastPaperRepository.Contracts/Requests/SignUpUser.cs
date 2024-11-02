﻿namespace PastPaperRepository.Contracts.Requests;

public class SignUpUser
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}