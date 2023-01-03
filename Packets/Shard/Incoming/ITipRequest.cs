﻿namespace Packets.Shard.Incoming
{
    public interface ITipRequest
    {
        int Tip { set; }

        internal void ReadTipRequest<TData>(TData data)
            where TData : IData
        {
            Tip = data.ReadShort();
        }
    }
}