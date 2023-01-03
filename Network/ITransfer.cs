﻿namespace Network;

public interface ITransfer<TData>
    where TData : IData
{
    TData LeaseData();

    void ReleaseData(TData data);
}