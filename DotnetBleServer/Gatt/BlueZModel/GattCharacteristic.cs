﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetBleServer.Core;
using Tmds.DBus;

namespace DotnetBleServer.Gatt.BlueZModel
{
    internal class GattCharacteristic : PropertiesBase<GattCharacteristic1Properties>, IGattCharacteristic1, IObjectManagerProperties
    {
        public IList<GattDescriptor> Descriptors { get; } = new List<GattDescriptor>();

        private readonly ICharacteristic _CharacteristicSource;

        public GattCharacteristic(ObjectPath objectPath, GattCharacteristic1Properties properties, ICharacteristic characteristicSource) : base(objectPath, properties)
        {
            _CharacteristicSource = characteristicSource;
        }

        public Task<byte[]> ReadValueAsync(IDictionary<string, object> options)
        {
            return _CharacteristicSource.ReadValueAsync();
        }

        public Task WriteValueAsync(byte[] value, IDictionary<string, object> options)
        {
            return _CharacteristicSource.WriteValueAsync(value);
        }

        public Task StartNotifyAsync()
        {
            if (Properties.Notifying)
            {
                return Task.CompletedTask;
            }

            return _CharacteristicSource.StartUpdatesAsync();
        }

        public Task StopNotifyAsync()
        {
            return _CharacteristicSource.StopUpdatesAsync();
        }

        public IDictionary<string, IDictionary<string, object>> GetProperties()
        {
            return new Dictionary<string, IDictionary<string, object>>
            {
                {
                    "org.bluez.GattCharacteristic1", new Dictionary<string, object>
                    {
                        {"Service", Properties.Service},
                        {"UUID", Properties.UUID},
                        {"Flags", Properties.Flags},
                        {"Notifying", Properties.Notifying },
                        {"Descriptors", Descriptors.Select(d => d.ObjectPath).ToArray()}
                    }
                }
            };
        }

        public GattDescriptor AddDescriptor(GattDescriptor1Properties gattDescriptorProperties)
        {
            gattDescriptorProperties.Characteristic = ObjectPath;
            var gattDescriptor = new GattDescriptor(NextDescriptorPath(), gattDescriptorProperties);
            Descriptors.Add(gattDescriptor);
            return gattDescriptor;
        }

        private ObjectPath NextDescriptorPath()
        {
            return ObjectPath + "/descriptor" + Descriptors.Count;
        }
    }
}