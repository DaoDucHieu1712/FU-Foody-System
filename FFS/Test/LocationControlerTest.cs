using AutoMapper;
using FFS.Application.Controllers;
using FFS.Application.Data;
using FFS.Application.DTOs.Common;
using FFS.Application.Entities;
using FFS.Application.Repositories.Impls;
using FFS.Application.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class LocationControlerTest
    {
        private Mock<IOptionsMonitor<AppSetting>> mockOptionsMonitor;
        private Mock<ApplicationDbContext> mockDbContext;
        private Mock<IMapper> mockMapper;
        private Mock<DapperContext> mockDapperContext;
        private Mock<IConfiguration> mockConfiguration;
        private DapperContext dapperContext;
        private LocationRepository authRepository;
        private LocationController controller;
        public LocationControlerTest()
        {

        }
    }
}
