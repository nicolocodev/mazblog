using System;
using System.Net;
using System.Security.Authentication.ExtendedProtection;

namespace Mazblog.Tests
{
    public class FakeTransport : TransportContext
    {
        public override ChannelBinding GetChannelBinding(ChannelBindingKind kind)
        {
            throw new NotImplementedException();
        }
    }
}