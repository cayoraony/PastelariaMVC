using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace PastelariaMvc.Infra
{
    public static class DecodeToken
    {
        public static int getId(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var readToken = handler.ReadToken(token) as JwtSecurityToken;

            var idUsuario = readToken.Claims.ToList()[0].Value;

            return int.Parse(idUsuario);
        }

        public static bool getEGestor(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var readToken = handler.ReadToken(token) as JwtSecurityToken;

            var eGestor = readToken.Claims.ToList()[1].Value;

            return bool.Parse(eGestor);
        }
        
        public static string getNome(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var readToken = handler.ReadToken(token) as JwtSecurityToken;

            var nomeUsuario = readToken.Claims.ToList()[2].Value;

            return nomeUsuario;
        }
    }
}