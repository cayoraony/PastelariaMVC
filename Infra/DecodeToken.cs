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
            var tokenTest = handler.ReadToken(token) as JwtSecurityToken;

            var idUsuario = tokenTest.Claims.ToList()[0].Value;

            return int.Parse(idUsuario);
        }

        public static bool getEGestor(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenTest = handler.ReadToken(token) as JwtSecurityToken;

            var eGestor = tokenTest.Claims.ToList()[1].Value;

            return bool.Parse(eGestor);
        }
    }
}