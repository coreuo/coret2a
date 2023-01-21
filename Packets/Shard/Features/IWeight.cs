﻿using Packets.Shared;

namespace Packets.Shard.Features;

public interface IWeight
{
    ushort Weight { get; set; }

    internal void ReadWeight<TData>(TData data)
        where TData : IData
    {
        Weight = data.ReadUShort();
    }

    internal void WriteWeight<TData>(TData data)
        where TData : IData
    {
        data.WriteUShort(Weight);
    }
}