using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Validations;

namespace InfinniPlatform.DesignControls.Controls.Properties
{
    public static class Action
    {
        public static Dictionary<string, IControlProperty> GetActions()
        {
            return new Dictionary<string, IControlProperty>
            {
                {
                    "AddAction", new ObjectProperty(new Dictionary<string, IControlProperty>
                    {
                        {"DataSource", new SimpleProperty(string.Empty)},
                        {
                            "View",
                            new ObjectProperty(new Dictionary<string, IControlProperty>().GetLinkViews(),
                                new Dictionary<string, CollectionProperty>())
                        }
                    },
                        new Dictionary<string, CollectionProperty>(),
                        new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>
                        {
                            {"DataSource", Common.CreateNullOrEmptyValidator("AddAction", "DataSource")},
                            {"View", Common.CreateNullOrEmptyValidator("AddAction", "View")}
                        })
                },
                {
                    "AddItemAction", new ObjectProperty(new Dictionary<string, IControlProperty>
                    {
                        {
                            "Items",
                            new ObjectProperty(new Dictionary<string, IControlProperty>().GetBindings(),
                                new Dictionary<string, CollectionProperty>())
                        },
                        {
                            "View",
                            new ObjectProperty(new Dictionary<string, IControlProperty>().GetLinkViews(),
                                new Dictionary<string, CollectionProperty>())
                        }
                    },
                        new Dictionary<string, CollectionProperty>(),
                        new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>
                        {
                            {"Items", Common.CreateNullOrEmptyValidator("AddAItemction", "Items")},
                            {"View", Common.CreateNullOrEmptyValidator("AddItemAction", "View")}
                        })
                },


                {
                    "EditAction", new ObjectProperty(new Dictionary<string, IControlProperty>
                    {
                        {"DataSource", new SimpleProperty(string.Empty)},
                        {
                            "View", new ObjectProperty(new Dictionary<string, IControlProperty>().GetLinkViews(),
                                new Dictionary<string, CollectionProperty>(),
                                new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>
                                {
                                    {"DataSource", Common.CreateNullOrEmptyValidator("EditAction", "DataSource")},
                                    {"View", Common.CreateNullOrEmptyValidator("EditAction", "View")}
                                })
                        }
                    }, new Dictionary<string, CollectionProperty>())
                },
                {
                    "EditItemAction", new ObjectProperty(new Dictionary<string, IControlProperty>
                    {
                        {
                            "Items",
                            new ObjectProperty(new Dictionary<string, IControlProperty>().GetBindings(),
                                new Dictionary<string, CollectionProperty>())
                        },
                        {
                            "View",
                            new ObjectProperty(new Dictionary<string, IControlProperty>().GetLinkViews(),
                                new Dictionary<string, CollectionProperty>())
                        }
                    },
                        new Dictionary<string, CollectionProperty>(),
                        new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>
                        {
                            {"Items", Common.CreateNullOrEmptyValidator("EditItemAction", "Items")},
                            {"View", Common.CreateNullOrEmptyValidator("EditItemAction", "View")}
                        })
                },
                {
                    "SaveItemAction", new ObjectProperty(new Dictionary<string, IControlProperty>
                    {
                        {"DataSource", new SimpleProperty(string.Empty)},
                        {"CanClose", new SimpleProperty(true)}
                    }, new Dictionary<string, CollectionProperty>(),
                        new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>
                        {
                            {"DataSource", Common.CreateNullOrEmptyValidator("SaveItemAction", "DataSource")}
                        })
                },
                {
                    "DeleteAction", new ObjectProperty(new Dictionary<string, IControlProperty>
                    {
                        {"DataSource", new SimpleProperty(string.Empty)},
                        {
                            "View",
                            new ObjectProperty(new Dictionary<string, IControlProperty>().GetLinkViews(),
                                new Dictionary<string, CollectionProperty>())
                        }
                    }, new Dictionary<string, CollectionProperty>(),
                        new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>
                        {
                            {"DataSource", Common.CreateNullOrEmptyValidator("DeleteAction", "DataSource")},
                            {"View", Common.CreateNullOrEmptyValidator("DeleteAction", "View")}
                        })
                },
                {
                    "DeleteItemAction", new ObjectProperty(new Dictionary<string, IControlProperty>
                    {
                        {
                            "Items",
                            new ObjectProperty(new Dictionary<string, IControlProperty>().GetBindings(),
                                new Dictionary<string, CollectionProperty>())
                        },
                        {"Accept", new SimpleProperty(false)}
                    },
                        new Dictionary<string, CollectionProperty>(),
                        new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>
                        {
                            {"Items", Common.CreateNullOrEmptyValidator("AddAItemction", "Items")}
                        })
                },
                {
                    "SaveAction", new ObjectProperty(new Dictionary<string, IControlProperty>
                    {
                        {"DataSource", new SimpleProperty(string.Empty)},
                        {"CanClose", new SimpleProperty(true)}
                    }, new Dictionary<string, CollectionProperty>(),
                        new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>
                        {
                            {"DataSource", Common.CreateNullOrEmptyValidator("SaveAction", "DataSource")}
                        })
                },
                {
                    "AcceptAction", new ObjectProperty(new Dictionary<string, IControlProperty>
                    {
                        {"Name", new SimpleProperty("")}
                    }
                        , new Dictionary<string, CollectionProperty>())
                },
                {
                    "CancelAction", new ObjectProperty(new Dictionary<string, IControlProperty>
                    {
                        {"Name", new SimpleProperty("")}
                    }, new Dictionary<string, CollectionProperty>())
                },

                {
                    "SelectAction", new ObjectProperty(new Dictionary<string, IControlProperty>
                    {
                        {
                            "SourceValue",
                            new ObjectProperty(new Dictionary<string, IControlProperty>().GetBindings(),
                                new Dictionary<string, CollectionProperty>())
                        },
                        {
                            "DestinationValue",
                            new ObjectProperty(new Dictionary<string, IControlProperty>().GetBindings(),
                                new Dictionary<string, CollectionProperty>())
                        },
                        {
                            "View",
                            new ObjectProperty(new Dictionary<string, IControlProperty>().GetLinkViews(),
                                new Dictionary<string, CollectionProperty>())
                        }
                    },
                        new Dictionary<string, CollectionProperty>(),
                        new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>
                        {
                            {"SourceValue", Common.CreateNullOrEmptyValidator("SelectAction", "SourceValue")},
                            {"DestinationValue", Common.CreateNullOrEmptyValidator("SelectAction", "DestinationValue")},
                            {"View", Common.CreateNullOrEmptyValidator("SelectAction", "View")}
                        })
                }
            };
        }
    }
}