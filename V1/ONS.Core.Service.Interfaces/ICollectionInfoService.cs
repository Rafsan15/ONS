﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using FMS.FrameWork;
using ONS.Core.Entities;

namespace ONS.Core.Service.Interfaces
{
  public  interface ICollectionInfoService:IService<CollectionInfo>
  {
      Result<List<CollectionInfo>> GetAllById(int CollectionId);
  }
}
