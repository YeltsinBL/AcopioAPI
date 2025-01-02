using AcopioAPIs.DTOs.Login;
using AcopioAPIs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AcopioAPIs.Repositories
{
    public class AuthorizationRepository : IAuthorization
    {
        private readonly DbacopioContext _context;
        private readonly IConfiguration _configuration;

        public AuthorizationRepository(DbacopioContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<bool> VerifyRegisterPassword(AuthorizationRequest authorizationRequest)
        {
            try
            {
                return await _context.Users.AnyAsync(u => u.UserName.Equals(authorizationRequest.UserName)
                    && u.UserPassword.Equals(Encoding.UTF8.GetBytes(authorizationRequest.UserPassword))
                    && u.UserResetPassword);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> RegisterNewPassword(RegisterPasswordRequest resetPasswordRequest)
        {
            try
            {
                var user = await _context.Users.Where(u 
                    => u.UserName.Equals(resetPasswordRequest.UserName)).FirstOrDefaultAsync()
                    ?? throw new KeyNotFoundException("Usuario no encontrado");

                CreatePasswordHash(resetPasswordRequest.UserPassword, out var claveHash, out var claveSalt);

                var verifyToken = CreateRandomVerifyToken(); 

                user.UserPassword = claveHash;
                user.UserKeySalt = claveSalt;
                user.UserResetPassword = false;
                user.VerificarToken = verifyToken;
                user.UserModifiedAt = resetPasswordRequest.UserModifiedAt;
                user.UserModifiedName = resetPasswordRequest.UserName;

                await _context.SaveChangesAsync();

                return true;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<AuthorizationResponse> TokenResponse(AuthorizationRequest authorizationRequest)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(authorizationRequest);
                var user = await _context.Users.FirstOrDefaultAsync(u
                   => u.UserName.Equals(authorizationRequest.UserName))
                   ?? throw new KeyNotFoundException("Usuario incorrecto");
                if(!user.UserStatus && user.UserResetPassword)
                    throw new ArgumentException("Usuario no activo");
                if(!VerifyPasswordHash(authorizationRequest.UserPassword,user.UserPassword,
                    user.UserKeySalt!))
                    throw new KeyNotFoundException("Contraseña incorrecta");

                string tokenCreado = CreateToken(user.UserId.ToString(), user.UserName);

                return await SaveHistoryRefreshToken(
                    user.UserId, tokenCreado, CreateRefreshToken());
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<AuthorizationResponse> RefreshTokenResponse(RefreshTokenRequest refreshTokenRequest)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenSupuestamenteExperido = tokenHandler.ReadJwtToken(refreshTokenRequest.TokenExpirado);

            if (tokenSupuestamenteExperido.ValidTo > DateTime.UtcNow)
            {
                throw new ArgumentException("Token no ha expirado");
            }

            // Obtenemos el IdUsuario que está dentro del JWT
            string idUsuario = tokenSupuestamenteExperido.Claims.FirstOrDefault(x =>
                x.Type == ClaimTypes.NameIdentifier)!.Value.ToString();
            string nombre_Usuario = tokenSupuestamenteExperido.Claims.FirstOrDefault(x =>
                x.Type == ClaimTypes.Name)!.Value.ToString();

            var refreshTokenRegistrado = _context.HistorialRefreshTokens.FirstOrDefault(X =>
                X.Token == refreshTokenRequest.TokenExpirado &&
                X.RefreshToken == refreshTokenRequest.RefreshToken &&
                X.UserId == int.Parse(idUsuario))
                ?? throw new ArgumentException("No existe refreshToken");
            // Generar ambos Tokens
            var refreshTokenCreado = CreateRefreshToken();
            var tokenCreado = CreateToken(idUsuario, nombre_Usuario);

            return await SaveHistoryRefreshToken(int.Parse(idUsuario), tokenCreado, refreshTokenCreado);
        }


        private static void CreatePasswordHash(string password, out byte[] claveHash, out byte[] claveSalt)
        {
            using var hmac = new HMACSHA512();
            claveSalt = hmac.Key;// genera una clave aleatoria
            claveHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
        private static bool VerifyPasswordHash(string password, byte[] claveHash, byte[] claveSalt)
        {
            // indicamos a partir de cual contraseña calculamos el Hash
            using var hmac = new HMACSHA512(claveSalt);
            var calcularHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return calcularHash.SequenceEqual(claveHash);
        }
        private static string CreateRandomVerifyToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
        private string CreateToken(string idUsuario, string nombre_usuario)
        {
            // Accedemos a la clave secreta para el JWT
            var key = _configuration.GetValue<string>("JwtSetting:secretKey");
            // convetimos la clave en array
            var keyBytes = Encoding.ASCII.GetBytes(key!);
            // Agregar la información del usuario al Token
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, idUsuario),
                new(ClaimTypes.Name, nombre_usuario)
            };
            // Agregar Roles dinámicos
            var roles = new[]
            {
                "Administrador", "Supervisor"
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            // Crear credencial para el Token
            var credencialesToken = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature
                );

            // Descripción del Token
            var tok = new JwtSecurityToken(
                "", // identificar quién emitió el JWT
                "", // identificar el destinatario del JWT
                claims, // información del usuario
                expires: DateTime.UtcNow.AddMinutes(1), // tiempo de expiración del JWT
                signingCredentials: credencialesToken // credenciales del JWT
                );

            // Crear los controladores del JWT
            var tokenHandler = new JwtSecurityTokenHandler();

            // obtener el Token
            string tokenCreado = tokenHandler.WriteToken(tok);

            return tokenCreado;

        }
        private async Task<AuthorizationResponse> SaveHistoryRefreshToken(int idUsuario,
            string token, string refreshToken)
        {
            var historyRefreshToken = new HistorialRefreshToken
            {
                UserId = idUsuario,
                Token = token,
                RefreshToken = refreshToken,
                FechaCreacion = DateTime.UtcNow,
                FechaExpiracion = DateTime.UtcNow.AddMinutes(2)
            };

            await _context.HistorialRefreshTokens.AddAsync(historyRefreshToken);
            await _context.SaveChangesAsync();
            var usuario_registrado = _context.Users.FirstOrDefault(x =>
                x.UserId == idUsuario
            );

            return new AuthorizationResponse()
            {
                Token = token,
                RefreshToken = refreshToken,
                Resultado = true
            };

        }
        private static string CreateRefreshToken()
        {
            var byteArray = new byte[64];
            var refreshToken = "";

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(byteArray);
                refreshToken = Convert.ToBase64String(byteArray);
            }
            return refreshToken;
        }

    }
}
