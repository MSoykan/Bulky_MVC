using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository {
	internal class IUnityOfWork {
		ICategoryRepository CategoryRepository { get; set; }
		//void Save();
	}
}
