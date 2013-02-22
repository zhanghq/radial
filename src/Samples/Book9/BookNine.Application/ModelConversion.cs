using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookNine.Domain.Model;
using BookNine.TransferObject;

namespace BookNine.Application
{
    public static class ModelConversion
    {
        public static void RegisterMappers()
        {
            Mapper.CreateMap<User, UserModel>();
        }
    }
}
