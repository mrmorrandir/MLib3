namespace MLib3.Protocols.Measurements.Xml.UnitTests;

public static class DummyProtocolGenerator
{
    public static IProtocol Generate()
    {
        return new Measurements.Protocol
        {
            Product = new Measurements.Product
            {
                Material = "Test material",
                MaterialText = "Test material text",
                Order = "Test order",
                Equipment = "Test equipment",
                Extensions = new Measurements.Extensions
                {
                    new Measurements.Extension("Key", new { Name = "Type", Description = "SomeComplexType" })
                }
            },
            Meta = new Measurements.Meta
            {
                Extensions = new Measurements.Extensions
                {
                    new Measurements.Extension("Key",
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
                Operator = "Test Operator",

            },
            Results = new Measurements.Results
            {
                Ok = true,
                Extensions = new Measurements.Extensions
                {
                    new Measurements.Extension("Key", "Value")
                },
                Data = new List<IElement>()
                {
                    // new Measurements.Section
                    // {
                    //     Ok = true,
                    //     Extensions = new Measurements.Extensions
                    //     {
                    //         new Measurements.Extension("Key", "Value")
                    //     },
                    //     Data = new List<IElement>
                    //     {
                    //         new Measurements.Comment
                    //         {
                    //             Extensions = new Measurements.Extensions
                    //             {
                    //                 new Measurements.Extension("Key", "Value")
                    //             },
                    //             Name = "Test CommentName",
                    //             Description = "Test CommentDescription",
                    //             Text = "Test CommentText"
                    //         },
                    //         new Measurements.Info
                    //         {
                    //             Extensions = new Measurements.Extensions
                    //             {
                    //                 new Measurements.Extension("Key",
                    //                     "Value")
                    //             },
                    //             Name = "Test InfoName",
                    //             Description = "Test InfoDescription",
                    //             Precision = 0.1,
                    //             Unit = "mm",
                    //             Value = 1.0,
                    //         },
                    //         new Measurements.Flag()
                    //         {
                    //             Extensions = new Measurements.Extensions
                    //             {
                    //                 new Measurements.Extension("Key", "Value")
                    //             },
                    //             Name = "Test FlagName",
                    //             Description = "Test FlagDescription",
                    //             Ok = true,
                    //         },
                    //         new Measurements.Value()
                    //         {
                    //             Extensions = new Measurements.Extensions
                    //             {
                    //                 new Measurements.Extension("Key", "Value")
                    //             },
                    //             Name = "Test ValueName",
                    //             Description = "Test ValueDescription",
                    //             Unit = "mm",
                    //             Precision = 0.1,
                    //             Min = 0.0,
                    //             Nom = 1.0,
                    //             Max = 2.0,
                    //             MinLimitType = ValueLimitType.Value,
                    //             MaxLimitType = ValueLimitType.Value,
                    //             Result = 1.0,
                    //             Ok = true,
                    //         },
                            new Measurements.RawData
                            {
                                Extensions = new Measurements.Extensions
                                {
                                    new Measurements.Extension("Key", "Value")
                                },
                                Name = "Test RawData",
                                Description = "Test RawDataDescription",
                                Format = "csv",
                                Raw = "Hello;World"
                            },
                            new Measurements.RawDataSetting
                            {
                                Extensions = new Measurements.Extensions
                                {
                                    new Measurements.Extension("Key", "Value")
                                },
                                Name = "Test RawDataSetting",
                                Description = "Test RawDataDescription",
                                Format = "csv"
                            }
                            // },
                        // Name = "Test Section 1",
                        // Description = "Test Description"
                    // }
                }
            },
            Specification = "Test",
            Version = "1.0"
        };
    }
}