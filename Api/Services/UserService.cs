using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Helpers;
using Dominio.Entities;
using Dominio.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services;
    public class UserService : IUserService
    {
        private readonly JWT _jwt;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(IUnitOfWork unitOfWork, IOptions<JWT> jwt, IPasswordHasher<User> passwordHasher)
        {
            _jwt= jwt.Value;
            _unitOfWork= unitOfWork;
            _passwordHasher= passwordHasher;
        }

        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            var usuario= new User
            {
                Email= registerDto.Email,
                UserName= registerDto.Username,
            };
            usuario.Password = _passwordHasher.HashPassword(usuario, registerDto.Password);
            var usuarioExiste= _unitOfWork.Users.Find(u => u.UserName.ToLower()== registerDto.Username.ToLower()).FirstOrDefault();
            if (usuarioExiste==null)
            {
                try
                {
                    //usuario.Rols.Add(rolPredeterminado);
                    _unitOfWork.Users.Add(usuario);
                    await _unitOfWork.SaveAsync();

                    return $"El Usuario {registerDto.Username} ha sido registrado exitosamente";
                }
                catch (Exception ex)
                {
                    var message= ex.Message;
                    return $"Error: {message}";
                }
            }
            else{
                return $"El Usuario {registerDto.Username} ya se encuentra registrado.";
            }
        }
        public async Task<string> AddRoleAsync(AddRoleDto model)
        {
            var usuario = await _unitOfWork.Users.GetByUsernameAsync(model.Username);
            if(usuario == null)
            {
                return $"No existe algun usuario registrao con esta cuenta. Olvido algun caracter?{model.Username} ";
            }

            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, model.Password);
            if (resultado == PasswordVerificationResult.Success)
            {
                var rolExiste= _unitOfWork.Rols.Find(u => u.Nombre.ToLower()== model.Rol.ToLower()).FirstOrDefault();

                if(rolExiste != null)
                {
                    var usuarioTieneRol= usuario.Rols.Any(u => u.Id==rolExiste.Id);
                    if(usuarioTieneRol == false)
                    {
                       usuario.Rols.Add(rolExiste);
                       _unitOfWork.Users.Update(usuario);
                       await _unitOfWork.SaveAsync();
                    }
                    return $"Rol {model.Rol} agregado a la cuenta {model.Username} de forma exitosa.";
                }
                return $"Rol {model.Rol} no encontrado.";
            }
            return $"Credenciales incorrectas para el usuario: {model.Username} .";
        }

        public async Task<DatoUsuarioDto> GetTokenAsync(LoginDto model)
        {
            DatoUsuarioDto datoUsuarioDto = new DatoUsuarioDto();
            var usuario = await _unitOfWork.Users.GetByUsernameAsync(model.Username);

            if (usuario == null)
            {
                datoUsuarioDto.EstaAutenticado = false;
                datoUsuarioDto.Mensaje = $"No existe ningun usuario con el username {model.Username}";
                return datoUsuarioDto;
            }

            var result = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, model.Password);
            if (result == PasswordVerificationResult.Success)
            {
                datoUsuarioDto.Mensaje= "Ok";
                datoUsuarioDto.EstaAutenticado= true;
                if (usuario != null)
                {
                    JwtSecurityToken jwtSecurityToken = CreateJwtToken(usuario);
                    datoUsuarioDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                    datoUsuarioDto.UserName = usuario.UserName;
                    datoUsuarioDto.Email= usuario.Email;
                    datoUsuarioDto.Roles= usuario.Rols.Select(p => p.Nombre).ToList();

                    return datoUsuarioDto;
                }
                else{
                    datoUsuarioDto.EstaAutenticado = false;
                    datoUsuarioDto.Mensaje = $"Credenciales incorrectas para el usuario {usuario.UserName}";
            
                    return datoUsuarioDto;
                }
            }
            datoUsuarioDto.EstaAutenticado = false;
            datoUsuarioDto.Mensaje = $"Credenciales incorrectas para el usuario {usuario.UserName}";
            
            return datoUsuarioDto;
        }
        private JwtSecurityToken CreateJwtToken(User usuario)
        {
            if(usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo");
            }

            var Rols= usuario.Rols;
            var roleClaims = new List<Claim>();
            foreach (var role in Rols)
            {
                roleClaims.Add(new Claim("roles", role.Nombre));
            }

            var claims = new []
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("uid", usuario.Id.ToString())
            }
            .Union(roleClaims);

            if(string.IsNullOrEmpty(_jwt.Key) || string.IsNullOrEmpty(_jwt.Issuer) || string.IsNullOrEmpty(_jwt.Audience))
            {
                throw new ArgumentException("La configuracion del JWT es nula o o vacía.");
            }

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
           
            var signningCredentials= new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            
            var  JwtSecurityToken= new JwtSecurityToken(
                issuer : _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signningCredentials
            );
            return JwtSecurityToken;

        }
    }
