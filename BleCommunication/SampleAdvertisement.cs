﻿using System.Threading.Tasks;
using BleServer.Infrastructure.BlueZ.Advertisements;
using BleServer.Infrastructure.BlueZ.Core;

namespace BleServer
{
    public class SampleAdvertisement
    {
        public static async Task RegisterSampleAdvertisement(ServerContext serverContext)
        {
            var advertisement = new Advertisement("/org/bluez/example/advertisement0", new AdvertisementProperties
            {
                Type = "peripheral",
                ServiceUUIDs = new[] { "12345678-1234-5678-1234-56789abcdef0"},
                LocalName = "A",
            });

            await new AdvertisingManager(serverContext).RegisterAdvertisement(advertisement);
        }
    }
}