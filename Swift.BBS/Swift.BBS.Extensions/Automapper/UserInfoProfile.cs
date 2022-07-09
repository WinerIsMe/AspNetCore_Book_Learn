using AutoMapper;
using Swift.BBS.Model.Models;
using Swift.BBS.Model.ViewModels.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift.BBS.Extensions.AutoMapper
{
    public class UserInfoProfile : Profile
    {
        public UserInfoProfile()
        {
            CreateMap<CreateUserInfoInputDto, UserInfo>();
            CreateMap<UserInfo, UserInfoDto>();
            CreateMap<UserInfo, UserInfoDetailsDto>();
        }
    }
}
