﻿namespace Webport.ERP.Common.Domain.Abstractions;

public interface IMustHaveTenant
{
    public int TenantId { get; set; }
}
