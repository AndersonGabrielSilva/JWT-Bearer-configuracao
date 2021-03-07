using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Shop.Models;
using Microsoft.IdentityModel.Tokens;

namespace Shop.Services
{
    public static class TokenService
    {
        public static string GenerateToken(User user)
        {
            //Chave secreta => somente o servidor contem está chave
            //Convertemos está chave para bytes
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
             
            //SecurityTokenDescriptor => Contém as informaçoes do token/ de como ele será configurado
            //Ele é dividido em 3 partes : Subject, Expires e Signing Credentials
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]//Definimos o que vamos deixar disponivel para a api,
                {                                       //Será utilizado nos controlers ná Autorização 
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),

                //Expires => Data de expiração do tolken
                Expires = DateTime.UtcNow.AddHours(2),

                //SigningCredentials => Informamnos a chave e o tipo da incriptação
                SigningCredentials = 
                new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)                 
            };

           

           //JwtSecurityTokenHandler => è o cara que gera o Token
            var tokenHandler = new JwtSecurityTokenHandler();

            //Criando o Token de acordo com a definição do token descriptor
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //Escrevendo o token => Tranformando na string
            return tokenHandler.WriteToken(token); 
        }
    }
}