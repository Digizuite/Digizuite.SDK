﻿using System.Threading.Tasks;
using Digizuite.Models;

namespace Digizuite
{
    public interface IProductService
    {
        Task<DigiResponse<string>> GetProductItemGuidFromVersionId(string versionId, string accessKey = null);
    }
}