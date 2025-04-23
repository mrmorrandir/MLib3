namespace MLib3.Protocols.Measurements.TestCore;

public static class DummyProtocolGenerator
{
    public static Protocol Generate()
    {
        return new Protocol
        {
            Product = new Product
            {
                Material = "Test material",
                MaterialText = "Test material text",
                Order = "Test order",
                Equipment = "Test equipment",
                Extensions = new Extensions
                {
                    new Extension("Key", new { Name = "Type", Description = "SomeComplexType" })
                }
            },
            Meta = new Meta
            {
                Extensions = new Extensions
                {
                    new Extension("Key",
                        "Value")
                },
                DeviceId = "Test DeviceId",
                DeviceName = "Test DeviceName",
                Software = "Test Software",
                SoftwareVersion = "Test SoftwareVersion",
                TestRoutine = "Test TestRoutine",
                TestRoutineVersion = "Test TestRoutineVersion",
                Timestamp = DateTime.Now,
                Type = "Test Type",
                Operator = "Test Operator"
            },
            Results = new Results
            {
                Ok = true,
                Extensions = new Extensions
                {
                    new Extension("Key", "Value")
                },
                Data = new List<Element>
                {
                    new Section
                    {
                        Ok = true,
                        Extensions = new Extensions
                        {
                            new Extension("Key", "Value")
                        },
                        Data = new List<Element>
                        {
                            new Comment
                            {
                                Extensions = new Extensions
                                {
                                    new Extension("Key", "Value")
                                },
                                Name = "Test CommentName",
                                Description = "Test CommentDescription",
                                Text = "Test CommentText"
                            },
                            new Info
                            {
                                Extensions = new Extensions
                                {
                                    new Extension("Key",
                                        "Value")
                                },
                                Name = "Test InfoName",
                                Description = "Test InfoDescription",
                                Precision = 0.1,
                                Unit = "mm",
                                Value = 1.0
                            },
                            new Flag
                            {
                                Extensions = new Extensions
                                {
                                    new Extension("Key", "Value")
                                },
                                Name = "Test FlagName",
                                Description = "Test FlagDescription",
                                Ok = true
                            },
                            new Value
                            {
                                Extensions = new Extensions
                                {
                                    new Extension("Key", "Value")
                                },
                                Name = "Test ValueName",
                                Description = "Test ValueDescription",
                                Unit = "mm",
                                Precision = 0.1,
                                Min = 0.0,
                                Nom = 1.0,
                                Max = 2.0,
                                MinLimitType = ValueLimitType.Value,
                                MaxLimitType = ValueLimitType.Value,
                                Result = 1.0,
                                Ok = true
                            },
                            new RawData
                            {
                                Extensions = new Extensions
                                {
                                    new Extension("Key", "Value")
                                },
                                Name = "Test RawData",
                                Description = "Test RawDataDescription",
                                Format = "csv",
                                Raw = "Hello;World"
                            }
                        },
                        Name = "Test Section 1",
                        Description = "Test Description"
                    },
                    new Comment
                    {
                        Extensions = new Extensions
                        {
                            new Extension("Key", "Value")
                        },
                        Name = "Test CommentName",
                        Description = "Test CommentDescription",
                        Text = "Test CommentText"
                    },
                    new Info
                    {
                        Extensions = new Extensions
                        {
                            new Extension("Key",
                                "Value")
                        },
                        Name = "Test InfoName",
                        Description = "Test InfoDescription",
                        Precision = 0.1,
                        Unit = "mm",
                        Value = 1.0
                    },
                    new Flag
                    {
                        Extensions = new Extensions
                        {
                            new Extension("Key", "Value")
                        },
                        Name = "Test FlagName",
                        Description = "Test FlagDescription",
                        Ok = true
                    },
                    new Value
                    {
                        Extensions = new Extensions
                        {
                            new Extension("Key", "Value")
                        },
                        Name = "Test ValueName",
                        Description = "Test ValueDescription",
                        Unit = "mm",
                        Precision = 0.1,
                        Min = 0.0,
                        Nom = 1.0,
                        Max = 2.0,
                        MinLimitType = ValueLimitType.Value,
                        MaxLimitType = ValueLimitType.Value,
                        Result = 1.0,
                        Ok = true
                    },
                    new RawData
                    {
                        Extensions = new Extensions
                        {
                            new Extension("Key", "Value")
                        },
                        Name = "Test RawData",
                        Description = "Test RawDataDescription",
                        Format = "csv",
                        Raw = "Hello;World"
                    },
                    new RawData()
                    {
                        Name = "Test empty RawData",
                        Description = "Test empty RawDataDescription",
                        Format = "csv"
                    },
                    new Comment()
                    {
                        Extensions = new Extensions()
                    },
                    new FlagSetting()
                    {
                        Extensions = new Extensions()
                        {
                            new Extension("Key", "Value")
                        },
                        Name = "Test FlagSetting",
                        Description = "Test FlagSettingDescription"
                    },
                    new ValueSetting
                    {
                        Extensions = new Extensions()
                        {
                            new Extension("Key", "Value")
                        },
                        Name = "Test ValueSetting",
                        Description = "Test ValueSettingDescription",
                        Unit = "mm",
                        Precision = 0.1,
                        Min = 0.0,
                        Nom = 0.5,
                        Max = 1.0,
                        MinLimitType = ValueLimitType.Natural,
                        MaxLimitType = ValueLimitType.Value
                    },
                    new Value
                    {
                        Name = "Test ValueName",
                        Result = 0.0,
                        Ok = false
                    },
                    new ValueSetting
                    {
                        Name = "Test ValueSetting",
                    },
                    new InfoSetting
                    {
                        Extensions = new Extensions()
                        {
                            new Extension("Key", "Value")
                        },
                        Name = "Test InfoSetting",
                        Description = "Test InfoSettingDescription",
                        Precision = 0.1,
                        Unit = "mm",

                    },
                    new CommentSetting
                    {
                        Extensions = new Extensions()
                        {
                            new Extension("Key", "Value")
                        },
                        Name = "Test CommentSetting",
                        Description = "Test CommentSettingDescription",
                    }
                }
            },
            Specification = "Test",
            Version = "1.0"
        };
    }
}