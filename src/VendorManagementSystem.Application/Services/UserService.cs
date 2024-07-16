using Microsoft.EntityFrameworkCore;
using System.Text;
using VendorManagementSystem.Application.Dtos.ModelDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Exceptions;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Application.Utilities;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IErrorLoggingService _errorLog;

        public UserService(IUserRepository userRepository, ITokenService tokenService, IEmailService emailService, IUserTokenRepository userTokenRepository, IErrorLoggingService errorLog)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _emailService = emailService;
            _userTokenRepository = userTokenRepository;
            _errorLog = errorLog;
        }
        //delete
        public User CreateSuperAdmin(SuperAdminDto superAdminDto)
        {
            ArgumentNullException.ThrowIfNull(superAdminDto);
            User user = new()
            {
                Email = superAdminDto.Email,
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(superAdminDto.Password)),
                UserName = superAdminDto.UserName,
                Role = "superadmin",
                Status = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = 1,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = 1,
            };
            try
            {
                int userAddition = _userRepository.AddUser(user);
                if (userAddition > 0) return user;
                else throw new DbUpdateException("Unable to add user to Database");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new User();
            }
        }
        public ApplicationResponseDto<User> CreateUser(CreateUserDto userDto, string jwtToken)
        {
            try
            {
                User? existingUser = _userRepository.GetUserByEmail(userDto.Email);
                if (existingUser != null)
                {
                    _errorLog.LogError($"in the funciton CreateUser, Provided a duplicate email {existingUser}");

                    return new ApplicationResponseDto<User>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.DuplicateEntryError,
                            Message = new List<string> { $"Email {userDto.Email} already present" }
                        },
                    };
                }
                string currentUser = _tokenService.ExtractUserDetials(jwtToken, "id");
                User user = new()
                {
                    Email = userDto.Email,
                    UserName = userDto.UserName,
                    Role = userDto.Role,
                    Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("Dummy")),
                    Status = false,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = Int32.Parse(currentUser),
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = Int32.Parse(currentUser),
                };
                using (var transaction = _userRepository.BeginTransaction())
                {
                    try
                    {
                        TokenDto tokenDto = _tokenService.JwtToken(user, "newUser");
                        int userAddition = _userRepository.AddUser(user);
                        int tokenAddition = _userTokenRepository.AdDtoken(user.Email, tokenDto.Token);
                        if (userAddition > 0 && tokenAddition > 0)
                        {
                            EmailDetailsDto emailDetailsDto = new()
                            {
                                ToAddress = userDto.Email,
                                ToName = userDto.UserName.Replace("$", " "),
                                Body = EmailUtility.GetInvitationBody(userDto.UserName, currentUser, $"{userDto.RedirectUrl}{tokenDto.Token}"),
                                Subject = "VMS Invitation",

                            };
                            _emailService.SendLoginEmail(emailDetailsDto);
                            transaction.Commit();
                            return new ApplicationResponseDto<User>
                            {
                                Data = user,
                            };
                        }
                        else
                        {
                            transaction.Rollback();
                            _errorLog.LogError($"{(int)ErrorCodes.DatabaseError} In Function CreateUser, User didn't aded to db");
                            return new ApplicationResponseDto<User>
                            {
                                Error = new()
                                {
                                    Code = (int)ErrorCodes.DatabaseError,
                                    Message = new List<string> { "Unable to add user to Database" },
                                },
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                        return new ApplicationResponseDto<User>
                        {
                            Error = new()
                            {
                                Code = (int)ErrorCodes.InternalError,
                                Message = new List<string> { ex.Message },
                            },
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<User>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    },
                };
            }
        }

        public ApplicationResponseDto<PagenationResponseDto> GetAllUsers(PaginationDto paginationDto, string? filter)
        {
            try
            {
                if (paginationDto.Cursor < 0 || paginationDto.Size <= 0)
                {
                    return new ApplicationResponseDto<PagenationResponseDto>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.InvalidInputFields,
                            Message = new List<string>() { $"Inputs {paginationDto.Cursor} or {paginationDto.Size} is invalid. 'LastId should not be negative and Pagesize should be greater than 0'" }
                        }
                    };
                }

                IEnumerable<User> users = _userRepository.GetUsers(paginationDto, filter);
                int currentLastId = users.Any() ? users.ElementAt(users.Count() - 1).Id : 0;
                int currentFirstId = users.Any() ? users.ElementAt(0).Id : 0;

                return new ApplicationResponseDto<PagenationResponseDto>
                {
                    Data = new PagenationResponseDto
                    {
                        PagenationData = users,
                        Cursor = currentLastId,
                        PreviousCursor = currentFirstId,
                        /*Size = users.Count(),*/
                        HasNextPage = (currentLastId != 0) && _userRepository.NeighbourExistance(currentLastId, true),
                        HasPreviousPage = (currentFirstId != 0) && _userRepository.NeighbourExistance(currentFirstId, false),
                    }
                };
            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<PagenationResponseDto>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    },
                };
            }
        }

        public ApplicationResponseDto<TokenDto> Login(LoginDto loginDto)
        {
            try
            {
                User? user = _userRepository.GetUserByEmail(loginDto.Email);
                if (user == null)
                {
                    _errorLog.LogError($"in the funciton CreateUser, provided invalid email '{loginDto.Email}'.");
                    return new ApplicationResponseDto<TokenDto>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.NullArgument,
                            Message = new List<string> { $"provided invalid email '{loginDto.Email}'" }
                        },
                    };
                }

                var encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(loginDto.Password));

                if (loginDto.Email == user.Email && encodedPassword == user.Password)
                {
                    TokenDto tokenDto;
                    if (loginDto.RememberMe)
                    {
                        tokenDto = _tokenService.JwtToken(user, "rememberme");
                    }
                    else
                    {
                        tokenDto = _tokenService.JwtToken(user, "login");
                    }
                    return new ApplicationResponseDto<TokenDto>
                    {
                        Data = tokenDto,
                    };
                }

                else
                {
                    _errorLog.LogError($"{(int)ErrorCodes.InvalidCredintials}. In function Login, Provided Invalid login credentials");
                    return new ApplicationResponseDto<TokenDto>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.InvalidCredintials,
                            Message = new List<string> { "The Given Credintials are Invalid" },
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<TokenDto>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    },
                };
            }
        }

        public ApplicationResponseDto<bool> SetUserPassword(UpdatePasswordDto updatePasswordDto, string _token)
        {
            var email = updatePasswordDto.Email;
            var newPassword = updatePasswordDto.Password;

            try
            {
                var isTokenValid = _tokenService.ValidateToken(_token);
                var isTokenInDb = _userTokenRepository.GetToken(email);

                if (!isTokenValid || isTokenInDb != _token)
                {
                    Console.WriteLine($"{isTokenInDb} {_token}");
                    return new ApplicationResponseDto<bool>
                    {
                        Error = new Error
                        {
                            Code = (int)ErrorCodes.InvaliDtoken,
                            Message = new List<string>()
                            {
                                "The Token is Invalid or Expired",
                            }
                        },
                        Data = false,
                    };
                }

                string tokenEmail = _tokenService.ExtractUserDetials(_token, "email");
                if (tokenEmail == null || tokenEmail != email)
                {
                    return new ApplicationResponseDto<bool>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.AuthenthicationError,
                            Message = new List<string> { "Token is invalid" }
                        },
                        Data = false,
                    };
                }
                User? user = _userRepository.GetUserByEmail(tokenEmail!);
                if (user == null)
                {
                    _errorLog.LogError($"{(int)ErrorCodes.NotFound} In function SetUserPassword, Email '{email}' not found in the database.");
                    return new ApplicationResponseDto<bool>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.NotFound,
                            Message = new List<string> { $"Email '{email}' not found in the database." }
                        },
                        Data = false,
                    };
                }

                string hashedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(newPassword));
                if (user!.Password == hashedPassword)
                {
                    return new ApplicationResponseDto<bool>
                    {
                        Error = new Error
                        {
                            Code = (int)ErrorCodes.DuplicateEntryError,
                            Message = new List<string>()
                            {
                                "Provide a new password",
                            }
                        },
                        Data = false,
                    };
                }
                var transaction = _userRepository.BeginTransaction();
                bool res = _userRepository.UpdatePassword(email, hashedPassword);
                _userTokenRepository.DeleteToken(email);
                transaction.Commit();

                return new ApplicationResponseDto<bool>
                {
                    Data = res,
                };
            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<bool>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    },
                };
            }
        }

        public ApplicationResponseDto<bool> SendForgetPasswordEmail(string email, string redirectUrl)
        {
            try
            {
                User? user = _userRepository.GetUserByEmail(email);
                if (user == null)
                {
                    _errorLog.LogError($"{(int)ErrorCodes.NotFound} In function SetUserPassword, Email '{email}' not found in the database.");
                    return new ApplicationResponseDto<bool>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.NotFound,
                            Message = new List<string> { $"Email '{email}' not found in the database." }
                        },
                        Data = false,
                    };
                }

                using (var transaction = _userRepository.BeginTransaction())
                {
                    try
                    {
                        TokenDto tokenDto = _tokenService.JwtToken(user, "resetpassword");
                        _userTokenRepository.AdDtoken(user.Email, tokenDto.Token);
                        EmailDetailsDto emailDetailsDto = new()
                        {
                            ToAddress = email,
                            ToName = user.UserName.Replace("$", " "),
                            Body = EmailUtility.ForgetPasswordBody(user.UserName, $"{redirectUrl}{tokenDto.Token}"),
                            Subject = "Reset Password",

                        };

                        _emailService.SendLoginEmail(emailDetailsDto);
                        transaction.Commit();
                        return new ApplicationResponseDto<bool>
                        {
                            Data = true,
                        };
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                        return new ApplicationResponseDto<bool>
                        {
                            Error = new()
                            {
                                Code = (int)ErrorCodes.InternalError,
                                Message = new List<string> { ex.Message },
                            },
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<bool>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    },
                };
            }

        }

        public ApplicationResponseDto<string> ValidateToken(string token)
        {
            try
            {
                if (_tokenService.ValidateToken(token))
                {
                    string email = _tokenService.ExtractUserDetials(token, "email");
                    var dbToken = _userTokenRepository.GetToken(email);
                    if (dbToken == token)
                    {
                        Console.WriteLine($"{dbToken}");
                        Console.WriteLine($"{token}");
                    }

                    if (dbToken != token)
                    {
                        return new ApplicationResponseDto<string>
                        {
                            Error = new Error
                            {
                                Code = (int)ErrorCodes.InvaliDtoken,
                                Message = new List<string>()
                                {
                                    "The token is invalid or Expired",
                                }
                            },
                            Data = string.Empty,
                        };
                    }
                    return new ApplicationResponseDto<string>
                    {
                        Data = email,
                    };
                }
                else
                {
                    return new ApplicationResponseDto<string>
                    {
                        Error = new Error
                        {
                            Code = (int)ErrorCodes.InvaliDtoken,
                            Message = new List<string>()
                                {
                                    "The token is invalid or Expired",
                                }
                        },
                        Data = string.Empty,
                    };
                }
            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<string>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    },
                };
            }
        }
    }
}
