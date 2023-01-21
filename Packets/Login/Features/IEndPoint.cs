﻿using Core.Abstract.Attributes;

namespace Packets.Login.Features
{
    public interface IEndPoint
    {
        [Length(15)]
        Span<char> IpAddress { get; }

        int Port { get; }
    }
}
