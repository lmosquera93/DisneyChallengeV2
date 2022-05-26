using System.Collections.Generic;

namespace DisneyChallengeV2.Models.Users
{
    public class UserLoginResponse
    {
        //token de seguridad.
        public string Token { get; set; }
        //authenticated o no.
        public bool Login { get; set; }
        //lista de errores.
        public List<string> Errors { get; set; }
    }
}
