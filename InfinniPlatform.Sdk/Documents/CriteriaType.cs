﻿namespace InfinniPlatform.Sdk.Documents
{
    public enum CriteriaType
    {
        IsEquals = 1,
        IsNotEquals = 2,
        IsMoreThan = 4,
        IsLessThan = 8,
        IsMoreThanOrEquals = 16,
        IsLessThanOrEquals = 32,
        IsContains = 64,
        IsNotContains = 128,
        IsEmpty = 256,
        IsNotEmpty = 512,
        IsStartsWith = 1024,
        IsNotStartsWith = 2048,
        IsEndsWith = 4096,
        IsNotEndsWith = 8192,
        IsIn = 16384,
        Script = 32768,
        FullTextSearch = 65536,
        IsIdIn = 131072
    }
}