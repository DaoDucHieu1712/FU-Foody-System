using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;

using Moq;

namespace Test.Mocks {
    public class MockIStoreRepository {
        public static Mock<IStoreRepository> GetMock()
        {
            var mock = new Mock<IStoreRepository>();

            Store store = new Store()
            {

            };
          

            mock.Setup(m => m.FindById(It.IsAny<int>(), null))
                .Returns(() => Task.FromResult(store));

            return mock;
        }
    }
}
