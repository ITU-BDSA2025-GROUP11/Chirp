/*using Microsoft.AspNetCore.Identity;
using System;
namespace Chirp.Infrastructure;

public class Hash
{
    public static void main(string[] args)
    {
        PasswordHasher<IdentityUser> hasher = new PasswordHasher<IdentityUser>();

         IdentityUser helge = new IdentityUser { UserName = "ropf@itu.dk", Email = "ropf@itu.dk" }; 
         IdentityUser adrian = new IdentityUser { UserName = "adho@itu.dk", Email = "adho@itu.dk" };
    
        string helgeHash = hasher.HashPassword(helge, "LetM31n!");
        string adrianHash = hasher.HashPassword(adrian, "M32Want_Access");

        Console.WriteLine($"Helge Hash: {helgeHash}");
        Console.WriteLine($"Adrian Hash: {adrianHash}");
    }
}*/