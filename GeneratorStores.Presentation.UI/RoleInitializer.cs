﻿using GeneratorStores.DataAccess.Entities;
using GeneratorStores.Presentation.UI.Data;
using Microsoft.AspNetCore.Identity;


namespace RentConnect.Presentation.UI;

public class RoleInitializer
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedRolesAsync()
    {
        string[] roleNames = { "User", "Admin", "SuperAdmin" };

        foreach (var roleName in roleNames)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Seed a default SuperAdmin
        var superAdminEmail = "Saherzaid1997@gmail.com";
        var superAdmin = await _userManager.FindByEmailAsync(superAdminEmail);
        if (superAdmin == null)
        {
            var newSuperAdmin = new ApplicationUser
            {
                UserName = superAdminEmail,
                Email = superAdminEmail,
                FullName = "Saher Zaid"  // Provide a value for FullName
            };
            await _userManager.CreateAsync(newSuperAdmin, "Hej123!!!");
            await _userManager.AddToRoleAsync(newSuperAdmin, "SuperAdmin");
        }
    }
}