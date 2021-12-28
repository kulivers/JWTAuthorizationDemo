using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace JWT_Generation
{
    public class AuthOptions
    {
        //кто генерит токен(обычно url, но у нас для наглядности строка)
        public string Issuer { get; set; }

        //для кого(обычно url, но у нас для наглядности строка)
        public string Audience { get; set; }

        //секретная строка для генерации кея
        public string Secret { get; set; }

        //длительность жизни токена, через это время он не валидный
        public int TokenLifeTime { get; set; }

        public SymmetricSecurityKey Getsymmetricsecuritykey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
        }
    }
}