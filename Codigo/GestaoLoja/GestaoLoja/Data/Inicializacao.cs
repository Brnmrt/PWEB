
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace GestaoLoja.Data
{
    public static class Inicializacao
    {
        static IDbContextFactory<GestaoLoja.Data.ApplicationDbContext> DbFactory;
        public static async Task CriaDadosIniciais(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Adicionar default Roles
            string[] roles = { "Administrador", "Funcionario", "Cliente" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    IdentityRole roleRole = new IdentityRole(role);
                    await roleManager.CreateAsync(roleRole);
                }
            }

            

            // Adicionar default User - Admin
            var defaultUser = new ApplicationUser
            {
                UserName = "admin@localhost.com",
                Email = "admin@localhost.com", //tem de ter formato de email
                Nome = "Administrador",
                Apelido = "Local",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Estado = "Activo",
                produtosFavoritos = new()
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Is3C..00"); //o .net tem especificacao para passwords
                    // 8 caracteres, 1 letra maiuscula, 1 letra minuscula, 1 numero, 1 caracter especial
                    await userManager.AddToRoleAsync(defaultUser, "Administrador");
                }
            }

            // Adicionar default User - Funcionario
            var defaultUser2 = new ApplicationUser
            {
                UserName = "func@localhost.com",
                Email = "func@localhost.com", //tem de ter formato de email
                Nome = "Funcionario1",
                Apelido = "Local",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Estado = "Activo",
                produtosFavoritos = new()
            };
            if (userManager.Users.All(u => u.Id != defaultUser2.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser2.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser2, "Is3C..00"); //o .net tem especificacao para passwords
                    // 8 caracteres, 1 letra maiuscula, 1 letra minuscula, 1 numero, 1 caracter especial
                    await userManager.AddToRoleAsync(defaultUser2, "Funcionario");
                }
            }

            // Adicionar default User - Cliente
            var defaultUser3 = new ApplicationUser
            {
                UserName = "cliente@localhost.com",
                Email = "cliente@localhost.com", //tem de ter formato de email
                Nome = "ClienteAtivo",
                Apelido = "Local",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Estado = "Activo",
                produtosFavoritos = new()
            };
            if (userManager.Users.All(u => u.Id != defaultUser3.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser3.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser3, "Is3C..00"); //o .net tem especificacao para passwords
                    // 8 caracteres, 1 letra maiuscula, 1 letra minuscula, 1 numero, 1 caracter especial
                    await userManager.AddToRoleAsync(defaultUser3, "Cliente");
                }
            }

            // Adicionar default User - Admin
            var defaultUser4 = new ApplicationUser
            {
                UserName = "clientependente@localhost.com",
                Email = "clientependente@localhost.com", //tem de ter formato de email
                Nome = "ClientePendente",
                Apelido = "Local",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Estado = "Pendente",
                produtosFavoritos = new()

            };
            if (userManager.Users.All(u => u.Id != defaultUser4.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser4.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser4, "Is3C..00"); //o .net tem especificacao para passwords
                    // 8 caracteres, 1 letra maiuscula, 1 letra minuscula, 1 numero, 1 caracter especial
                    await userManager.AddToRoleAsync(defaultUser4, "Cliente");
                }
            }
        }
    }
}
